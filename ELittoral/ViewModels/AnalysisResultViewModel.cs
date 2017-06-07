using ELittoral.Helpers;
using ELittoral.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELittoral.ViewModels
{
    public class AnalysisResultViewModel : Observable
    {
        private AnalysisResultModel _item;
        public AnalysisResultModel Item
        {
            get { return _item; }
            set { Set(ref _item, value); }
        }
    }
}
