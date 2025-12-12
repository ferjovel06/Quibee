using Avalonia.Controls;
using Quibee.ViewModels;

namespace Quibee.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainWindowViewModel();
        }
    }
}