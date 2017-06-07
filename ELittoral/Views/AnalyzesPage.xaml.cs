using ELittoral.ViewModels;
using System.Diagnostics;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace ELittoral.Views
{
    public sealed partial class AnalyzesPage : Page
    {
        public AnalyzesViewModel ViewModel { get; } = new AnalyzesViewModel();
        public AnalyzesPage()
        {
            InitializeComponent();
        }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            
            await ViewModel.LoadDataAsync(WindowStates.CurrentState);
        }
    }
}
