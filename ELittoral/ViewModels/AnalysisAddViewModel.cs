using ELittoral.Helpers;
using ELittoral.Models;
using ELittoral.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Xaml;

namespace ELittoral.ViewModels
{
    public class AnalysisAddViewModel : Observable
    {
        public ObservableCollection<FlightplanModel> FlightPlanItems { get; private set; } = new ObservableCollection<FlightplanModel>();

        public ICommand CancelClickCommand { get; private set; }

        public async Task LoadDataAsync()
        {
            FlightPlanItems.Clear();

            var service = new FlightplanModelService();
            var data = await service.GetDataAsync();

            foreach (var item in data)
            {
                FlightPlanItems.Add(item);
            }
        }

        public AnalysisAddViewModel()
        {
            CancelClickCommand = new RelayCommand<RoutedEventArgs>(OnCancelClick);
        }

        /// <summary>
        /// On click on cancel button
        /// </summary>
        /// <param name="args"></param>
        private void OnCancelClick(RoutedEventArgs args)
        {
            if (NavigationService.CanGoBack) { NavigationService.GoBack();  }
        }
    }
}
