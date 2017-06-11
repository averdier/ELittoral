using ELittoral.Helpers;
using ELittoral.Models;
using ELittoral.Services;
using ELittoral.Services.Rest;
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
    public class FlightplanReconViewModel : Observable
    {
        private ReconModel _item;
        public ReconModel Item
        {
            get { return _item; }
            set { Set(ref _item, value); }
        }

        public ICommand RefreshItemClickCommand { get; private set; }
        public ICommand DeleteItemClickCommand { get; private set; }
        public ICommand ItemClickCommand { get; private set; }

        private double PreferredImageWidth = 260;

        private double _imageWidth;
        public double ImageWidth
        {
            get { return _imageWidth; }
            set
            {
                if (Math.Abs(value - _imageWidth) > 0.001)
                {
                    Set(ref _imageWidth, value);
                }
            }
        }

        private double _imageHeight;
        public double ImageHeight
        {
            get { return _imageHeight; }
            set
            {
                if (Math.Abs(value - _imageHeight) > 0.001)
                {
                    Set(ref _imageHeight, value);
                }
            }
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

        private RESTReconModelService _modelService;

        public ObservableCollection<ReconResourceModel> ResourceItems { get; private set; } = new ObservableCollection<ReconResourceModel>();

        public FlightplanReconViewModel()
        {
            ItemClickCommand = new RelayCommand<ItemClickEventArgs>(OnItemClickCommand);
            RefreshItemClickCommand = new RelayCommand<RoutedEventArgs>(OnRefreshItemClick);
            DeleteItemClickCommand = new RelayCommand<RoutedEventArgs>(OnDeleteItemClick);
            _modelService = new RESTReconModelService("http://vps361908.ovh.net/dev/elittoral/api/");
        }

        private void OnItemClickCommand(ItemClickEventArgs args)
        {
            ReconResourceModel item = args?.ClickedItem as ReconResourceModel;
            if (item != null)
            {
                NavigationService.Navigate<Views.ImagePage>(item as ImageModel);
            }
        }

        private async void RefreshCurrentItem(ReconModel item)
        {
            try
            {
                ResourceItems.Clear();
                IsLoading = true;
                LoadingMessage = "Chargement de la reconnaissance";

                var recon = await _modelService.GetReconFromIdAsync(item.Id);

                IsLoading = false;
                LoadingMessage = "";

                if (recon != null)
                {
                    Item = recon;
                    foreach (ReconResourceModel res in Item.Resources)
                    {
                        ResourceItems.Add(res);
                    }
                }
                else
                {
                    var unknowErrordialog = new Windows.UI.Popups.MessageDialog(
                            "Une erreur est survenue",
                            "Erreur");
                    unknowErrordialog.Commands.Add(new Windows.UI.Popups.UICommand("Fermer") { Id = 0 });

                    unknowErrordialog.DefaultCommandIndex = 0;

                    var resultUnknow = await unknowErrordialog.ShowAsync();
                }
            }
            catch (Exception ex)
            {
                var dialog = new Windows.UI.Popups.MessageDialog(
                    ex.Message,
                    "Erreur"
                    );
                dialog.Commands.Add(new Windows.UI.Popups.UICommand("Fermer") { Id = 0 });

                dialog.DefaultCommandIndex = 0;

                var result = await dialog.ShowAsync();
            }
        }

        public void OnNavigatedToPageAsync(ReconModel item)
        {
            RefreshCurrentItem(item);
        }

        private void OnRefreshItemClick(RoutedEventArgs args)
        {
            RefreshCurrentItem(Item);
        }

        private async void OnDeleteItemClick(RoutedEventArgs args)
        {
            var dialog = new Windows.UI.Popups.MessageDialog(
                    "Voulez vous vraiment supprimer la reconnaissance ?",
                    "Supprimer une reconnaissance"
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
                    if (await _modelService.DeleteReconFromIdAsync(Item.Id))
                    {
                        NavigationService.GoBack();
                    }
                    else
                    {
                        var unknowErrordialog = new Windows.UI.Popups.MessageDialog(
                            "Une erreur est survenue",
                            "Erreur"
                            );
                        unknowErrordialog.Commands.Add(new Windows.UI.Popups.UICommand("Fermer") { Id = 0 });

                        unknowErrordialog.DefaultCommandIndex = 0;

                        await unknowErrordialog.ShowAsync();
                    }
                }
                catch (Exception ex)
                {
                    var errordialog = new Windows.UI.Popups.MessageDialog(
                            ex.Message,
                            "Erreur"
                            );
                    errordialog.Commands.Add(new Windows.UI.Popups.UICommand("Fermer") { Id = 0 });

                    errordialog.DefaultCommandIndex = 0;

                    await errordialog.ShowAsync();
                }
            } 
        }

        public void UpdateImageHeight(double pageWidth)
        {
            int additionalHeightPerItem;

            if (pageWidth < 720)
            {
                additionalHeightPerItem = (int)(ImageWidth / 3.25);
            }
            else if (pageWidth > 1900)
            {
                additionalHeightPerItem = (int)(ImageWidth / 2);
            }
            else
            {
                additionalHeightPerItem = (int)(ImageWidth / 2.75);
            }

            ImageHeight = ImageWidth + additionalHeightPerItem;
        }

        public void UpdateImageWidth(double pageWidth)
        {
            if (pageWidth < 720)
            {
                ImageWidth = pageWidth;
            }
            else
            {
                var itemsPerRow = (int)(pageWidth / PreferredImageWidth);
                var rest = (int)(pageWidth % PreferredImageWidth);
                var additionalWidthPerItem = rest / itemsPerRow;

                ImageWidth = PreferredImageWidth + additionalWidthPerItem;
                
            }
        }
    }
}
