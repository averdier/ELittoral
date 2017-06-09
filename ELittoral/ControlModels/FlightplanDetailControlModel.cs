using ELittoral.Helpers;
using ELittoral.Models;
using ELittoral.Services.Rest;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Controls.Maps;

namespace ELittoral.ControlModels
{
    public class FlightplanDetailControlModel : Observable
    {
        private FlightplanModel _item;
        public FlightplanModel Item
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

        private RESTFlightplanModelService _modelService;

        private MapControl _map;
        private Geopoint _mapCenter;
        private Double _mapCenterRadius = 100;

        public FlightplanDetailControlModel(MapControl map)
        {
            _modelService = new RESTFlightplanModelService("http://vps361908.ovh.net/dev/elittoral/api/");
            _map = map;
        }

        private void PopulateMapFromItem()
        {
            if (_map != null && Item != null)
            {
                _map.MapElements.Clear();
                var coords = new List<Geopoint>();

                foreach (WaypointModel wpm in Item.Waypoints)
                {
                    MapIcon mapIcon = new MapIcon()
                    {
                        Location = wpm.Parameters.Coord,
                        Image = RandomAccessStreamReference.CreateFromUri(new Uri("ms-appx:///Assets/map.png")),
                        CollisionBehaviorDesired = MapElementCollisionBehavior.RemainVisible
                    };
                    _map.MapElements.Add(mapIcon);
                    coords.Add(wpm.Parameters.Coord);
                }

                _mapCenter = Geographical.GetCentralGeopoint(coords);
            }
        }

        private async void SetMapSceneAsync()
        {
            if (_map != null && _mapCenter != null)
            {
                if (Item.Waypoints != null)
                {
                    await _map.TrySetSceneAsync(MapScene.CreateFromLocationAndRadius(_mapCenter, _mapCenterRadius));
                }
                
            }
        }

        public async void OnMasterItemChanged(FlightplanModel item)
        {
            IsLoading = true;
            LoadingMessage = "Chargement du plan de vol";

            try
            {
                Item = await _modelService.GetFlightplanFromIdAsync(item.Id);
                PopulateMapFromItem();
            }
            catch (TaskCanceledException)
            {
                Debug.WriteLine("Task canceled");
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

            IsLoading = false;
            LoadingMessage = "";

            SetMapSceneAsync();
        }
    }
}
