using ELittoral.Helpers;
using ELittoral.Models;
using ELittoral.Services;
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

        public ICommand StateChangedCommand { get; private set; }

        public ICommand DeleteItemClickCommand { get; private set; }

        private FlightplanModel _item;
        public FlightplanModel Item
        {
            get { return _item; }
            set { Set(ref _item, value); }
        }

        public FlightplanDetailViewModel()
        {
            DeleteItemClickCommand = new RelayCommand<RoutedEventArgs>(OnDeleteItemClick);
            StateChangedCommand = new RelayCommand<VisualStateChangedEventArgs>(OnStateChanged);
        }

        public void LoadData(FlightplanModel item)
        {
            Item = item;
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
