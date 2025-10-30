using System;
using System.Collections.Generic;
using Parking.Mobile.Common;
using Parking.Mobile.Interface.Interfaces;
using Parking.Mobile.Interface.Message.Request;
using Parking.Mobile.Interface.Message.Response;

namespace Parking.Mobile.ApplicationCore
{
    public class AppPayment : IPaymentController
    {
        public ServiceResponseDefault<GetListPaymentMethodResponse> GetListPaymentMethod(GetListPaymentMethodRequest request)
        {
            try
            {

                var result = AppContextGeneral.SignalRClient
                        .SendMessageAsync<ServiceResponseDefault<GetListPaymentMethodResponse>>(
                            "GetListPaymentMethod",
                            request
                        )
                    .GetAwaiter()
                    .GetResult();

                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Erro ao enviar mensagem SignalR: {ex.Message}");
                return new ServiceResponseDefault<GetListPaymentMethodResponse>
                {
                    Success = false,
                    Message = "Erro de comunicação com o servidor",
                    Data = null
                };
            }
        }

        public ServiceResponseDefault<ProcessPaymentResponse> ProcessPayment(ProcessPaymentRequest request)
        {
            return new ServiceResponseDefault<ProcessPaymentResponse>()
            {
                Success = true,
                Data = new ProcessPaymentResponse()
                {
                    DateExit = DateTime.Now,
                    DateLimitExit = DateTime.Now.AddMinutes(20),
                    DatePayment = DateTime.Now,
                    RpsNumber = "001-000001"
                }
             };
        }

        public ServiceResponseDefault<ValidateSealResponse> ValidateSeal(ValidateSealRequest request)
        {
            return new ServiceResponseDefault<ValidateSealResponse>()
            {
                Success = true,
                Data = new ValidateSealResponse()
                {
                    IDParkingCodeSeal = 1,
                    ValueSeal = 10
                }
            };
        }
    }
}

