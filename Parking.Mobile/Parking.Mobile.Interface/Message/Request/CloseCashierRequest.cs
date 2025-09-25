using System;
namespace Parking.Mobile.Interface.Message.Request
{
    public class CloseCashierRequest : RequestDefault
    {
        public int CashTransactionId { get; set; }
        public int IdUser { get; set; }
        public decimal Amount { get; set; }
    }
}

