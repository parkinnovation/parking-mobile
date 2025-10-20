using System;
using System.Collections.Generic;

namespace Parking.Mobile.Interface.Message.Response
{
    public class ProcessPaymentResponse
    {
        public DateTime? DateLimitExit { get; set; }
        public DateTime? DatePayment { get; set; }
        public DateTime? DateExit { get; set; }
        public string RpsNumber { get; set; }
        public NfceInfo Nfce { get; set; }
        public string QrCodeContingency { get; set; }
    }

    public class NfceInfo
    {
        public decimal Amount { get; set; }
        public string CpfCnpj { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string AddressNumber { get; set; }
        public string Complement { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PostalCode { get; set; }
        public string Telephone { get; set; }
        public string Email { get; set; }
        public bool Contingency { get; set; }
        public string RpsSerie { get; set; }
        public string RpsNumber { get; set; }
        public string UrlSefaz { get; set; }
        public string TaxInformation { get; set; }
        public string Message { get; set; }
        public List<NfcePaymentInfo> Payments { get; set; }
        public string Deductions { get; set; }
        public string FederalWithholding { get; set; }
        public DateTime ProcessingDate { get; set; }
        public string Number { get; set; }
        public string VerificationCode { get; set; }
    }

    public class NfcePaymentInfo
    {
        public string Description { get; set; }
        public decimal Amount { get; set; }
    }
}

