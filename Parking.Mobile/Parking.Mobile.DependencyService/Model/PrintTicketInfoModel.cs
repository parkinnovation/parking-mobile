using System;
using System.Collections.Generic;

namespace Parking.Mobile.DependencyService.Model
{
	public class PrintTicketInfoModel
	{
		public string TicketNumber { get; set; }
		public string CredentialNumber { get; set; }
		public string CredentialName { get; set; }
		public DateTime DateEntry { get; set; }
		public DateTime DateLimitExit { get; set; }
		public DateTime DatePayment { get; set; }
        public List<PrintPaymentInfoModel> Payments { get; set; }
		public string Prism { get; set; }

        private string plate;

        public string Plate
        {
            get
            {
                if (!String.IsNullOrEmpty(plate))
                {
                    if (plate.Length >= 7 && !plate.Contains("-"))
                    {
                        return plate.Substring(0, 3) + "-" + plate.Substring(3, 4);
                    }
                    else
                    {
                        return plate;
                    }
                }
                else
                {
                    return plate;
                }
            }

            set
            {
                plate = value;
            }
        }

        public string VehicleModel { get; set; }
		public string VehicleColor { get; set; }
        public decimal Amount { get; set; }

        public string Stay
        {
            get
            {
                TimeSpan permanency = this.DatePayment - this.DateEntry;

                return permanency.ToString(@"dd\.hh\:mm\:ss");
            }
        }

        public DateTime CashierOpen { get; set; }
        public DateTime CashierClose { get; set; }
        public string UserName { get; set; }
        public int CashierNumber { get; set; }
        public double CashierAmount { get; set; }
    }

    public class PrintPaymentInfoModel
    {
        public string PaymentMethod { get; set; }
        public decimal Amount { get; set; }
    }
}
