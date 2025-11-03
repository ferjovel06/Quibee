using System;
using System.Windows.Input;

namespace Quibee.ViewModels
{
    public class WelcomeViewModel : ViewModelBase
    {
        public WelcomeViewModel()
        {
            StartCommand = new RelayCommand(OnStart);
        }

        public ICommand StartCommand { get; }

        private void OnStart()
        {
            // Aquí irá la lógica para navegar a la siguiente pantalla
            // Por ejemplo, mostrar la pantalla de login o menú principal
        }
    }

    // Implementación simple de ICommand
    public class RelayCommand : ICommand
    {
        private readonly Action _execute;
        private readonly Func<bool>? _canExecute;

        public RelayCommand(Action execute, Func<bool>? canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        public event EventHandler? CanExecuteChanged;

        public bool CanExecute(object? parameter) => _canExecute?.Invoke() ?? true;

        public void Execute(object? parameter) => _execute();

        public void RaiseCanExecuteChanged() => CanExecuteChanged?.Invoke(this, EventArgs.Empty);
    }
}
