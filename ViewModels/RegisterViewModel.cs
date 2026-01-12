using System;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Quibee.Models;

namespace Quibee.ViewModels;

public class RegisterViewModel : ViewModelBase
{
    private readonly MainWindowViewModel? _mainWindowViewModel;
    private string _nombres = "";
    private string _apellidos = "";
    private DateTimeOffset? _fechaNacimiento;
    private string _claveAcceso = "";

    public RegisterViewModel(MainWindowViewModel? mainWindowViewModel = null)
    {
        _mainWindowViewModel = mainWindowViewModel;
        RegresarCommand = new RelayCommand(OnRegresar);
        ConfirmarCommand = new RelayCommand(OnConfirmar, CanConfirmar);
    }

    public string Nombres
    {
        get => _nombres;
        set
        {
            if (_nombres != value)
            {
                _nombres = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(PuedeConfirmar));
                ((RelayCommand)ConfirmarCommand).RaiseCanExecuteChanged();
            }
        }
    }

    public string Apellidos
    {
        get => _apellidos;
        set
        {
            if (_apellidos != value)
            {
                _apellidos = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(PuedeConfirmar));
                ((RelayCommand)ConfirmarCommand).RaiseCanExecuteChanged();
            }
        }
    }

    public DateTimeOffset? FechaNacimiento
    {
        get => _fechaNacimiento;
        set
        {
            if (_fechaNacimiento != value)
            {
                _fechaNacimiento = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(PuedeConfirmar));
                ((RelayCommand)ConfirmarCommand).RaiseCanExecuteChanged();
            }
        }
    }

    public string ClaveAcceso
    {
        get => _claveAcceso;
        set
        {
            if (_claveAcceso != value)
            {
                _claveAcceso = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(PuedeConfirmar));
                ((RelayCommand)ConfirmarCommand).RaiseCanExecuteChanged();
            }
        }
    }

    public bool PuedeConfirmar => CanConfirmar();

    public ICommand RegresarCommand { get; }
    public ICommand ConfirmarCommand { get; }

    private void OnRegresar()
    {
        _mainWindowViewModel?.NavigateToLogin();
    }

    private void OnConfirmar()
    {
        // Crear objeto con los datos del usuario
        var userData = new UserRegistrationData
        {
            Nombres = Nombres,
            Apellidos = Apellidos,
            FechaNacimiento = FechaNacimiento?.DateTime,
            ClaveAcceso = ClaveAcceso
        };
        
        // Navegar a la pantalla de selección de género pasando los datos
        _mainWindowViewModel?.NavigateToGenderSelection(userData);
    }

    private bool CanConfirmar()
    {
        return !string.IsNullOrWhiteSpace(Nombres) &&
               !string.IsNullOrWhiteSpace(Apellidos) &&
               FechaNacimiento.HasValue &&
               !string.IsNullOrWhiteSpace(ClaveAcceso) &&
               ClaveAcceso.Length == 4 &&
               ClaveAcceso.All(char.IsDigit);
    }
}
