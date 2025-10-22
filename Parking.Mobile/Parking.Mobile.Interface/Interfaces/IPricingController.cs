using System;
using Parking.Mobile.Interface.Message.Request;
using Parking.Mobile.Interface.Message.Response;

namespace Parking.Mobile.Interface.Interfaces
{
    public interface IPricingController
    {
        ServiceResponseDefault<GetTicketPriceResponse> GetTicketPrice(GetTicketPriceRequest request);

        ServiceResponseDefault<GetListPriceTableResponse> GetListPriceTable(GetListPriceTableRequest request);

        ServiceResponseDefault<GetListDiscountResponse> GetListDiscount(string parkingCode, int idPriceTable);
    }
}

