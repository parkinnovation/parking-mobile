using System;
using System.Collections.Generic;
using Parking.Mobile.ViewModel;
using Xamarin.Forms;

namespace Parking.Mobile.View
{	
	public partial class MenuFinancialPage : ContentPage
	{	
		public MenuFinancialPage ()
		{
			InitializeComponent ();

            BindingContext = new MenuViewModel(true, this.Navigation);
        }
	}
}

