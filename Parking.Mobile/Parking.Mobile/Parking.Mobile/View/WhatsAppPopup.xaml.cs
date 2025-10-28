using Xamarin.Forms;
using Xamarin.CommunityToolkit.UI.Views;
using System.Text.RegularExpressions;

namespace Parking.Mobile.View
{
    public partial class WhatsAppPopup : Popup
    {
        public WhatsAppPopup()
        {
            InitializeComponent();
            BindingContext = this;
        }

        public Command<string> CloseCommand => new Command<string>((param) =>
        {
            if (param == "Cancel")
                Dismiss(null);
            else
            {
                var phone = entPhone.Text?.Trim();
                if (IsValidPhone(phone))
                    Dismiss(phone);
                else
                    Application.Current.MainPage.DisplayAlert("Erro", "Número de telefone inválido. Use o formato (99) 99999-9999.", "OK");
            }
        });

        private bool IsValidPhone(string phone)
        {
            if (string.IsNullOrWhiteSpace(phone))
                return false;

            // Aceita formatos como (11) 91234-5678
            return Regex.IsMatch(phone, @"^\(?\d{2}\)?\s?\d{4,5}-?\d{4}$");
        }
    }
}
