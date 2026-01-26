using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Quibee.Models;

public class Achievement
{
    [Key]
    [Column("id_achievement")]
    public int IdAchievement { get; set; }

    [Required]
    [Column("achievement_name")]
    [MaxLength(100)]
    public string AchievementName { get; set; } = string.Empty;

    [Column("description")]
    [MaxLength(500)]
    public string? Description { get; set; }

    [Column("icon")]
    [MaxLength(100)]
    public string? Icon { get; set; }

    [Column("criteria")]
    [MaxLength(1000)]
    public string? Criteria { get; set; } // JSON con criterios

    [Column("points_reward")]
    public int? PointsReward { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}
