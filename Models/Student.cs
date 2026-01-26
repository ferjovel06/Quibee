using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Quibee.Models;

public class Student
{
    [Key]
    [Column("id_student")]
    public int IdStudent { get; set; }

    [Required]
    [Column("username")]
    [MaxLength(50)]
    public string Username { get; set; } = string.Empty;

    [Required]
    [Column("first_name")]
    [MaxLength(100)]
    public string FirstName { get; set; } = string.Empty;

    [Required]
    [Column("last_name")]
    [MaxLength(100)]
    public string LastName { get; set; } = string.Empty;

    [Required]
    [Column("date_of_birth")]
    public DateTime DateOfBirth { get; set; }

    [Required]
    [Column("grade_level")]
    public int GradeLevel { get; set; } // 1, 2 o 3

    [Required]
    [Column("gender")]
    [MaxLength(20)]
    public string Gender { get; set; } = string.Empty; // male, female, other, prefer_not_to_say

    [Column("email")]
    [MaxLength(100)]
    public string? Email { get; set; }

    [Required]
    [Column("access_code")]
    [MaxLength(4)]
    public string AccessCode { get; set; } = string.Empty; // PIN de 4 dígitos

    [Column("is_active")]
    public bool IsActive { get; set; } = true;

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    [Column("updated_at")]
    public DateTime? UpdatedAt { get; set; }

    // Relaciones
    public StudentStats? Stats { get; set; }
    public ICollection<StudentLevelProgress> LevelProgress { get; set; } = new List<StudentLevelProgress>();
    public ICollection<StudentLessonProgress> LessonProgress { get; set; } = new List<StudentLessonProgress>();
    public ICollection<ExerciseAttempt> ExerciseAttempts { get; set; } = new List<ExerciseAttempt>();
    public ICollection<StudentAchievement> Achievements { get; set; } = new List<StudentAchievement>();
}
