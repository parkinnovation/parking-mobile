using System;
using System.Collections.Generic;
using Parking.Mobile.Interface.Interfaces;
using Parking.Mobile.Interface.Message.Request;
using Parking.Mobile.Interface.Message.Response;

namespace Parking.Mobile.ApplicationCore
{
    public class AppPricing : IPricingController
    {
        public ResponseDefault<GetListDiscountResponse> GetListDiscount(string parkingCode, int idPriceTable)
        {
            List<DiscountInfo> lst = new List<DiscountInfo>();

            lst.Add(new DiscountInfo() { Description = "NETPARK 50%", Percent = 1, IdDiscount = 1 });
            lst.Add(new DiscountInfo() { Description = "DESC 20%", Percent = 0.1m, IdDiscount = 2 });

            return new ResponseDefault<GetListDiscountResponse>()
            {

                Success = true,
                Data = new GetListDiscountResponse()
                {
                    Discounts = lst
                }
            };
        }

        public ResponseDefault<GetListPriceTableResponse> GetListPriceTable(GetListPriceTableRequest request)
        {
            List<PriceTableInfo> lst = new List<PriceTableInfo>();

            lst.Add(new PriceTableInfo() { IdPriceTable = 1, Description = "Rotativo" });
            lst.Add(new PriceTableInfo() { IdPriceTable = 2, Description = "Convenio" });

            return new ResponseDefault<GetListPriceTableResponse>()
            {
                Success = true,
                Data = new GetListPriceTableResponse()
                {
                    PriceTables = lst
                }
            };
        }

        public ResponseDefault<GetTicketPriceResponse> GetTicketPrice(GetTicketPriceRequest request)
        {
            return new ResponseDefault<GetTicketPriceResponse>()
            {
                Success = true,
                Data = new GetTicketPriceResponse()
                {
                    DateEntry = DateTime.Now,
                    DateLimitExit = DateTime.Now.AddMinutes(10),
                    IdParkingLot = 999,
                    Price = request.IdPriceTable == 1 ? 30 : 15,
                    DiscountPercent = request.IdDiscount==1 ? 50m : (request.IdDiscount == 2 ? 20m : 0.00m)
                }
            };
        }
    }
}

