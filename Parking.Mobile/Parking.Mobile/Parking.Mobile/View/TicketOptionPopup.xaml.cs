using Xamarin.Forms;
using Xamarin.CommunityToolkit.UI.Views;
using Parking.Mobile.ViewModel;

namespace Parking.Mobile.View
{
    public partial class TicketOptionPopup : Popup
    {
        public TicketOptionPopup()
        {
            InitializeComponent();
            BindingContext = this;
        }

        public Command<string> ClosePopupCommand => new Command<string>((option) =>
        {
            Dismiss(option);
        });
    }
}
