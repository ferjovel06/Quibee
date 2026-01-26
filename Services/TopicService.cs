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
            .Include(t => t.Level)
            .Where(t => t.Level != null && t.Level.LevelNumber == levelNumber && t.IsActive)
            .OrderBy(t => t.OrderIndex)
            .ToListAsync();
    }

    /// <summary>
    /// Obtiene un tema específico por ID
    /// </summary>
    public async Task<Topic?> GetTopicByIdAsync(int topicId)
    {
        return await _context.Topics
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
