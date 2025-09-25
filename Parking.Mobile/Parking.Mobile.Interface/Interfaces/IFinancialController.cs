using System;
using Parking.Mobile.Interface.Message.Request;
using Parking.Mobile.Interface.Message.Response;

namespace Parking.Mobile.Interface.Interfaces
{
    public interface IFinancialController
    {
        ResponseDefault<GetCurrentCashierResponse> GetCurrentCashier(string parkingCode, int idDevice);

        ResponseDefault<OpenCashierResponse> OpenCashier(OpenCashierRequest request);

        ResponseDefault<CloseCashierResponse> CloseCashier(CloseCashierRequest request);
    }
}

