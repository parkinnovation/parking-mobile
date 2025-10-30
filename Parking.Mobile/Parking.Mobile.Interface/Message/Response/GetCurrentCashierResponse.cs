using System;
namespace Parking.Mobile.Interface.Message.Response
{
    public class GetCurrentCashierResponse
    {
        public int CashTransactionId { get; set; }
        public DateTime DateOpen { get; set; }
        public int IdUser { get; set; }
        public int IdDevice { get; set; }
        public Guid IDCashier { get; set; }
    }
}

