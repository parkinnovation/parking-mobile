using System;
using Parking.Mobile.Common;
using Parking.Mobile.Interface.Interfaces;
using Parking.Mobile.Interface.Message.Request;
using Parking.Mobile.Interface.Message.Response;
using Xamarin.Forms;

namespace Parking.Mobile.ApplicationCore
{
	public class AppFinancial : IFinancialController
    {
		public ServiceResponseDefault<CloseCashierResponse> CloseCashier(CloseCashierRequest request)
        {
            try
            {

                var result = AppContextGeneral.SignalRClient
                        .SendMessageAsync<ServiceResponseDefault<CloseCashierResponse>>(
                            "CloseCashier",
                            request
                        )
                    .GetAwaiter()
                    .GetResult();

                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Erro ao enviar mensagem SignalR: {ex.Message}");
                return new ServiceResponseDefault<CloseCashierResponse>
                {
                    Success = false,
                    Message = "Erro de comunicação com o servidor",
                    Data = null
                };
            }
        }

        public ServiceResponseDefault<GetCurrentCashierResponse> GetCurrentCashier(string parkingCode, int idDevice)
        {
            try
            {

                var result = AppContextGeneral.SignalRClient
                        .SendMessageAsync<ServiceResponseDefault<GetCurrentCashierResponse>>(
                            "GetCurrentCashier",
                            parkingCode,
                            idDevice
                        )
                    .GetAwaiter()
                    .GetResult();

                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Erro ao enviar mensagem SignalR: {ex.Message}");
                return new ServiceResponseDefault<GetCurrentCashierResponse>
                {
                    Success = false,
                    Message = "Erro de comunicação com o servidor",
                    Data = null
                };
            }
        }

        public ServiceResponseDefault<OpenCashierResponse> OpenCashier(OpenCashierRequest request)
        {
            try
            {

                var result = AppContextGeneral.SignalRClient
                        .SendMessageAsync<ServiceResponseDefault<OpenCashierResponse>>(
                            "OpenCashier",
                            request         
                        )
                    .GetAwaiter()
                    .GetResult();

                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Erro ao enviar mensagem SignalR: {ex.Message}");
                return new ServiceResponseDefault<OpenCashierResponse>
                {
                    Success = false,
                    Message = "Erro de comunicação com o servidor",
                    Data = null
                };
            }
        }
    }
}

