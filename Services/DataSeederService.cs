using System;
using System.IO;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Quibee.Database;
using Quibee.Models;

namespace Quibee.Services;

/// <summary>
/// Servicio para inicializar/poblar la base de datos con datos de ejemplo
/// </summary>
public class DataSeederService
{
    private readonly QuibeeDbContext _context;

    public DataSeederService(QuibeeDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Inicializa los niveles y temas del mapa de lecciones
    /// </summary>
    public async Task SeedLevelsAndTopicsAsync()
    {
        // Offline-first: crear base/tablas locales en el primer arranque.
        await _context.Database.EnsureCreatedAsync();

        var hasCurriculum = await _context.Lessons.AnyAsync()
            && await _context.LessonContents.AnyAsync()
            && await _context.Exercises.AnyAsync();

        if (hasCurriculum)
        {
            Console.WriteLine("✓ La base de datos ya contiene currículo. No se inicializará.");
            return;
        }

        if (await TrySeedCurriculumFromTemplateAsync())
        {
            Console.WriteLine("✓ Currículo cargado desde plantilla SQLite embebida.");
            return;
        }

        // Verificar si ya hay datos
        if (await _context.Levels.AnyAsync())
        {
            Console.WriteLine("⚠️ La base contiene niveles pero no lecciones. No se encontró plantilla para completar currículo.");
            return;
        }

        Console.WriteLine("📚 Inicializando niveles y temas...");

        // Crear los 3 niveles (Grados)
        var levels = new[]
        {
            new Level
            {
                LevelNumber = 1,
                LevelName = "Primer Nivel",
                Description = "Nivel básico de matemáticas para primer nivel",
                Icon = "avares://Quibee/Assets/Images/Grade1Icon.png",
                IsActive = true
            },
            new Level
            {
                LevelNumber = 2,
                LevelName = "Segundo Nivel",
                Description = "Nivel intermedio de matemáticas para segundo nivel",
                Icon = "avares://Quibee/Assets/Images/Grade2Icon.png",
                IsActive = true
            },
            new Level
            {
                LevelNumber = 3,
                LevelName = "Tercer Nivel",
                Description = "Nivel avanzado de matemáticas para tercer nivel",
                Icon = "avares://Quibee/Assets/Images/Grade3Icon.png",
                IsActive = true
            }
        };

        await _context.Levels.AddRangeAsync(levels);
        await _context.SaveChangesAsync();

        // Obtener los IDs de los niveles creados
        var level1 = await _context.Levels.FirstAsync(l => l.LevelNumber == 1);
        var level2 = await _context.Levels.FirstAsync(l => l.LevelNumber == 2);
        var level3 = await _context.Levels.FirstAsync(l => l.LevelNumber == 3);

        // Crear temas para Primer Grado
        var grade1Topics = new[]
        {
            new Topic
            {
                IdLevel = level1.IdLevel,
                TopicName = "Conozcamos y escribamos los números del 1 al 10",
                Description = "Tema 1: Conozcamos y escribamos\nlos números del 1 al 10",
                Icon = "avares://Quibee/Assets/Images/SmallStar.png",
                OrderIndex = 1,
                IsActive = true
            },
            new Topic
            {
                IdLevel = level1.IdLevel,
                TopicName = "Relacionemos números y objetos",
                Description = "Tema 2: Relacionemos\nnúmeros y objetos",
                Icon = "avares://Quibee/Assets/Images/LilacPlanet2.png",
                OrderIndex = 2,
                IsActive = true
            },
            new Topic
            {
                IdLevel = level1.IdLevel,
                TopicName = "Conozcamos los números del 11 al 20",
                Description = "Tema 3: Conozcamos los\nnúmeros del 11 al 20",
                Icon = "avares://Quibee/Assets/Images/PinkPlanet3.png",
                OrderIndex = 3,
                IsActive = true
            },
            new Topic
            {
                IdLevel = level1.IdLevel,
                TopicName = "Sumemos hasta 10",
                Description = "Tema 4: Sumemos\nhasta 10",
                Icon = "avares://Quibee/Assets/Images/OrangePlanet.png",
                OrderIndex = 4,
                IsActive = true
            },
            new Topic
            {
                IdLevel = level1.IdLevel,
                TopicName = "Restemos hasta 10",
                Description = "Tema 5: Restemos\nhasta 10",
                Icon = "avares://Quibee/Assets/Images/BluePlanet.png",
                OrderIndex = 5,
                IsActive = true
            }
        };

        // Crear temas para Segundo Grado
        var grade2Topics = new[]
        {
            new Topic
            {
                IdLevel = level2.IdLevel,
                TopicName = "Números hasta 100",
                Description = "Tema 1: Números hasta 100",
                Icon = "avares://Quibee/Assets/Images/SmallStar.png",
                OrderIndex = 1,
                IsActive = true
            },
            new Topic
            {
                IdLevel = level2.IdLevel,
                TopicName = "Suma y resta con llevada",
                Description = "Tema 2: Suma y resta con llevada",
                Icon = "avares://Quibee/Assets/Images/LilacPlanet2.png",
                OrderIndex = 2,
                IsActive = true
            },
            new Topic
            {
                IdLevel = level2.IdLevel,
                TopicName = "Introducción a la multiplicación",
                Description = "Tema 3: Introducción a la multiplicación",
                Icon = "avares://Quibee/Assets/Images/PinkPlanet3.png",
                OrderIndex = 3,
                IsActive = true
            },
            new Topic
            {
                IdLevel = level2.IdLevel,
                TopicName = "Medidas de longitud",
                Description = "Tema 4: Medidas de longitud",
                Icon = "avares://Quibee/Assets/Images/OrangePlanet.png",
                OrderIndex = 4,
                IsActive = true
            },
            new Topic
            {
                IdLevel = level2.IdLevel,
                TopicName = "Figuras geométricas",
                Description = "Tema 5: Figuras geométricas",
                Icon = "avares://Quibee/Assets/Images/BluePlanet.png",
                OrderIndex = 5,
                IsActive = true
            }
        };

        // Crear temas para Tercer Grado
        var grade3Topics = new[]
        {
            new Topic
            {
                IdLevel = level3.IdLevel,
                TopicName = "Números hasta 1000",
                Description = "Tema 1: Números hasta 1000",
                Icon = "avares://Quibee/Assets/Images/SmallStar.png",
                OrderIndex = 1,
                IsActive = true
            },
            new Topic
            {
                IdLevel = level3.IdLevel,
                TopicName = "Multiplicación y división",
                Description = "Tema 2: Multiplicación y división",
                Icon = "avares://Quibee/Assets/Images/LilacPlanet2.png",
                OrderIndex = 2,
                IsActive = true
            },
            new Topic
            {
                IdLevel = level3.IdLevel,
                TopicName = "Fracciones básicas",
                Description = "Tema 3: Fracciones básicas",
                Icon = "avares://Quibee/Assets/Images/PinkPlanet3.png",
                OrderIndex = 3,
                IsActive = true
            },
            new Topic
            {
                IdLevel = level3.IdLevel,
                TopicName = "Perímetro y área",
                Description = "Tema 4: Perímetro y área",
                Icon = "avares://Quibee/Assets/Images/OrangePlanet.png",
                OrderIndex = 4,
                IsActive = true
            },
            new Topic
            {
                IdLevel = level3.IdLevel,
                TopicName = "Resolución de problemas",
                Description = "Tema 5: Resolución de problemas",
                Icon = "avares://Quibee/Assets/Images/BluePlanet.png",
                OrderIndex = 5,
                IsActive = true
            }
        };

        await _context.Topics.AddRangeAsync(grade1Topics);
        await _context.Topics.AddRangeAsync(grade2Topics);
        await _context.Topics.AddRangeAsync(grade3Topics);
        await _context.SaveChangesAsync();

        Console.WriteLine($"✓ Creados {levels.Length} niveles");
        Console.WriteLine($"✓ Creados {grade1Topics.Length + grade2Topics.Length + grade3Topics.Length} temas");
    }

    private async Task<bool> TrySeedCurriculumFromTemplateAsync()
    {
        var candidatePaths = new[]
        {
            Path.Combine(AppContext.BaseDirectory, "Database", "Seeds", "curriculum.seed.db"),
            Path.Combine(AppContext.BaseDirectory, "curriculum.seed.db"),
            Path.Combine(Directory.GetCurrentDirectory(), "Database", "Seeds", "curriculum.seed.db")
        };

        var seedPath = candidatePaths.FirstOrDefault(File.Exists);
        if (string.IsNullOrWhiteSpace(seedPath))
        {
            return false;
        }

        await using var transaction = await _context.Database.BeginTransactionAsync();

        try
        {
            await _context.Database.ExecuteSqlRawAsync("PRAGMA foreign_keys=OFF;");

            await _context.Database.ExecuteSqlRawAsync("DELETE FROM EXERCISE;");
            await _context.Database.ExecuteSqlRawAsync("DELETE FROM LESSON_CONTENT;");
            await _context.Database.ExecuteSqlRawAsync("DELETE FROM LESSON;");
            await _context.Database.ExecuteSqlRawAsync("DELETE FROM TOPIC;");
            await _context.Database.ExecuteSqlRawAsync("DELETE FROM LEVEL;");
            await _context.Database.ExecuteSqlRawAsync("DELETE FROM ACHIEVEMENT;");

            await ExecuteSqlWithParameterAsync("ATTACH DATABASE @seedPath AS seed;", "@seedPath", seedPath);

            await _context.Database.ExecuteSqlRawAsync(@"
INSERT INTO LEVEL (id_level, level_number, level_name, description, icon, is_active, created_at, updated_at)
SELECT id_level, level_number, level_name, description, icon, is_active, created_at, updated_at
FROM seed.LEVEL;");

            await _context.Database.ExecuteSqlRawAsync(@"
INSERT INTO TOPIC (id_topic, id_level, topic_name, description, icon, order_index, icon_width, icon_height, is_active, created_at, updated_at)
SELECT id_topic, id_level, topic_name, description, icon, order_index, icon_width, icon_height, is_active, created_at, updated_at
FROM seed.TOPIC;");

            await _context.Database.ExecuteSqlRawAsync(@"
INSERT INTO LESSON (id_lesson, id_topic, title, content, multimedia_url, multimedia_type, order_index, estimated_duration_minutes, is_active, created_at, updated_at)
SELECT id_lesson, id_topic, title, content, multimedia_url, multimedia_type, order_index, estimated_duration_minutes, is_active, created_at, updated_at
FROM seed.LESSON;");

            await _context.Database.ExecuteSqlRawAsync(@"
INSERT INTO LESSON_CONTENT (id_content, id_lesson, content_type, content_data, order_index, section_type, is_active, created_at, updated_at)
SELECT id_content, id_lesson, content_type, content_data, order_index, section_type, is_active, created_at, updated_at
FROM seed.LESSON_CONTENT;");

            await _context.Database.ExecuteSqlRawAsync(@"
INSERT INTO EXERCISE (id_exercise, id_lesson, section_type, instructions, exercise_type, question_text, config, solution, difficulty, points, order_index, is_active, created_at, updated_at)
SELECT id_exercise, id_lesson, section_type, instructions, exercise_type, question_text, config, solution, difficulty, points, order_index, is_active, created_at, updated_at
FROM seed.EXERCISE;");

            await _context.Database.ExecuteSqlRawAsync(@"
INSERT INTO ACHIEVEMENT (id_achievement, achievement_name, description, icon, criteria, points_reward, created_at)
SELECT id_achievement, achievement_name, description, icon, criteria, points_reward, created_at
FROM seed.ACHIEVEMENT;");

            await _context.Database.ExecuteSqlRawAsync("DETACH DATABASE seed;");
            await _context.Database.ExecuteSqlRawAsync("PRAGMA foreign_keys=ON;");

            await transaction.CommitAsync();

            return await _context.Lessons.AnyAsync()
                && await _context.LessonContents.AnyAsync()
                && await _context.Exercises.AnyAsync();
        }
        catch
        {
            await transaction.RollbackAsync();
            return false;
        }
    }

    private async Task ExecuteSqlWithParameterAsync(string sql, string parameterName, string value)
    {
        var connection = _context.Database.GetDbConnection();

        if (connection.State != System.Data.ConnectionState.Open)
        {
            await connection.OpenAsync();
        }

        await using var command = connection.CreateCommand();
        command.CommandText = sql;

        var currentTransaction = _context.Database.CurrentTransaction?.GetDbTransaction();
        if (currentTransaction is not null)
        {
            command.Transaction = currentTransaction;
        }

        var parameter = command.CreateParameter();
        parameter.ParameterName = parameterName;
        parameter.Value = value;
        command.Parameters.Add(parameter);

        await command.ExecuteNonQueryAsync();
    }
}
