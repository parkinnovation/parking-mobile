using System;
using Parking.Mobile.Common;
using Parking.Mobile.View;
using System.ComponentModel;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.CommunityToolkit.Extensions;

namespace Parking.Mobile.ViewModel
{
    public class MenuViewModel : INotifyPropertyChanged
    {
        private INavigation Navigation;

        public string Version
        {
            get
            {
                var version = "v " + AppContextGeneral.Version + " - " + AppContextGeneral.Model;

                return version;
            }
        }

        public bool ShowParkingLotCounter
        {
            get
            {
                return AppContextGeneral.userInfo.ShowParkingLotCounter;
            }
        }

        public bool ShowEntry
        {
            get
            {
                return AppContextGeneral.userInfo.ShowEntry;
            }
        }

        public bool ShowPayment
        {
            get
            {
                return AppContextGeneral.userInfo.ShowPayment;
            }
        }

        public bool ShowTicketLost
        {
            get
            {
                return AppContextGeneral.userInfo.ShowTicketLost;
            }
        }

        public bool ShowFinancialReport
        {
            get
            {
                return AppContextGeneral.userInfo.ShowFinancialReport;
            }
        }

        public bool ShowOpenCloseCashier
        {
            get
            {
                return AppContextGeneral.userInfo.ShowOpenCloseCashier;
            }
        }

        public bool ShowCancelTicket
        {
            get
            {
                return AppContextGeneral.userInfo.ShowCancelTicket;
            }
        }

        public int HeightRowParkingLotCounter
        {
            get
            {
                if (this.ShowParkingLotCounter)
                {
                    return 80;
                }
                else
                {
                    return 0;
                }
            }
        }

        public string WelcomeMessage
        {
            get
            {
                return "Olá, 1" + AppContextGeneral.userInfo.Name;
            }
        }
        private int qtyRotary = 0;
        private int qtyCredential = 0;

        public int QtyRotary
        {
            get
            {
                return this.qtyRotary;
            }

            set
            {
                this.qtyRotary = value;

                OnPropertyChanged("QtyRotary");
            }
        }

        public int QtyCredential
        {
            get
            {
                return this.qtyCredential;
            }

            set
            {
                this.qtyCredential = value;

                OnPropertyChanged("QtyCredential");
            }
        }

        public string ParkingDescription { get; set; }
        private bool isRefreshing = false;
        public bool IsRefreshing
        {
            get
            {
                return isRefreshing;
            }

            set
            {
                isRefreshing = value;

                OnPropertyChanged("IsRefreshing");
            }
        }

        public Command<string> ActionOpenPage { get; set; }
        public Command<string> ActionRefresh { get; set; }

        public MenuViewModel(bool loadCounter, INavigation navigation)
        {
            this.Navigation = navigation;

            ActionOpenPage = new Command<string>(OpenPage);
            ActionRefresh = new Command<string>(RefreshPage);

            this.ParkingDescription = AppContextGeneral.parkingInfo.Description;

            /*if (loadCounter)
            {
                LoadCounter();
            }*/
        }

        public void RefreshPage(string parameter)
        {
            Task.Run(() =>
            {
                IsRefreshing = true;

                LoadCounter();

                IsRefreshing = false;
            });
        }

        private void LoadCounter()
        {
            if (this.ShowParkingLotCounter)
            {
                /*AppParkingLot appParkingLot = new AppParkingLot();

                var response = appParkingLot.GetParkingLotInfo(AppContextGeneral.configurationApp.ParkingCode);

                if (response.Success)
                {
                    this.QtyRotary = response.Data.QtyRotary;
                    this.QtyCredential = response.Data.QtyCredential;
                }
                else
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        Application.Current.MainPage.DisplayAlert("Erro", response.Message, "Ok");
                    });
                }*/
            }
        }

        public void OpenPage(string page)
        {
            switch (page)
            {
                case "Entry":

                    Navigation.PushAsync(new EntryPage());

                    break;
                case "ChangeSector":

                    Navigation.PushAsync(new ChangeSectorPage());

                    break;

                case "ParkingLotMonthly":

                    //Navigation.PushAsync(new ParkingLotReportPage(ParkingLotType.Monthly));

                    break;

                case "Payment":

                    Navigation.PushAsync(new PaymentPage());

                    break;

                case "FinancialReport":

                    //Navigation.PushAsync(new FinancialReportPage());

                    break;

                case "OpenCashier":

                    if (AppContextGeneral.cashierInfo != null)
                    {
                        Application.Current.MainPage.DisplayAlert("Erro", "O caixa já está aberto", "Ok");
                    }
                    else
                    {
                        var retOpenCashier = Navigation.ShowPopupAsync<object>(new OpenCashierPage());
                    }

                    break;

                case "CloseCashier":

                    if (AppContextGeneral.cashierInfo == null)
                    {
                        Application.Current.MainPage.DisplayAlert("Erro", "Não existe caixa aberto", "Ok");
                    }
                    else
                    {
                        var retCloseCashier = Navigation.ShowPopupAsync<object>(new CloseCashierPage());
                    }
                    break;

                case "Exit":

                    //AppContextGeneral.connectionMonitoringDep.Stop();
                    //AppContextGeneral.syncReceive.Stop();
                    //AppContextGeneral.syncSend.Stop();

                    /*
                    if (AppContextGeneral.cashierInfo != null)
                    {
                        CloseCashierPage closeCashierPage = new CloseCashierPage();
                        closeCashierPage.Dismissed += CloseCashierPage_Dismissed;
                        var ret = Navigation.ShowPopupAsync<object>(closeCashierPage);
                    }
                    else
                    {
                        Application.Current.MainPage = new LoginPage();
                    }*/
                    break;

                case "TicketLost":

                    if (CheckRPS())
                    {
                        //Navigation.PushAsync(new TicketLostPage());
                    }

                    break;

                case "CancelTicket":

                    //Navigation.PushAsync(new CancelTicketPage());

                    break;

                case "SearchCredential":

                    Navigation.PushAsync(new SearchCredentialPage());

                    break;

                case "SystemInfo":

                    //Navigation.PushAsync(new SystemInfoPage());

                    break;

                default:
                    Application.Current.MainPage.DisplayAlert("Não Implementado", page, "Ok");
                    ((MasterDetailPage)Application.Current.MainPage).IsPresented = false;
                    break;
            }


        }

        private bool CheckRPS()
        {
            if (!AppContextGeneral.deviceInfo.RPSSeries.HasValue)
            {
                Application.Current.MainPage.DisplayAlert("Erro", "Terminal sem RPS configurada", "Ok");

                return false;
            }

            return true;
        }

        private void CloseCashierPage_Dismissed(object sender, Xamarin.CommunityToolkit.UI.Views.PopupDismissedEventArgs e)
        {
            if ((bool)e.Result)
            {
                Task.Run(() =>
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        Application.Current.MainPage = new LoginPage();

                    });
                });

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

