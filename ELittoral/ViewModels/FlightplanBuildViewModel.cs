using ELittoral.Helpers;
using ELittoral.Models;
using ELittoral.Services;
using ELittoral.Services.Rest;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.Devices.Geolocation;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Maps;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Input;

namespace ELittoral.ViewModels
{
    public class FlightplanBuildViewModel : Observable
    {
        public ICommand AddClickCommand { get; private set; }

        public ICommand CancelClickCommand { get; private set; }

        public ICommand MapRightTappedCommand { get; private set; }

        public ICommand MapMenuFlyoutBuildToCommand { get; private set; }

        public ICommand MapMenuFlyoutBuildFromCommand { get; private set; }

        private MapControl _map;
        private MenuFlyout _mapFlyout;
        private Geopoint _mapCenter;
        private int _zoomLevel = 5;

        public BuildOptionsModel BuildOptions { get; } = new BuildOptionsModel();

        private RESTFlightplanModelService _modelService;

        private Geopoint _lastSelectedPosition;
        private MapIcon _buildToIcon;
        private MapIcon _buildFromIcon;
        private MapPolyline _buildPath;

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

        public FlightplanBuildViewModel(MapControl map, MenuFlyout mapMenuFlyout)
        {
            AddClickCommand = new RelayCommand<RoutedEventArgs>(OnAddClick);
            CancelClickCommand = new RelayCommand<RoutedEventArgs>(OnCancelClick);
            MapRightTappedCommand = new RelayCommand<MapRightTappedEventArgs>(OnMapRightTapped);
            MapMenuFlyoutBuildFromCommand = new RelayCommand<RoutedEventArgs>(OnMapMenuFlyoutBuildFromClick);
            MapMenuFlyoutBuildToCommand = new RelayCommand<RoutedEventArgs>(OnMapMenuFlyoutBuildToClick);

            _map = map;
            _mapFlyout = mapMenuFlyout;
            _mapCenter = new Geopoint(new BasicGeoposition
            {
                Latitude = 46.52863469527167,
                Longitude = 2.43896484375
            });

            _buildFromIcon = new MapIcon
            {
                Visible = false
            };
            _buildToIcon = new MapIcon
            {
                Visible = false
            };

            _buildPath = new MapPolyline
            {
                ZIndex = 0,
                StrokeColor = Colors.Black,
                StrokeThickness = 1
            };
            _modelService = new RESTFlightplanModelService("http://vps361908.ovh.net/dev/elittoral/api/");
        }

        public void OnNavigatedToPage()
        {
            if (_map != null)
            {
                _map.Center = _mapCenter;
                _map.ZoomLevel = _zoomLevel;

                _map.MapElements.Add(_buildToIcon);
                _map.MapElements.Add(_buildFromIcon);
            }
        }

        public void OnMapRightTapped(MapRightTappedEventArgs args)
        {
            if (args != null)
            {
                _mapFlyout.ShowAt(_map, new Point(args.Position.X, args.Position.Y));
                _lastSelectedPosition = args.Location;
            }
        }

        private bool CanDrawPath()
        {
            return BuildOptions.BuildToLatitude != null &&
                BuildOptions.BuildToLongitude != null &&
                BuildOptions.BuildFromLatitude != null &&
                BuildOptions.BuildFromLongitude != null;
        }

        private async void FocusOnBuildToAsync()
        {
            if (BuildOptions.BuildToLongitude != null && BuildOptions.BuildToLatitude != null)
            {
                double lat, lon = 0;
                double.TryParse(BuildOptions.BuildToLatitude, NumberStyles.Float, CultureInfo.InvariantCulture, out lat);
                double.TryParse(BuildOptions.BuildToLongitude, NumberStyles.Float, CultureInfo.InvariantCulture, out lon);

                var buildToPosition = new BasicGeoposition
                {
                    Latitude = lat,
                    Longitude = lon
                };

                _buildToIcon.Location = new Geopoint(buildToPosition);

                await _map.TrySetSceneAsync(MapScene.CreateFromLocationAndRadius(new Geopoint(new BasicGeoposition
                {
                    Latitude = lat,
                    Longitude = lon,
                    Altitude = 1
                }), 200));

                _buildToIcon.Visible = true;
            }
        }

        private async void FocusOnBuildFromAsync()
        {
            if (BuildOptions.BuildFromLongitude != null && BuildOptions.BuildFromLatitude != null)
            {
                double lat, lon = 0;
                double.TryParse(BuildOptions.BuildFromLatitude, NumberStyles.Float, CultureInfo.InvariantCulture, out lat);
                double.TryParse(BuildOptions.BuildFromLongitude, NumberStyles.Float, CultureInfo.InvariantCulture, out lon);

                var buildFromPosition = new BasicGeoposition
                {
                    Latitude = lat,
                    Longitude = lon
                };

                _buildFromIcon.Location = new Geopoint(buildFromPosition);

                await _map.TrySetSceneAsync(MapScene.CreateFromLocationAndRadius(new Geopoint(new BasicGeoposition
                {
                    Latitude = lat,
                    Longitude = lon,
                    Altitude = 1
                }), 200));

                _buildFromIcon.Visible = true;
            }
        }

