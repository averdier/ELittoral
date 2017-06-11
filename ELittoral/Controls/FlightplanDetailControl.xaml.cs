using ELittoral.ControlModels;
using ELittoral.Models;
using System;
using System.Collections.Generic;
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

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace ELittoral.Controls
{
    public sealed partial class FlightplanDetailControl : UserControl
    {
        public FlightplanModel MasterMenuItem
        {
            get { return GetValue(MasterMenuItemProperty) as FlightplanModel; }
            set { SetValue(MasterMenuItemProperty, value); }
        }

        public int ThumbnailImageSideLength
        {
            get { return (int)GetValue(ThumbnailImageSideLengthProperty); }
            set { SetValue(ThumbnailImageSideLengthProperty, value);  }
        }

        public static DependencyProperty MasterMenuItemProperty = DependencyProperty.Register("MasterMenuItem", typeof(FlightplanModel), typeof(FlightplanDetailControl), new PropertyMetadata(null));
        public static DependencyProperty ThumbnailImageSideLengthProperty = DependencyProperty.Register("ThumbnailImageSideLength", typeof(int), typeof(FlightplanDetailControl), new PropertyMetadata(null));



        public FlightplanDetailControlModel ControlModel { get; private set; } 

        public FlightplanDetailControl()
        {
            this.InitializeComponent();
            ThumbnailImageSideLength = 100;
            ControlModel = new FlightplanDetailControlModel(MapControl);
            this.RegisterPropertyChangedCallback(MasterMenuItemProperty, OnMasterMenuItemPropertyChanged);
            this.SizeChanged += FlightplanDetailControl_SizeChanged;
            
        }

        private void FlightplanDetailControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            UpdateThumbnailSize();
        }

        private void OnMasterMenuItemPropertyChanged(DependencyObject sender, DependencyProperty dp)
        {
            if (dp == MasterMenuItemProperty)
            {
                ControlModel.OnMasterItemChanged(MasterMenuItem);
            }
            else
            {
                ControlModel.Item = null;
            }
        }

        private void UpdateThumbnailSize()
        {
            
            if (controlRoot.ActualWidth > 1300)
            {
                ThumbnailImageSideLength = 150;
            }
            else
            {
                ThumbnailImageSideLength = 100;
            }
        }
    }
}
