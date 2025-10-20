using System;
using Acr.UserDialogs;
using Parking.Mobile.View;
using System.ComponentModel;
using System.Threading.Tasks;
using Xamarin.Forms;
using Parking.Mobile.ApplicationCore;
using Parking.Mobile.Common;
using Parking.Mobile.Data.Model;
using System.Collections.Generic;
using System.Linq;

namespace Parking.Mobile.ViewModel
{
    public class SearchCredentialDetailViewModel : INotifyPropertyChanged
    {
        private ClientInfoDetailModel clientInfo;

        public ClientInfoDetailModel ClientInfo
        {
            get
            {
                return this.clientInfo;
            }

            set
            {
                this.clientInfo = value;

                OnPropertyChanged("ClientInfo");
            }
        }

        public Command<string> ActionPage { get; set; }

        private INavigation Navigation;

        public SearchCredentialDetailViewModel(INavigation navigation, int idClient)
        {
            this.Navigation = navigation;

            ActionPage = new Command<string>(ActionButton);

            LoadClient(idClient);
        }

        private void LoadClient(int idClient)
        {
            UserDialogs.Instance.ShowLoading("Processando...");

            Task.Run(async () =>
            {
                AppCredential appCredential = new AppCredential();

                var response = appCredential.GetClientInfo(
                    AppContextGeneral.parkingInfo.ParkingCode,
                    idClient);

                if (response.Success)
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        UserDialogs.Instance.HideLoading();

                        if (response.Data != null)
                        {
                            var clientInfoObj = new ClientInfoDetailModel()
                            {
                                Code = response.Data.Code,
                                Name = response.Data.Name,
                                Active = response.Data.Active,
                                DateEnd = response.Data.DateEnd,
                                DateStart = response.Data.DateStart,
                                Document = response.Data.Document,
                                GroupName = response.Data.GroupName,
                                IDClient = response.Data.IDClient,
                                VacancyLimit = response.Data.VacancyLimit,
                                Credentials = response.Data.Credentials != null ? (from l in response.Data.Credentials
                                                                                   select new CredentialInfoModel()
                                                                                   {
                                                                                       Code = l.Code,
                                                                                       Active = l.Active,
                                                                                       DateStart = l.DateStart,
                                                                                       DateEnd = l.DateEnd,
                                                                                       Name = l.Name,
                                                                                   }).ToList() : new List<CredentialInfoModel>(),
                                Plates = response.Data.Plates != null ? (from l in response.Data.Plates
                                                                         select new PlateInfoModel()
                                                                         {
                                                                             Active = l.Active,
                                                                             Plate = l.Plate
                                                                         }).ToList() : new List<PlateInfoModel>()
                            };

                            this.ClientInfo = clientInfoObj;
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

        private void ActionButton(string parameter)
        {
            if (parameter == "Cancel")
            {
                Application.Current.MainPage = new MenuPage();
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

