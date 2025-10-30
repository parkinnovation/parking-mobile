using System;
using Parking.Mobile.Interface.Message.Response;
using System.Collections.Generic;

namespace Parking.Mobile.Interface.Message.Request
{
    public class ProcessPaymentRequest : RequestDefault
    {
        public string TicketNumber { get; set; }
        public DateTime DatePayment { get; set; }
        public DateTime? DateBillingLimit { get; set; }
        public int IDDevice { get; set; }
        public int IDUser { get; set; }
        public Guid IDPriceTable { get; set; }
        public int IDCashTransaction { get; set; }
        public string CPF { get; set; }
        public string CNPJ { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PostalCode { get; set; }
        public DateTime? DateExit { get; set; }
        public List<PaymentItemInfo> Payments { get; set; }
        public List<TicketPriceInfo> TicketPriceHistory { get; set; }
    }

    public class PaymentItemInfo
    {
        public Int64 IDPayment { get; set; }
        public int? IDPaymentMethod { get; set; }
        public Guid? IDDiscount { get; set; }
        public int? IDParkingSeal { get; set; }
        public decimal? Amount { get; set; }
        public string Nsu { get; set; }
        public string NsuHost { get; set; }
        public string Brand { get; set; }
        public string AuthorizationCode { get; set; }
        public string CardNumberTruncated { get; set; }
        public string SealCode { get; set; }
        public int? SealNumber { get; set; }
        public string SealCodeRead { get; set; }
        public decimal? SealValue { get; set; }
        public decimal? PriceTableValue { get; set; }
        public string SealTypeAccess { get; set; }
        public int? IDBrand { get; set; }
    }
}

