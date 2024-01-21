using Presentation.wpf.ViewModels;
using System.Windows;

namespace Presentation.wpf
{
    public partial class MainWindow : Window
    {
        public MainWindow(MainViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }
    }
}