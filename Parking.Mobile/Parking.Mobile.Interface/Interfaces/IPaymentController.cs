using System;
using Parking.Mobile.Interface.Message.Request;
using Parking.Mobile.Interface.Message.Response;

namespace Parking.Mobile.Interface.Interfaces
{
    public interface IPaymentController
    {
        ServiceResponseDefault<GetListPaymentMethodResponse> GetListPaymentMethod(GetListPaymentMethodRequest request);

        ServiceResponseDefault<ValidateSealResponse> ValidateSeal(ValidateSealRequest request);

        ServiceResponseDefault<ProcessPaymentResponse> ProcessPayment(ProcessPaymentRequest request);

    }
}

