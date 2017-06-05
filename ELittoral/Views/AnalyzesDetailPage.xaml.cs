using ELittoral.Models;
using ELittoral.ViewModels;

using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace ELittoral.Views
{
    public sealed partial class AnalyzesDetailPage : Page
    {
        public AnalyzesDetailViewModel ViewModel { get; } = new AnalyzesDetailViewModel();
        public AnalyzesDetailPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            ViewModel.Item = e.Parameter as AnalysisModel;
        }
    }
}
