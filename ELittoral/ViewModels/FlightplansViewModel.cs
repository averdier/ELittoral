using System;

using ELittoral.Helpers;
using Windows.UI.Xaml;
using ELittoral.Models;
using System.Windows.Input;
using Windows.UI.Xaml.Controls;
using System.Collections.ObjectModel;
using ELittoral.Services;
using System.Threading.Tasks;

namespace ELittoral.ViewModels
{
    public class FlightplansViewModel : Observable
    {
        const string NarrowStateName = "NarrowState";
        const string WideStateName = "WideState";

        private VisualState _currentState;

        private FlightplanModel _selected;

        public FlightplanModel Selected
        {
            get { return _selected; }
            set { Set(ref _selected, value); }
        }

        public ObservableCollection<FlightplanModel> FlightplansItems { get; private set; } = new ObservableCollection<FlightplanModel>();

        public ICommand ItemClickCommand { get; private set; }

        public ICommand AddItemClickCommand { get; private set; }

        public ICommand StateChangedCommand { get; private set; }

        public FlightplansViewModel()
        {
            ItemClickCommand = new RelayCommand<ItemClickEventArgs>(OnItemClick);
            AddItemClickCommand = new RelayCommand<RoutedEventArgs>(OnAddItemClick);
            StateChangedCommand = new RelayCommand<VisualStateChangedEventArgs>(OnStateChanged);
        }

        public async Task LoadDataAsync(VisualState currentState)
        {
            _currentState = currentState;
            FlightplansItems.Clear();

            var service = new FlightplanModelService();
            var data = await service.GetDataAsync();

            foreach (var item in data)
            {
                FlightplansItems.Add(item);
            }
            if (FlightplansItems.Count > 0)
            {
                Selected = FlightplansItems[0];
            }
        }

        public void OnSelectionChanged(object sender, SelectionChangedEventArgs args)
        {

        }

        private void OnItemClick(ItemClickEventArgs args)
        {
            FlightplanModel item = args?.ClickedItem as FlightplanModel;
            if (item != null)
            {
                if (_currentState.Name == NarrowStateName)
                {
                    NavigationService.Navigate<Views.FlightplanDetailPage>(item);
                }
                else
                {
                    Selected = item;
                }
            }
        }

        private void OnAddItemClick(RoutedEventArgs args)
        {
            NavigationService.Navigate<Views.FlightplanBuildPage>();
        }

        private void OnStateChanged(VisualStateChangedEventArgs args)
        {
            _currentState = args.NewState;
        }
    }
}
