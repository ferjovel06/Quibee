namespace Quibee.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private ViewModelBase _currentView;

        public MainWindowViewModel()
        {
            // Iniciar con la vista de bienvenida
            _currentView = new WelcomeViewModel(this);
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
    }
}