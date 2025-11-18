using System;
using Acr.UserDialogs;
using Parking.Mobile.ApplicationCore;
using Parking.Mobile.Common;
using Parking.Mobile.Data.Model;
using Parking.Mobile.DependencyService.Interfaces;
using Parking.Mobile.View;
using System.ComponentModel;
using System.Threading.Tasks;
using Xamarin.Forms;
using Parking.Mobile.Interface.Message.Request;
using Xamarin.CommunityToolkit.Extensions;

namespace Parking.Mobile.ViewModel
{
	public class PrintTicketViewModel : INotifyPropertyChanged
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

        public PrintTicketViewModel(INavigation navigation)
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
                try
                {
                    var print = Xamarin.Forms.DependencyService.Get<IPrinterService>();

                    print.PrintTicketEntry(new DependencyService.Model.PrintTicketInfoModel()
                    {
                        CredentialName = this.TicketInfo.CredentialName,
                        CredentialNumber = this.TicketInfo.Credential,
                        DateEntry = this.TicketInfo.DateEntry,
                        Plate = this.TicketInfo.Plate,
                        Prism = this.TicketInfo.Prism,
                        TicketNumber = this.TicketInfo.Ticket,
                        VehicleColor = this.TicketInfo.VehicleColor,
                        VehicleModel = this.TicketInfo.VehicleModel
                    });

                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        UserDialogs.Instance.HideLoading();

                        Application.Current.MainPage = new MenuPage();
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
            });

        }

        private async void ConfirmAction()
        {
            var option = await Application.Current.MainPage.ShowPopupAsync(new TicketOptionPopup(true));

            if (option == null)
                return;

            switch (option)
            {
                case "Print":
                    var print = Xamarin.Forms.DependencyService.Get<IPrinterService>();

                    print.PrintChangeSector(new DependencyService.Model.PrintTicketInfoModel()
                    {
                        TicketNumber = TicketInfo.Ticket
                    });

                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        UserDialogs.Instance.HideLoading();

                        Application.Current.MainPage = new MenuPage();

                    });
                    break;

                case "WhatsApp":
                    var phone = await Application.Current.MainPage.ShowPopupAsync(new WhatsAppPopup());

                    if (phone != null)
                    {
                        AppParkingLot appParkingLot = new AppParkingLot();

                        _ = Task.Run(async () =>
                        {
                            var responseSendTicket = appParkingLot.SendTicket(new SendTicketRequest()
                            {
                                ParkingCode = AppContextGeneral.parkingInfo.ParkingCode,
                                TicketNumber = this.TicketInfo.Ticket,
                                PhoneNumber = phone.ToString(),
                                Plate = TicketInfo.Plate,
                                Type = 2

                            });

                            Device.BeginInvokeOnMainThread(async () =>
                            {
                                UserDialogs.Instance.HideLoading();

                                if (responseSendTicket.Success)
                                {

                                    Application.Current.MainPage = new MenuPage();
                                }
                                else
                                {
                                    await Application.Current.MainPage.DisplayAlert("Erro", responseSendTicket.Message, "Ok");
                                }
                            });
                        });
                    }
                    break;

                case "None":
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        UserDialogs.Instance.HideLoading();

                        Application.Current.MainPage = new MenuPage();

                    });
                    break;
            }
        }
        private void ResetScreen()
        {
            Application.Current.MainPage = new MenuPage();
        }

        private void OnPropertyChanged(string name) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}

