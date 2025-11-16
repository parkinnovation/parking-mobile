using System;
namespace Parking.Mobile.Interface.Message.Response
{
	public class ValidateEntryPlateResponse
	{
		public Guid IDMessage { get; set; }
		public string VehicleModel { get; set; }
		public string VehicleColor { get; set; }
	}
}

