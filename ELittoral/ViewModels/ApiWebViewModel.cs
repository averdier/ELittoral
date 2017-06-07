using ELittoral.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELittoral.ViewModels
{
    public class ApiWebViewModel : Observable
    {
        private const string defaultUrl = "http://vps361908.ovh.net/dev/elittoral/api/";

        private Uri _source;
        public Uri Source
        {
            get { return _source; }
            set { Set(ref _source, value); }
        }

        public ApiWebViewModel()
        {
            Source = new Uri(defaultUrl);
        }
    }
}
