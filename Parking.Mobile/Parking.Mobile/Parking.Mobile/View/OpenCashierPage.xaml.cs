using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Acr.UserDialogs;
using Parking.Mobile.ViewModel;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Forms;

namespace Parking.Mobile.View
{
    public partial class OpenCashierPage : Popup
    {
        public OpenCashierViewModel ViewModel => (OpenCashierViewModel)BindingContext;

        public OpenCashierPage()
        {
            InitializeComponent();

            BindingContext = new OpenCashierViewModel();
        }

        public void btnConfirm_Clicked(System.Object sender, System.EventArgs e)
        {
            UserDialogs.Instance.ShowLoading("Processando...");

            Task.Run(() =>
            {
                if (ViewModel.OpenCashier())
                {
                    Dismiss(null);
                }
            });
        }
    }
}

