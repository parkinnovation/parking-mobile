using System;
using Parking.Mobile.Interface.Message.Response;

namespace Parking.Mobile.Interface.Interfaces
{
	public interface IConfigurationController
	{
        ServiceResponseDefault<GetParkingInfoMobileResponse> GetParkingInfoMobile(string parkingCode);

        ServiceResponseDefault<GetTerminalInfoResponse> GetTerminalInfo(string parkingCode, int idDevice);

        ServiceResponseDefault<GetListVehicleModelResponse> GetListVehicleModel(string parkingCode);

        ServiceResponseDefault<GetListColorResponse> GetListColor(string parkingCode);
    }
}

