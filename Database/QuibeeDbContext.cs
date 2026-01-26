using System;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Quibee.Models;

namespace Quibee.Database;

public class QuibeeDbContext : DbContext
{
    // DbSets (Tablas)
    public DbSet<Student> Students { get; set; }
    public DbSet<Level> Levels { get; set; }
    public DbSet<Topic> Topics { get; set; }
    public DbSet<Lesson> Lessons { get; set; }
    public DbSet<Exercise> Exercises { get; set; }
    public DbSet<StudentLevelProgress> StudentLevelProgress { get; set; }
    public DbSet<StudentLessonProgress> StudentLessonProgress { get; set; }
    public DbSet<ExerciseAttempt> ExerciseAttempts { get; set; }
    public DbSet<StudentStats> StudentStats { get; set; }
    public DbSet<Achievement> Achievements { get; set; }
    public DbSet<StudentAchievement> StudentAchievements { get; set; }
    public DbSet<SessionLog> SessionLogs { get; set; }

    // Constructor para inyección de dependencias
    public QuibeeDbContext(DbContextOptions<QuibeeDbContext> options) 
        : base(options)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            // Leer configuración desde appsettings.json
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile("appsettings.Development.json", optional: true, reloadOnChange: true)
                .Build();

            var connectionString = configuration.GetConnectionString("QuibeeDatabase");
            
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new InvalidOperationException(
                    "No se encontró la cadena de conexión 'QuibeeDatabase' en appsettings.json"
                );
            }

            optionsBuilder.UseMySql(
                connectionString,
                new MySqlServerVersion(new Version(8, 0, 0))
            );
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configurar nombres de tablas en mayúsculas (según schema)
        modelBuilder.Entity<Student>().ToTable("STUDENT");
        modelBuilder.Entity<Level>().ToTable("LEVEL");
        modelBuilder.Entity<Topic>().ToTable("TOPIC");
        modelBuilder.Entity<Lesson>().ToTable("LESSON");
        modelBuilder.Entity<Exercise>().ToTable("EXERCISE");
        modelBuilder.Entity<StudentLevelProgress>().ToTable("STUDENT_LEVEL_PROGRESS");
        modelBuilder.Entity<StudentLessonProgress>().ToTable("STUDENT_LESSON_PROGRESS");
        modelBuilder.Entity<ExerciseAttempt>().ToTable("EXERCISE_ATTEMPT");
        modelBuilder.Entity<StudentStats>().ToTable("STUDENT_STATS");
        modelBuilder.Entity<Achievement>().ToTable("ACHIEVEMENT");
        modelBuilder.Entity<StudentAchievement>().ToTable("STUDENT_ACHIEVEMENT");
        modelBuilder.Entity<SessionLog>().ToTable("SESSION_LOG");

        // Relaciones
        ConfigureRelationships(modelBuilder);
    }

    private void ConfigureRelationships(ModelBuilder modelBuilder)
    {
        // Level -> Topics (1:N)
        modelBuilder.Entity<Topic>()
            .HasOne(t => t.Level)
            .WithMany(l => l.Topics)
            .HasForeignKey(t => t.IdLevel);

        // Topic -> Lessons (1:N)
        modelBuilder.Entity<Lesson>()
            .HasOne(l => l.Topic)
            .WithMany(t => t.Lessons)
            .HasForeignKey(l => l.IdTopic);

        // Lesson -> Exercises (1:N)
        modelBuilder.Entity<Exercise>()
            .HasOne(e => e.Lesson)
            .WithMany(l => l.Exercises)
            .HasForeignKey(e => e.IdLesson);

        // Student -> StudentLevelProgress (1:N)
        modelBuilder.Entity<StudentLevelProgress>()
            .HasOne(slp => slp.Student)
            .WithMany(s => s.LevelProgress)
            .HasForeignKey(slp => slp.IdStudent);

        // Student -> StudentLessonProgress (1:N)
        modelBuilder.Entity<StudentLessonProgress>()
            .HasOne(slp => slp.Student)
            .WithMany(s => s.LessonProgress)
            .HasForeignKey(slp => slp.IdStudent);

        // Student -> ExerciseAttempts (1:N)
        modelBuilder.Entity<ExerciseAttempt>()
            .HasOne(ea => ea.Student)
            .WithMany(s => s.ExerciseAttempts)
            .HasForeignKey(ea => ea.IdStudent);

        // Student -> StudentStats (1:1)
        modelBuilder.Entity<StudentStats>()
            .HasOne(ss => ss.Student)
            .WithOne(s => s.Stats)
            .HasForeignKey<StudentStats>(ss => ss.IdStudent);

        // Student -> StudentAchievements (1:N)
        modelBuilder.Entity<StudentAchievement>()
            .HasOne(sa => sa.Student)
            .WithMany(s => s.Achievements)
            .HasForeignKey(sa => sa.IdStudent);
    }
}
