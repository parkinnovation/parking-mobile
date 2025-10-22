using System;
using System.Collections.Generic;
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

        public ServiceResponseDefault<GetParkingInfoMobileResponse> GetParkingInfoMobile(string parkingCode)
        {
            return new ServiceResponseDefault<GetParkingInfoMobileResponse>
            {
                Success = true,
                Data = new GetParkingInfoMobileResponse()
                {
                    ParkingCode = parkingCode,
                    Address = "Lab",
                    Cnpj = "99999",
                    Description = "Parking Dev MOCK",
                    AllowEntryWithoutPlate = false
                }
            };
            
        }

        public ServiceResponseDefault<GetTerminalInfoResponse> GetTerminalInfo(string parkingCode, int idDevice)
        {
            return new ServiceResponseDefault<GetTerminalInfoResponse>()
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

        public ServiceResponseDefault<GetListVehicleModelResponse> GetListVehicleModel(string parkingCode)
        {
            List<VehicleModelInfo> lst = new List<VehicleModelInfo>();

            lst.Add(new VehicleModelInfo()
            {
                Description = "Gol",
                Type = 1
            });

            lst.Add(new VehicleModelInfo()
            {
                Description = "Uno",
                Type = 1
            });

            lst.Add(new VehicleModelInfo()
            {
                Description = "Argo",
                Type = 1
            });

            lst.Add(new VehicleModelInfo()
            {
                Description = "Kwid",
                Type = 1
            });

            return new ServiceResponseDefault<GetListVehicleModelResponse>()
            {
                Data = new GetListVehicleModelResponse()
                {
                    Models = lst
                },
                Success = true
            };
        }

        public ServiceResponseDefault<GetListColorResponse> GetListColor(string parkingCode)
        {
            List<ColorInfo> lst = new List<ColorInfo>();

            lst.Add(new ColorInfo() { Description = "Branco" });
            lst.Add(new ColorInfo() { Description = "Preto" });
            lst.Add(new ColorInfo() { Description = "Prata" });
            lst.Add(new ColorInfo() { Description = "Vermelho" });

            return new ServiceResponseDefault<GetListColorResponse>()
            {
                Success = true,
                Data = new GetListColorResponse()
                {
                    Colors = lst
                }
            };
        }
    }
}

