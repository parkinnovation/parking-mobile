using Xamarin.Forms;
using Xamarin.CommunityToolkit.UI.Views;
using Parking.Mobile.ViewModel;

namespace Parking.Mobile.View
{
    public partial class SelectVehiclePopup : Popup
    {
        public SelectVehiclePopup()
        {
            InitializeComponent();

            // Definindo o BindingContext
            BindingContext = new SelectVehiclePopupViewModel(this);
        }
    }
}
