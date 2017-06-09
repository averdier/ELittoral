using ELittoral.Helpers;
using ELittoral.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Xaml;

namespace ELittoral.ViewModels
{
    public class FlightplanBuildViewModel : Observable
    {
        public ICommand CancelClickCommand { get; private set; }

        public FlightplanBuildViewModel()
        {
            CancelClickCommand = new RelayCommand<RoutedEventArgs>(OnCancelClick);
        }

        private void OnCancelClick(RoutedEventArgs args)
        {
            if (NavigationService.CanGoBack) { NavigationService.GoBack(); }
        }
    }
}
