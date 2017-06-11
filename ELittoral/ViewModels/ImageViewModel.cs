using ELittoral.Helpers;
using ELittoral.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELittoral.ViewModels
{
    public class ImageViewModel : Observable
    {
        private ImageModel _item;
        public ImageModel Item
        {
            get { return _item; }
            set { Set(ref _item, value); }
        }

        private double _pageWidth;
        public double PageWidth
        {
            get { return _pageWidth; }
            set { Set(ref _pageWidth, value); }
        }

        private double _pageHeight;
        public double PageHeight
        {
            get { return _pageHeight; }
            set { Set(ref _pageHeight, value); }
        }
    }
}
