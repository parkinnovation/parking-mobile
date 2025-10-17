using System;
using Parking.Mobile.ApplicationCore;
using Parking.Mobile.Common;
using Parking.Mobile.Data.Model;
using Parking.Mobile.Interface.Message.Request;
using System.ComponentModel;
using Xamarin.Forms;
using Acr.UserDialogs;

namespace Parking.Mobile.ViewModel
{
    public class CloseCashierViewModel : INotifyPropertyChanged
    {
        //private AppPrint print = new AppPrint();

        private string cashFund = "0,00";

        public CashierInfoModel CashierInfo { get; set; }

        public string CashFund
        {
            get
            {
                return this.cashFund;
            }

            set
            {
                this.cashFund = Convert.ToDouble(String.IsNullOrEmpty(value) ? "0" : value).ToString("F2");

                OnPropertyChanged("CashFund");
            }
        }

        public CloseCashierViewModel()
        {
            this.CashierInfo = AppContextGeneral.cashierInfo;
        }

        public bool CloseCashier()
        {
            decimal amount = Convert.ToDecimal(this.CashFund);

            AppFinancial appFinancial = new AppFinancial();

            var response = appFinancial.CloseCashier(new CloseCashierRequest()
            {
                ParkingCode = AppContextGeneral.configurationApp.ParkingCode,
                Amount = amount,
                CashTransactionId = this.CashierInfo.CashTransactionId,
                IdUser = AppContextGeneral.userInfo.IdUser
            });

            if (!response.Success)
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    UserDialogs.Instance.HideLoading();

                    Application.Current.MainPage.DisplayAlert("Erro", response.Message, "OK");
                });

                return false;
            }
            else
            {
                PrintReport(this.CashierInfo.CashTransactionId);

                AppContextGeneral.cashierInfo = null;

                return true;
            }
        }

        public bool PrintReport(int idCashTransaction)
        {
            AppFinancial appFinancial = new AppFinancial();

            //var response = appFinancial.GetPartialFinanceReport(AppContextGeneral.parkingInfo.ParkingCode, idCashTransaction);

            if (false)//!response.Success)
            {
                /*Device.BeginInvokeOnMainThread(() =>
                {
                    UserDialogs.Instance.HideLoading();

                    Application.Current.MainPage.DisplayAlert("Erro", response.Message, "OK");
                });*/

                return false;
            }
            else
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    try
                    {
                        /*print.PrintFinancialReport(new PrintFinancialReportModel()
                        {
                            DateStart = response.Data.DateOpen,
                            DateEnd = response.Data.DateClose,
                            IDCashierTransaction = idCashTransaction,
                            AmountOpen = response.Data.AmountOpen,
                            AmountClose = response.Data.AmountClose,

                            Transactions = (from l in response.Data.Transactions
                                            where l.Qty > 0
                                            select new FinancialReportTransactionInfo()
                                            {
                                                Amount = l.Amount,
                                                Description = l.Description,
                                                Qty = l.Qty
                                            }).ToList(),

                            Payments = (from l in response.Data.Payments
                                        where l.Qty > 0
                                        select new FinancialReportPaymentInfo()
                                        {
                                            Amount = l.Amount,
                                            Description = l.Description,
                                            Qty = l.Qty
                                        }).ToList(),

                            Compositions = (from l in response.Data.Compositions
                                            where l.Qty > 0
                                            select new FinancialReportCompositionInfo()
                                            {
                                                Amount = l.Amount,
                                                Description = l.Description,
                                                Qty = l.Qty
                                            }).ToList(),

                            Cancelations = (from l in response.Data.Cancellations
                                            where l.Qty > 0
                                            select new FinancialReportCancelInfo()
                                            {
                                                Amount = l.Amount,
                                                Description = l.Description,
                                                Qty = l.Qty
                                            }).ToList()
                        });*/

                        UserDialogs.Instance.HideLoading();
                    }
                    catch (Exception ex)
                    {
                        UserDialogs.Instance.HideLoading();

                        Application.Current.MainPage.DisplayAlert("Erro", ex.Message, "Ok");
                    }
                });

                return true;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string nameProperty)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(nameProperty));
            }
        }
    }
}

