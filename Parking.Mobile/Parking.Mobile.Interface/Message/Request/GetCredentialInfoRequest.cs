using System;
namespace Parking.Mobile.Interface.Message.Request
{
    public class GetCredentialInfoRequest : RequestDefault
    {
        public string AccessCode { get; set; }
        public string Plate { get; set; }
        public int IDDevice { get; set; }
    }
}

