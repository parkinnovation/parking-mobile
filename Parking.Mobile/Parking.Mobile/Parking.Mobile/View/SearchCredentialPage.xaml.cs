using System;
using System.Collections.Generic;
using Parking.Mobile.Data.Model;
using Parking.Mobile.ViewModel;
using Xamarin.Forms;

namespace Parking.Mobile.View
{
    public partial class SearchCredentialPage : ContentPage
    {
        public SearchCredentialViewModel ViewModel => (SearchCredentialViewModel)BindingContext;

        public SearchCredentialPage()
        {
            InitializeComponent();

            BindingContext = new SearchCredentialViewModel(this.Navigation);
        }

        void ListView_ItemTapped(System.Object sender, Xamarin.Forms.ItemTappedEventArgs e)
        {
            ViewModel.SelectItem(((ClientInfoModel)e.Item).IDClient);
        }
    }
}

