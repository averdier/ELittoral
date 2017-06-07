using ELittoral.Helpers;
using ELittoral.Models;
using ELittoral.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace ELittoral.ViewModels
{
    public class AnalysisAddViewModel : Observable
    {
        private FlightplanModel _selectedFlightplan;
        public FlightplanModel SelectedFlightplan
        {
            get { return _selectedFlightplan; }
            set { Set(ref _selectedFlightplan, value); }
        }

        private ReconModel _selectedReconA;
        public ReconModel SelectedReconA
        {
            get { return _selectedReconA;  }
            set { Set(ref _selectedReconA, value); }
        }

        private ReconModel _selectedReconB;
        public ReconModel SelectedReconB
        {
            get { return _selectedReconB; }
            set { Set(ref _selectedReconB, value); }
        }

        public ObservableCollection<FlightplanModel> FlightPlanItems { get; private set; } = new ObservableCollection<FlightplanModel>();


        public ICommand CancelClickCommand { get; private set; }

        public ICommand LaunchClickCommand { get; private set; }

        public ICommand FlightplanSelectionChangedCommand { get; private set; }

        public ICommand ReconASelectionChangedCommand { get; private set; }

        public ICommand ReconBSelectionChangedCommand { get; private set; }


        public bool LaunchBtn_IsEnabled { get { return _selectedFlightplan != null && _selectedReconA != null && _selectedReconB != null; } }


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
            LaunchClickCommand = new RelayCommand<RoutedEventArgs>(OnLaunchClick);
            CancelClickCommand = new RelayCommand<RoutedEventArgs>(OnCancelClick);
            FlightplanSelectionChangedCommand = new RelayCommand<SelectionChangedEventArgs>(OnFlightplanSelectionChanged);
            ReconASelectionChangedCommand = new RelayCommand<SelectionChangedEventArgs>(OnReconASelectionChanged);
            ReconBSelectionChangedCommand = new RelayCommand<SelectionChangedEventArgs>(OnReconBSelectionChanged);
        }

        private void OnLaunchClick(RoutedEventArgs args)
        {
            // Do analysis

            var createdAnalysis = new Object();
            NavigationService.Navigate<Views.AnalyzesPage>(createdAnalysis);
        }

        private void OnReconASelectionChanged(SelectionChangedEventArgs args)
        {
            var selectedRecon = args.AddedItems[0] as ReconModel;
            SelectedReconA = selectedRecon;
            OnPropertyChanged(nameof(LaunchBtn_IsEnabled));
        }

        private void OnReconBSelectionChanged(SelectionChangedEventArgs args)
        {
            var selectedRecon = args.AddedItems[0] as ReconModel;
            SelectedReconB = selectedRecon;
            OnPropertyChanged(nameof(LaunchBtn_IsEnabled));
        }

        private void OnFlightplanSelectionChanged(SelectionChangedEventArgs args)
        {
            var selectedFlightplan = args.AddedItems[0] as FlightplanModel;
            SelectedFlightplan = selectedFlightplan;
            OnPropertyChanged(nameof(LaunchBtn_IsEnabled));
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
