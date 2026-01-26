using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Quibee.Models;

public class Topic
{
    [Key]
    [Column("id_topic")]
    public int IdTopic { get; set; }

    [Required]
    [Column("id_level")]
    public int IdLevel { get; set; }

    [Required]
    [Column("topic_name")]
    [MaxLength(100)]
    public string TopicName { get; set; } = string.Empty;

    [Column("description")]
    [MaxLength(500)]
    public string? Description { get; set; }

    [Column("icon")]
    [MaxLength(255)]
    public string? Icon { get; set; }

    [Column("order_index")]
    public int OrderIndex { get; set; }

    // Campos de posicionamiento en el mapa de lecciones
    [Column("position_x")]
    public double PositionX { get; set; }

    [Column("position_y")]
    public double PositionY { get; set; }

    [Column("icon_width")]
    public double IconWidth { get; set; } = 100;

    [Column("icon_height")]
    public double IconHeight { get; set; } = 100;

    [Column("text_on_left")]
    public bool TextOnLeft { get; set; } = false;

    [Column("text_on_right")]
    public bool TextOnRight { get; set; } = false;

    [Column("rotation_angle")]
    public double RotationAngle { get; set; } = 0;

    [Column("is_active")]
    public bool IsActive { get; set; } = true;

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    [Column("updated_at")]
    public DateTime? UpdatedAt { get; set; }

    // Relaciones
    [ForeignKey("IdLevel")]
    public Level? Level { get; set; }
    
    public ICollection<Lesson> Lessons { get; set; } = new List<Lesson>();
}
