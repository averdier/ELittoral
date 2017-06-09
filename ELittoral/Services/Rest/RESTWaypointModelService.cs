using ELittoral.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;

namespace ELittoral.Services.Rest
{
    public class RESTWaypointModelService
    {
        public static GimbalModel GimbalToGimbalModel(Gimbal gimbal)
        {
            var model = new GimbalModel
            {
                Yaw = gimbal.yaw,
                Pitch = gimbal.pitch,
                Roll = gimbal.roll
            };

            return model;
        }

        public static DroneParametersModel DroneParmetersToDroneParametersModel(DroneParameters parameters)
        {
            var model = new DroneParametersModel
            {
                Gimbal = GimbalToGimbalModel(parameters.gimbal),
                Rotation = parameters.rotation,
                Coord = new Geopoint(new BasicGeoposition
                {
                    Latitude = parameters.coord.lat,
                    Longitude = parameters.coord.lon,
                    Altitude = parameters.coord.alt
                })
            };

            return model;
        }
 
        public static WaypointModel WaypointToWaypointModel(Waypoint waypoint)
        {
            var model = new WaypointModel
            {
                Number = waypoint.number,
                Parameters = DroneParmetersToDroneParametersModel(waypoint.parameters)
            };

            if (waypoint.id != null)
            {
                model.Id = Convert.ToInt32(waypoint.id);
            }

            return model;
        }
    }
}
