using System;
namespace Parking.Mobile.Data.Model
{
    public class CashierInfoModel
    {
        public int CashTransactionId { get; set; }
        public DateTime DateOpen { get; set; }
        public string DateOpenText
        {
            get
            {
                return this.DateOpen.ToString("dd/MM/yyyy HH:mm");
            }
        }
        public Guid IDCashier { get; set; }
    }
}

