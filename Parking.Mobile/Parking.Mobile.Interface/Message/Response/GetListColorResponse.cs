using System;
using System.Collections.Generic;

namespace Parking.Mobile.Interface.Message.Response
{
    public class GetListColorResponse
    {
        public List<ColorInfo> Colors { get; set; }
    }

    public class ColorInfo
    {
        public string Description { get; set; }
        public string Hexa { get; set; }
    }
}

