using System;
using Parking.Mobile.Interface.Message;
using Parking.Mobile.Interface.Message.Request;
using Parking.Mobile.Interface.Message.Response;

namespace Parking.Mobile.Interface.Interfaces
{
    public interface IAuthenticationController
    {
        ServiceResponseDefault<AuthenticateUserMobileResponse> AuthenticateUserMobile(AuthenticateUserMobileRequest request);
    }
}

