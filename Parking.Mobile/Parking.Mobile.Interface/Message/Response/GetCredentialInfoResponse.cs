using System;
namespace Parking.Mobile.Interface.Message.Response
{
    public class GetCredentialInfoResponse
    {
        public int IDClient { get; set; }
        public string Name { get; set; }
        public string Credential { get; set; }
        public bool CredentialActive { get; set; }
        public bool ClientActive { get; set; }
        public DateTime DateStart { get; set; }
        public DateTime DateEnd { get; set; }
        public bool RentalPartner { get; set; }
        public bool Exceeded { get; set; }
        public bool Booking { get; set; }
    }
}

