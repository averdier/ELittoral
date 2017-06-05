using ELittoral.ViewModels;

using Windows.UI.Xaml.Controls;

namespace ELittoral.Views
{
    public sealed partial class FlightplansPage : Page
    {
        public FlightplansViewModel ViewModel { get; } = new FlightplansViewModel();
        public FlightplansPage()
        {
            InitializeComponent();
        }
    }
}
