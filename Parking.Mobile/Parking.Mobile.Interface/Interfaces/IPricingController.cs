using System;
using Parking.Mobile.Interface.Message.Request;
using Parking.Mobile.Interface.Message.Response;

namespace Parking.Mobile.Interface.Interfaces
{
    public interface IPricingController
    {
        ResponseDefault<GetTicketPriceResponse> GetTicketPrice(GetTicketPriceRequest request);

        ResponseDefault<GetListPriceTableResponse> GetListPriceTable(GetListPriceTableRequest request);

        ResponseDefault<GetListDiscountResponse> GetListDiscount(string parkingCode, int idPriceTable);
    }
}

