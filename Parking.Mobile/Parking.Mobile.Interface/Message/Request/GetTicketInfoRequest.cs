using System;
namespace Parking.Mobile.Interface.Message.Request
{
    public class GetTicketInfoRequest : RequestDefault
    {
        public string AccessCode { get; set; }
        public int IDDevice { get; set; }
        public int IDUser { get; set; }
    }
}

