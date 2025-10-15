using System;
using Parking.Mobile.Interface.Message.Request;
using Parking.Mobile.Interface.Message.Response;

namespace Parking.Mobile.Interface.Interfaces
{
    public interface IParkingLotController
    {
        ResponseDefault<ProcessEntryResponse> ProcessEntry(ProcessEntryRequest request);

        ResponseDefault<ProcessTicketLossResponse> ProcessTicketLoss(ProcessTicketLossRequest request);

        ResponseDefault<GetTicketInfoResponse> GetTicketInfo(GetTicketInfoRequest request);

        ResponseDefault<ValidateEntryPlateResponse> ValidateEntryPlate(string parkingCode, string plate);

        ResponseDefault<CancelTicketResponse> CancelTicket(CancelTicketRequest request);
    }
}

