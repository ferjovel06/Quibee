using System;
using System.Windows.Input;
using Quibee.Models;

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

        private void OnContinuar()
        {
            // TODO: Guardar los datos y navegar a la siguiente pantalla
            _mainWindowViewModel?.NavigateToWelcome();
        }
    }
}
