using System;
using Acr.UserDialogs;
using Parking.Mobile.Common;
using Parking.Mobile.View;
using System.ComponentModel;
using System.Threading.Tasks;
using Xamarin.Forms;
using Parking.Mobile.ApplicationCore;
using Parking.Mobile.Interface.Message.Request;
using Xamarin.CommunityToolkit.Extensions;
using Parking.Mobile.DependencyService.Interfaces;

namespace Parking.Mobile.ViewModel
{
    public class EntryViewModel : INotifyPropertyChanged
    {
        private string credential;
        private string credentialName;
        private string plate;
        private string vehicle;
        private string color;
        private string prism;
        private bool loading;
        private bool enableButton;
        private INavigation Navigation;

        public event PropertyChangedEventHandler PropertyChanged;

        #region Propriedades Bindáveis

        public string Credential
        {
            get => credential;
            set { credential = value; OnPropertyChanged(nameof(Credential)); }
        }

        public string CredentialName
        {
            get => credentialName;
            set { credentialName = value; OnPropertyChanged(nameof(CredentialName)); }
        }

        public string Plate
        {
            get => plate;
            set
            {
                plate = value;
                
                if(!String.IsNullOrEmpty(value) && plate.Length>=8)
                {
                    if(CheckPlate())
                    {
                        CheckCredential();
                    }
                }

                OnPropertyChanged(nameof(Plate));

                ValidateButton();
            }
        }

        public string Vehicle
        {
            get => vehicle;
            set { vehicle = value; OnPropertyChanged(nameof(Vehicle)); ValidateButton(); }
        }

        public string Color
        {
            get => color;
            set { color = value; OnPropertyChanged(nameof(Color)); ValidateButton(); }
        }

        public string Prism
        {
            get => prism;
            set { prism = value; OnPropertyChanged(nameof(Prism)); ValidateButton(); }
        }

        public bool Loading
        {
            get => loading;
            set { loading = value; OnPropertyChanged(nameof(Loading)); }
        }

        public bool EnableButton
        {
            get => enableButton;
            set { enableButton = value; OnPropertyChanged(nameof(EnableButton)); }
        }

        public string Title => "Entrada de veículo";

        #endregion

        #region Comandos
        public Command<string> ActionPage { get; set; }
        public Command OpenVehiclePopup { get; set; }
        public Command OpenColorPopup { get; set; }
        #endregion

        public EntryViewModel(INavigation navigation)
        {
            Navigation = navigation;

            ActionPage = new Command<string>(ActionButton);
            OpenVehiclePopup = new Command(OpenVehiclePopupAction);
            OpenColorPopup = new Command(OpenColorPopupAction);

            if (AppContextGeneral.parkingInfo.AllowEntryWithoutPlate)
                EnableButton = true;

            AppContextGeneral.scannerDep.ClearDelegates();
            AppContextGeneral.scannerDep.OnScannerReader += ScannerDep_OnScannerReader;
        }

        private void ValidateButton()
        {
            EnableButton = !string.IsNullOrWhiteSpace(Plate)
                           && !string.IsNullOrWhiteSpace(Vehicle)
                           && !string.IsNullOrWhiteSpace(Color)
                           && !string.IsNullOrWhiteSpace(Prism);
        }

        private async void OpenVehiclePopupAction()
        {
            var popup = new SelectVehiclePopup();
            var result = await Application.Current.MainPage.ShowPopupAsync(popup);
            if (result is string selected)
                Vehicle = selected;
        }

        private async void OpenColorPopupAction()
        {
            var popup = new SelectColorPopup();
            var result = await Application.Current.MainPage.ShowPopupAsync(popup);
            if (result is string selected)
                Color = selected;
        }

        private void ScannerDep_OnScannerReader(string barCode)
        {
            Credential = barCode;

            if (!string.IsNullOrEmpty(barCode))
                CheckCredential();
            else
                Device.BeginInvokeOnMainThread(() => UserDialogs.Instance.HideLoading());
        }

        private void ActionButton(string parameter)
        {
            switch (parameter)
            {
                case "Cancel":
                    Application.Current.MainPage = new MenuPage();
                    break;

                case "Confirm":
                    ConfirmAction();
                    break;

                case "NextPage":
                    // código existente mantido
                    break;
            }
        }

        private void ConfirmAction()
        {
            ProcessEntry();
           
        }

