using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Quibee.Models;

public class ExerciseAttempt
{
    [Key]
    [Column("id_exercise_attempt")]
    public int IdExerciseAttempt { get; set; }

    [Required]
    [Column("id_student")]
    public int IdStudent { get; set; }

    [Required]
    [Column("id_exercise")]
    public int IdExercise { get; set; }

    [Column("attempt_number")]
    public int AttemptNumber { get; set; } = 1;

    [Column("student_answer")]
    [MaxLength(1000)]
    public string? StudentAnswer { get; set; } // JSON

    [Column("is_correct")]
    public bool IsCorrect { get; set; } = false;

    [Column("points_earned")]
    public int? PointsEarned { get; set; }

    [Column("time_taken_seconds")]
    public int? TimeTakenSeconds { get; set; }

    [Column("attempt_date")]
    public DateTime AttemptDate { get; set; } = DateTime.Now;

    // Relaciones
    [ForeignKey("IdStudent")]
    public Student? Student { get; set; }
}
