using System;
using System.Collections.Generic;
using System.Linq;

namespace Quibee.Services;

public readonly struct SectionCheckResult
{
    public SectionCheckResult(bool ready, int correctCount, int totalCount)
    {
        Ready = ready;
        CorrectCount = correctCount;
        TotalCount = totalCount;
    }

    public bool Ready { get; }
    public int CorrectCount { get; }
    public int TotalCount { get; }
}

public readonly struct SectionCheckAggregate
{
    public SectionCheckAggregate(bool hasParticipants, bool allReady, int correctCount, int totalCount)
    {
        HasParticipants = hasParticipants;
        AllReady = allReady;
        CorrectCount = correctCount;
        TotalCount = totalCount;
    }

    public bool HasParticipants { get; }
    public bool AllReady { get; }
    public int CorrectCount { get; }
    public int TotalCount { get; }
}

public static class SectionCheckCoordinator
{
    private sealed class Participant
    {
        public required WeakReference<object> Owner { get; init; }
        public required int OrderIndex { get; init; }
        public required Func<SectionCheckResult> Evaluate { get; init; }
        public required Action<bool> SetHostVisible { get; init; }
    }

    private static readonly Dictionary<string, List<Participant>> Groups = new();

    public static void Register(
        string key,
        object owner,
        int orderIndex,
        Func<SectionCheckResult> evaluate,
        Action<bool> setHostVisible)
    {
        if (!Groups.TryGetValue(key, out var group))
        {
            group = new List<Participant>();
            Groups[key] = group;
        }

        group.RemoveAll(p => !p.Owner.TryGetTarget(out var target) || ReferenceEquals(target, owner));
        group.Add(new Participant
        {
            Owner = new WeakReference<object>(owner),
            OrderIndex = orderIndex,
            Evaluate = evaluate,
            SetHostVisible = setHostVisible
        });

        RefreshHost(key);
    }

    public static void Unregister(string key, object owner)
    {
        if (!Groups.TryGetValue(key, out var group))
        {
            return;
        }

        group.RemoveAll(p => !p.Owner.TryGetTarget(out var target) || ReferenceEquals(target, owner));

        if (group.Count == 0)
        {
            Groups.Remove(key);
            return;
        }

        RefreshHost(key);
    }

    public static bool IsHost(string key, object owner)
    {
        var live = GetLiveParticipants(key);
        if (live.Count == 0)
        {
            return true;
        }

        var host = live[^1];
        return host.Owner.TryGetTarget(out var target) && ReferenceEquals(target, owner);
    }

    public static SectionCheckAggregate EvaluateGroup(string key)
    {
        var live = GetLiveParticipants(key);
        if (live.Count == 0)
        {
            return new SectionCheckAggregate(false, false, 0, 0);
        }

        var allReady = true;
        var total = 0;
        var correct = 0;

        foreach (var participant in live)
        {
            var result = participant.Evaluate();
            allReady = allReady && result.Ready;
            total += result.TotalCount;
            correct += result.CorrectCount;
        }

        return new SectionCheckAggregate(true, allReady, correct, total);
    }

    private static void RefreshHost(string key)
    {
        var live = GetLiveParticipants(key);
        if (live.Count == 0)
        {
            return;
        }

        for (var i = 0; i < live.Count; i++)
        {
            var isHost = i == live.Count - 1;
            live[i].SetHostVisible(isHost);
        }
    }

    private static List<Participant> GetLiveParticipants(string key)
    {
        if (!Groups.TryGetValue(key, out var group))
        {
            return new List<Participant>();
        }

        group.RemoveAll(p => !p.Owner.TryGetTarget(out _));

        if (group.Count == 0)
        {
            Groups.Remove(key);
            return new List<Participant>();
        }

        return group
            .OrderBy(p => p.OrderIndex)
            .ToList();
    }
}
