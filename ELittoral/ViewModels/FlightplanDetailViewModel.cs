using ELittoral.Helpers;
using ELittoral.Models;
using ELittoral.Services;
using ELittoral.Services.Rest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Xaml;

namespace ELittoral.ViewModels
{
    public class FlightplanDetailViewModel : Observable
    {
        const string NarrowStateName = "NarrowState";
        const string WideStateName = "WideState";

        private RESTFlightplanModelService _modelService;

        public ICommand StateChangedCommand { get; private set; }
        public ICommand RefreshItemClickCommand { get; private set; }
        public ICommand EditItemClickCommand { get; private set; }
        public ICommand DeleteItemClickCommand { get; private set; }

        private FlightplanModel _item;
        public FlightplanModel Item
        {
            get { return _item; }
            set { Set(ref _item, value); }
        }

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

        public FlightplanDetailViewModel()
        {
            RefreshItemClickCommand = new RelayCommand<RoutedEventArgs>(OnRefreshItemClick);
            DeleteItemClickCommand = new RelayCommand<RoutedEventArgs>(OnDeleteItemClick);
            StateChangedCommand = new RelayCommand<VisualStateChangedEventArgs>(OnStateChanged);
            _modelService = new RESTFlightplanModelService("http://vps361908.ovh.net/dev/elittoral/api/");
        }

        public void LoadData(FlightplanModel item)
        {
            Item = item;
        }

        private async void OnRefreshItemClick(RoutedEventArgs args)
        {
            try
            {
                IsLoading = true;
                LoadingMessage = "Chargement du plan de vol";

                var flightplan = await _modelService.GetFlightplanFromIdAsync(Item.Id);

                IsLoading = false;
                LoadingMessage = "";

                if (flightplan != null)
                {
                    Item = flightplan;
                }
                else
                {
                    var unknowErrorDialog = new Windows.UI.Popups.MessageDialog(
                                "Une erreur est survenue",
                                "Erreur");
                    unknowErrorDialog.Commands.Add(new Windows.UI.Popups.UICommand("Ok") { Id = 0 });
                    await unknowErrorDialog.ShowAsync();
                }

            } catch (Exception ex)
            {
                var errorDialog = new Windows.UI.Popups.MessageDialog(
                            ex.Message,
                            "Erreur");
                errorDialog.Commands.Add(new Windows.UI.Popups.UICommand("Ok") { Id = 0 });
                await errorDialog.ShowAsync();
            }
        }

        private async void OnDeleteItemClick(RoutedEventArgs args)
        {
            if (Item != null)
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

                if ((int)result.Id == 0)
                {
                    try
                    {
                        if (await _modelService.DeleteFlightplanFromIdAsync(Item))
                        {
                            NavigationService.GoBack();
                        }
                        else
                        {
                            var unknowErrorDialog = new Windows.UI.Popups.MessageDialog(
                                "Une erreur est survenue",
                                "Erreur");
                            unknowErrorDialog.Commands.Add(new Windows.UI.Popups.UICommand("Ok") { Id = 0 });
                            await unknowErrorDialog.ShowAsync();
                        }

                    }
                    catch (Exception ex)
                    {
                        var errorDialog = new Windows.UI.Popups.MessageDialog(
                            ex.Message,
                            "Erreur");
                        errorDialog.Commands.Add(new Windows.UI.Popups.UICommand("Ok") { Id = 0 });
                        await errorDialog.ShowAsync();
                    }
                }
            }
        }

        private void OnStateChanged(VisualStateChangedEventArgs args)
        {
            if (args.OldState.Name == NarrowStateName && args.NewState.Name == WideStateName)
            {
                NavigationService.GoBack();
            }
        }
    }
}
