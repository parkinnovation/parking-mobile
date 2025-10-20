using System;
namespace Parking.Mobile.Interface.Message.Request
{
	public class ChangeSectorRequest : RequestDefault
    {
		public string TicketNumber { get; set; }
		public string Plate { get; set; }
		public int IDDevice { get; set; }
	}
}

