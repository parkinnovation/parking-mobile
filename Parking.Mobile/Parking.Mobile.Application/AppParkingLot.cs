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
            throw new NotImplementedException();
        }

        public ResponseDefault<ProcessEntryResponse> ProcessEntry(ProcessEntryRequest request)
        {
            throw new NotImplementedException();
        }

        public ResponseDefault<ProcessTicketLossResponse> ProcessTicketLoss(ProcessTicketLossRequest request)
        {
            throw new NotImplementedException();
        }

        public ResponseDefault<ValidateEntryPlateResponse> ValidateEntryPlate(string parkingCode, string plate)
        {
            throw new NotImplementedException();
        }
    }
}

