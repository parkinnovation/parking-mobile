using System;
using Parking.Mobile.ApplicationCore;
using Parking.Mobile.Common;
using Parking.Mobile.Interface.Message.Request;
using System.ComponentModel;
using Xamarin.Forms;
using Acr.UserDialogs;
using Parking.Mobile.Data.Model;

namespace Parking.Mobile.ViewModel
{
    public class OpenCashierViewModel : INotifyPropertyChanged
    {
        private string cashFund = "0,00";

        public string CashFund
        {
            get
            {
                return this.cashFund;
            }

            set
            {
                this.cashFund = Convert.ToDouble(String.IsNullOrEmpty(value) ? "0" : value).ToString("F2");
            }
        }

        public OpenCashierViewModel()
        {
        }

        public bool OpenCashier()
        {
            decimal amount = Convert.ToDecimal(this.CashFund);

            AppFinancial appFinancial = new AppFinancial();

            var response = appFinancial.OpenCashier(new OpenCashierRequest()
            {
                ParkingCode = AppContextGeneral.configurationApp.ParkingCode,
                IdDevice = AppContextGeneral.deviceInfo.IDDevice,
                IdUser = AppContextGeneral.userInfo.IdUser,
                Amount = amount,
                UserName = AppContextGeneral.userInfo.Name
            });

            if (!response.Success)
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    UserDialogs.Instance.HideLoading();

                    Application.Current.MainPage.DisplayAlert("Erro", response.Message, "Ok");
                });

                return false;
            }
            else
            {
                AppContextGeneral.cashierInfo = new CashierInfoModel()
                {
                    CashTransactionId = response.Data.CashTransactionId,
                    DateOpen = response.Data.DateOpen,
                    IDCashier = response.Data.IDCashier
                };

                Device.BeginInvokeOnMainThread(() =>
                {
                    UserDialogs.Instance.HideLoading();
                });

                return true;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}

