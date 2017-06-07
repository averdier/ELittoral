using ELittoral.Models;
using ELittoral.Services;
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
    public sealed partial class ContentImageControl : UserControl
    {
        public ImageModel ImageItem
        {
            get { return GetValue(ImageItemProperty) as ImageModel; }
            set { SetValue(ImageItemProperty, value); }
        }

        public static DependencyProperty ImageItemProperty = DependencyProperty.Register("ImageItem", typeof(ImageModel), typeof(ThumbnailImageControl), new PropertyMetadata(null));

        public ContentImageControl()
        {
            this.InitializeComponent();
        }

        private void Control_Tapped(object sender, TappedRoutedEventArgs e)
        {
            NavigationService.Navigate<Views.ImagePage>(ImageItem);
        }
    }
}
