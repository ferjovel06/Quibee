using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Quibee.Models;

public class StudentLessonProgress
{
    [Key]
    [Column("id_student_lesson_progress")]
    public int IdStudentLessonProgress { get; set; }

    [Required]
    [Column("id_student")]
    public int IdStudent { get; set; }

    [Required]
    [Column("id_lesson")]
    public int IdLesson { get; set; }

    [Column("is_completed")]
    public bool IsCompleted { get; set; } = false;

    [Column("time_spent_minutes")]
    public int? TimeSpentMinutes { get; set; }

    [Column("last_accessed")]
    public DateTime? LastAccessed { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    [Column("updated_at")]
    public DateTime? UpdatedAt { get; set; }

    // Relaciones
    [ForeignKey("IdStudent")]
    public Student? Student { get; set; }
}
