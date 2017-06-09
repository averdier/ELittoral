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

        private FlightplanModel _item;
        public FlightplanModel Item
        {
            get { return _item; }
            set { Set(ref _item, value); }
        }

        public FlightplanDetailViewModel()
        {
            StateChangedCommand = new RelayCommand<VisualStateChangedEventArgs>(OnStateChanged);
        }

        public void LoadData(FlightplanModel item)
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
    }
}
