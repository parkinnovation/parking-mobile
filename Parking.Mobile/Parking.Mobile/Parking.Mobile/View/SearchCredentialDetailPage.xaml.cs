using System;
using System.Collections.Generic;
using Parking.Mobile.ViewModel;
using Xamarin.Forms;

namespace Parking.Mobile.View
{
    public partial class SearchCredentialDetailPage : ContentPage
    {
        public SearchCredentialDetailPage(int idClient)
        {
            InitializeComponent();

            BindingContext = new SearchCredentialDetailViewModel(this.Navigation, idClient);
        }
    }
}

