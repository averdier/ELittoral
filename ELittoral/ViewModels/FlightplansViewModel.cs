using System;

using ELittoral.Helpers;
using Windows.UI.Xaml;

namespace ELittoral.ViewModels
{
    public class FlightplansViewModel : Observable
    {
        const string NarrowStateName = "NarrowState";
        const string WideStateName = "WideState";

        private VisualState _currentState;


        public FlightplansViewModel()
        {
        }
    }
}
