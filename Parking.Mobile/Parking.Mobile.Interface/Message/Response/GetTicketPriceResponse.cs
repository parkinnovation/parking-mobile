using System;
using System.Collections.Generic;

namespace Parking.Mobile.Interface.Message.Response
{
    public class GetTicketPriceResponse
    {
        public Int64 IdParkingLot { get; set; }
        public DateTime DateEntry { get; set; }
        public DateTime DateLimitExit { get; set; }
        public decimal Price { get; set; }
        public decimal Discount { get; set; }
        public decimal Amount { get; set; }
        public bool DisableDiscount { get; set; }
    }

    public class TicketPriceInfo
    {
        public int IDPriceTable { get; set; }
        public int IDPriceTableRule { get; set; }
        public int IDPriceTableRuleValue { get; set; }
        public DateTime QtyHour { get; set; }
        public decimal Value { get; set; }
        public int DayReference { get; set; }
        public DateTime? DateReference { get; set; }
    }
}

