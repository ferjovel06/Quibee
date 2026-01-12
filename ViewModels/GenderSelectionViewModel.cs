using System.Windows.Input;
using Quibee.Models;

namespace Quibee.ViewModels;

public class GenderSelectionViewModel : ViewModelBase
{
    private readonly MainWindowViewModel? _mainWindowViewModel;
    private readonly UserRegistrationData? _userData;
    private string _generoSeleccionado = "";

    public GenderSelectionViewModel(MainWindowViewModel? mainWindowViewModel = null, UserRegistrationData? userData = null)
    {
        _mainWindowViewModel = mainWindowViewModel;
        _userData = userData;
        SelectMasculinoCommand = new RelayCommand(OnSelectMasculino);
        SelectFemeninoCommand = new RelayCommand(OnSelectFemenino);
    }

    public string GeneroSeleccionado
    {
        get => _generoSeleccionado;
        set
        {
            if (_generoSeleccionado != value)
            {
                _generoSeleccionado = value;
                OnPropertyChanged();
            }
        }
    }

    public ICommand SelectMasculinoCommand { get; }
    public ICommand SelectFemeninoCommand { get; }

    private void OnSelectMasculino()
    {
        GeneroSeleccionado = "Masculino";
        if (_userData != null)
        {
            _userData.Genero = "Masculino";
        }
        _mainWindowViewModel?.NavigateToGradeSelection(_userData);
    }

    private void OnSelectFemenino()
    {
        GeneroSeleccionado = "Femenino";
        if (_userData != null)
        {
            _userData.Genero = "Femenino";
        }
        _mainWindowViewModel?.NavigateToGradeSelection(_userData);
    }
}
