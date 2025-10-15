using System;
namespace Parking.Mobile.Data.Model
{
	public class ParkingInfoModel
	{
        public string ParkingCode { get; set; }
        public string Description { get; set; }
        public string Address { get; set; }
        public string Cnpj { get; set; }
        public DateTime ToleranceExit { get; set; }
        public DateTime TolerancePayment { get; set; }
        public DateTime TolerancePeriod { get; set; }
        public bool AllowEntryWithoutPlate { get; set; }
    }
}

