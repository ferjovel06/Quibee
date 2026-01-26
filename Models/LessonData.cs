using System.Collections.Generic;

namespace Quibee.Models;

/// <summary>
/// Modelo de datos para una lección completa
/// </summary>
public class LessonData
{
    /// <summary>
    /// Título de la lección
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Número de lección
    /// </summary>
    public int LessonNumber { get; set; }

    /// <summary>
    /// Tema al que pertenece
    /// </summary>
    public string ThemeName { get; set; } = string.Empty;

    /// <summary>
    /// Secciones de la lección (Introducción, Analicemos, etc.)
    /// </summary>
    public List<LessonSection> Sections { get; set; } = new List<LessonSection>();

    /// <summary>
    /// ID del estudiante que está tomando la lección
    /// </summary>
    public int StudentId { get; set; }

    /// <summary>
    /// Grado al que pertenece la lección (1, 2, o 3)
    /// </summary>
    public int GradeLevel { get; set; }

    /// <summary>
    /// Número de tema (1-5)
    /// </summary>
    public int ThemeNumber { get; set; }
}
