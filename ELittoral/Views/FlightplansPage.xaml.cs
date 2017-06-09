using ELittoral.ViewModels;

using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace ELittoral.Views
{
    public sealed partial class FlightplansPage : Page
    {
        public FlightplansViewModel ViewModel { get; } = new FlightplansViewModel();
        public FlightplansPage()
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
