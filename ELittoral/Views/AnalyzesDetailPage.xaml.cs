using ELittoral.Models;
using ELittoral.Services;
using ELittoral.ViewModels;
using System.Diagnostics;
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

            ViewModel.LoadData(e.Parameter as AnalysisModel);

            this.Loaded += AnalyzesDetailPage_Loaded;
        }

        private void AnalyzesDetailPage_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            if (WindowStates.CurrentState.Name =="WideState")
            {
                NavigationService.GoBack();
            }
        }
    }
}
