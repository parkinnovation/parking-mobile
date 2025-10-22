using System;
namespace Parking.Mobile.Interface.Message.Request
{
    public class AuthenticateUserMobileRequest : RequestDefault
    {
        public string User { get; set; }
        public string Password { get; set; }
    }
}

