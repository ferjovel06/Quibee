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
    private static readonly string SeedLogPath = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
        "Quibee",
        "seed-diagnostic.log");

    public DataSeederService(QuibeeDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Inicializa los niveles y temas del mapa de lecciones
    /// </summary>
    public async Task SeedLevelsAndTopicsAsync()
    {
        Log("SeedLevelsAndTopicsAsync start");

        // Offline-first: crear base/tablas locales en el primer arranque.
        await _context.Database.EnsureCreatedAsync();
        Log("EnsureCreatedAsync done");

        var hasCurriculum = await _context.Lessons.AnyAsync()
            && await _context.LessonContents.AnyAsync()
            && await _context.Exercises.AnyAsync();
        Log($"hasCurriculum={hasCurriculum}");

        if (hasCurriculum)
        {
            Console.WriteLine("✓ La base de datos ya contiene currículo. No se inicializará.");
            return;
        }

        var hasTemplate = HasCurriculumTemplate();

        if (await TrySeedCurriculumFromTemplateAsync())
        {
            Console.WriteLine("✓ Currículo cargado desde plantilla SQLite embebida.");
            Log("Seed from template succeeded");
            return;
        }

        if (hasTemplate)
        {
            Console.WriteLine("❌ Se encontró plantilla de currículo, pero la importación falló. Se cancela seed básico para evitar datos incompletos.");
            Log("Template exists but import failed; aborting basic seed fallback");
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
        Log("Basic seed finished");
    }

    private async Task<bool> TrySeedCurriculumFromTemplateAsync()
    {
        var candidatePaths = GetSeedCandidatePaths();

        var seedPath = candidatePaths.FirstOrDefault(File.Exists);
        if (string.IsNullOrWhiteSpace(seedPath))
        {
            Console.WriteLine("ℹ️ No se encontró archivo curriculum.seed.db en rutas esperadas.");
            Log("Template seed not found in candidate paths");
            return false;
        }

        Log($"TrySeedCurriculumFromTemplateAsync using seedPath={seedPath}");

        await using var transaction = await _context.Database.BeginTransactionAsync();

        try
        {
            await _context.Database.ExecuteSqlRawAsync("PRAGMA foreign_keys=OFF;");

            await AttachSeedDatabaseAsync(seedPath);
            Log("Seed DB attached");

            // Validar la plantilla antes de tocar la base local.
            var seedLevels = await ExecuteScalarLongAsync("SELECT COUNT(*) FROM seed.LEVEL;");
            var seedTopics = await ExecuteScalarLongAsync("SELECT COUNT(*) FROM seed.TOPIC;");
            var seedLessons = await ExecuteScalarLongAsync("SELECT COUNT(*) FROM seed.LESSON;");
            var seedContents = await ExecuteScalarLongAsync("SELECT COUNT(*) FROM seed.LESSON_CONTENT;");
            var seedExercises = await ExecuteScalarLongAsync("SELECT COUNT(*) FROM seed.EXERCISE;");

            if (seedLevels == 0 || seedTopics == 0 || seedLessons == 0 || seedContents == 0 || seedExercises == 0)
            {
                Console.WriteLine($"❌ Plantilla inválida. Conteos seed -> LEVEL:{seedLevels}, TOPIC:{seedTopics}, LESSON:{seedLessons}, LESSON_CONTENT:{seedContents}, EXERCISE:{seedExercises}");
                Log($"Invalid seed counts LEVEL={seedLevels} TOPIC={seedTopics} LESSON={seedLessons} LESSON_CONTENT={seedContents} EXERCISE={seedExercises}");
                await _context.Database.ExecuteSqlRawAsync("PRAGMA foreign_keys=ON;");
                await transaction.RollbackAsync();
                return false;
            }

            await _context.Database.ExecuteSqlRawAsync("DELETE FROM EXERCISE;");
            await _context.Database.ExecuteSqlRawAsync("DELETE FROM LESSON_CONTENT;");
            await _context.Database.ExecuteSqlRawAsync("DELETE FROM LESSON;");
            await _context.Database.ExecuteSqlRawAsync("DELETE FROM TOPIC;");
            await _context.Database.ExecuteSqlRawAsync("DELETE FROM LEVEL;");
            await _context.Database.ExecuteSqlRawAsync("DELETE FROM ACHIEVEMENT;");

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

            var importedLessons = await ExecuteScalarLongAsync("SELECT COUNT(*) FROM LESSON;");
            var importedContents = await ExecuteScalarLongAsync("SELECT COUNT(*) FROM LESSON_CONTENT;");
            var importedExercises = await ExecuteScalarLongAsync("SELECT COUNT(*) FROM EXERCISE;");

            if (importedLessons == 0 || importedContents == 0 || importedExercises == 0)
            {
                throw new InvalidOperationException("La importación de currículo quedó vacía después del copiado.");
            }

            Log($"Import completed LESSON={importedLessons} LESSON_CONTENT={importedContents} EXERCISE={importedExercises}");

            try
            {
                // En algunos entornos SQLite mantiene lock temporal sobre la BD adjunta.
                // Si falla el detach, no debe revertir una importación ya correcta.
                await _context.Database.ExecuteSqlRawAsync("DETACH DATABASE seed;");
            }
            catch (Exception detachEx)
            {
                Log($"DETACH warning (ignored): {detachEx.Message}");
            }

            await _context.Database.ExecuteSqlRawAsync("PRAGMA foreign_keys=ON;");

            await transaction.CommitAsync();

            return await _context.Lessons.AnyAsync()
                && await _context.LessonContents.AnyAsync()
                && await _context.Exercises.AnyAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Error importando currículo desde plantilla: {ex.Message}");
            Console.WriteLine($"   Ruta seed usada: {seedPath}");
            Console.WriteLine($"   Rutas candidatas: {string.Join(" | ", candidatePaths)}");
            Log($"Import exception: {ex}");

            try
            {
                await _context.Database.ExecuteSqlRawAsync("PRAGMA foreign_keys=ON;");
            }
            catch
            {
                // Ignorar errores secundarios durante cleanup.
            }

            await transaction.RollbackAsync();
            return false;
        }
    }

    private static void Log(string message)
    {
        try
        {
            var dir = Path.GetDirectoryName(SeedLogPath);
            if (!string.IsNullOrWhiteSpace(dir))
            {
                Directory.CreateDirectory(dir);
            }

            File.AppendAllText(SeedLogPath, $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] {message}{Environment.NewLine}");
        }
        catch
        {
            // No bloquear flujo principal por errores de logging.
        }
    }

    private bool HasCurriculumTemplate()
    {
        return GetSeedCandidatePaths().Any(File.Exists);
    }

    private string[] GetSeedCandidatePaths()
    {
        return new[]
        {
            Path.Combine(AppContext.BaseDirectory, "Database", "Seeds", "curriculum.seed.db"),
            Path.Combine(AppContext.BaseDirectory, "curriculum.seed.db"),
            Path.Combine(Directory.GetCurrentDirectory(), "Database", "Seeds", "curriculum.seed.db")
        };
    }

    private async Task AttachSeedDatabaseAsync(string seedPath)
    {
        var connection = _context.Database.GetDbConnection();

        if (connection.State != System.Data.ConnectionState.Open)
        {
            await connection.OpenAsync();
        }

        var escapedSeedPath = seedPath.Replace("'", "''");

        await using var command = connection.CreateCommand();
        command.CommandText = $"ATTACH DATABASE '{escapedSeedPath}' AS seed;";

        var currentTransaction = _context.Database.CurrentTransaction?.GetDbTransaction();
        if (currentTransaction is not null)
        {
            command.Transaction = currentTransaction;
        }

        await command.ExecuteNonQueryAsync();
    }

    private async Task<long> ExecuteScalarLongAsync(string sql)
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

        var result = await command.ExecuteScalarAsync();
        return Convert.ToInt64(result);
    }
}
