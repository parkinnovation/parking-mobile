using System;
using System.Collections.Generic;
using Parking.Mobile.ViewModel;
using Xamarin.Forms;

namespace Parking.Mobile.View
{	
	public partial class LoginPage : ContentPage
	{	
		public LoginPage ()
		{
            InitializeComponent();

            BindingContext = new LoginViewModel(this.Navigation);
        }
	}
}

