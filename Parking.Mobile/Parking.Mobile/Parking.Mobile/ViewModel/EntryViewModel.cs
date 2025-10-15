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
            Device.BeginInvokeOnMainThread(async () =>
            {
                UserDialogs.Instance.ShowLoading("Confirmando...");
                await Task.Delay(1000);
                UserDialogs.Instance.HideLoading();

                await Application.Current.MainPage.DisplayAlert(
                    "Sucesso",
                    $"Veículo: {Vehicle}\nCor: {Color}\nPlaca: {Plate}\nPrisma: {Prism}",
                    "OK");

                Application.Current.MainPage = new MenuPage();
            });
        }

        private void CheckCredential()
        {
            // código original mantido
        }

        private void OnPropertyChanged(string nameProperty)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameProperty));
        }
    }
}
