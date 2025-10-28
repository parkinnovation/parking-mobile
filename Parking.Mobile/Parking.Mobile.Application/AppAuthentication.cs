using System;
using Parking.Mobile.Common;
using Parking.Mobile.Interface.Interfaces;
using Parking.Mobile.Interface.Message.Request;
using Parking.Mobile.Interface.Message.Response;

namespace Parking.Mobile.ApplicationCore
{
    public class AppAuthentication : IAuthenticationController
    {
        public ServiceResponseDefault<AuthenticateUserMobileResponse> AuthenticateUserMobile(AuthenticateUserMobileRequest request)
        {
            try
            {
                var result = AppContextGeneral.SignalRClient
                    .SendMessageAsync<ServiceResponseDefault<AuthenticateUserMobileResponse>>(
                        "AuthenticateUserMobile",
                        request
                    )
                    .GetAwaiter()
                    .GetResult();

                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Erro ao enviar mensagem SignalR: {ex.Message}");
                return new ServiceResponseDefault<AuthenticateUserMobileResponse>
                {
                    Success = false,
                    Message = "Erro de comunicação com o servidor",
                    Data = null
                };
            }
        }
    }
}

