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
    private string _confirmarClaveAcceso = "";
    private string _mensajeErrorClave = "";
    private bool _mostrarErrorClave;

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

    public string MensajeErrorClave
    {
        get => _mensajeErrorClave;
        private set
        {
            if (_mensajeErrorClave != value)
            {
                _mensajeErrorClave = value;
                OnPropertyChanged();
            }
        }
    }

    public bool MostrarErrorClave
    {
        get => _mostrarErrorClave;
        private set
        {
            if (_mostrarErrorClave != value)
            {
                _mostrarErrorClave = value;
                OnPropertyChanged();
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
                UpdateClaveValidationMessage();
                ((RelayCommand)ConfirmarCommand).RaiseCanExecuteChanged();
            }
        }
    }

    public string ConfirmarClaveAcceso
    {
        get => _confirmarClaveAcceso;
        set
        {
            if (_confirmarClaveAcceso != value)
            {
                _confirmarClaveAcceso = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(PuedeConfirmar));
                UpdateClaveValidationMessage();
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
             !string.IsNullOrWhiteSpace(ConfirmarClaveAcceso) &&
               ClaveAcceso.Length == 4 &&
             ConfirmarClaveAcceso.Length == 4 &&
             ClaveAcceso.All(char.IsDigit) &&
             ConfirmarClaveAcceso.All(char.IsDigit) &&
             ClaveAcceso == ConfirmarClaveAcceso;
    }

    private void UpdateClaveValidationMessage()
    {
        var hasBothValues = !string.IsNullOrWhiteSpace(ClaveAcceso)
            && !string.IsNullOrWhiteSpace(ConfirmarClaveAcceso);

        if (!hasBothValues)
        {
            MensajeErrorClave = "";
            MostrarErrorClave = false;
            return;
        }

        if (ClaveAcceso != ConfirmarClaveAcceso)
        {
            MensajeErrorClave = "Las claves de acceso no coinciden.";
            MostrarErrorClave = true;
            return;
        }

        MensajeErrorClave = "";
        MostrarErrorClave = false;
    }
}
