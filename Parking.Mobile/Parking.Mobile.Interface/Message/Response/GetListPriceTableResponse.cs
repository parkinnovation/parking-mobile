using System;
using System.Collections.Generic;

namespace Parking.Mobile.Interface.Message.Response
{
    public class GetListPriceTableResponse
    {
        public List<PriceTableInfo> PriceTables { get; set; }
    }

    public class PriceTableInfo
    {
        public int IdPriceTable { get; set; }
        public string Description { get; set; }
        public bool Default { get; set; }
    }
}

