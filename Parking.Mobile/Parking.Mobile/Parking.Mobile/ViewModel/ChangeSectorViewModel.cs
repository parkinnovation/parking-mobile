using System;
using Acr.UserDialogs;
using Parking.Mobile.View;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using Xamarin.Forms;
using Parking.Mobile.Data.Model;
using Parking.Mobile.Common;
using System.Threading.Tasks;
using Parking.Mobile.Interface.Message.Request;
using Parking.Mobile.ApplicationCore;
using System.Linq;

namespace Parking.Mobile.ViewModel
{
    public class ChangeSectorViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private string CodeRead;

        private string plate;
        public string Plate { get => plate; set { plate = value; OnPropertyChanged(nameof(Plate)); } }

        private string ticket;
        public string Ticket { get => ticket; set { ticket = value; OnPropertyChanged(nameof(Ticket)); } }

        private bool isSearchMode = true;
        public bool IsSearchMode { get => isSearchMode; set { isSearchMode = value; OnPropertyChanged(nameof(IsSearchMode)); } }

        private bool isTicketMode;
        public bool IsTicketMode { get => isTicketMode; set { isTicketMode = value; OnPropertyChanged(nameof(IsTicketMode)); } }

        private TicketInfoModel ticketInfo;
        public TicketInfoModel TicketInfo { get => ticketInfo; set { ticketInfo = value; OnPropertyChanged(nameof(TicketInfo)); } }

        
        public Command<string> ActionPage { get; }
        private readonly INavigation Navigation;

        public ChangeSectorViewModel(INavigation navigation)
        {
            Navigation = navigation;

            AppContextGeneral.scannerDep.ClearDelegates();
            AppContextGeneral.scannerDep.OnScannerReader += ScannerDep_OnScannerReader;

            ActionPage = new Command<string>(ActionButton);
        }

        private void ScannerDep_OnScannerReader(string barCode)
        {
            this.CodeRead = barCode;

            if (!String.IsNullOrEmpty(barCode))
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    SearchTicket();
                });
            }
        }

        private void ActionButton(string parameter)
        {
            switch (parameter)
            {
                case "Search":
                    SearchTicket();
                    break;

                case "Scanner":
                    AppContextGeneral.scannerDep.ScanAsync();

                    break;

                case "Confirm":
                    ConfirmChange();
                    break;

                case "Cancel":
                    ResetScreen();
                    break;
            }
        }

        private void SearchTicket()
        {
            if (string.IsNullOrEmpty(Plate) && string.IsNullOrEmpty(Ticket) && string.IsNullOrEmpty(this.CodeRead))
            {
                Application.Current.MainPage.DisplayAlert("Aviso", "Informe a placa ou o ticket.", "OK");
                return;
            }

            UserDialogs.Instance.ShowLoading("Buscando ticket...");

            GetTicketInfo();
        }

        private void GetTicketInfo()
        {
            UserDialogs.Instance.ShowLoading("Processando...");

            Task.Run(async () =>
            {
                AppParkingLot appParkingLot = new AppParkingLot();

                string accessCode = "";

                if (!String.IsNullOrEmpty(Plate))
                {
                    accessCode = this.Plate;
                }

                if (!String.IsNullOrEmpty(ticket))
                {
                    accessCode = this.Ticket;
                }

                if (!String.IsNullOrEmpty(CodeRead))
                {
                    accessCode = this.CodeRead;
                }

                var response = appParkingLot.GetTicketInfo(new GetTicketInfoRequest()
                {
                    AccessCode = accessCode,
                    IDDevice = AppContextGeneral.deviceInfo.IDDevice,
                    IDUser = AppContextGeneral.userInfo.IdUser,
                    ParkingCode = AppContextGeneral.parkingInfo.ParkingCode
                });

                if (response.Success)
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        UserDialogs.Instance.HideLoading();

                        TicketInfo = new TicketInfoModel()
                        {
                            Credential = response.Data.Credential,
                            CredentialName = response.Data.CredentialName,
                            DateEntry = response.Data.DateEntry,
                            IDParkingLot = response.Data.IDParkingLot,
                            Plate = response.Data.Plate,
                            Prism = response.Data.Prism,
                            Stay = response.Data.Stay,
                            Ticket = response.Data.Ticket,
                            VehicleColor = response.Data.VehicleColor,
                            VehicleModel = response.Data.VehicleModel
                        };

                        IsSearchMode = false;
                        IsTicketMode = true;
                    });


                    Device.BeginInvokeOnMainThread(() =>
                    {
                        UserDialogs.Instance.HideLoading();
                    });
                }
                else
                {
                    string codeAux = this.CodeRead;

                    this.CodeRead = null;

                    Device.BeginInvokeOnMainThread(() =>
                    {
                        UserDialogs.Instance.HideLoading();

                        Application.Current.MainPage.DisplayAlert("Erro", response.Message + " - " + codeAux, "Ok");
                    });
                }
            });
        }

        private void ConfirmChange()
        {
            UserDialogs.Instance.ShowLoading("Processando...");

            Task.Run(async () =>
            {
                AppParkingLot appParkingLot = new AppParkingLot();

                var response = appParkingLot.ChangeSector(new ChangeSectorRequest()
                {
                    IDDevice = AppContextGeneral.deviceInfo.IDDevice,
                    ParkingCode = AppContextGeneral.parkingInfo.ParkingCode,
                    Plate = TicketInfo.Plate,
                    TicketNumber = TicketInfo.Ticket
                });

                if (response.Success)
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        UserDialogs.Instance.HideLoading();

                        Application.Current.MainPage.DisplayAlert("Sucesso",
                        $"Mudança confirmada!\n\nTicket: {TicketInfo.Ticket}",
                        "OK");

                                ResetScreen();
                            });
                }
                else
                {
                    string codeAux = this.CodeRead;

                    this.CodeRead = null;

                    Device.BeginInvokeOnMainThread(() =>
                    {
                        UserDialogs.Instance.HideLoading();

                        Application.Current.MainPage.DisplayAlert("Erro", response.Message, "Ok");
                    });
                }
            });
            
        }

        private void ResetScreen()
        {
            Application.Current.MainPage = new MenuPage();
        }

        private void OnPropertyChanged(string name) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}

