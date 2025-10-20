
using System;
namespace Parking.Mobile.Interface.Message.Response
{
    public class ValidateSealResponse
    {
        public decimal ValuePayment { get; set; }
        public decimal ValueSeal { get; set; }
        public int IDParkingCodeSeal { get; set; }
        public int SealNumber { get; set; }
        public decimal ValuePriceTable { get; set; }
        public string SealType { get; set; }
        public string SealAccessType { get; set; }
    }
}