        private void ProcessEntry()
        {
            UserDialogs.Instance.ShowLoading("Processando...");

            Task.Run(async () =>
            {
                AppParkingLot appParkingLot = new AppParkingLot();

                var response = appParkingLot.ProcessEntry(new ProcessEntryRequest()
                {
                    Color = this.Color,
                    Credential = this.Credential,
                    IDDevice = AppContextGeneral.configurationApp.IDDevice,
                    IDUser = AppContextGeneral.userInfo.IdUser,
                    ParkingCode = AppContextGeneral.parkingInfo.ParkingCode,
                    Plate = this.Plate,
                    Prism = this.Prism,
                    VehicleModel = this.Vehicle
                });

                if (response.Success)
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {

                    try
                    {

                        var print = Xamarin.Forms.DependencyService.Get<IPrinterService>();

                            print.PrintTicketEntry(new DependencyService.Model.PrintTicketInfoModel()
                            {
                                CredentialName = CredentialName,
                                CredentialNumber = Credential,
                                DateEntry = response.Data.DateEntry,
                                Plate = Plate,
                                Prism = Prism,
                                TicketNumber = response.Data.Ticket,
                                VehicleColor = Color,
                                VehicleModel = Vehicle
                            });
                            
                            /*Application.Current.MainPage.DisplayAlert(
                            "Sucesso",
                            $"Veículo: {Vehicle}\nCor: {Color}\nPlaca: {Plate}\nPrisma: {Prism} {response.Data.Ticket}",
                            "OK");*/
                            /*if (String.IsNullOrEmpty(this.Credential) || (!String.IsNullOrEmpty(this.Credential))
                            {
                                for (int i = 0; i < AppContextGeneral.parkingInfo.NumberPrintEntryCopies; i++)
                                {

                                    print.PrintTicketEntry(response.Data.Ticket, this.Credential, this.CredentialName, response.Data.DateEntry, this.Prism, this.Plate, this.VehicleModel, this.Color, response.Data.QrCodeContingency, (PrintTicketMobileLevel)AppContextGeneral.parkingInfo.PrintTicketMobileLevel, AppContextGeneral.parkingInfo.QrCodeFooter);
                                }
                            }*/

                            Application.Current.MainPage = new MenuPage();

                            UserDialogs.Instance.HideLoading();
                        }
                        catch (Exception ex)
                        {
                            UserDialogs.Instance.HideLoading();

                            Application.Current.MainPage.DisplayAlert("Erro", ex.Message, "Ok");
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

        private bool CheckPlate()
        {
            
            AppParkingLot appParkingLot = new AppParkingLot();

            var response = appParkingLot.ValidateEntryPlate(AppContextGeneral.parkingInfo.ParkingCode, this.Plate);

            if (response.Success)
            {
                return true;
            }
            else
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    UserDialogs.Instance.HideLoading();

                    Application.Current.MainPage.DisplayAlert("Erro", response.Message, "Ok");

                    Plate = null;
                });

                

                return false;
            }
        }

        private void CheckCredential()
        {
            AppCredential appCredential = new AppCredential();

            var response = appCredential.GetCredentialInfo(new GetCredentialInfoRequest()
            {
                AccessCode = null,
                IDDevice = AppContextGeneral.deviceInfo.IDDevice,
                ParkingCode = AppContextGeneral.parkingInfo.ParkingCode,
                Plate = this.Plate
            });

            Device.BeginInvokeOnMainThread(() =>
            {
                if (response.Success)
                {

                    if (response.Data.ClientActive)
                    {
                        if (response.Data.CredentialActive)
                        {
                            if (DateTime.Now >= response.Data.DateStart && DateTime.Now <= response.Data.DateEnd)
                            {
                                this.Credential = response.Data.Credential;
                                this.CredentialName = response.Data.Name;

                                UserDialogs.Instance.HideLoading();
                            }
                            else
                            {
                                this.Credential = null;
                                this.CredentialName = null;

                                UserDialogs.Instance.HideLoading();

                                this.CredentialName = "Credencial expirada";
                            }
                        }
                        else
                        {
                            this.Credential = null;
                            this.CredentialName = null;

                            UserDialogs.Instance.HideLoading();

                            this.CredentialName = "Credencial inativa";
                        }
                    }
                    else
                    {
                        this.Credential = null;
                        this.CredentialName = null;

                        UserDialogs.Instance.HideLoading();

                        this.CredentialName = "Cliente inativo";
                    }

                }
                else
                {
                    this.Credential = null;
                    this.CredentialName = null;
                }
            });
        
        }

        private void OnPropertyChanged(string nameProperty)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameProperty));
        }
    }
}
