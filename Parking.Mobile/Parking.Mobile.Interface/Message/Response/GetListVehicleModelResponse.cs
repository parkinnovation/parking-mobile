using System;
using System.Collections.Generic;

namespace Parking.Mobile.Interface.Message.Response
{
    public class GetListVehicleModelResponse
    {
        public List<VehicleModelInfo> Models { get; set; }
    }

    public class VehicleModelInfo
    {
        public string Description { get; set; }
        public int Type { get; set; }
    }
}

