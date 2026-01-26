using System;
using System.Windows.Input;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Quibee.Database;
using Quibee.Services;

namespace Quibee.ViewModels
{
    public class LoginViewModel : ViewModelBase
    {
        private string _nombreCompleto = string.Empty;
        private string _claveAcceso = string.Empty;
        private readonly MainWindowViewModel? _mainWindowViewModel;

        public LoginViewModel(MainWindowViewModel? mainWindowViewModel = null)
        {
            _mainWindowViewModel = mainWindowViewModel;
            RetrocederCommand = new RelayCommand(OnRetroceder);
            NuevoUsuarioCommand = new RelayCommand(OnNuevoUsuario);
            IngresarCommand = new RelayCommand(OnIngresar, CanIngresar);
        }

        /// <summary>
        /// Nombre completo del usuario
        /// </summary>
        public string NombreCompleto
        {
            get => _nombreCompleto;
            set
            {
                if (_nombreCompleto != value)
                {
                    _nombreCompleto = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(PuedeIngresar));
                    ((RelayCommand)IngresarCommand).RaiseCanExecuteChanged();
                }
            }
        }

        /// <summary>
        /// Clave de acceso (4 dígitos)
        /// </summary>
        public string ClaveAcceso
        {
            get => _claveAcceso;
            set
            {
                if (_claveAcceso != value)
                {
                    _claveAcceso = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(PuedeIngresar));
                    ((RelayCommand)IngresarCommand).RaiseCanExecuteChanged();
                }
            }
        }

        public ICommand RetrocederCommand { get; }
        public ICommand NuevoUsuarioCommand { get; }
        public ICommand IngresarCommand { get; }

        private void OnRetroceder()
        {
            // Navegar de regreso a la pantalla de bienvenida
            _mainWindowViewModel?.NavigateToWelcome();
        }

        private void OnNuevoUsuario()
        {
            // Navegar a la pantalla de registro de nuevo usuario
            _mainWindowViewModel?.NavigateToRegister();
        }

        private async void OnIngresar()
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

                Console.WriteLine($"🔐 Intentando login: {NombreCompleto}");

                // Validar login
                var student = await studentService.LoginAsync(NombreCompleto, ClaveAcceso);

                if (student != null)
                {
                    Console.WriteLine($"✅ Login exitoso! Bienvenido {student.FirstName}");
                    
                    // ✅ Navegar al mapa de lecciones
                    _mainWindowViewModel?.NavigateToLessonsMap(student.IdStudent, student.GradeLevel);
                }
                else
                {
                    Console.WriteLine("❌ Credenciales incorrectas");
                    // TODO: Mostrar mensaje de error al usuario
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error en login: {ex.Message}");
            }
        }

        /// <summary>
        /// Indica si el botón de ingresar está habilitado
        /// </summary>
        public bool PuedeIngresar => CanIngresar();

        private bool CanIngresar()
        {
            // Validar que los campos estén completos
            return !string.IsNullOrWhiteSpace(NombreCompleto) &&
                   !string.IsNullOrWhiteSpace(ClaveAcceso) &&
                   ClaveAcceso.Length == 4;
        }

        private bool ValidarCredenciales()
        {
            // TODO: Implementar validación real contra base de datos
            // Por ahora solo validamos formato
            return ClaveAcceso.Length == 4 && 
                   int.TryParse(ClaveAcceso, out _);
        }
    }
}
