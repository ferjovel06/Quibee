using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Quibee.Models;

public class Lesson
{
    [Key]
    [Column("id_lesson")]
    public int IdLesson { get; set; }

    [Required]
    [Column("id_topic")]
    public int IdTopic { get; set; }

    [Required]
    [Column("title")]
    [MaxLength(200)]
    public string Title { get; set; } = string.Empty;

    [Column("content")]
    [MaxLength(2000)]
    public string? Content { get; set; }

    [Column("multimedia_url")]
    [MaxLength(255)]
    public string? MultimediaUrl { get; set; }

    [Column("multimedia_type")]
    [MaxLength(20)]
    public string? MultimediaType { get; set; } // video, audio, image

    [Column("order_index")]
    public int OrderIndex { get; set; }

    [Column("estimated_duration_minutes")]
    public int? EstimatedDurationMinutes { get; set; }

    [Column("is_active")]
    public bool IsActive { get; set; } = true;

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    [Column("updated_at")]
    public DateTime? UpdatedAt { get; set; }

    // Relaciones
    [ForeignKey("IdTopic")]
    public Topic? Topic { get; set; }
    
    public ICollection<Exercise> Exercises { get; set; } = new List<Exercise>();
}
