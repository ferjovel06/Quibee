using Avalonia;
using System;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Quibee.Database;
using Quibee.Services;
using Velopack;

namespace Quibee;

class Program
{
    // Initialization code. Don't use any Avalonia, third-party APIs or any
    // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
    // yet and stuff might break.
    [STAThread]
    public static void Main(string[] args)
    {
        // Si se pasa el argumento "--test-service", ejecutar pruebas del servicio
        if (args.Length > 0 && args[0] == "--test-service")
        {
            TestLessonContentService().Wait();
            return;
        }

        VelopackApp.Build().Run();

        // Iniciar la aplicación normalmente
        BuildAvaloniaApp().StartWithClassicDesktopLifetime(args);
    }

    private static async System.Threading.Tasks.Task TestLessonContentService()
    {
        Console.WriteLine("╔════════════════════════════════════════════════════════╗");
        Console.WriteLine("║   PRUEBA REAL DE LESSONCONTENTSERVICE                 ║");
        Console.WriteLine("╚════════════════════════════════════════════════════════╝\n");

        try 
        {
            // Obtener el servicio
            Console.WriteLine("🔧 Obteniendo servicio...");
            var service = ServiceLocator.GetLessonContentService();
            Console.WriteLine("✅ Servicio obtenido correctamente\n");

            // Test 1: Contar contenidos
            Console.WriteLine("📊 Test 1: Contar contenidos de lección 2");
            var count = await service.GetContentCountAsync(2);
            Console.WriteLine($"✅ Total: {count} contenidos\n");

            // Test 2: Obtener secciones
            Console.WriteLine("📂 Test 2: Obtener secciones disponibles");
            var sections = await service.GetLessonSectionsAsync(2);
            Console.WriteLine($"✅ Secciones encontradas: {sections.Count}");
            foreach (var section in sections)
            {
                Console.WriteLine($"   - {section}");
            }
            Console.WriteLine();

            // Test 3: Obtener contenido de introducción
            Console.WriteLine("📝 Test 3: Contenido de 'introduccion'");
            var introContent = await service.GetLessonContentBySectionAsync(2, "introduccion");
            Console.WriteLine($"✅ Contenidos: {introContent.Count}");
            
            foreach (var content in introContent)
            {
                Console.WriteLine($"\n   [{content.OrderIndex}] {content.ContentType}");
                if (content.Data?.Text != null)
                {
                    var preview = content.Data.Text.Length > 60 
                        ? content.Data.Text.Substring(0, 60) + "..." 
                        : content.Data.Text;
                    Console.WriteLine($"       {preview}");
                }
            }
            Console.WriteLine();

            // Test 4: Verificar deserialización de visual_example
            Console.WriteLine("🎨 Test 4: Deserialización de visual_example");
            var allContent = await service.GetLessonContentAsync(2);
            var visualExample = allContent.Find(c => c.ContentType == "visual_example");
            
            if (visualExample?.Data != null)
            {
                Console.WriteLine($"✅ Visual example ID: {visualExample.IdContent}");
                Console.WriteLine($"   - Layout: {visualExample.Data.Layout}");
                Console.WriteLine($"   - Spacing: {visualExample.Data.Spacing}");
                Console.WriteLine($"   - Objects: {visualExample.Data.Objects?.Count ?? 0}");
                
                if (visualExample.Data.Objects != null && visualExample.Data.Objects.Count > 0)
                {
                    var firstObj = visualExample.Data.Objects[0];
                    if (firstObj.Emoji != null)
                        Console.WriteLine($"   - Primer objeto: Emoji '{firstObj.Emoji}' x{firstObj.Count}");
                }
            }

            Console.WriteLine("\n╔════════════════════════════════════════════════════════╗");
            Console.WriteLine("║   ✅ TODAS LAS PRUEBAS PASARON EXITOSAMENTE          ║");
            Console.WriteLine("╚════════════════════════════════════════════════════════╝");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"\n❌ ERROR: {ex.Message}");
            Console.WriteLine($"\nStack trace:\n{ex.StackTrace}");
            if (ex.InnerException != null)
            {
                Console.WriteLine($"\nInner exception: {ex.InnerException.Message}");
            }
        }
    }

    private static void TestDatabaseConnection()
    {
        Console.WriteLine("\n🔍 Probando conexión a la base de datos...\n");
        
        try
        {
            // Cargar configuración
            var configuration = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: false)
                .AddJsonFile("appsettings.Development.json", optional: true)
                .Build();

            // Crear DbContext
            var optionsBuilder = new DbContextOptionsBuilder<QuibeeDbContext>();
            optionsBuilder.UseSqlite(SqliteConnectionHelper.GetConnectionString(configuration));
            
            using var context = new QuibeeDbContext(optionsBuilder.Options);
            var testService = new DatabaseTestService(context);
            
            // Probar conexión
            testService.TestConnection();
            
            Console.WriteLine("\n✅ Prueba completada. Iniciando aplicación...\n");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"\n❌ Error en la prueba: {ex.Message}\n");
            Console.WriteLine("⚠️  La aplicación continuará, pero la base de datos podría no estar disponible.\n");
        }
    }

    // Avalonia configuration, don't remove; also used by visual designer.
    public static AppBuilder BuildAvaloniaApp()
        => AppBuilder.Configure<App>()
            .UsePlatformDetect()
            .WithInterFont()
            .LogToTrace();
}
