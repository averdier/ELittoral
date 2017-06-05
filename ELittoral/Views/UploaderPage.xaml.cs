using ELittoral.ViewModels;

using Windows.UI.Xaml.Controls;

namespace ELittoral.Views
{
    public sealed partial class UploaderPage : Page
    {
        public UploaderViewModel ViewModel { get; } = new UploaderViewModel();
        public UploaderPage()
        {
            InitializeComponent();
        }
    }
}
