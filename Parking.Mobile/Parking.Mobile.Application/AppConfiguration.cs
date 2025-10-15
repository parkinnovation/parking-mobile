using System;
using Parking.Mobile.Common;
using Parking.Mobile.Data.Repository;
using Parking.Mobile.Entity;
using Parking.Mobile.Interface.Interfaces;
using Parking.Mobile.Interface.Message.Response;

namespace Parking.Mobile.ApplicationCore
{
	public class AppConfiguration : IConfigurationController
    {
        public ConfigurationApp GetConfigurationApp()
        {
            ConfigurationAppRepository configurationAppRepository = new ConfigurationAppRepository(AppContextGeneral.databaseInstance.Connection);

            return configurationAppRepository.Get();
        }

        public void SaveConfigurationApp(ConfigurationApp configurationApp)
        {
            ConfigurationAppRepository configurationAppRepository = new ConfigurationAppRepository(AppContextGeneral.databaseInstance.Connection);

            if (String.IsNullOrEmpty(configurationApp.IDConfigurationApp))
            {
                configurationApp.IDConfigurationApp = Guid.NewGuid().ToString();
            }

            configurationAppRepository.Save(configurationApp);

            AppContextGeneral.configurationApp = configurationApp;
        }

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
                    Description = "Parking Dev MOCK",
                    AllowEntryWithoutPlate = false
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

