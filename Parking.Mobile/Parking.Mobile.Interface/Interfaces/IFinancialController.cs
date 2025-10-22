using System;
using Parking.Mobile.Interface.Message.Request;
using Parking.Mobile.Interface.Message.Response;

namespace Parking.Mobile.Interface.Interfaces
{
    public interface IFinancialController
    {
        ServiceResponseDefault<GetCurrentCashierResponse> GetCurrentCashier(string parkingCode, int idDevice);

        ServiceResponseDefault<OpenCashierResponse> OpenCashier(OpenCashierRequest request);

        ServiceResponseDefault<CloseCashierResponse> CloseCashier(CloseCashierRequest request);
    }
}

