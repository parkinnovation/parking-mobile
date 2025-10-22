using System;
using Parking.Mobile.Interface.Message.Request;
using Parking.Mobile.Interface.Message.Response;

namespace Parking.Mobile.Interface.Interfaces
{
    public interface IParkingLotController
    {
        ServiceResponseDefault<ProcessEntryResponse> ProcessEntry(ProcessEntryRequest request);

        ServiceResponseDefault<ProcessTicketLossResponse> ProcessTicketLoss(ProcessTicketLossRequest request);

        ServiceResponseDefault<GetTicketInfoResponse> GetTicketInfo(GetTicketInfoRequest request);

        ServiceResponseDefault<ValidateEntryPlateResponse> ValidateEntryPlate(string parkingCode, string plate);

        ServiceResponseDefault<CancelTicketResponse> CancelTicket(CancelTicketRequest request);

        ServiceResponseDefault<ChangeSectorResponse> ChangeSector(ChangeSectorRequest request);
    }
}

