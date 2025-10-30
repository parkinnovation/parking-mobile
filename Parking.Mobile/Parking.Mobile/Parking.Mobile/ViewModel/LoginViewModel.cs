using System;
using System.ComponentModel;
using Acr.UserDialogs;
using System.Threading.Tasks;
using Xamarin.Forms;
using Parking.Mobile.Common;
using Xamarin.Essentials;
using Parking.Mobile.DependencyService.Interfaces;
using Parking.Mobile.Interface.Message.Request;
using Parking.Mobile.ApplicationCore;
using Parking.Mobile.Data.Model;
using Parking.Mobile.View;
using System.Linq;

namespace Parking.Mobile.ViewModel
{
	public class LoginViewModel : INotifyPropertyChanged
    {
        private INavigation Navigation;
        private bool enableButton = false;
        private string user;
        private string password;

        public string Version
        {
            get
            {
                return "v" + AppContextGeneral.Version;
            }
        }

        public string User
        {
            get
            {
                return this.user;
            }

            set
            {
                this.user = value;

                if (!String.IsNullOrEmpty(this.User) && !String.IsNullOrEmpty(this.Password) && this.User.Length > 0 && this.Password.Length > 0)
                {
                    this.EnableButton = true;
                }
                else
                {
                    this.EnableButton = false;
                }

                OnPropertyChanged("User");
            }
        }

        public string Password
        {
            get
            {
                return this.password;
            }

            set
            {
                this.password = value;

                if (!String.IsNullOrEmpty(this.User) && !String.IsNullOrEmpty(this.Password) && this.User.Length > 0 && this.Password.Length > 0)
                {
                    this.EnableButton = true;
                }
                else
                {
                    this.EnableButton = false;
                }

                OnPropertyChanged("Password");
            }
        }

        public bool EnableButton
        {
            set
            {
                this.enableButton = value;

                OnPropertyChanged("EnableButton");
            }

            get
            {
                return this.enableButton;
            }
        }

        public Command<string> ActionLogin { get; set; }

