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

        private FlightplanModel _itemFlightplan;
        public FlightplanModel ItemFlightplan
        {
            get { return _itemFlightplan; }
            set { Set(ref _itemFlightplan, value); }
        }

        private ReconModel _minuendRecon;
        public ReconModel MinuendRecon
        {
            get { return _minuendRecon;  }
            set { Set(ref _minuendRecon, value); }
        }

        private ReconModel _subtrahendRecon;
        public ReconModel SubtrahendRecon
        {
            get { return _subtrahendRecon; }
            set { Set(ref _subtrahendRecon, value); }
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
        private RESTFlightplanModelService _flightplanModelService;
        private RESTReconModelService _reconModelService;
        private RESTResourceModelService _resourceModelService;


        public AnalysisDetailControlModel()
        {
            _modelService = new RESTAnalysisModelService("http://vps361908.ovh.net/dev/elittoral/api/");
            _flightplanModelService = new RESTFlightplanModelService("http://vps361908.ovh.net/dev/elittoral/api/");
            _reconModelService = new RESTReconModelService("http://vps361908.ovh.net/dev/elittoral/api/");
            _resourceModelService = new RESTResourceModelService("http://vps361908.ovh.net/dev/elittoral/api/");
        }

        public async void OnMasterItemChanged(AnalysisModel item)
        {

            IsLoading = true;
            LoadingMessage = "Chargement de l'analyse";

            if (item.MinuendRecon != null)
            {
                MinuendRecon = item.MinuendRecon;
            }
            if (item.SubtrahendRecon != null)
            {
                SubtrahendRecon = item.SubtrahendRecon;
            }

            try
            {
                var analysis = await _modelService.GetAnalysisFromIdAsync(item.Id);
                
                if (MinuendRecon != null)
                {
                    Debug.WriteLine(MinuendRecon.FlightplanId);
                    ItemFlightplan = await _flightplanModelService.GetFlightplanFromIdAsync(MinuendRecon.FlightplanId);
                }

                Item = analysis;
                IsLoading = false;
                LoadingMessage = "";
            }
            
            catch (Exception ex)
            {
                var dialog = new Windows.UI.Popups.MessageDialog(
                    ex.Message,
                    "Erreur"
                    );
                dialog.Commands.Add(new Windows.UI.Popups.UICommand("Fermer") { Id = 0 });

                dialog.DefaultCommandIndex = 0;

                var result = await dialog.ShowAsync();
            }
        }
    }
}
