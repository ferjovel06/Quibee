using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Quibee.Views;
using Quibee.ViewModels;
using System;
using System.Threading.Tasks;

namespace Quibee;

public partial class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            // Inicializar base de datos de forma asíncrona
            _ = InitializeDatabaseAsync();

            desktop.MainWindow = new MainWindow
            {
                DataContext = new MainWindowViewModel()
            };
        }

        base.OnFrameworkInitializationCompleted();
    }

    /// <summary>
    /// Inicializa la base de datos y carga datos iniciales si es necesario
    /// </summary>
    private async Task InitializeDatabaseAsync()
    {
        try
        {
            Console.WriteLine("🔄 Inicializando base de datos...");
            
            // Obtener el seeder service
            var seederService = ServiceLocator.GetDataSeederService();
            
            // Inicializar niveles y temas
            await seederService.SeedLevelsAndTopicsAsync();
            
            Console.WriteLine("✓ Base de datos inicializada correctamente");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Error inicializando base de datos: {ex.Message}");
            Console.WriteLine($"   Stack: {ex.StackTrace}");
        }
    }
}