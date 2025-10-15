using Xamarin.Forms;
using Xamarin.CommunityToolkit.UI.Views;
using Parking.Mobile.ViewModel;

namespace Parking.Mobile.View
{
    public partial class SelectColorPopup : Popup
    {
        public SelectColorPopup()
        {
            InitializeComponent();

            // Definindo o BindingContext
            BindingContext = new SelectColorPopupViewModel(this);
        }
    }
}

