using System;
using System.Collections.Generic;
using Parking.Mobile.ViewModel;
using Xamarin.Forms;

namespace Parking.Mobile.View
{	
	public partial class PrintTicketPage : ContentPage
	{	
		public PrintTicketPage ()
		{
			InitializeComponent ();

            BindingContext = new PrintTicketViewModel(this.Navigation);
        }
	}
}

