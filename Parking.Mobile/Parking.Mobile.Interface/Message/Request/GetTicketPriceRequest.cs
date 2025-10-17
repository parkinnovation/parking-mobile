using System;
namespace Parking.Mobile.Interface.Message.Request
{
    public class GetTicketPriceRequest : RequestDefault
    {
        public string AccessCode { get; set; }
        public Int64 IdParkingLot { get; set; }
        public int IdPriceTable { get; set; }
        public int IdDevice { get; set; }
        public DateTime? DatePriceScheduller { get; set; }
        public DateTime? DateBillingLimit { get; set; }
        public int IdDiscount { get; set; }
    }
}

