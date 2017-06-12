using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

using ELittoral.Helpers;
using ELittoral.Models;
using ELittoral.Services;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using System.Collections.Generic;
using ELittoral.Services.Rest;

namespace ELittoral.ViewModels
{
    public class AnalyzesViewModel : Observable
    {
        const string NarrowStateName = "NarrowState";
        const string WideStateName = "WideState";

        private RESTAnalysisModelService _modelService;

        private VisualState _currentState;

        private AnalysisModel _selected;
        public AnalysisModel Selected
        {
            get { return _selected; }
            set { Set(ref _selected, value); }
        }

        public ICommand ItemClickCommand { get; private set; }

        public ICommand RefreshClickCommand { get; private set; }

        public ICommand AddItemClickCommand { get; private set; }

        public ICommand DeleteItemClickCommand { get; private set; }

        public ICommand StateChangedCommand { get; private set; }

        public ObservableCollection<AnalysisModel> AnalysisItems { get; private set; } = new ObservableCollection<AnalysisModel>();


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

        private int _loadingColumnSpan;
        public int LoadingColumnSpan
        {
            get { return _loadingColumnSpan; }
            set { Set(ref _loadingColumnSpan, value); }
        }


        public AnalyzesViewModel()
        {
            RefreshClickCommand = new RelayCommand<ItemClickEventArgs>(OnRefreshClick);
            ItemClickCommand = new RelayCommand<ItemClickEventArgs>(OnItemClick);
            AddItemClickCommand = new RelayCommand<RoutedEventArgs>(OnAddItemClick);
            DeleteItemClickCommand = new RelayCommand<RoutedEventArgs>(OnDeleteItemClick);
            StateChangedCommand = new RelayCommand<VisualStateChangedEventArgs>(OnStateChanged);
            _modelService = new RESTAnalysisModelService("http://vps361908.ovh.net/dev/elittoral/api/");
        }


        private async Task RefreshItemsAsync()
        {
            await Task.CompletedTask;

            LoadingColumnSpan = (_currentState.Name == NarrowStateName) ? 1 : 2;
            IsLoading = true;
            LoadingMessage = "Chargement des analyses";

            try
            {
                AnalysisItems.Clear();
                var data = await _modelService.GetAnalysesAsync();

                foreach (var item in data)
                {
                    AnalysisItems.Add(item);
                }
                if (AnalysisItems.Count > 0)
                {
                    Selected = AnalysisItems[0];
                }

                IsLoading = false;
                LoadingMessage = "";
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

        private async void OnRefreshClick(ItemClickEventArgs args)
        {
            await RefreshItemsAsync();

            foreach (var item in AnalysisItems)
            {
                if (item.Id == Selected.Id)
                {
                    Selected = item;
                    break;
                }
            }
        }

        public async void LoadData(VisualState currentState)
        {
            _currentState = currentState;
            await RefreshItemsAsync();
            if (AnalysisItems.Count > 0)
            {
                Selected = AnalysisItems[0];
            }

            OnPropertyChanged(nameof(IsViewState));
        }

        private void OnStateChanged(VisualStateChangedEventArgs args)
        {
            _currentState = args.NewState;
            OnPropertyChanged(nameof(IsViewState));
        }

        /// <summary>
        /// On click on analysis
        /// </summary>
        /// <param name="args"></param>
        private void OnItemClick(ItemClickEventArgs args)
        {
            AnalysisModel item = args?.ClickedItem as AnalysisModel;
            if (item != null)
            {
                if (_currentState.Name == NarrowStateName)
                {
                    NavigationService.Navigate<Views.AnalyzesDetailPage>(item);
                }
                else
                {
                    Selected = item;
                    OnPropertyChanged(nameof(IsViewState));
                }
            }
        }

        /// <summary>
        /// On click on button add analysis
        /// </summary>
        /// <param name="args"></param>
        private void OnAddItemClick(RoutedEventArgs args)
        {
            NavigationService.Navigate<Views.AnalysisAddPage>();
        }

        private async void OnDeleteItemClick(RoutedEventArgs args)
        {
            if (Selected != null)
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
                        if (await _modelService.DeleteAnalysisFromIdAsync(Selected.Id))
                        {
                            AnalysisItems.Remove(Selected);
                            if (AnalysisItems.Count > 0)
                            {
                                Selected = AnalysisItems[AnalysisItems.Count - 1];
                            }
                            else
                            {
                                Selected = null;
                                OnPropertyChanged(nameof(IsViewState));
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
}
