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
    public sealed partial class UploadingControl : UserControl
    {
        public int ProgressValue
        {
            get { return (int)GetValue(ProgressValueProperty); }
            set { SetValue(ProgressValueProperty, value); }
        }
        public static DependencyProperty ProgressValueProperty = DependencyProperty.Register("ProgressValue", typeof(int), typeof(UploadingControl), new PropertyMetadata(null));

        public int ProgressMaximum
        {
            get { return (int)GetValue(ProgressMaximumProperty); }
            set { SetValue(ProgressMaximumProperty, value); }
        }
        public static DependencyProperty ProgressMaximumProperty = DependencyProperty.Register("ProgressMaximum", typeof(int), typeof(UploadingControl), new PropertyMetadata(null));

        public string Message
        {
            get { return GetValue(MessageProperty) as string; }
            set { SetValue(MessageProperty, value); }
        }

        public static DependencyProperty MessageProperty = DependencyProperty.Register("Message", typeof(string), typeof(UploadingControl), new PropertyMetadata(null));

        public UploadingControl()
        {
            this.InitializeComponent();
        }
    }
}
