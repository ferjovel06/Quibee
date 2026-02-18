using System;
using System.Windows.Input;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Quibee.Database;
using Quibee.Models;
using Quibee.Services;

namespace Quibee.ViewModels
{
    public class RegistrationConfirmationViewModel : ViewModelBase
    {
        private readonly MainWindowViewModel? _mainWindowViewModel;
        private readonly UserRegistrationData _userData;

        public RegistrationConfirmationViewModel(
            MainWindowViewModel mainWindowViewModel,
            UserRegistrationData userData)
        {
            _mainWindowViewModel = mainWindowViewModel;
            _userData = userData;
            
            ContinuarCommand = new RelayCommand(OnContinuar);
        }

        public string NombreCompleto => _userData.NombreCompleto;

        public string FechaNacimientoFormateada => 
            _userData.FechaNacimiento?.ToString("dd/MM/yyyy") ?? "";

        public string Genero => _userData.Genero;

        public string Nivel => _userData.Nivel;

        public string NivelNumero
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_userData.Nivel))
                    return "-";
                if (_userData.Nivel.Contains("Primer"))
                    return "1";
                else if (_userData.Nivel.Contains("Segundo"))
                    return "2";
                else if (_userData.Nivel.Contains("Tercer"))
                    return "3";
                return "1"; // Default
            }
        }

        public ICommand ContinuarCommand { get; }

        private async void OnContinuar()
        {
            try
            {
                // Crear DbContext y servicio
                var configuration = new ConfigurationBuilder()
                    .SetBasePath(AppContext.BaseDirectory)
                    .AddJsonFile("appsettings.json", optional: false)
                    .AddJsonFile("appsettings.Development.json", optional: true)
                    .Build();

                var connectionString = configuration.GetConnectionString("QuibeeDb");
                
                var optionsBuilder = new DbContextOptionsBuilder<QuibeeDbContext>();
                optionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
                
                using var context = new QuibeeDbContext(optionsBuilder.Options);
                var studentService = new StudentService(context);

                Console.WriteLine("💾 Guardando estudiante en la base de datos...");

                // Registrar estudiante
                var student = await studentService.RegisterStudentAsync(_userData);

                if (student != null)
                {
                    Console.WriteLine($"✅ ¡Registro completado exitosamente!");
                    Console.WriteLine($"   👤 Usuario: {student.Username}");
                    Console.WriteLine($"   🎓 Nivel: {student.LevelNumber}°");
                    Console.WriteLine($"   🔑 ID: {student.IdStudent}");
                    
                    // Navegar a la selección de nivel después del registro.
                    _mainWindowViewModel?.NavigateToGradeSelection(student.IdStudent);
                }
                else
                {
                    Console.WriteLine("❌ Error: No se pudo registrar el estudiante");
                    // TODO: Mostrar mensaje de error al usuario
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error al guardar: {ex.Message}");
            }
        }
    }
}
