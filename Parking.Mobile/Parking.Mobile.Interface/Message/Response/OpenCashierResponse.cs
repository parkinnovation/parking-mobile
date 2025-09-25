using System;
namespace Parking.Mobile.Interface.Message.Response
{
    public class OpenCashierResponse
    {
        public int CashTransactionId { get; set; }
        public DateTime DateOpen { get; set; }
        public int IdUser { get; set; }
        public int IdDevice { get; set; }
        public string IDGlobalCashTransaction { get; set; }
    }
}

