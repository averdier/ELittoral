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

namespace ELittoral.ViewModels
{
    public class AnalyzesViewModel : Observable
    {
        const string NarrowStateName = "NarrowState";
        const string WideStateName = "WideState";

        private VisualState _currentState;

        private AnalysisModel _selected;
        public AnalysisModel Selected
        {
            get { return _selected; }
            set { Set(ref _selected, value); }
        }

        public ICommand ItemClickCommand { get; private set; }

        public ICommand AddItemClickCommand { get; private set; }

        public ICommand StateChangedCommand { get; private set; }

        public ObservableCollection<AnalysisModel> AnalysisItems { get; private set; } = new ObservableCollection<AnalysisModel>();


        public bool IsViewState { get { return Selected != null && _currentState.Name != NarrowStateName; } }



        public AnalyzesViewModel()
        {
            ItemClickCommand = new RelayCommand<ItemClickEventArgs>(OnItemClick);
            AddItemClickCommand = new RelayCommand<RoutedEventArgs>(OnAddItemClick);
            StateChangedCommand = new RelayCommand<VisualStateChangedEventArgs>(OnStateChanged);
        }

        public async Task LoadDataAsync(VisualState currentState)
        {
            _currentState = currentState;
            AnalysisItems.Clear();

            var service = new AnalysisModelService();
            var data = await service.GetDataAsync();

            foreach (var item in data)
            {
                AnalysisItems.Add(item);
            }
            Selected = AnalysisItems.First();

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
    }
}
