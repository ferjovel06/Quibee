using Avalonia;
using System;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using Quibee.Database;
using Quibee.Services;

namespace Quibee;

class Program
{
    // Initialization code. Don't use any Avalonia, third-party APIs or any
    // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
    // yet and stuff might break.
    [STAThread]
    public static void Main(string[] args)
    {
        // Iniciar la aplicación normalmente
        BuildAvaloniaApp().StartWithClassicDesktopLifetime(args);
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

            var connectionString = configuration.GetConnectionString("QuibeeDb");
            
            if (string.IsNullOrEmpty(connectionString))
            {
                Console.WriteLine("❌ No se encontró el connection string");
                return;
            }

            // Crear DbContext
            var optionsBuilder = new DbContextOptionsBuilder<QuibeeDbContext>();
            optionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
            
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
