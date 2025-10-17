using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Acr.UserDialogs;
using Parking.Mobile.ViewModel;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Forms;

namespace Parking.Mobile.View
{
    public partial class CloseCashierPage : Popup
    {
        public CloseCashierViewModel ViewModel => (CloseCashierViewModel)BindingContext;

        public CloseCashierPage()
        {
            InitializeComponent();

            BindingContext = new CloseCashierViewModel();
        }

        public void btnConfirm_Clicked(System.Object sender, System.EventArgs e)
        {
            UserDialogs.Instance.ShowLoading("Processando...");

            Task.Run(() =>
            {
                if (ViewModel.CloseCashier())
                {
                    Dismiss(true);
                }
            });
        }

        public void btnCancel_Clicked(System.Object sender, System.EventArgs e)
        {
            Dismiss(false);
        }
    }
}

