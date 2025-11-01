using System;
using Parking.Mobile.Interface.Message.Response;
using System.Collections.Generic;

namespace Parking.Mobile.Interface.Message.Request
{
    public class ProcessPaymentRequest : RequestDefault
    {
        public string TicketNumber { get; set; }
        public DateTime DatePayment { get; set; }
        public DateTime DateLimit { get; set; }
        public DateTime DateEntry { get; set; }
        public int IDDevice { get; set; }
        public int IDDeviceEntry { get; set; }
        public int IDUser { get; set; }
        public int IDCashTransaction { get; set; }
        public DateTime? DateExit { get; set; }
        public List<PaymentItemInfo> Payments { get; set; }
        public string PriceTableName { get; set; }
        public string UserName { get; set; }
        public Guid IDMessage { get; set; }
        public string Plate { get; set; }
        public bool ExitVehicle { get; set; }
        public string Credential { get; set; }
    }

    public class PaymentItemInfo
    {
        public int IDPaymentMethod { get; set; }
        public int PaymentMethodType { get; set; }
        public string PaymentMethod { get; set; }
        public double Discount { get; set; }
        public double Amount { get; set; }
    }
}

