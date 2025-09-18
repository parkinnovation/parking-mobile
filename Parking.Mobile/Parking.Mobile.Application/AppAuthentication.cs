using System;
using Parking.Mobile.Interface.Interfaces;
using Parking.Mobile.Interface.Message.Request;
using Parking.Mobile.Interface.Message.Response;

namespace Parking.Mobile.ApplicationCore
{
    public class AppAuthentication : IAuthenticationController
    {
        public ResponseDefault<AuthenticateUserResponse> AuthenticateUser(AuthenticateUserRequest request)
        {
            return new ResponseDefault<AuthenticateUserResponse>()
            {
                Success = true,
                Data = new AuthenticateUserResponse()
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

