using Quibee.Models;

namespace Quibee.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private ViewModelBase _currentView;

        public MainWindowViewModel()
        {
            // Iniciar con la vista de bienvenida
            _currentView = new WelcomeViewModel(this);
            
            // TEMPORAL: Iniciar con la vista de confirmación de registro para pruebas
            /*
            var testData = new UserRegistrationData
            {
                Nombres = "Jairo José",
                Apellidos = "Martínez Álvarez",
                FechaNacimiento = new System.DateTime(2006, 4, 28),
                Genero = "Masculino",
                Grado = "Segundo grado",
                ClaveAcceso = "1234"
            };
            _currentView = new RegistrationConfirmationViewModel(this, testData);
            */
        }

        /// <summary>
        /// Vista actual que se está mostrando en la ventana principal
        /// </summary>
        public ViewModelBase CurrentView
        {
            get => _currentView;
            set
            {
                if (_currentView != value)
                {
                    _currentView = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Navega a la vista de login
        /// </summary>
        public void NavigateToLogin()
        {
            CurrentView = new LoginViewModel(this);
        }

        /// <summary>
        /// Navega de regreso a la vista de bienvenida
        /// </summary>
        public void NavigateToWelcome()
        {
            CurrentView = new WelcomeViewModel(this);
        }

        /// <summary>
        /// Navega a la vista de registro
        /// </summary>
        public void NavigateToRegister()
        {
            CurrentView = new RegisterViewModel(this);
        }

        /// <summary>
        /// Navega a la vista de selección de género
        /// </summary>
        public void NavigateToGenderSelection(UserRegistrationData? userData = null)
        {
            CurrentView = new GenderSelectionViewModel(this, userData);
        }

        /// <summary>
        /// Navega a la vista de selección de grado
        /// </summary>
        public void NavigateToGradeSelection(UserRegistrationData? userData = null)
        {
            CurrentView = new GradeSelectionViewModel(this, userData);
        }

        /// <summary>
        /// Navega a la vista de confirmación de registro
        /// </summary>
        public void NavigateToRegistrationConfirmation(UserRegistrationData userData)
        {
            CurrentView = new RegistrationConfirmationViewModel(this, userData);
        }
    }
}