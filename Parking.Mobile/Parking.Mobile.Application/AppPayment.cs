using System;
using System.Collections.Generic;
using Parking.Mobile.Interface.Interfaces;
using Parking.Mobile.Interface.Message.Request;
using Parking.Mobile.Interface.Message.Response;

namespace Parking.Mobile.ApplicationCore
{
    public class AppPayment : IPaymentController
    {
        public ResponseDefault<GetListPaymentMethodResponse> GetListPaymentMethod(GetListPaymentMethodRequest request)
        {
            List<PaymentMethodInfo> lst = new List<PaymentMethodInfo>();

            lst.Add(new PaymentMethodInfo() { IDPaymentMethod = 1, Description = "POS Crédito", Type = 1 });
            lst.Add(new PaymentMethodInfo() { IDPaymentMethod = 1, Description = "POS Débito", Type = 2 });
            lst.Add(new PaymentMethodInfo() { IDPaymentMethod = 1, Description = "Dinheiro", Type = 3 });
            lst.Add(new PaymentMethodInfo() { IDPaymentMethod = 1, Description = "PIX", Type = 4 });


            return new ResponseDefault<GetListPaymentMethodResponse>()
            {
                Success = true,
                Data = new GetListPaymentMethodResponse()
                {
                    Methods = lst
                }
            };
        }

        public ResponseDefault<ProcessPaymentResponse> ProcessPayment(ProcessPaymentRequest request)
        {
            return new ResponseDefault<ProcessPaymentResponse>()
            {
                Success = true,
                Data = new ProcessPaymentResponse()
                {
                    DateExit = DateTime.Now,
                    DateLimitExit = DateTime.Now.AddMinutes(20),
                    DatePayment = DateTime.Now,
                    RpsNumber = "001-000001"
                }
             };
        }

        public ResponseDefault<ValidateSealResponse> ValidateSeal(ValidateSealRequest request)
        {
            return new ResponseDefault<ValidateSealResponse>()
            {
                Success = true,
                Data = new ValidateSealResponse()
                {
                    IDParkingCodeSeal = 1,
                    ValueSeal = 10
                }
            };
        }
    }
}

