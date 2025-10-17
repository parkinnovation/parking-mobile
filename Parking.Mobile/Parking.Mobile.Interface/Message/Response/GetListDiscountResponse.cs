using System;
using System.Collections.Generic;

namespace Parking.Mobile.Interface.Message.Response
{
    public class GetListDiscountResponse
    {
        public List<DiscountInfo> Discounts { get; set; }
    }

    public class DiscountInfo
    {
        public int IdDiscount { get; set; }
        public int DiscountType { get; set; }
        public string Description { get; set; }
        public decimal Percent { get; set; }
    }
}

