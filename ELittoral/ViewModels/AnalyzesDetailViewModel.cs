using System;
using System.Windows.Input;

using ELittoral.Helpers;
using ELittoral.Models;
using ELittoral.Services;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using System.Diagnostics;
using ELittoral.Services.Rest;

namespace ELittoral.ViewModels
{
    public class AnalyzesDetailViewModel : Observable
    {
        const string NarrowStateName = "NarrowState";
        const string WideStateName = "WideState";

        public ICommand StateChangedCommand { get; private set; }

        public ICommand DeleteItemClickCommand { get; private set; }

        private AnalysisModel _item;
        public AnalysisModel Item
        {
            get { return _item; }
            set { Set(ref _item, value); }
        }

        private RESTAnalysisModelService _modelService;

        public AnalyzesDetailViewModel()
        {
            StateChangedCommand = new RelayCommand<VisualStateChangedEventArgs>(OnStateChanged);
            _modelService = new RESTAnalysisModelService("http://vps361908.ovh.net/dev/elittoral/api/");
        }

        public void LoadData(AnalysisModel item)
        {
            Item = item;
        }
        
        private void OnStateChanged(VisualStateChangedEventArgs args)
        {
            if (args.OldState.Name == NarrowStateName && args.NewState.Name == WideStateName)
            {
                NavigationService.GoBack();
            }
        }

        private async void OnDeleteItemClick(RoutedEventArgs args)
        {
            var dialog = new Windows.UI.Popups.MessageDialog(
                    "Supprimer une analyse supprime toutes les resultats associés, voulez vous continuer ?",
                    "Supprimer une analyse"
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
                    if (await _modelService.DeleteAnalysisFromIdAsync(Item.Id))
                    {
                        NavigationService.GoBack();
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
                    var errorDialog = new Windows.UI.Popups.MessageDialog(
                    ex.Message,
                    "Erreur"
                    );
                    errorDialog.Commands.Add(new Windows.UI.Popups.UICommand("Fermer") { Id = 0 });

                    errorDialog.DefaultCommandIndex = 0;

                    var errorResult = await dialog.ShowAsync();
                }
            }
        }
    }
}
