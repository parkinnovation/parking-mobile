using System;
namespace Parking.Mobile.Interface.Message.Request
{
    public class GetListPriceTableRequest : RequestDefault
    {
        public Int64 IdParkingLot { get; set; }
        public int IdDevice { get; set; }
        public string Code { get; set; }
    }
}

