using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;

namespace Quibee.Models;

/// <summary>
/// Representa un elemento de contenido dinámico dentro de una lección
/// Puede ser texto, imagen, ejemplo visual, etc.
/// </summary>
public class LessonContent
{
    [Key]
    [Column("id_content")]
    public int IdContent { get; set; }

    [Required]
    [Column("id_lesson")]
    public int IdLesson { get; set; }

    [Required]
    [Column("content_type")]
    [MaxLength(50)]
    public string ContentType { get; set; } = string.Empty; // 'text', 'image', 'visual_example', 'heading', etc.

    [Required]
    [Column("content_data")]
    public string ContentDataJson { get; set; } = string.Empty; // JSON string from database

    [Column("order_index")]
    public int OrderIndex { get; set; }

    [Column("section_type")]
    [MaxLength(50)]
    public string? SectionType { get; set; } // 'introduccion', 'analicemos', 'ejercitemos', 'resolvamos', 'desafio'

    [Column("is_active")]
    public bool IsActive { get; set; } = true;

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    [Column("updated_at")]
    public DateTime? UpdatedAt { get; set; }

    // Relaciones
    [ForeignKey("IdLesson")]
    public virtual Lesson? Lesson { get; set; }

    // Propiedad calculada (no mapeada a BD)
    /// <summary>
    /// Datos deserializados del JSON
    /// </summary>
    [NotMapped]
    public LessonContentData? Data { get; set; }

    /// <summary>
    /// Deserializa el JSON en el objeto Data
    /// </summary>
    public void DeserializeData()
    {
        if (!string.IsNullOrEmpty(ContentDataJson))
        {
            try
            {
                Data = JsonSerializer.Deserialize<LessonContentData>(
                    ContentDataJson, 
                    new JsonSerializerOptions 
                    { 
                        PropertyNameCaseInsensitive = true 
                    }
                );
            }
            catch (JsonException ex)
            {
                Console.WriteLine($"Error deserializing content data for id_content {IdContent}: {ex.Message}");
                Data = null;
            }
        }
    }
}
