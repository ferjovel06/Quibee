using Quibee.Models;
using Quibee;

namespace Quibee.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private ViewModelBase _currentView;

        public MainWindowViewModel()
        {
            // Producción: Iniciar con la vista de bienvenida
            _currentView = new WelcomeViewModel(this);
            
            // TEMPORAL: Para testing directo del mapa
            // _currentView = new LessonsMapViewModel(this, studentId: 1, gradeLevel: 1, ServiceLocator.GetTopicService());
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

        /// <summary>
        /// Navega al mapa de lecciones después del login
        /// </summary>
        /// <param name="studentId">ID del estudiante logueado</param>
        /// <param name="gradeLevel">Grado del estudiante (1, 2 o 3)</param>
        public void NavigateToLessonsMap(int studentId, int gradeLevel)
        {
            CurrentView = new LessonsMapViewModel(this, studentId, gradeLevel, ServiceLocator.GetTopicService());
        }

        /// <summary>
        /// Navega a una lección genérica con los datos proporcionados
        /// </summary>
        /// <param name="lessonData">Datos de la lección</param>
        public void NavigateToGenericLesson(Quibee.Models.LessonData lessonData)
        {
            CurrentView = new GenericLessonViewModel(this, lessonData);
        }
    }
}