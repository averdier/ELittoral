using ELittoral.ViewModels;

using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace ELittoral.Views
{
    public sealed partial class HomePage : Page
    {
        public HomeViewModel ViewModel { get; } = new HomeViewModel();
        public HomePage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            this.DataContext = ViewModel;
        }
    }
}
