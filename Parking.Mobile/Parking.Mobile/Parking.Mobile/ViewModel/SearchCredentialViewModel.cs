using System;
using Acr.UserDialogs;
using Parking.Mobile.Common;
using Parking.Mobile.View;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Xamarin.Forms;
using Parking.Mobile.Data.Model;
using Parking.Mobile.ApplicationCore;
using Parking.Mobile.Interface.Message.Request;
using System.Linq;

namespace Parking.Mobile.ViewModel
{
    public class SearchCredentialViewModel : INotifyPropertyChanged
    {
        private List<ClientInfoModel> clients;

        public List<ClientInfoModel> Clients
        {
            get
            {
                return this.clients;
            }

            set
            {
                this.clients = value;

                OnPropertyChanged("Clients");
            }
        }

        private bool enableButton;

        public bool EnableButton
        {
            set
            {
                if (enableButton != value)
                {
                    enableButton = value;
                    OnPropertyChanged("EnableButton");
                }
            }
            get
            {
                return enableButton;
            }
        }

        private string parameter;

        public string Parameter
        {
            get
            {
                return this.parameter;
            }

            set
            {
                this.parameter = value;

                this.Clients = new List<ClientInfoModel>();

                if (!String.IsNullOrEmpty(this.parameter))
                {
                    EnableButton = true;
                }
                else
                {
                    EnableButton = false;
                }

                OnPropertyChanged("Parameter");
            }
        }

        public Command<string> ActionPage { get; set; }

        private INavigation Navigation;

        public SearchCredentialViewModel(INavigation navigation)
        {
            this.Navigation = navigation;

            AppContextGeneral.scannerDep.ClearDelegates();
            AppContextGeneral.scannerDep.OnScannerReader += ScannerDep_OnScannerReader; ; ;

            ActionPage = new Command<string>(ActionButton);
        }

        public void SelectItem(int idClient)
        {
            Navigation.PushAsync(new SearchCredentialDetailPage(idClient));
        }

        private void ActionButton(string parameter)
        {
            if (parameter == "Search")
            {
                UserDialogs.Instance.ShowLoading("Processando...");

                this.Clients = new List<ClientInfoModel>();

                Task.Run(async () =>
                {
                    AppCredential appCredential = new AppCredential();

                    var response = appCredential.ListClient(new ListClientRequest()
                    {
                        Parameter = this.Parameter,
                        ParkingCode = AppContextGeneral.parkingInfo.ParkingCode
                    });

                    if (response.Success)
                    {
                        Device.BeginInvokeOnMainThread(() =>
                        {
                            UserDialogs.Instance.HideLoading();

                            if (response.Data != null && response.Data.Clients.Count > 0)
                            {
                                this.Clients = (from l in response.Data.Clients
                                                select new ClientInfoModel()
                                                {
                                                    Active = l.Active,
                                                    Code = l.Code,
                                                    IDClient = l.IDClient,
                                                    Name = l.Name
                                                }).ToList();
                            }
                            else
                            {
                                Application.Current.MainPage.DisplayAlert("Informação", response.Message, "Ok");
                            }
                        });
                    }
                    else
                    {
                        Device.BeginInvokeOnMainThread(() =>
                        {
                            UserDialogs.Instance.HideLoading();

                            Application.Current.MainPage.DisplayAlert("Erro", response.Message, "Ok");
                        });
                    }
                });

            }

            if (parameter == "Scanner")
            {
                Parameter = null;

                AppContextGeneral.scannerDep.ScanAsync();
            }

            if (parameter == "Cancel")
            {
                Application.Current.MainPage = new MenuPage();
            }
        }

        private void ScannerDep_OnScannerReader(string barCode)
        {
            Parameter = barCode;
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

