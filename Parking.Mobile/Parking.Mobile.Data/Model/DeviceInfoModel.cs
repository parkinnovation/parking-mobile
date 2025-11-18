using System;
namespace Parking.Mobile.Data.Model
{
	public class DeviceInfoModel
	{
        public int IDDevice { get; set; }
        public string Description { get; set; }
        public bool PaymentInEntry { get; set; }
        public string TEFId { get; set; }
        public string TEFMobileId { get; set; }
        public int Type { get; set; }
        public string TypeDescription { get; set; }
        public int? RPSSeries { get; set; }
        public bool ModelRequired { get; set; }
        public bool ColorRequired { get; set; }
        public bool PrismRequired { get; set; }
        public string TicketHeader1 { get; set; }
        public string TicketHeader2 { get; set; }
        public string TicketHeader3 { get; set; }
        public string TicketFooter1 { get; set; }
        public string TicketFooter2 { get; set; }
        public string TicketFooter3 { get; set; }
        public string ReceiptFooter1 { get; set; }
        public string ReceiptFooter2 { get; set; }
        public string ReceiptFooter3 { get; set; }
        public bool MobileEntry { get; set; }
        public bool MobileExit { get; set; }
        public bool MobilePayment { get; set; }
        public bool MobileChangeSector { get; set; }
        public bool MobileSecondCopy { get; set; }

    }
}

