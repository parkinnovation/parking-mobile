using System;
using Parking.Mobile.Interface.Message.Response;

namespace Parking.Mobile.Interface.Interfaces
{
	public interface IConfigurationController
	{
        ResponseDefault<GetParkingInfoResponse> GetParkingInfo(string parkingCode);

        ResponseDefault<GetTerminalInfoResponse> GetTerminalInfo(string parkingCode, int idDevice);

        ResponseDefault<GetListVehicleModelResponse> GetListVehicleModel(string parkingCode);

        ResponseDefault<GetListColorResponse> GetListColor(string parkingCode);
    }
}

