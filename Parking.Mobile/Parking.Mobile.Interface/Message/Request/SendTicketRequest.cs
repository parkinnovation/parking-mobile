using System;
namespace Parking.Mobile.Interface.Message.Request
{
	public class SendTicketRequest : RequestDefault
	{
		public string TicketNumber { get; set; }
		public DateTime DateEntry { get; set; }
		public string Plate { get; set; }
		public string PhoneNumber { get; set; }
	}
}

