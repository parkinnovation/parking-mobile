using System;
using System.Collections.Generic;
using System.Linq;

namespace Parking.Mobile.Data.Model
{
    public class TicketInfoModel
    {
        private string stay;
        private decimal paymentValue;

        public CashierInfoModel CashierInfo { get; set; }



        public decimal PaymentValue
        {
            get
            {
                return this.Price - DiscountValue;
            }
        }

        public string PaymentValueText
        {
            get
            {
                return "R$ " + this.PaymentValue.ToString("F2");
            }
        }

        public string Plate { get; set; }
        public string VehicleModel { get; set; }
        public string VehicleColor { get; set; }
        public string Prism { get; set; }
        public Int64 IDParkingLot { get; set; }
        public string Ticket { get; set; }
        public DateTime DateEntry { get; set; }
        public string Credential { get; set; }
        public string CredentialName { get; set; }
        public string Cpf { get; set; }
        public string Cnpj { get; set; }
        public string Phone { get; set; }
        public string Name { get; set; }
        public string Renavan { get; set; }
        public TefReceiptInfo TefReceipt { get; set; }
        public bool IsTagPorto { get; set; }
        public bool PaymentInEntry { get; set; }
        public bool RequestVehicle { get; set; }
        public string Stay
        {
            get
            {
                if (!this.DatePriceScheduller.HasValue)
                {
                    return this.stay;
                }
                else
                {
                    TimeSpan permanency = this.DatePriceScheduller.Value - this.DateEntry;

                    return permanency.ToString(@"dd\.hh\:mm\:ss");
                }
            }

            set
            {
                this.stay = value;
            }
        }

        public DateTime? DateLimitExit { get; set; }
        public decimal Price { get; set; }
        public bool DiscountAuthorized { get; set; }
        public decimal Discount { get; set; }
        public string DiscountHash { get; set; }
        public DateTime? DatePriceScheduller { get; set; }
        public DateTime? DateBillingLimit { get; set; }

        public string DateEntryText
        {
            get
            {
                return DateEntry.ToString("dd/MM/yyyy HH:mm");
            }
        }

        public string PriceText
        {
            get
            {
                return "R$ " + this.Price.ToString("F2");
            }
        }

        public decimal DiscountValue
        {
            get
            {
                return Discount;
            }
        }

        public string DiscountValueText
        {
            get
            {
                return "R$ -" + this.DiscountValue.ToString("F2");
            }
        }

        public decimal NetPrice
        {
            get
            {
                decimal paymentAmount = 0;

                if (this.Payments != null)
                {
                    paymentAmount = (from l in Payments select l.Amount.Value).Sum();
                }

                return this.Price - paymentAmount;
            }
        }

        public string NetPriceText
        {
            get
            {
                return "R$ " + this.NetPrice.ToString("F2");
            }
        }

        public bool EnableChangePlate
        {
            get
            {
                return !String.IsNullOrEmpty(this.Plate);
            }
        }

        public bool DisabledChangePlate
        {
            get
            {
                return String.IsNullOrEmpty(this.Plate);
            }
        }

        public List<TicketPaymentItemInfo> Payments { get; set; }
        public List<TicketPaymentItemInfo> PaymentsOriginal { get; set; }
        public List<TicketHistoryPriceInfo> PriceHistory { get; set; }
    }

    public class TicketPaymentItemInfo
    {
        public Int64 IDPayment { get; set; }
        public int? IDPaymentMethod { get; set; }
        public Guid? IDDiscount { get; set; }
        public int? IDParkingSeal { get; set; }
        public decimal? Amount { get; set; }
        public string Description { get; set; }
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

        public string DescriptionFormated
        {
            get
            {
                return this.Description + ": - " + this.Amount.Value.ToString("F2");
            }
        }
    }

    public class TicketHistoryPriceInfo
    {
        public int IDPriceTable { get; set; }
        public int IDPriceTableRule { get; set; }
        public int IDPriceTableRuleValue { get; set; }
        public DateTime QtyHour { get; set; }
        public decimal Value { get; set; }
        public int DayReference { get; set; }
        public DateTime? DateReference { get; set; }
    }

    public class TefReceiptInfo
    {
        public string DateTransaction { get; set; }
        public string Authorization { get; set; }
        public string Brand { get; set; }
        public decimal Amount { get; set; }
        public string AID { get; set; }
        public string PaymentMethod { get; set; }
        public string PaymentType { get; set; }
        public string CardNumber { get; set; }
        public string MID { get; set; }
        public string Reference { get; set; }
    }
}

