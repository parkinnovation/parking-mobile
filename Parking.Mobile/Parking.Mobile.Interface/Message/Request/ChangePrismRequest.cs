using System;
namespace Parking.Mobile.Interface.Message.Request
{
    public class ChangePrismRequest : RequestDefault
    {
        public string TicketNumber { get; set; }
        public string Prism { get; set; }
        public Guid IDMessage { get; set; }
    }
}

