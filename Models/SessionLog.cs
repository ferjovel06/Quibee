using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Quibee.Models;

public class SessionLog
{
    [Key]
    [Column("id_session_log")]
    public int IdSessionLog { get; set; }

    [Required]
    [Column("id_student")]
    public int IdStudent { get; set; }

    [Column("session_start")]
    public DateTime SessionStart { get; set; } = DateTime.Now;

    [Column("session_end")]
    public DateTime? SessionEnd { get; set; }

    [Column("duration_minutes")]
    public int? DurationMinutes { get; set; }
}
