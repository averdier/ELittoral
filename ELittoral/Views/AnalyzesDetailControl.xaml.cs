using ELittoral.Helpers;
using ELittoral.Models;
using ELittoral.Services;
using System.Windows.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace ELittoral.Views
{
    public sealed partial class AnalyzesDetailControl : UserControl
    {
        public AnalysisModel MasterMenuItem
        {
            get { return GetValue(MasterMenuItemProperty) as AnalysisModel; }
            set { SetValue(MasterMenuItemProperty, value); }
        }

        public static DependencyProperty MasterMenuItemProperty = DependencyProperty.Register("MasterMenuItem",typeof(AnalysisModel),typeof(AnalyzesDetailControl),new PropertyMetadata(null));

        public ICommand ItemClickCommand { get; private set; }

        public AnalyzesDetailControl()
        {
            InitializeComponent();
            ItemClickCommand = new RelayCommand<ItemClickEventArgs>(OnItemClick);
        }

        private void OnItemClick(ItemClickEventArgs args)
        {
            AnalysisResultModel item = args?.ClickedItem as AnalysisResultModel;
            if (item != null)
            {
                NavigationService.Navigate<Views.AnalysisResultPage>(item);
            }
        }
    }
}
