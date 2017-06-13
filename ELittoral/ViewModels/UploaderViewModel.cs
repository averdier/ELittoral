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
using ELittoral.Services;

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

        private int _totalPictureToUpload;
        public int TotalPictureToUpload
        {
            get { return _totalPictureToUpload; }
            set { Set(ref _totalPictureToUpload, value); }
        }

        private int _currentPictureUploaded = 0;
        public int CurrentPictureUploaded
        {
            get { return _currentPictureUploaded; }
            set { Set(ref _currentPictureUploaded, value); }
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

        private bool _isUploading;
        public bool IsUploading
        {
            get { return _isUploading; }
            set { Set(ref _isUploading, value); }
        }

        private string _loadingMessage;
        public string LoadingMessage
        {
            get { return _loadingMessage; }
            set { Set(ref _loadingMessage, value); }
        }

        private string _uploadingMessage;
        public string UploadingMessage
        {
            get { return _uploadingMessage; }
            set { Set(ref _uploadingMessage, value); }
        }

        private string _statusMessage;
        public string StatusMessage
        {
            get { return _statusMessage; }
            set { Set(ref _statusMessage, value); }
        }

        public ICommand UploadClickCommand;

        public ICommand FlightplanSelectionChangedCommand { get; private set; }

        public ICommand FolderClickCommand { get; private set; }

        public ObservableCollection<FlightplanModel> FlightplanItems { get; } = new ObservableCollection<FlightplanModel>();

        private RESTFlightplanModelService _flightplanModelService;
        private RESTWaypointModelService _waypointModelService;
        private RESTReconModelService _reconModelService;
        private RESTResourceModelService _resourceModelService;



        public UploaderViewModel()
        {
            UploadClickCommand = new RelayCommand<ItemClickEventArgs>(OnUploadClick);
            FolderClickCommand = new RelayCommand<RoutedEventArgs>(OnFolderClick);
            FlightplanSelectionChangedCommand = new RelayCommand<SelectionChangedEventArgs>(OnFlightplanSelectionChanged);
            _flightplanModelService = new RESTFlightplanModelService("http://vps361908.ovh.net/dev/elittoral/api/");
            _reconModelService = new RESTReconModelService("http://vps361908.ovh.net/dev/elittoral/api/");
            _waypointModelService = new RESTWaypointModelService("http://vps361908.ovh.net/dev/elittoral/api/");
            _resourceModelService = new RESTResourceModelService("http://vps361908.ovh.net/dev/elittoral/api/");

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
                InformationsText = string.Format("Ressources trouvées : {0}", _fileList.Count);
            }
        }
        private async void OnUploadClick(ItemClickEventArgs args)
        {
            if (SelectedFlightplan != null && _fileList != null && _fileList.Count > 0)
            {
                try
                {
                    IsUploading = true;
                    UploadingMessage = "Chargement des points de passage";

                    List<WaypointModel> waypointContainer = await _waypointModelService.GetWaypointFromFlightplanIdAsync(SelectedFlightplan.Id);

                    if (waypointContainer != null && waypointContainer.Count > 0)
                    {

                        ReconModel recon = await _reconModelService.PostRecon(SelectedFlightplan.Id);

                        if (recon != null)
                        {
                            var maxPos = Math.Min(waypointContainer.Count, _fileList.Count);
                            TotalPictureToUpload = maxPos;

                            for (int i = 0; i < maxPos; i++)
                            {
                                var waypoint = waypointContainer[i];
                                var file = _fileList[i];

                                var resource = await _resourceModelService.PostReconResource(recon.Id, i, waypoint.Parameters);

                                if (resource != null)
                                {
                                    UploadingMessage = string.Format("upload {0}/{1}", i + 1, maxPos);

                                    if (await _resourceModelService.PostResourceContentAsync(resource.Id, file))
                                    {
                                        UploadingMessage = string.Format("photo {0} uploader", i + 1);
                                    }
                                    else
                                    {
                                        UploadingMessage = string.Format("erreur durant l'upload de la photo {0}", i + 1);
                                    }
                                    CurrentPictureUploaded = i + 1;
                                }
                                else
                                {
                                    UploadingMessage = string.Format("erreur durant la creation de la ressource pour le point de passage #", waypoint.Id);
                                }
                            }

                            NavigationService.Navigate<Views.FlightplanReconPage>(recon);
                        }
                        else
                        {
                            UploadingMessage = string.Format("erreur durant la creation de la reconnaissance");
                        }
                    }
                    else
                    {
                        IsUploading = false;
                        StatusMessage = "";

                        var errordialog = new Windows.UI.Popups.MessageDialog(
                            "Points de passage du plan de vol introuvable",
                            "Erreur");
                        errordialog.Commands.Add(new Windows.UI.Popups.UICommand("Fermer") { Id = 0 });

                        errordialog.DefaultCommandIndex = 0;

                        var resultUnknow = await errordialog.ShowAsync();
                    }
                }
                catch (Exception ex)
                {
                    IsUploading = false;
                    StatusMessage = "";

                    var dialog = new Windows.UI.Popups.MessageDialog(
                    ex.Message,
                    "Erreur"
                    );
                    dialog.Commands.Add(new Windows.UI.Popups.UICommand("Fermer") { Id = 0 });

                    dialog.DefaultCommandIndex = 0;

                    var result = await dialog.ShowAsync();
                }
            }
        }

        private void OnFlightplanSelectionChanged(SelectionChangedEventArgs args)
        {
            var selectedFlightplan = args.AddedItems[0] as FlightplanModel;
            SelectedFlightplan = selectedFlightplan;
        }
    }
}
