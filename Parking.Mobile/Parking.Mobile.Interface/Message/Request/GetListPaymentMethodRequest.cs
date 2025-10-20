using System;
namespace Parking.Mobile.Interface.Message.Request
{
    public class GetListPaymentMethodRequest : RequestDefault
    {
        public bool IsTagPorto { get; set; }
        public string Plate { get; set; }
    }
}

