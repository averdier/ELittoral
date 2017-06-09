using System;

using ELittoral.Helpers;
using Windows.UI.Xaml;
using ELittoral.Models;
using System.Windows.Input;
using Windows.UI.Xaml.Controls;
using System.Collections.ObjectModel;
using ELittoral.Services;
using System.Threading.Tasks;
using ELittoral.Services.Rest;
using System.Diagnostics;

namespace ELittoral.ViewModels
{
    public class FlightplansViewModel : Observable
    {
        const string NarrowStateName = "NarrowState";
        const string WideStateName = "WideState";

        private RESTFlightplanModelService _modelService;

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

        public ICommand DeleteItemClickCommand { get; private set; }

        public ICommand StateChangedCommand { get; private set; }

        public bool IsViewState { get { return Selected != null && _currentState.Name != NarrowStateName; } }

        private bool _isLoading;
        public bool IsLoading
        {
            get { return _isLoading; }
            set { Set(ref _isLoading, value); }
        }

        private string _loadingMessage;
        public string LoadingMessage
        {
            get { return _loadingMessage; }
            set { Set(ref _loadingMessage, value); }
        }

        public FlightplansViewModel()
        {
            ItemClickCommand = new RelayCommand<ItemClickEventArgs>(OnItemClick);
            AddItemClickCommand = new RelayCommand<RoutedEventArgs>(OnAddItemClick);
            DeleteItemClickCommand = new RelayCommand<RoutedEventArgs>(OnDeleteItemClick);
            StateChangedCommand = new RelayCommand<VisualStateChangedEventArgs>(OnStateChanged);
            _modelService = new RESTFlightplanModelService("http://vps361908.ovh.net/dev/elittoral/api/");
        }

        public async Task LoadDataAsync(VisualState currentState)
        {
            IsLoading = true;
            _currentState = currentState;
            FlightplansItems.Clear();

            try
            {
                LoadingMessage = "Chargement des plans de vols";

                var data = await _modelService.GetFlightplansAsync();

                LoadingMessage = "Traitement des données";

                foreach (var item in data)
                {
                    FlightplansItems.Add(item);
                }
                if (FlightplansItems.Count > 0)
                {
                    Selected = FlightplansItems[0];
                }

                IsLoading = false;
                OnPropertyChanged(nameof(IsViewState));
            }
            catch (TaskCanceledException)
            {
                Debug.WriteLine("Task canceled");
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
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
                    OnPropertyChanged(nameof(IsViewState));
                }
            }
        }

        private void OnAddItemClick(RoutedEventArgs args)
        {
            NavigationService.Navigate<Views.FlightplanBuildPage>();
        }

        private async void OnDeleteItemClick(RoutedEventArgs args)
        {
            if (Selected != null)
            {
                var dialog = new Windows.UI.Popups.MessageDialog(
                    "Supprimer un plan de vol supprime toutes les données associés, voulez vous continuer ?",
                    "Supprimer un plan de vol"
                    );
                dialog.Commands.Add(new Windows.UI.Popups.UICommand("Oui") { Id = 0 });
                dialog.Commands.Add(new Windows.UI.Popups.UICommand("Non") { Id = 1 });

                dialog.DefaultCommandIndex = 0;
                dialog.CancelCommandIndex = 1;

                var result = await dialog.ShowAsync();
            }
        }

        private void OnStateChanged(VisualStateChangedEventArgs args)
        {
            _currentState = args.NewState;
            OnPropertyChanged(nameof(IsViewState));
        }
    }
}
