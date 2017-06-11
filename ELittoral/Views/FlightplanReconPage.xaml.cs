using ELittoral.Models;
using ELittoral.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// Pour plus d'informations sur le modèle d'élément Page vierge, consultez la page https://go.microsoft.com/fwlink/?LinkId=234238

namespace ELittoral.Views
{
    /// <summary>
    /// Une page vide peut être utilisée seule ou constituer une page de destination au sein d'un frame.
    /// </summary>
    public sealed partial class FlightplanReconPage : Page
    {
        public FlightplanReconViewModel ViewModel { get; } = new FlightplanReconViewModel();

        public FlightplanReconPage()
        {
            this.InitializeComponent();
            this.SizeChanged += FlightplanReconPage_SizeChanged;
            this.Loaded += FlightplanReconPage_Loaded;
        }

        private void FlightplanReconPage_Loaded(object sender, RoutedEventArgs e)
        {
            var pageWidth = pageRoot.ActualWidth;
            Debug.WriteLine(pageWidth);
            ViewModel.UpdateImageHeight(pageWidth);
            ViewModel.UpdateImageWidth(pageWidth);
        }

        private void FlightplanReconPage_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            var pageWidth = pageRoot.ActualWidth;
            ViewModel.UpdateImageHeight(pageWidth);
            ViewModel.UpdateImageWidth(pageWidth);
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            
            ViewModel.OnNavigatedToPageAsync(e.Parameter as ReconModel);
        }
    }
}
