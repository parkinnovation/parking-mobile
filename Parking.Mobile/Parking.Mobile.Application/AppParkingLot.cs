using System;
using System.Threading.Tasks;
using Parking.Mobile.Common;
using Parking.Mobile.Interface.Interfaces;
using Parking.Mobile.Interface.Message.Request;
using Parking.Mobile.Interface.Message.Response;

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
            return new ServiceResponseDefault<ChangeSectorResponse>()
            {
                Success = true,
                Data = new ChangeSectorResponse()
            };
        }

        public ServiceResponseDefault<GetTicketInfoResponse> GetTicketInfo(GetTicketInfoRequest request)
        {
            return new ServiceResponseDefault<GetTicketInfoResponse>()
            {
                Success = request.AccessCode == "123456789012",
                Data = new GetTicketInfoResponse()
                {
                    DateEntry = DateTime.Now,
                    IDParkingLot = 999,
                    Plate = "AAA-0004",
                    Ticket = "123456789012",
                    VehicleColor = "Azul",
                    VehicleModel = "A5"
                },
                Message = request.AccessCode != "123456789012" ? "Ticket não encontrado" : null
            };
        }

        public ServiceResponseDefault<ProcessEntryResponse> ProcessEntry(ProcessEntryRequest request)
        {
            return new ServiceResponseDefault<ProcessEntryResponse>()
            {
                Success = true,
                Data = new ProcessEntryResponse()
                {
                    DateEntry = DateTime.Now,
                    PaymentInEntry = false,
                    QrCodeContingency = null,
                    Ticket = "123456789012"
                }
            };
        }

        public ServiceResponseDefault<ProcessTicketLossResponse> ProcessTicketLoss(ProcessTicketLossRequest request)
        {
            throw new NotImplementedException();
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

