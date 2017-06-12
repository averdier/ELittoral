using System;

using ELittoral.Helpers;
using System.Windows.Input;
using Windows.UI.Xaml.Controls;
using System.Collections.ObjectModel;
using ELittoral.Models;
using ELittoral.Services.Rest;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.Storage;
using System.Collections.Generic;

namespace ELittoral.ViewModels
{
    public class UploaderViewModel : Observable
    {
        private StorageFolder _selectedFolder;

        private IReadOnlyList<StorageFile> _fileList;

        private FlightplanModel _selectedFlightplan;
        public FlightplanModel SelectedFlightplan
        {
            get { return _selectedFlightplan; }
            set { Set(ref _selectedFlightplan, value); }
        }

        private string _selectedFolderText;
        public string SelectedFolderText
        {
            get { return _selectedFolderText; }
            set { Set(ref _selectedFolderText, value); }
        }

        private string _informationsText;
        public string InformationsText
        {
            get { return _informationsText; }
            set { Set(ref _informationsText, value); }
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

        public ICommand UploadClickCommand;

        public ICommand FlightplanSelectionChangedCommand { get; private set; }

        public ICommand FolderClickCommand { get; private set; }

        public ObservableCollection<FlightplanModel> FlightplanItems { get; } = new ObservableCollection<FlightplanModel>();

        private RESTFlightplanModelService _flightplanModelService;

        public UploaderViewModel()
        {
            UploadClickCommand = new RelayCommand<ItemClickEventArgs>(OnUploadClick);
            FolderClickCommand = new RelayCommand<RoutedEventArgs>(OnFolderClick);
            FlightplanSelectionChangedCommand = new RelayCommand<SelectionChangedEventArgs>(OnFlightplanSelectionChanged);
            _flightplanModelService = new RESTFlightplanModelService("http://vps361908.ovh.net/dev/elittoral/api/");
        }

        public async Task LoadDataAsync()
        {
            FlightplanItems.Clear();

            try
            {
                IsLoading = true;
                LoadingMessage = "Chargement des plans de vol";

                var data = await _flightplanModelService.GetFlightplansAsync();
                if (data != null)
                {
                    foreach(FlightplanModel fp in data)
                    {
                        FlightplanItems.Add(fp);
                    }
                }
                else
                {
                    var unknowErrorDialog = new Windows.UI.Popups.MessageDialog(
                                "Une erreur est survenue",
                                "Erreur");
                    unknowErrorDialog.Commands.Add(new Windows.UI.Popups.UICommand("Ok") { Id = 0 });
                    await unknowErrorDialog.ShowAsync();
                }

                IsLoading = false;
                LoadingMessage = "";
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

        public async void OnFolderClick(RoutedEventArgs args)
        {
            var folderPicker = new Windows.Storage.Pickers.FolderPicker();
            folderPicker.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.Desktop;
            folderPicker.FileTypeFilter.Add("*");

            StorageFolder folder = await folderPicker.PickSingleFolderAsync();

            if (folder != null)
            {
                Windows.Storage.AccessCache.StorageApplicationPermissions.
                FutureAccessList.AddOrReplace("PickedFolderToken", folder);

                _selectedFolder = folder;
                SelectedFolderText = "Dossier : " + folder.Name;

                _fileList = await folder.GetFilesAsync();
                TextBlockResourcesCount.Text = string.Format("Ressources trouvées : {0}", _fileList.Count);
            }
        }
        private void OnUploadClick(ItemClickEventArgs args)
        {

        }

        private void OnFlightplanSelectionChanged(SelectionChangedEventArgs args)
        {
            var selectedFlightplan = args.AddedItems[0] as FlightplanModel;
            SelectedFlightplan = selectedFlightplan;
        }
    }
}
