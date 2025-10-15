using System;
using System.Collections.Generic;
using Parking.Mobile.ViewModel;
using Xamarin.Forms;

namespace Parking.Mobile.View
{	
	public partial class MenuReportPage : ContentPage
	{	
		public MenuReportPage ()
		{
			InitializeComponent ();

            BindingContext = new MenuViewModel(true, this.Navigation);
        }
	}
}

