using System;
namespace Parking.Mobile.Interface.Message.Request
{
    public class CloseCashierRequest : RequestDefault
    {
        public Guid IDCashier { get; set; }
        public decimal Amount { get; set; }
    }
}

