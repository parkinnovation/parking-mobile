using System;
using Acr.UserDialogs;
using Parking.Mobile.View;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using Xamarin.Forms;
using Parking.Mobile.Data.Model;
using Parking.Mobile.Common;

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
                //Thread.Sleep(200);

                //Device.BeginInvokeOnMainThread(() =>
                //{
                //    ActionButton("NextPage");
                //});
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
            if (string.IsNullOrEmpty(Plate) && string.IsNullOrEmpty(Ticket))
            {
                Application.Current.MainPage.DisplayAlert("Aviso", "Informe a placa ou o ticket.", "OK");
                return;
            }

            UserDialogs.Instance.ShowLoading("Buscando ticket...");
            Thread.Sleep(500); // Simula busca

            TicketInfo = new TicketInfoModel
            {
                Ticket = Ticket ?? "0000123456",
                Plate = Plate ?? "ABC-1234",
                DateEntry = DateTime.Now.AddHours(-2),
                VehicleModel = "Fusca",
                VehicleColor = "Azul",
                Stay = "2h"
            };

            IsSearchMode = false;
            IsTicketMode = true;
    
            UserDialogs.Instance.HideLoading();
        }

        private void ConfirmChange()
        {
            Application.Current.MainPage.DisplayAlert("Sucesso",
                $"Mudança confirmada!\n\nTicket: {TicketInfo.Ticket}",
                "OK");

            ResetScreen();
        }

        private void ResetScreen()
        {
            Application.Current.MainPage = new MenuPage();
        }

        private void OnPropertyChanged(string name) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}