        private async void RefreshMapControlAsync()
        {
            if (CanDrawPath())
            {
                double lat, lon = 0;

                double.TryParse(BuildOptions.BuildToLatitude, NumberStyles.Float, CultureInfo.InvariantCulture, out lat);
                double.TryParse(BuildOptions.BuildToLongitude, NumberStyles.Float, CultureInfo.InvariantCulture, out lon);
                var buildToPosition = new BasicGeoposition
                {
                    Latitude = lat,
                    Longitude = lon
                };

                lat = lon = 0;
                double.TryParse(BuildOptions.BuildFromLatitude, NumberStyles.Float, CultureInfo.InvariantCulture, out lat);
                double.TryParse(BuildOptions.BuildFromLongitude, NumberStyles.Float, CultureInfo.InvariantCulture, out lon);
                var buildFromPosition = new BasicGeoposition
                {
                    Latitude = lat,
                    Longitude = lon
                };

                _buildToIcon.Location = new Geopoint(buildToPosition);
                _buildFromIcon.Location = new Geopoint(buildFromPosition);

                _buildToIcon.Visible = true;
                _buildFromIcon.Visible = true;

                _buildPath.Path = new Geopath(new List<BasicGeoposition>() {
                    buildToPosition,
                    buildFromPosition
                });

                if (!_map.MapElements.Contains(_buildPath))
                {
                    _map.MapElements.Add(_buildPath);
                }

                var upperLeft = new BasicGeoposition();
                upperLeft.Latitude = Math.Max(buildToPosition.Latitude, buildFromPosition.Latitude) + 0.0005;
                upperLeft.Longitude = Math.Min(buildToPosition.Longitude, buildFromPosition.Longitude) - 0.0005;

                var lowerRight = new BasicGeoposition();
                lowerRight.Latitude = Math.Min(buildToPosition.Latitude, buildFromPosition.Latitude) - 0.0005;
                lowerRight.Longitude = Math.Max(buildToPosition.Longitude, buildFromPosition.Longitude) + 0.0005;

                var geoboundingBox = new GeoboundingBox(upperLeft, lowerRight);

                await _map.TrySetSceneAsync(MapScene.CreateFromBoundingBox(geoboundingBox));
            }
            else
            {
                if (_map.MapElements.Contains(_buildPath))
                {
                    _map.MapElements.Remove(_buildPath);
                }

                if (BuildOptions.BuildFromLongitude != null && BuildOptions.BuildFromLatitude != null)
                {
                    FocusOnBuildFromAsync();
                }
                else
                {
                    _buildFromIcon.Visible = false;
                }

                if (BuildOptions.BuildToLongitude != null && BuildOptions.BuildToLatitude != null)
                {
                    FocusOnBuildToAsync();
                }
                else
                {
                    _buildToIcon.Visible = false;
                }

            }
        }

        private void OnMapMenuFlyoutBuildToClick(RoutedEventArgs args)
        {
            if (_lastSelectedPosition != null)
            {
                _buildFromIcon.Location = _lastSelectedPosition;

                var basicPos = _lastSelectedPosition.Position;
                BuildOptions.BuildToLatitude = basicPos.Latitude.ToString(CultureInfo.InvariantCulture);
                BuildOptions.BuildToLongitude = basicPos.Longitude.ToString(CultureInfo.InvariantCulture);

                RefreshMapControlAsync();
            }
        }

        private void OnMapMenuFlyoutBuildFromClick(RoutedEventArgs args)
        {
            if (_lastSelectedPosition != null)
            {
                _buildToIcon.Location = _lastSelectedPosition;

                var basicPos = _lastSelectedPosition.Position;
                BuildOptions.BuildFromLatitude = basicPos.Latitude.ToString(CultureInfo.InvariantCulture);
                BuildOptions.BuildFromLongitude = basicPos.Longitude.ToString(CultureInfo.InvariantCulture);

                RefreshMapControlAsync();
            }
        }

        private async void OnAddClick(RoutedEventArgs args)
        {
            var errorCount = BuildOptions.ErrorCount;
            if (errorCount == 0)
            {
                try
                {
                    IsLoading = true;
                    LoadingMessage = "Génération du plan de vol";

                    var result = await _modelService.BuildFlightplan(BuildOptions);

                    IsLoading = false;
                    LoadingMessage = "";

                    if (result != null)
                    {
                        NavigationService.Navigate<Views.FlightplansPage>(result);
                    }
                    else
                    {
                        var dialog = new Windows.UI.Popups.MessageDialog(
                            "Une erreur est survenue",
                            "Erreur");
                        dialog.Commands.Add(new Windows.UI.Popups.UICommand("Fermer") { Id = 0 });

                        dialog.DefaultCommandIndex = 0;

                        var resultUnknow = await dialog.ShowAsync();
                    }

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

        private void OnCancelClick(RoutedEventArgs args)
        {
            if (NavigationService.CanGoBack) { NavigationService.GoBack(); }
        }
    }
}
