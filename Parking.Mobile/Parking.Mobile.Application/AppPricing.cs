using System;
using System.Collections.Generic;
using Parking.Mobile.Common;
using Parking.Mobile.Interface.Interfaces;
using Parking.Mobile.Interface.Message.Request;
using Parking.Mobile.Interface.Message.Response;

namespace Parking.Mobile.ApplicationCore
{
    public class AppPricing : IPricingController
    {
        public ServiceResponseDefault<GetListDiscountResponse> GetListDiscount(string parkingCode, Guid idPriceTable)
        {
            try
            {

                var result = AppContextGeneral.SignalRClient
                        .SendMessageAsync<ServiceResponseDefault<GetListDiscountResponse>>(
                            "GetListDiscount",
                            parkingCode,
                            idPriceTable
                        )
                    .GetAwaiter()
                    .GetResult();

                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Erro ao enviar mensagem SignalR: {ex.Message}");
                return new ServiceResponseDefault<GetListDiscountResponse>
                {
                    Success = false,
                    Message = "Erro de comunicação com o servidor",
                    Data = null
                };
            }
        }

        public ServiceResponseDefault<GetListPriceTableResponse> GetListPriceTable(GetListPriceTableRequest request)
        {
            try
            {

                var result = AppContextGeneral.SignalRClient
                        .SendMessageAsync<ServiceResponseDefault<GetListPriceTableResponse>>(
                            "GetListPriceTable",
                            request
                        )
                    .GetAwaiter()
                    .GetResult();

                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Erro ao enviar mensagem SignalR: {ex.Message}");
                return new ServiceResponseDefault<GetListPriceTableResponse>
                {
                    Success = false,
                    Message = "Erro de comunicação com o servidor",
                    Data = null
                };
            }
        }

        public ServiceResponseDefault<GetTicketPriceResponse> GetTicketPrice(GetTicketPriceRequest request)
        {
            try
            {

                var result = AppContextGeneral.SignalRClient
                        .SendMessageAsync<ServiceResponseDefault<GetTicketPriceResponse>>(
                            "GetTicketPrice",
                            request
                        )
                    .GetAwaiter()
                    .GetResult();

                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Erro ao enviar mensagem SignalR: {ex.Message}");
                return new ServiceResponseDefault<GetTicketPriceResponse>
                {
                    Success = false,
                    Message = "Erro de comunicação com o servidor",
                    Data = null
                };
            }
        }
    }
}

