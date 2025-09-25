using System;
namespace Parking.Mobile.Data.Model
{
	public class DeviceInfoModel
	{
        public int IDDevice { get; set; }
        public string Description { get; set; }
        public bool PaymentInEntry { get; set; }
        public string TEFId { get; set; }
        public string TEFMobileId { get; set; }
        public int Type { get; set; }
        public string TypeDescription { get; set; }
        public int? RPSSeries { get; set; }
    }
}

