using System;

namespace Quibee.Services;

/// <summary>
/// Datos del evento cuando se completa un ejercicio.
/// </summary>
public class ExerciseCompletedEventArgs : EventArgs
{
    /// <summary>
    /// Cantidad de respuestas correctas.
    /// </summary>
    public int CorrectCount { get; set; }

    /// <summary>
    /// Total de respuestas.
    /// </summary>
    public int TotalCount { get; set; }

    /// <summary>
    /// Si todas las respuestas fueron correctas.
    /// </summary>
    public bool AllCorrect => CorrectCount == TotalCount;

    /// <summary>
    /// Puntos ganados.
    /// </summary>
    public int PointsEarned { get; set; }

    /// <summary>
    /// Tipo de sección donde se completó (resolvamos, desafio, etc.)
    /// </summary>
    public string SectionType { get; set; } = string.Empty;
}

/// <summary>
/// Messenger simple para comunicar eventos entre controles y ViewModels.
/// </summary>
public static class ExerciseMessenger
{
    /// <summary>
    /// Se dispara cuando un ejercicio se completa (bien o mal).
    /// </summary>
    public static event EventHandler<ExerciseCompletedEventArgs>? ExerciseCompleted;

    /// <summary>
    /// Notifica que un ejercicio fue completado.
    /// </summary>
    public static void NotifyExerciseCompleted(ExerciseCompletedEventArgs args)
    {
        ExerciseCompleted?.Invoke(null, args);
    }
}
