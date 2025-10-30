using System;
namespace Parking.Mobile.Interface.Message.Request
{
    public class OpenCashierRequest : RequestDefault
    {
        public int IdUser { get; set; }
        public int IdDevice { get; set; }
        public decimal Amount { get; set; }
        public string UserName { get; set; }
    }
}

