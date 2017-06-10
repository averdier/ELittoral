using ELittoral.Attributes;
using Prism.Windows.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELittoral.Models
{
    public class BuildOptionsModel : ValidatableBindableBase
    {
        private string _flightplanName;

        [Required(ErrorMessage = "Nom requis"), MinLength(3, ErrorMessage = "Doit comporter au minimum 3 caractères"), MaxLength(64, ErrorMessage = "Doit comporter au maximum 64 caractères")]
        public string FlightPlanName
        {
            get { return _flightplanName; }
            set { SetProperty(ref _flightplanName, value); }
        }

        private string _buildFromLatitude;

        [Required(ErrorMessage = "Latitude requise"), Double(ErrorMessage = "Doit être de type décimal"), DoubleBetween(-180, 180, ErrorMessage = "Doit être compris entre -180 et 180")]
        public string BuildFromLatitude
        {
            get { return _buildFromLatitude; }
            set { SetProperty(ref _buildFromLatitude, value); }
        }

        private string _buildFromLongitude;

        [Required(ErrorMessage = "Longitude requise"), Double(ErrorMessage = "Doit être de type décimal"), DoubleBetween(-180, 180, ErrorMessage = "Doit être compris entre -180 et 180")]
        public string BuildFromLongitude
        {
            get { return _buildFromLongitude; }
            set { SetProperty(ref _buildFromLongitude, value); }
        }

        private string _buildToLatitude;

        [Required(ErrorMessage = "Latitude requise"), Double(ErrorMessage = "Doit être de type décimal"), DoubleBetween(-180, 180, ErrorMessage = "Doit être compris entre -180 et 180")]
        public string BuildToLatitude
        {
            get { return _buildToLatitude; }
            set { SetProperty(ref _buildToLatitude, value); }
        }

        private string _buildToLongitude;

        [Required(ErrorMessage = "Longitude requise"), Double(ErrorMessage = "Doit être de type décimal"), DoubleBetween(-180, 180, ErrorMessage = "Doit être compris entre -180 et 180")]
        public string BuildToLongitude
        {
            get { return _buildToLongitude; }
            set { SetProperty(ref _buildToLongitude, value); }
        }

        private string _gimbalYaw = "0";

        [Required(ErrorMessage = "Yaw requis"), Double(ErrorMessage = "Doit être de type décimal"), DoubleBetween(-180, 180, ErrorMessage = "Doit être compris entre -180 et 180")]
        public string GimbalYaw
        {
            get { return _gimbalYaw; }
            set { SetProperty(ref _gimbalYaw, value); }
        }

        private string _gimbalPitch = "0";

        [Required(ErrorMessage = "Pitch requis"), Double(ErrorMessage = "Doit être de type décimal"), DoubleBetween(-180, 180, ErrorMessage = "Doit être compris entre -180 et 180")]
        public string GimbalPitch
        {
            get { return _gimbalPitch; }
            set { SetProperty(ref _gimbalPitch, value); }
        }

        private string _gimbalRoll = "0";

        [Required(ErrorMessage = "Roll requis"), Double(ErrorMessage = "Doit être de type décimal"), DoubleBetween(-180, 180, ErrorMessage = "Doit être compris entre -180 et 180")]
        public string GimbalRoll
        {
            get { return _gimbalRoll; }
            set { SetProperty(ref _gimbalRoll, value); }
        }

        private string _verticalIncrement;

        [Required(ErrorMessage = "Incrément vertical requis"), Double(ErrorMessage = "Doit être de type décimal"), DoubleUpperZero(ErrorMessage = "Doit être supérieur à 0")]
        public string VerticalIncrement
        {
            get { return _verticalIncrement; }
            set { SetProperty(ref _verticalIncrement, value); }
        }

        private string _horizontalIncrement;

        [Required(ErrorMessage = "Incrément horizontal requis"), Double(ErrorMessage = "Doit être de type décimal"), DoubleUpperZero(ErrorMessage = "Doit être supérieur à 0")]
        public string HorizontalIncrement
        {
            get { return _horizontalIncrement; }
            set { SetProperty(ref _horizontalIncrement, value); }
        }

        private string _startAltitude;

        [Required(ErrorMessage = "Altitude de départ requise"), Double(ErrorMessage = "Doit être de type décimal"), DoubleUpperZero(ErrorMessage = "Doit être supérieur à 0")]
        public string StartAltitude
        {
            get { return _startAltitude; }
            set
            {
                SetProperty(ref _startAltitude, value);
                Errors.ValidateProperty("EndAltitude");
            }
        }

        private string _endAltitude;

        [Required(ErrorMessage = "Altitude finale requise"), Double(ErrorMessage = "Doit être de type décimal"), DoubleUpperZero(ErrorMessage = "Doit être supérieur à 0"), DoubleUpperOrEqualThanProperty("StartAltitude", ErrorMessage = "Doit être supérieur ou égal à l'altitude de départ")]
        public string EndAltitude
        {
            get { return _endAltitude; }
            set { SetProperty(ref _endAltitude, value); }
        }

        private string _rotation = "0";

        [Required(ErrorMessage = "Rotation requise"), Double(ErrorMessage = "Doit être de type décimal"), DoubleBetween(-180, 180, ErrorMessage = "Doit être compris entre -180 et 180")]
        public string Rotation
        {
            get { return _rotation; }
            set { SetProperty(ref _rotation, value); }
        }

        public int ErrorCount {
            get
            {
                ValidateProperties();
                return this.Errors.Errors.Count;
            }
        }
    }
}
