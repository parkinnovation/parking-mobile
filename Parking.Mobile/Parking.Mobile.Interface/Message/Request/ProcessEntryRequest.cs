using System;
namespace Parking.Mobile.Interface.Message.Request
{
    public class ProcessEntryRequest : RequestDefault
    {
        public string Plate { get; set; }
        public string VehicleModel { get; set; }
        public string Color { get; set; }
        public string Prism { get; set; }
        public string Credential { get; set; }
        public int IDDevice { get; set; }
        public int IDUser { get; set; }
    }
}

