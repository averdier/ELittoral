using System;
using System.Windows.Input;

using ELittoral.Helpers;
using ELittoral.Models;
using ELittoral.Services;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace ELittoral.ViewModels
{
    public class AnalyzesDetailViewModel : Observable
    {
        const string NarrowStateName = "NarrowState";
        const string WideStateName = "WideState";

        public ICommand StateChangedCommand { get; private set; }

        private AnalysisModel _item;
        public AnalysisModel Item
        {
            get { return _item; }
            set { Set(ref _item, value); }
        }

        public AnalyzesDetailViewModel()
        {
            StateChangedCommand = new RelayCommand<VisualStateChangedEventArgs>(OnStateChanged);
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
