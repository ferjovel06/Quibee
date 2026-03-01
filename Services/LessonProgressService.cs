using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Quibee.Database;
using Quibee.Models;

namespace Quibee.Services;

/// <summary>
/// Servicio para gestionar el progreso de un estudiante en lecciones.
/// El progreso se calcula por secciones visitadas / total de secciones.
/// </summary>
public class LessonProgressService
{
    private readonly QuibeeDbContext _context;

    public LessonProgressService(QuibeeDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Registra que un estudiante visitó una sección de una lección.
    /// Actualiza el porcentaje de progreso automáticamente.
    /// </summary>
    public async Task<int> RecordSectionVisitAsync(int studentId, int lessonId, string sectionKey, int totalSections)
    {
        try
        {
            var progress = await _context.StudentLessonProgress
                .FirstOrDefaultAsync(p => p.IdStudent == studentId && p.IdLesson == lessonId);

            if (progress == null)
            {
                progress = new StudentLessonProgress
                {
                    IdStudent = studentId,
                    IdLesson = lessonId,
                    IsCompleted = false,
                    SectionsVisited = sectionKey,
                    TotalSections = totalSections,
                    LastAccessed = DateTime.Now,
                    CreatedAt = DateTime.Now
                };
                _context.StudentLessonProgress.Add(progress);
            }
            else
            {
                // Agregar sección si no existe ya
                var visited = ParseSections(progress.SectionsVisited);
                if (!visited.Contains(sectionKey))
                {
                    visited.Add(sectionKey);
                    progress.SectionsVisited = string.Join(",", visited);
                }

                progress.TotalSections = totalSections;
                progress.LastAccessed = DateTime.Now;
                progress.UpdatedAt = DateTime.Now;
            }

            // Calcular porcentaje
            var visitedSections = ParseSections(progress.SectionsVisited);
            int percentage = totalSections > 0
                ? (int)Math.Round(100.0 * visitedSections.Count / totalSections)
                : 0;

            progress.ProgressPercentage = Math.Clamp(percentage, 0, 100);
            progress.IsCompleted = progress.ProgressPercentage >= 100;

            await _context.SaveChangesAsync();

            Console.WriteLine($"✅ Progreso: Student={studentId}, Lesson={lessonId}, " +
                              $"Section={sectionKey}, {visitedSections.Count}/{totalSections} = {progress.ProgressPercentage}%");

            return progress.ProgressPercentage;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Error al guardar progreso: {ex.Message}");
            return 0;
        }
    }

    /// <summary>
    /// Obtiene el porcentaje de progreso de una lección para un estudiante.
    /// </summary>
    public async Task<int> GetLessonProgressAsync(int studentId, int lessonId)
    {
        try
        {
            var progress = await _context.StudentLessonProgress
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.IdStudent == studentId && p.IdLesson == lessonId);

            return progress?.ProgressPercentage ?? 0;
        }
        catch
        {
            return 0;
        }
    }

    /// <summary>
    /// Obtiene el progreso de múltiples lecciones (para el mapa).
    /// </summary>
    public async Task<Dictionary<int, int>> GetProgressByLessonIdsAsync(int studentId, IReadOnlyCollection<int> lessonIds)
    {
        try
        {
            if (lessonIds.Count == 0)
                return new Dictionary<int, int>();

            var progressRows = await _context.StudentLessonProgress
                .AsNoTracking()
                .Where(p => p.IdStudent == studentId && lessonIds.Contains(p.IdLesson))
                .Select(p => new { p.IdLesson, p.ProgressPercentage })
                .ToListAsync();

            var result = lessonIds.ToDictionary(id => id, _ => 0);
            foreach (var row in progressRows)
            {
                result[row.IdLesson] = row.ProgressPercentage;
            }

            return result;
        }
        catch
        {
            return lessonIds.ToDictionary(id => id, _ => 0);
        }
    }

    private static List<string> ParseSections(string? sectionsStr)
    {
        if (string.IsNullOrWhiteSpace(sectionsStr))
            return new List<string>();

        return sectionsStr.Split(',', StringSplitOptions.RemoveEmptyEntries)
            .Select(s => s.Trim())
            .Distinct()
            .ToList();
    }
}
