using System.Windows.Input;
using Quibee.Models;

namespace Quibee.ViewModels
{
    public class GradeSelectionViewModel : ViewModelBase
    {
        private readonly MainWindowViewModel? _mainWindowViewModel;
        private readonly UserRegistrationData? _userData;

        public GradeSelectionViewModel(MainWindowViewModel? mainWindowViewModel = null, UserRegistrationData? userData = null)
        {
            _mainWindowViewModel = mainWindowViewModel;
            _userData = userData;
            SelectFirstGradeCommand = new RelayCommand(SelectFirstGrade);
            SelectSecondGradeCommand = new RelayCommand(SelectSecondGrade);
            SelectThirdGradeCommand = new RelayCommand(SelectThirdGrade);
        }

        public ICommand SelectFirstGradeCommand { get; }
        public ICommand SelectSecondGradeCommand { get; }
        public ICommand SelectThirdGradeCommand { get; }

        private void SelectFirstGrade()
        {
            if (_userData != null)
            {
                _userData.Grado = "Primer grado";
                _mainWindowViewModel?.NavigateToRegistrationConfirmation(_userData);
            }
        }

        private void SelectSecondGrade()
        {
            if (_userData != null)
            {
                _userData.Grado = "Segundo grado";
                _mainWindowViewModel?.NavigateToRegistrationConfirmation(_userData);
            }
        }

        private void SelectThirdGrade()
        {
            if (_userData != null)
            {
                _userData.Grado = "Tercer grado";
                _mainWindowViewModel?.NavigateToRegistrationConfirmation(_userData);
            }
        }
    }
}