        public LoginViewModel(INavigation navigation)
        {
            this.Navigation = navigation;

            ActionLogin = new Command<string>(LoginApp);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string nameProperty)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(nameProperty));
            }
        }

        private bool Authenticate()
        {
            AppAuthentication appAuthentication = new AppAuthentication();

            var response = appAuthentication.AuthenticateUserMobile(new AuthenticateUserMobileRequest()
            {
                User = this.User,
                Password = this.Password,
                ParkingCode = AppContextGeneral.configurationApp.ParkingCode
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
                if (response.Data.Disabled)
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        UserDialogs.Instance.HideLoading();

                        Application.Current.MainPage.DisplayAlert("Erro", "Usuário desabilitado", "Ok");
                    });

                    return false;
                }
                else
                {
                    UserInfoModel userInfoModel = new UserInfoModel();

                    userInfoModel.IdUser = response.Data.IdUser;
                    userInfoModel.LastLogin = response.Data.LastLogin;
                    userInfoModel.Name = response.Data.Name;
                    userInfoModel.Profile = response.Data.Profile;
                    
                    AppContextGeneral.userInfo = userInfoModel;
                }
            }

            return true;
        }

        private bool LoadParkingInfo()
        {
            AppConfiguration appConfiguration = new AppConfiguration();

            var response = appConfiguration.GetParkingInfoMobile(AppContextGeneral.configurationApp.ParkingCode);

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
                ParkingInfoModel parkingInfoModel = new ParkingInfoModel();

                parkingInfoModel.Description = response.Data.Description;
                parkingInfoModel.ParkingCode = response.Data.ParkingCode;
                parkingInfoModel.ToleranceExit = response.Data.ToleranceExit;
                parkingInfoModel.TolerancePayment = response.Data.TolerancePayment;
                parkingInfoModel.TolerancePeriod = response.Data.TolerancePeriod;
                parkingInfoModel.Address = response.Data.Address;
                parkingInfoModel.Cnpj = response.Data.Cnpj;

                AppContextGeneral.parkingInfo = parkingInfoModel;

                return true;
            }
        }

        private bool LoadTerminalInfo()
        {
            AppConfiguration appConfiguration = new AppConfiguration();

            var response = appConfiguration.GetTerminalInfo(AppContextGeneral.configurationApp.ParkingCode, AppContextGeneral.configurationApp.IDDevice);


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
                
                DeviceInfoModel deviceInfoModel = new DeviceInfoModel();

                deviceInfoModel.Description = response.Data.Description;
                deviceInfoModel.IDDevice = response.Data.IDDevice;
                deviceInfoModel.PaymentInEntry = response.Data.PaymentInEntry;
                deviceInfoModel.TEFId = response.Data.TEFId;
                deviceInfoModel.Type = (int)response.Data.Type;
                deviceInfoModel.TypeDescription = response.Data.TypeDescription;
                deviceInfoModel.TEFMobileId = response.Data.TEFMobileId;
                deviceInfoModel.RPSSeries = response.Data.RPSSeries;
                deviceInfoModel.TicketHeader1 = response.Data.TicketHeader1;
                deviceInfoModel.TicketHeader2 = response.Data.TicketHeader2;
                deviceInfoModel.TicketHeader3 = response.Data.TicketHeader3;
                deviceInfoModel.TicketFooter1 = response.Data.TicketFooter1;
                deviceInfoModel.TicketFooter2 = response.Data.TicketFooter2;
                deviceInfoModel.TicketFooter3 = response.Data.TicketFooter3;
                deviceInfoModel.ReceiptFooter1 = response.Data.ReceiptFooter1;
                deviceInfoModel.ReceiptFooter2 = response.Data.ReceiptFooter2;
                deviceInfoModel.ReceiptFooter3 = response.Data.ReceiptFooter3;
                deviceInfoModel.ModelRequired = response.Data.ModelRequired;
                deviceInfoModel.PrismRequired = response.Data.PrismRequired;
                deviceInfoModel.ColorRequired = response.Data.ColorRequired;

                AppContextGeneral.deviceInfo = deviceInfoModel;

                return true;
            }
        }

        private bool LoadCashierInfo()
        {
            AppContextGeneral.cashierInfo = null;

            AppFinancial appFinancial = new AppFinancial();

            var response = appFinancial.GetCurrentCashier(AppContextGeneral.configurationApp.ParkingCode, AppContextGeneral.deviceInfo.IDDevice);

            if (response.Success)
            {
                if (response.Data != null)
                {
                    AppContextGeneral.cashierInfo = new CashierInfoModel()
                    {
                        CashTransactionId = response.Data.CashTransactionId,
                        DateOpen = response.Data.DateOpen,
                        IDCashier = response.Data.IDCashier
                    };

                    return true;
                }
                else
                {
                    return true;
                }
            }
            else
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    UserDialogs.Instance.HideLoading();

                    Application.Current.MainPage.DisplayAlert("Erro", response.Message, "Ok");
                });

                return false;
            }
        }

        private bool LoadVehicleModel()
        {
            AppConfiguration appConfiguration = new AppConfiguration();

            var response = appConfiguration.GetListVehicleModel(AppContextGeneral.parkingInfo.ParkingCode);

            if (response.Success)
            {
                AppContextGeneral.vehicles = response.Data.Models;

                return true;
            }
            else
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    Application.Current.MainPage.DisplayAlert("Erro", response.Message, "Ok");
                });

                return false;
            }
        }

        private bool LoadListColor()
        {
            AppConfiguration appConfiguration = new AppConfiguration();

            var response = appConfiguration.GetListColor(AppContextGeneral.parkingInfo.ParkingCode);

            if (response.Success)
            {
                AppContextGeneral.colors = response.Data.Colors;

                return true;
            }
            else
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    Application.Current.MainPage.DisplayAlert("Erro", response.Message, "Ok");
                });

                return false;
            }
        }

        public void LoginApp(string parameter)
        {

            if (parameter == "Login")
            {
                UserDialogs.Instance.ShowLoading("Processando...");

                Task.Run(() =>
                {
                    AppContextGeneral.Model = DeviceInfo.Model;

                    if (Authenticate())
                    {
                        if (LoadParkingInfo())
                        {
                            if (LoadTerminalInfo())
                            {
                                if (LoadCashierInfo())
                                {
                                    if (LoadVehicleModel())
                                    {
                                        if (LoadListColor())
                                        {
                                            AppContextGeneral.scannerDep = Xamarin.Forms.DependencyService.Get<ICamScanner>();

                                            try
                                            {
                                                Device.BeginInvokeOnMainThread(() =>
                                                {
                                                    Application.Current.MainPage = new MenuPage();

                                                    UserDialogs.Instance.HideLoading();

                                                });
                                            }
                                            catch (Exception ex)
                                            {
                                                Device.BeginInvokeOnMainThread(() =>
                                                {
                                                    UserDialogs.Instance.HideLoading();

                                                    Application.Current.MainPage.DisplayAlert("Erro", ex.Message, "Ok");
                                                });
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                });
            }
            else
            {
                Application.Current.MainPage = new ConfigurationPage();
            }

        }

        
    }
}

