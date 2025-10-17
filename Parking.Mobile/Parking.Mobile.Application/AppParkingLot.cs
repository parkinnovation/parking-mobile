using System;
using Parking.Mobile.Interface.Interfaces;
using Parking.Mobile.Interface.Message.Request;
using Parking.Mobile.Interface.Message.Response;

namespace Parking.Mobile.ApplicationCore
{
    public class AppParkingLot : IParkingLotController
    {
        public ResponseDefault<CancelTicketResponse> CancelTicket(CancelTicketRequest request)
        {
            throw new NotImplementedException();
        }

        public ResponseDefault<GetTicketInfoResponse> GetTicketInfo(GetTicketInfoRequest request)
        {
            return new ResponseDefault<GetTicketInfoResponse>()
            {
                Success = request.AccessCode == "12345678",
                Data = new GetTicketInfoResponse()
                {
                    DateEntry = DateTime.Now,
                    IDParkingLot = 999,
                    Plate = "AAA-0004",
                    Ticket = "12345678",
                    VehicleColor = "Azul",
                    VehicleModel = "A5"
                },
                Message = request.AccessCode != "12345678" ? "Ticket não encontrado" : null
            };
        }

        public ResponseDefault<ProcessEntryResponse> ProcessEntry(ProcessEntryRequest request)
        {
            return new ResponseDefault<ProcessEntryResponse>()
            {
                Success = true,
                Data = new ProcessEntryResponse()
                {
                    DateEntry = DateTime.Now,
                    PaymentInEntry = false,
                    QrCodeContingency = null,
                    Ticket = "1234567890"
                }
            };
        }

        public ResponseDefault<ProcessTicketLossResponse> ProcessTicketLoss(ProcessTicketLossRequest request)
        {
            throw new NotImplementedException();
        }

        public ResponseDefault<ValidateEntryPlateResponse> ValidateEntryPlate(string parkingCode, string plate)
        {
            return new ResponseDefault<ValidateEntryPlateResponse>()
            {
                Success = (plate == "AAA-0001" || plate == "AAA-0002"),
                Data = new ValidateEntryPlateResponse(),
                Message = (plate != "AAA-0001" && plate != "AAA-0002") ? "Placa no pátio" : null
           };
        }
    }
}

