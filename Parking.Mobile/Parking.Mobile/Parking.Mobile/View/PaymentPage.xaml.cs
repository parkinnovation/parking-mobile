using System;
using System.Collections.Generic;
using Parking.Mobile.ViewModel;
using Xamarin.Forms;

namespace Parking.Mobile.View
{	
	public partial class PaymentPage : ContentPage
	{	
		public PaymentPage ()
		{
			InitializeComponent ();

            BindingContext = new PaymentViewModel(this.Navigation);
        }
	}
}

