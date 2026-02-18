using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
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
        // Verificar si ya hay datos
        if (await _context.Levels.AnyAsync())
        {
            Console.WriteLine("✓ La base de datos ya contiene niveles. No se inicializará.");
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
}
