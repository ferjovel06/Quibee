using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Quibee.ViewModels;

namespace Quibee.Views
{
    public partial class GradeSelectionView : UserControl
    {
        public GradeSelectionView()
        {
            InitializeComponent();
        }

        private void OnFirstGradePressed(object? sender, PointerPressedEventArgs e)
        {
            if (DataContext is GradeSelectionViewModel viewModel)
            {
                viewModel.SelectFirstGradeCommand.Execute(null);
            }
        }

        private void OnSecondGradePressed(object? sender, PointerPressedEventArgs e)
        {
            if (DataContext is GradeSelectionViewModel viewModel)
            {
                viewModel.SelectSecondGradeCommand.Execute(null);
            }
        }

        private void OnThirdGradePressed(object? sender, PointerPressedEventArgs e)
        {
            if (DataContext is GradeSelectionViewModel viewModel)
            {
                viewModel.SelectThirdGradeCommand.Execute(null);
            }
        }
    }
}
