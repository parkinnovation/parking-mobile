using Xamarin.Forms;
using Xamarin.CommunityToolkit.UI.Views;
using Parking.Mobile.ViewModel;

namespace Parking.Mobile.View
{
    public partial class TicketOptionPopup : Popup
    {
        private bool showWithoutTicket;

        public bool ShowWithoutTicket { get => showWithoutTicket; set { showWithoutTicket = value; OnPropertyChanged(nameof(ShowWithoutTicket)); } }

        public TicketOptionPopup(bool _showWithoutTicket)
        {
            InitializeComponent();
            BindingContext = this;
            ShowWithoutTicket = _showWithoutTicket;
        }

        public Command<string> ClosePopupCommand => new Command<string>((option) =>
        {
            Dismiss(option);
        });
    }
}
