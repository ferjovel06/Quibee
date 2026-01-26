using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Quibee.Models;

public class StudentAchievement
{
    [Key]
    [Column("id_student_achievement")]
    public int IdStudentAchievement { get; set; }

    [Required]
    [Column("id_student")]
    public int IdStudent { get; set; }

    [Required]
    [Column("id_achievement")]
    public int IdAchievement { get; set; }

    [Column("earned_date")]
    public DateTime EarnedDate { get; set; } = DateTime.Now;

    // Relaciones
    [ForeignKey("IdStudent")]
    public Student? Student { get; set; }
}
