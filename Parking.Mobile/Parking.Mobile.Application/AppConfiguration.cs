using System;
using System.Collections.Generic;
using Parking.Mobile.Common;
using Parking.Mobile.Data.Repository;
using Parking.Mobile.Entity;
using Parking.Mobile.Interface.Interfaces;
using Parking.Mobile.Interface.Message.Response;
using Xamarin.Forms;

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
            try
            {
                var result = AppContextGeneral.SignalRClient
                    .SendMessageAsync<ServiceResponseDefault<GetParkingInfoMobileResponse>>(
                        "GetParkingInfoMobile",
                        parkingCode
                    )
                    .GetAwaiter()
                    .GetResult();

                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Erro ao enviar mensagem SignalR: {ex.Message}");
                return new ServiceResponseDefault<GetParkingInfoMobileResponse>
                {
                    Success = false,
                    Message = "Erro de comunicação com o servidor",
                    Data = null
                };
            }

        }

        public ServiceResponseDefault<GetTerminalInfoResponse> GetTerminalInfo(string parkingCode, int idDevice)
        {
            try
            {
                var result = AppContextGeneral.SignalRClient
                    .SendMessageAsync<ServiceResponseDefault<GetTerminalInfoResponse>>(
                        "GetTerminalInfo",
                        parkingCode,
                        idDevice
                    )
                    .GetAwaiter()
                    .GetResult();

                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Erro ao enviar mensagem SignalR: {ex.Message}");
                return new ServiceResponseDefault<GetTerminalInfoResponse>
                {
                    Success = false,
                    Message = "Erro de comunicação com o servidor",
                    Data = null
                };
            }
        }

        public ServiceResponseDefault<GetListVehicleModelResponse> GetListVehicleModel(string parkingCode)
        {
            try
            {
                var result = AppContextGeneral.SignalRClient
                    .SendMessageAsync<ServiceResponseDefault<GetListVehicleModelResponse>>(
                        "GetListVehicleModel",
                        parkingCode
                    )
                    .GetAwaiter()
                    .GetResult();

                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Erro ao enviar mensagem SignalR: {ex.Message}");
                return new ServiceResponseDefault<GetListVehicleModelResponse>
                {
                    Success = false,
                    Message = "Erro de comunicação com o servidor",
                    Data = null
                };
            }
        }

        public ServiceResponseDefault<GetListColorResponse> GetListColor(string parkingCode)
        {
            try
            {
                var result = AppContextGeneral.SignalRClient
                    .SendMessageAsync<ServiceResponseDefault<GetListColorResponse>>(
                        "GetListColor",
                        parkingCode
                    )
                    .GetAwaiter()
                    .GetResult();

                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Erro ao enviar mensagem SignalR: {ex.Message}");
                return new ServiceResponseDefault<GetListColorResponse>
                {
                    Success = false,
                    Message = "Erro de comunicação com o servidor",
                    Data = null
                };
            }
        }
    }
}

