using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Quibee.Models;

public class Level
{
    [Key]
    [Column("id_level")]
    public int IdLevel { get; set; }

    [Required]
    [Column("level_number")]
    public int LevelNumber { get; set; }

    [Required]
    [Column("level_name")]
    [MaxLength(100)]
    public string LevelName { get; set; } = string.Empty;

    [Column("description")]
    [MaxLength(500)]
    public string? Description { get; set; }

    [Column("icon")]
    [MaxLength(100)]
    public string? Icon { get; set; }

    [Column("is_active")]
    public bool IsActive { get; set; } = true;

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    [Column("updated_at")]
    public DateTime? UpdatedAt { get; set; }

    // Relaciones
    public ICollection<Topic> Topics { get; set; } = new List<Topic>();
}
