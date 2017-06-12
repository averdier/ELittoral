using ELittoral.Helpers;
using ELittoral.Models;
using ELittoral.Services.Rest;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELittoral.ControlModels
{
    public class AnalysisDetailControlModel : Observable
    {
        private AnalysisModel _item;
        public AnalysisModel Item
        {
            get { return _item; }
            set { Set(ref _item, value); }
        }

        private bool _isLoading;
        public bool IsLoading
        {
            get { return _isLoading; }
            set { Set(ref _isLoading, value); }
        }

        private string _loadingMessage;
        public string LoadingMessage
        {
            get { return _loadingMessage; }
            set { Set(ref _loadingMessage, value); }
        }

        private RESTAnalysisModelService _modelService;


        public AnalysisDetailControlModel()
        {
            _modelService = new RESTAnalysisModelService("http://vps361908.ovh.net/dev/elittoral/api/");
        }

        public async void OnMasterItemChanged(AnalysisModel item)
        {
            IsLoading = true;
            LoadingMessage = "Chargement de l'analyse";

            try
            {
                Item = await _modelService.GetAnalysisFromIdAsync(item.Id);

                IsLoading = false;
                LoadingMessage = "";
            }
            catch (TaskCanceledException)
            {
                Debug.WriteLine("Task canceled");
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }
    }
}
