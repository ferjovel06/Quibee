using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Quibee.Models;

public class Exercise
{
    [Key]
    [Column("id_exercise")]
    public int IdExercise { get; set; }

    [Required]
    [Column("id_lesson")]
    public int IdLesson { get; set; }

    [Required]
    [Column("exercise_type")]
    [MaxLength(30)]
    public string ExerciseType { get; set; } = string.Empty; // drag_drop, ordering, matching, multiple_choice, fill_blank

    [Required]
    [Column("question_text")]
    [MaxLength(500)]
    public string QuestionText { get; set; } = string.Empty;

    [Column("config")]
    [MaxLength(2000)]
    public string? Config { get; set; } // JSON con opciones, imágenes, etc.

    [Column("solution")]
    [MaxLength(1000)]
    public string? Solution { get; set; } // JSON con respuesta correcta

    [Column("difficulty")]
    [MaxLength(10)]
    public string? Difficulty { get; set; } // easy, medium, hard

    [Column("points")]
    public int Points { get; set; } = 10;

    [Column("order_index")]
    public int OrderIndex { get; set; }

    [Column("is_active")]
    public bool IsActive { get; set; } = true;

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    [Column("updated_at")]
    public DateTime? UpdatedAt { get; set; }

    // Relaciones
    [ForeignKey("IdLesson")]
    public Lesson? Lesson { get; set; }
}
