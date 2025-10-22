using System;
using Parking.Mobile.Interface.Interfaces;
using Parking.Mobile.Interface.Message.Request;
using Parking.Mobile.Interface.Message.Response;

namespace Parking.Mobile.ApplicationCore
{
	public class AppFinancial : IFinancialController
    {
		public ServiceResponseDefault<CloseCashierResponse> CloseCashier(CloseCashierRequest request)
        {
            return new ServiceResponseDefault<CloseCashierResponse>()
            {
                Success = true,
                Data = new CloseCashierResponse()
                {
                    CashTransactionId = 1,
                    DateOpen = DateTime.Now,
                    IdDevice = 1,
                    IdUser = 1,
                    DateClose = DateTime.Now
                }
            };
        }

        public ServiceResponseDefault<GetCurrentCashierResponse> GetCurrentCashier(string parkingCode, int idDevice)
        {
            return new ServiceResponseDefault<GetCurrentCashierResponse>()
            {
                Success = true,
                Data = new GetCurrentCashierResponse()
                {
                    CashTransactionId = 1,
                    DateOpen = DateTime.Now,
                    IdDevice = 1,
                    IDGlobalCashTransaction = Guid.NewGuid().ToString(),
                    IdUser = 1
                }
            };
        }

        public ServiceResponseDefault<OpenCashierResponse> OpenCashier(OpenCashierRequest request)
        {
            return new ServiceResponseDefault<OpenCashierResponse>()
            {
                Success = true,
                Data = new OpenCashierResponse()
                {
                    CashTransactionId = 1,
                    DateOpen = DateTime.Now,
                    IdDevice = 1,
                    IDGlobalCashTransaction = Guid.NewGuid().ToString(),
                    IdUser = 1
                }
            };
        }
    }
}

