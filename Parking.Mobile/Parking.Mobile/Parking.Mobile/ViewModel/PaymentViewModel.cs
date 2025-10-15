using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using Xamarin.Forms;
using Acr.UserDialogs;
using Parking.Mobile.ApplicationCore;
using Parking.Mobile.View;

namespace Parking.Mobile.ViewModel
{
    public class PaymentViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private string plate;
        public string Plate { get => plate; set { plate = value; OnPropertyChanged(nameof(Plate)); } }

        private string ticket;
        public string Ticket { get => ticket; set { ticket = value; OnPropertyChanged(nameof(Ticket)); } }

        private bool isSearchMode = true;
        public bool IsSearchMode { get => isSearchMode; set { isSearchMode = value; OnPropertyChanged(nameof(IsSearchMode)); } }

        private bool isTicketMode;
        public bool IsTicketMode { get => isTicketMode; set { isTicketMode = value; OnPropertyChanged(nameof(IsTicketMode)); } }

        private bool isPaymentMode;
        public bool IsPaymentMode { get => isPaymentMode; set { isPaymentMode = value; OnPropertyChanged(nameof(IsPaymentMode)); } }

        private TicketInfoModel ticketInfo;
        public TicketInfoModel TicketInfo { get => ticketInfo; set { ticketInfo = value; OnPropertyChanged(nameof(TicketInfo)); } }

        public List<string> PriceTables { get; set; } = new List<string> { "Normal", "Premium", "VIP" };

        public List<string> Discounts { get; set; } = new List<string> { "NETPARK", "10%" };

        private string selectedPriceTable;
        public string SelectedPriceTable
        {
            get => selectedPriceTable;
            set
            {
                selectedPriceTable = value;
                OnPropertyChanged(nameof(SelectedPriceTable));
                if (!string.IsNullOrEmpty(selectedPriceTable))
                {
                    UpdateCalculatedPrice();
                    IsPaymentMode = true;
                }
            }
        }

        private string selectedDiscount;
        public string SelectedDiscount
        {
            get => selectedDiscount;
            set
            {
                selectedDiscount = value;
                OnPropertyChanged(nameof(SelectedDiscount));
                /*if (!string.IsNullOrEmpty(selectedDiscount))
                {
                    UpdateCalculatedPrice();
                    IsPaymentMode = true;
                }*/
            }
        }

        private decimal calculatedPrice;
        public decimal CalculatedPrice { get => calculatedPrice; set { calculatedPrice = value; OnPropertyChanged(nameof(CalculatedPrice)); } }

        public List<string> PaymentMethods { get; set; } = new List<string> { "Dinheiro", "Cartão", "PIX" };

        private string selectedPaymentMethod;
        public string SelectedPaymentMethod { get => selectedPaymentMethod; set { selectedPaymentMethod = value; OnPropertyChanged(nameof(SelectedPaymentMethod)); } }

        public Command<string> ActionPage { get; }
        private readonly INavigation Navigation;

        public PaymentViewModel(INavigation navigation)
        {
            Navigation = navigation;
            ActionPage = new Command<string>(ActionButton);
        }

        private void ActionButton(string parameter)
        {
            switch (parameter)
            {
                case "Search":
                    SearchTicket();
                    break;

                case "Scanner":
                    Application.Current.MainPage.DisplayAlert("Scanner", "Abrindo leitor de código...", "OK");
                    // Aqui você pode chamar seu serviço de scanner ZXing
                    break;

                case "Confirm":
                    ConfirmPayment();
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
                Stay = "2h",
                BasePrice = 20m
            };

            IsSearchMode = false;
            IsTicketMode = true;
            IsPaymentMode = false;

            UserDialogs.Instance.HideLoading();
        }

        private void ConfirmPayment()
        {
            if (string.IsNullOrEmpty(SelectedPaymentMethod))
            {
                Application.Current.MainPage.DisplayAlert("Aviso", "Selecione a forma de pagamento.", "OK");
                return;
            }

            Application.Current.MainPage.DisplayAlert("Sucesso",
                $"Pagamento confirmado!\n\nTicket: {TicketInfo.Ticket}\nPreço: R$ {CalculatedPrice:F2}\nForma: {SelectedPaymentMethod}",
                "OK");

            ResetScreen();
        }

        private void UpdateCalculatedPrice()
        {
            if (TicketInfo == null) return;

            CalculatedPrice = SelectedPriceTable switch
            {
                "Normal" => TicketInfo.BasePrice,
                "Premium" => TicketInfo.BasePrice * 1.2m,
                "VIP" => TicketInfo.BasePrice * 1.5m,
                _ => TicketInfo.BasePrice
            };
        }

        private void ResetScreen()
        {
            Application.Current.MainPage = new MenuPage();
        }

        private void OnPropertyChanged(string name) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }

    public class TicketInfoModel
    {
        public string Ticket { get; set; }
        public string Plate { get; set; }
        public DateTime DateEntry { get; set; }
        public string VehicleModel { get; set; }
        public string VehicleColor { get; set; }
        public string Stay { get; set; }
        public decimal BasePrice { get; set; }
    }
}
