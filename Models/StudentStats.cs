using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Quibee.Models;

public class StudentStats
{
    [Key]
    [Column("id_student_stats")]
    public int IdStudentStats { get; set; }

    [Required]
    [Column("id_student")]
    public int IdStudent { get; set; }

    [Column("total_points")]
    public int TotalPoints { get; set; } = 0;

    [Column("lessons_completed")]
    public int LessonsCompleted { get; set; } = 0;

    [Column("exercises_completed")]
    public int ExercisesCompleted { get; set; } = 0;

    [Column("current_streak")]
    public int CurrentStreak { get; set; } = 0;

    [Column("longest_streak")]
    public int LongestStreak { get; set; } = 0;

    [Column("accuracy_percentage")]
    public decimal? AccuracyPercentage { get; set; }

    [Column("total_time_spent_minutes")]
    public int? TotalTimeSpentMinutes { get; set; }

    [Column("last_activity_date")]
    public DateTime? LastActivityDate { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    [Column("updated_at")]
    public DateTime? UpdatedAt { get; set; }

    // Relaciones
    [ForeignKey("IdStudent")]
    public Student? Student { get; set; }
}
