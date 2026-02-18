using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Quibee.Database;
using Quibee.Models;

namespace Quibee.Services;

/// <summary>
/// Servicio para gestionar temas (Topics) del mapa de lecciones
/// </summary>
public class TopicService
{
    private readonly QuibeeDbContext _context;

    public TopicService(QuibeeDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Obtiene todos los temas de un nivel específico, ordenados
    /// </summary>
    public async Task<List<Topic>> GetTopicsByLevelAsync(int levelNumber)
    {
        return await _context.Topics
            .AsNoTracking()
            .Include(t => t.Level)
            .Include(t => t.Lessons)
            .Where(t => t.Level != null && t.Level.LevelNumber == levelNumber && t.IsActive)
            .OrderBy(t => t.OrderIndex)
            .ToListAsync();
    }

    /// <summary>
    /// Obtiene el porcentaje de progreso del estudiante para un nivel.
    /// </summary>
    public async Task<int?> GetStudentLevelProgressPercentageAsync(int studentId, int levelNumber)
    {
        var levelId = await _context.Levels
            .AsNoTracking()
            .Where(l => l.LevelNumber == levelNumber)
            .Select(l => l.IdLevel)
            .FirstOrDefaultAsync();

        if (levelId == 0)
        {
            return null;
        }

        var percentage = await _context.StudentLevelProgress
            .AsNoTracking()
            .Where(p => p.IdStudent == studentId && p.IdLevel == levelId)
            .Select(p => p.CompletionPercentage)
            .FirstOrDefaultAsync();

        if (!percentage.HasValue)
        {
            return null;
        }

        var normalized = Math.Clamp((int)Math.Round(percentage.Value), 0, 100);
        return normalized;
    }

    /// <summary>
    /// Obtiene progreso por lección para un estudiante.
    /// Como la tabla no tiene porcentaje por lección, se mapea is_completed a 100/0.
    /// </summary>
    public async Task<Dictionary<int, int>> GetStudentLessonProgressByLessonIdsAsync(
        int studentId,
        IReadOnlyCollection<int> lessonIds)
    {
        if (lessonIds.Count == 0)
        {
            return new Dictionary<int, int>();
        }

        var progressRows = await _context.StudentLessonProgress
            .AsNoTracking()
            .Where(p => p.IdStudent == studentId && lessonIds.Contains(p.IdLesson))
            .Select(p => new { p.IdLesson, p.IsCompleted })
            .ToListAsync();

        var progressByLesson = lessonIds.ToDictionary(id => id, _ => 0);
        foreach (var row in progressRows)
        {
            progressByLesson[row.IdLesson] = row.IsCompleted ? 100 : 0;
        }

        return progressByLesson;
    }

    /// <summary>
    /// Obtiene un tema específico por ID
    /// </summary>
    public async Task<Topic?> GetTopicByIdAsync(int topicId)
    {
        return await _context.Topics
            .AsNoTracking()
            .Include(t => t.Level)
            .Include(t => t.Lessons)
            .FirstOrDefaultAsync(t => t.IdTopic == topicId);
    }

    /// <summary>
    /// Crea o actualiza un tema
    /// </summary>
    public async Task<Topic> SaveTopicAsync(Topic topic)
    {
        if (topic.IdTopic == 0)
        {
            // Nuevo tema
            topic.CreatedAt = DateTime.Now;
            _context.Topics.Add(topic);
        }
        else
        {
            // Actualizar
            topic.UpdatedAt = DateTime.Now;
            _context.Topics.Update(topic);
        }

        await _context.SaveChangesAsync();
        return topic;
    }

    /// <summary>
    /// Elimina un tema (soft delete - marca como inactivo)
    /// </summary>
    public async Task<bool> DeleteTopicAsync(int topicId)
    {
        var topic = await _context.Topics.FindAsync(topicId);
        if (topic == null) return false;

        topic.IsActive = false;
        topic.UpdatedAt = DateTime.Now;
        await _context.SaveChangesAsync();
        return true;
    }
}
