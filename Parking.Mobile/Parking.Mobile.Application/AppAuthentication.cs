using System;
using Parking.Mobile.Interface.Interfaces;
using Parking.Mobile.Interface.Message.Request;
using Parking.Mobile.Interface.Message.Response;

namespace Parking.Mobile.ApplicationCore
{
    public class AppAuthentication : IAuthenticationController
    {
        public ServiceResponseDefault<AuthenticateUserMobileResponse> AuthenticateUserMobile(AuthenticateUserMobileRequest request)
        {
            return new ServiceResponseDefault<AuthenticateUserMobileResponse>()
            {
                Success = true,
                Data = new AuthenticateUserMobileResponse()
                {
                    ChangePasswordNextLogin = false,
                    Disabled = false,
                    IdUser = 1,
                    Name = request.User,
                    Profile = "ADMIN"
                }
            };
        }
    }
}

