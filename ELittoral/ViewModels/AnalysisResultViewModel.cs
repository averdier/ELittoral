using ELittoral.Helpers;
using ELittoral.Models;
using ELittoral.Services;
using Microsoft.Toolkit.Uwp.UI.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Xaml.Input;

namespace ELittoral.ViewModels
{
    public class AnalysisResultViewModel : Observable
    {
        private const string ResultImageName = "ResultImage";
        private const string MinuendImageName = "MinuendImage";
        private const string SubtrahendImageName = "SubtrahendImage";

        private AnalysisResultModel _item;
        public AnalysisResultModel Item
        {
            get { return _item; }
            set { Set(ref _item, value); }
        }

        public ObservableCollection<ImageModel> Images { get; private set; } = new ObservableCollection<ImageModel>();

        private ImageModel _selected;
        public ImageModel Selected
        {
            get { return _selected; }
            set { Set(ref _selected, value); }
        }

        public string TitlePage
        {
            get
            {
                return "Analysis result #" + Item.Id;
            }
        }

        public string Result
        {
            get
            {
                return Item.Result + "%";
            }
        }
        

        public AnalysisResultViewModel()
        {

        }

        public void LoadData(AnalysisResultModel item)
        {
            Item = item;
            Images.Add(Item);
            Images.Add(Item.MinuendRessource);
            Images.Add(Item.SubtrahendRessource);
        }

        public void OnImageTapped(ImageModel model)
        {
            NavigationService.Navigate<Views.ImagePage>(model);
        }
    }
}
