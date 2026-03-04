using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Quibee.Database;
using Quibee.Models;

namespace Quibee.Services;

/// <summary>
/// Servicio para gestionar el contenido dinámico de las lecciones
/// </summary>
public class LessonContentService
{
    private readonly QuibeeDbContext _context;

    public LessonContentService(QuibeeDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Obtiene todo el contenido de una lección ordenado por section_type y order_index
    /// </summary>
    /// <param name="lessonId">ID de la lección</param>
    /// <returns>Lista de contenidos con datos deserializados</returns>
    public async Task<List<LessonContent>> GetLessonContentAsync(int lessonId)
    {
        var contents = await _context.LessonContents
            .AsNoTracking()
            .Where(lc => lc.IdLesson == lessonId && lc.IsActive)
            .OrderBy(lc => lc.SectionType)
            .ThenBy(lc => lc.OrderIndex)
            .ToListAsync();

        // Deserializar el JSON de cada contenido
        foreach (var content in contents)
        {
            content.DeserializeData();
        }

        return contents;
    }

    /// <summary>
    /// Obtiene el contenido de una sección específica de una lección.
    /// Combina contenido estático (LESSON_CONTENT) con ejercicios interactivos (EXERCISE),
    /// ordenados por order_index para respetar el flujo de la lección.
    /// </summary>
    public async Task<List<LessonContent>> GetLessonContentBySectionAsync(int lessonId, string sectionType)
    {
        // 1. Contenido estático desde LESSON_CONTENT
        var staticContents = await _context.LessonContents
            .AsNoTracking()
            .Where(lc => lc.IdLesson == lessonId
                      && lc.SectionType == sectionType
                      && lc.IsActive)
            .OrderBy(lc => lc.OrderIndex)
            .ToListAsync();

        foreach (var content in staticContents)
            content.DeserializeData();

        // 2. Ejercicios interactivos desde EXERCISE → mapeados a LessonContent
        var exercises = await _context.Exercises
            .AsNoTracking()
            .Where(e => e.IdLesson == lessonId
                     && e.SectionType == sectionType
                     && e.IsActive)
            .OrderBy(e => e.OrderIndex)
            .ToListAsync();

        var exerciseContents = exercises.Select(e => new LessonContent
        {
            IdContent = -e.IdExercise, // negativo para distinguir de LESSON_CONTENT
            IdLesson   = e.IdLesson,
            ContentType = MapExerciseType(e.ExerciseType),
            ContentDataJson = e.Config ?? "{}",
            OrderIndex  = e.OrderIndex,
            SectionType = e.SectionType,
            IsActive    = e.IsActive,
        }).ToList();

        foreach (var ec in exerciseContents)
            ec.DeserializeData();

        // 3. Combinar y ordenar por order_index
        return staticContents
            .Concat(exerciseContents)
            .OrderBy(c => c.OrderIndex)
            .ToList();
    }

    /// <summary>
    /// Mapea el exercise_type de EXERCISE al content_type que entiende DynamicContentControl.
    /// </summary>
    private static string MapExerciseType(string exerciseType) => exerciseType switch
    {
        "visual_count" => "visual_example",
        "matching"     => "matching_exercise",
        _              => exerciseType
    };

    /// <summary>
    /// Obtiene todas las secciones distintas de una lección.
    /// Incluye secciones de LESSON_CONTENT y de EXERCISE (unión de ambas).
    /// </summary>
    public async Task<List<string>> GetLessonSectionsAsync(int lessonId)
    {
        // Secciones con su primer id_content (para ordenar por aparición original)
        var fromContent = await _context.LessonContents
            .AsNoTracking()
            .Where(lc => lc.IdLesson == lessonId && lc.IsActive && lc.SectionType != null)
            .GroupBy(lc => lc.SectionType!)
            .Select(g => new { SectionType = g.Key, MinId = g.Min(x => x.IdContent) })
            .ToListAsync();

        // Secciones que únicamente existen en EXERCISE (sin contenido estático)
        var contentSections = fromContent.Select(x => x.SectionType).ToHashSet();
        var fromExercises = await _context.Exercises
            .AsNoTracking()
            .Where(e => e.IdLesson == lessonId && e.IsActive && e.SectionType != null
                     && !contentSections.Contains(e.SectionType!))
            .GroupBy(e => e.SectionType!)
            .Select(g => new { SectionType = g.Key, MinId = -g.Min(x => x.IdExercise) })
            .ToListAsync();

        static (int rank, int subRank) GetSectionOrder(string sectionType)
        {
            if (sectionType.Equals("introduccion", StringComparison.OrdinalIgnoreCase)) return (1, 0);

            if (sectionType.StartsWith("analicemos", StringComparison.OrdinalIgnoreCase))
            {
                var parts = sectionType.Split('_');
                if (parts.Length > 1 && int.TryParse(parts[1], out var num))
                {
                    return (2, num);
                }

                return (2, 0);
            }

            if (sectionType.Equals("practiquemos", StringComparison.OrdinalIgnoreCase)) return (3, 0);
            if (sectionType.Equals("resolvamos", StringComparison.OrdinalIgnoreCase)) return (4, 0);
            if (sectionType.Equals("desafio", StringComparison.OrdinalIgnoreCase)) return (5, 0);
            return (int.MaxValue, int.MaxValue);
        }

        return fromContent
            .Concat(fromExercises)
            .OrderBy(x => GetSectionOrder(x.SectionType).rank)
            .ThenBy(x => GetSectionOrder(x.SectionType).subRank)
            .ThenBy(x => x.MinId)
            .Select(x => x.SectionType)
            .ToList();
    }

    /// <summary>
    /// Obtiene un contenido específico por su ID
    /// </summary>
    /// <param name="contentId">ID del contenido</param>
    /// <returns>Contenido con datos deserializados</returns>
    public async Task<LessonContent?> GetContentByIdAsync(int contentId)
    {
        var content = await _context.LessonContents
            .AsNoTracking()
            .FirstOrDefaultAsync(lc => lc.IdContent == contentId && lc.IsActive);

        if (content != null)
        {
            content.DeserializeData();
        }

        return content;
    }

    /// <summary>
    /// Cuenta el número total de contenidos de una lección
    /// </summary>
    /// <param name="lessonId">ID de la lección</param>
    /// <returns>Cantidad de contenidos activos</returns>
    public async Task<int> GetContentCountAsync(int lessonId)
    {
        return await _context.LessonContents
            .Where(lc => lc.IdLesson == lessonId && lc.IsActive)
            .CountAsync();
    }

    /// <summary>
    /// Agrupa el contenido por sección con estadísticas
    /// </summary>
    /// <param name="lessonId">ID de la lección</param>
    /// <returns>Diccionario con sección y lista de contenidos</returns>
    public async Task<Dictionary<string, List<LessonContent>>> GetContentGroupedBySectionAsync(int lessonId)
    {
        var allContents = await GetLessonContentAsync(lessonId);

        return allContents
            .GroupBy(lc => lc.SectionType ?? "sin_seccion")
            .ToDictionary(
                g => g.Key,
                g => g.ToList()
            );
    }

    /// <summary>
    /// Obtiene el contenido de una lección con información de la lección incluida
    /// </summary>
    /// <param name="lessonId">ID de la lección</param>
    /// <returns>Tupla con la lección y sus contenidos</returns>
    public async Task<(Lesson? lesson, List<LessonContent> contents)> GetLessonWithContentAsync(int lessonId)
    {
        var lesson = await _context.Lessons
            .AsNoTracking()
            .Include(l => l.Topic)
            .FirstOrDefaultAsync(l => l.IdLesson == lessonId);

        var contents = await GetLessonContentAsync(lessonId);

        return (lesson, contents);
    }
}
