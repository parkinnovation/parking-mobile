using System;
namespace Parking.Mobile.Interface.Message.Response
{
    public class ProcessEntryResponse
    {
        public string Ticket { get; set; }
        public DateTime DateEntry { get; set; }
        public bool PaymentInEntry { get; set; }
        public string QrCodeContingency { get; set; }
    }
}

