using System;
using System.Threading.Tasks;
using Parking.Mobile.Common;
using Parking.Mobile.Interface.Interfaces;
using Parking.Mobile.Interface.Message.Request;
using Parking.Mobile.Interface.Message.Response;
using Xamarin.Forms;

namespace Parking.Mobile.ApplicationCore
{
    public class AppParkingLot : IParkingLotController
    {
        public ServiceResponseDefault<CancelTicketResponse> CancelTicket(CancelTicketRequest request)
        {
            throw new NotImplementedException();
        }

        public ServiceResponseDefault<ChangeSectorResponse> ChangeSector(ChangeSectorRequest request)
        {
            try
            {

                var result = AppContextGeneral.SignalRClient
                        .SendMessageAsync<ServiceResponseDefault<ChangeSectorResponse>>(
                            "ChangeSector",
                            request
                        )
                    .GetAwaiter()
                    .GetResult();

                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Erro ao enviar mensagem SignalR: {ex.Message}");
                return new ServiceResponseDefault<ChangeSectorResponse>
                {
                    Success = false,
                    Message = "Erro de comunicação com o servidor",
                    Data = null
                };
            }
        }

        public ServiceResponseDefault<GetTicketInfoResponse> GetTicketInfo(GetTicketInfoRequest request)
        {
            try
            {

                var result = AppContextGeneral.SignalRClient
                        .SendMessageAsync<ServiceResponseDefault<GetTicketInfoResponse>>(
                            "GetTicketInfo",
                            request
                        )
                    .GetAwaiter()
                    .GetResult();

                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Erro ao enviar mensagem SignalR: {ex.Message}");
                return new ServiceResponseDefault<GetTicketInfoResponse>
                {
                    Success = false,
                    Message = "Erro de comunicação com o servidor",
                    Data = null
                };
            }
        }

        public ServiceResponseDefault<ProcessEntryResponse> ProcessEntry(ProcessEntryRequest request)
        {
            try
            {

                var result = AppContextGeneral.SignalRClient
                        .SendMessageAsync<ServiceResponseDefault<ProcessEntryResponse>>(
                            "ProcessEntry",
                            request
                        )
                    .GetAwaiter()
                    .GetResult();

                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Erro ao enviar mensagem SignalR: {ex.Message}");
                return new ServiceResponseDefault<ProcessEntryResponse>
                {
                    Success = false,
                    Message = "Erro de comunicação com o servidor",
                    Data = null
                };
            }
        }

        public ServiceResponseDefault<ProcessTicketLossResponse> ProcessTicketLoss(ProcessTicketLossRequest request)
        {
            throw new NotImplementedException();
        }

        public ServiceResponseDefault<SendTicketResponse> SendTicket(SendTicketRequest request)
        {
            try
            {

                var result = AppContextGeneral.SignalRClient
                        .SendMessageAsync<ServiceResponseDefault<SendTicketResponse>>(
                            "SendTicket",
                            request
                        )
                    .GetAwaiter()
                    .GetResult();

                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Erro ao enviar mensagem SignalR: {ex.Message}");
                return new ServiceResponseDefault<SendTicketResponse>
                {
                    Success = false,
                    Message = "Erro de comunicação com o servidor",
                    Data = null
                };
            }
        }

        public ServiceResponseDefault<ValidateEntryPlateResponse> ValidateEntryPlate(string parkingCode, string plate, int idDevice)
        {
            try
            {

                var result = AppContextGeneral.SignalRClient
                        .SendMessageAsync<ServiceResponseDefault<ValidateEntryPlateResponse>>(
                            "ValidateEntryPlate",
                            parkingCode,
                            plate,
                            idDevice
                        )
                    .GetAwaiter()
                    .GetResult();

                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Erro ao enviar mensagem SignalR: {ex.Message}");
                return new ServiceResponseDefault<ValidateEntryPlateResponse>
                {
                    Success = false,
                    Message = "Erro de comunicação com o servidor",
                    Data = null
                };
            }
        }
    }
}

