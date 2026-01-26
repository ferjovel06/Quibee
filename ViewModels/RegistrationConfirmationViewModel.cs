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

        public string Grado => _userData.Grado;

        public string GradoNumero
        {
            get
            {
                if (_userData.Grado.Contains("Primer"))
                    return "1";
                else if (_userData.Grado.Contains("Segundo"))
                    return "2";
                else if (_userData.Grado.Contains("Tercer"))
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
                    Console.WriteLine($"   🎓 Grado: {student.GradeLevel}°");
                    Console.WriteLine($"   🔑 ID: {student.IdStudent}");
                    
                    // ✅ Navegar al mapa de lecciones con el estudiante logueado
                    _mainWindowViewModel?.NavigateToLessonsMap(student.IdStudent, student.GradeLevel);
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
