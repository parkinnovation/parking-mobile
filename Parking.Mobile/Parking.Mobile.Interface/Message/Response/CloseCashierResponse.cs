using System;
namespace Parking.Mobile.Interface.Message.Response
{
    public class CloseCashierResponse
    {
        public int CashTransactionId { get; set; }
        public DateTime DateOpen { get; set; }
        public DateTime DateClose { get; set; }
        public int IdUser { get; set; }
        public int IdDevice { get; set; }
    }
}

