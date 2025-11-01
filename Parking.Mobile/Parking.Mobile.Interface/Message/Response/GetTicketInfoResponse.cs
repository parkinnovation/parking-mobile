using System;
using System.Collections.Generic;

namespace Parking.Mobile.Interface.Message.Response
{
    public class GetTicketInfoResponse
    {
        public string Plate { get; set; }
        public string VehicleModel { get; set; }
        public string VehicleColor { get; set; }
        public string Prism { get; set; }
        public Int64 IDParkingLot { get; set; }
        public string Ticket { get; set; }
        public DateTime DateEntry { get; set; }
        public string Credential { get; set; }
        public string CredentialName { get; set; }
        public string Stay
        {
            get
            {
                TimeSpan permanencia = DateTime.Now - DateEntry;

                return permanencia.ToString(@"dd\.hh\:mm\:ss");
            }
        }
        public bool InValet { get; set; }
        public List<TicketPaymentInfo> Payments { get; set; }
        public int IDDeviceEntry { get; set; }
    }

    public class TicketPaymentInfo
    {
        public Int64 IDPayment { get; set; }
        public int? IDPaymentMethod { get; set; }
        public Guid? IDDiscount { get; set; }
        public int? IDParkingSeal { get; set; }
        public decimal? Amount { get; set; }
        public string Description { get; set; }
    }
}

