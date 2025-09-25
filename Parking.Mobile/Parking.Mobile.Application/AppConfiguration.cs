using System;
using Parking.Mobile.Interface.Interfaces;
using Parking.Mobile.Interface.Message.Response;

namespace Parking.Mobile.ApplicationCore
{
	public class AppConfiguration : IConfigurationController
    {
		public ResponseDefault<GetParkingInfoResponse> GetParkingInfo(string parkingCode)
        {
            return new ResponseDefault<GetParkingInfoResponse>
            {
                Success = true,
                Data = new GetParkingInfoResponse()
                {
                    ParkingCode = parkingCode,
                    Address = "Lab",
                    Cnpj = "99999",
                    Description = "Parking Dev MOCK"
                }
            };
            
        }

        public ResponseDefault<GetTerminalInfoResponse> GetTerminalInfo(string parkingCode, int idDevice)
        {
            return new ResponseDefault<GetTerminalInfoResponse>()
            {
                Success = true,
                Data = new GetTerminalInfoResponse()
                {
                    Description = "POS MOCK",
                    IDDevice = 1,
                    Type = Interface.Message.Enum.TerminalTypeEnum.EntradaSaida,
                    NfceEnable = false,
                    PaymentInEntry =false,
                    RPSSeries = 1,
                    TEFId ="000",
                    TEFMobileId ="AAA",
                    TypeDescription = "ENTRY"
                }
            };
        }
    }
}

