using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Quibee.Models;

public class StudentLevelProgress
{
    [Key]
    [Column("id_student_level_progress")]
    public int IdStudentLevelProgress { get; set; }

    [Required]
    [Column("id_student")]
    public int IdStudent { get; set; }

    [Required]
    [Column("id_level")]
    public int IdLevel { get; set; }

    [Column("is_unlocked")]
    public bool IsUnlocked { get; set; } = false;

    [Column("is_completed")]
    public bool IsCompleted { get; set; } = false;

    [Column("completion_percentage")]
    public decimal? CompletionPercentage { get; set; }

    [Column("stars_earned")]
    public int? StarsEarned { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    [Column("updated_at")]
    public DateTime? UpdatedAt { get; set; }

    // Relaciones
    [ForeignKey("IdStudent")]
    public Student? Student { get; set; }
}
