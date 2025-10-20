using System;
using System.Collections.Generic;

namespace Parking.Mobile.Interface.Message.Request
{
    public class ValidateSealRequest : RequestDefault
    {
        public string SealBarcode { get; set; }
        public Int64 IDParkingLot { get; set; }
        public decimal Balance { get; set; }
        public decimal Discount { get; set; }

        public List<ParkingLotPriceInfo> ParkingLotPrices { get; set; }
    }

    public class ParkingLotPriceInfo
    {
        public int IDParkingLotPrice { get; set; }
        public Int64 IDParkingLot { get; set; }
        public int IDPriceTable { get; set; }
        public int IDPriceTableRule { get; set; }
        public int IDPriceTableRuleValue { get; set; }
        public DateTime Time { get; set; }
        public decimal Value { get; set; }
        public int DayReference { get; set; }
    }
}

