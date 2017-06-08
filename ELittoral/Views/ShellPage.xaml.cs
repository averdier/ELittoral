using ELittoral.ViewModels;

using Windows.UI.Xaml.Controls;

namespace ELittoral.Views
{
    public sealed partial class ShellPage : Page
    {
        public ShellViewModel ViewModel { get; } = new ShellViewModel();

        public ShellPage()
        {
            InitializeComponent();
            DataContext = ViewModel;
            ViewModel.Initialize(shellFrame);

            this.Loaded += ShellPage_Loaded;
        }

        private void ShellPage_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            ViewModel.OnPageLoaded(WindowStates.CurrentState);
        }
    }
}
