using System.Windows.Input;
using System.Threading.Tasks;
using Quibee.Services;
using Quibee;

namespace Quibee.ViewModels
{
    public class GradeSelectionViewModel : ViewModelBase
    {
        private readonly MainWindowViewModel? _mainWindowViewModel;
        private readonly int _studentId;
        private readonly StudentService _studentService;

        public GradeSelectionViewModel(MainWindowViewModel? mainWindowViewModel = null, int studentId = 0)
        {
            _mainWindowViewModel = mainWindowViewModel;
            _studentId = studentId;
            _studentService = new StudentService(ServiceLocator.GetDbContext());
            SelectFirstGradeCommand = new RelayCommand(SelectFirstGrade);
            SelectSecondGradeCommand = new RelayCommand(SelectSecondGrade);
            SelectThirdGradeCommand = new RelayCommand(SelectThirdGrade);
        }

        public ICommand SelectFirstGradeCommand { get; }
        public ICommand SelectSecondGradeCommand { get; }
        public ICommand SelectThirdGradeCommand { get; }

        private void SelectFirstGrade()
        {
            _ = SelectLevelAsync(1);
        }

        private void SelectSecondGrade()
        {
            _ = SelectLevelAsync(2);
        }

        private void SelectThirdGrade()
        {
            _ = SelectLevelAsync(3);
        }

        private async Task SelectLevelAsync(int levelNumber)
        {
            if (_studentId <= 0)
            {
                return;
            }

            var updated = await _studentService.UpdateLevelNumberAsync(_studentId, levelNumber);
            if (updated)
            {
                _mainWindowViewModel?.NavigateToLessonsMap(_studentId, levelNumber);
            }
        }
    }
}
