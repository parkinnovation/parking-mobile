using System;
using System.Collections.Generic;
using Parking.Mobile.ViewModel;
using Xamarin.Forms;

namespace Parking.Mobile.View
{	
	public partial class ChangeSectorPage : ContentPage
	{	
		public ChangeSectorPage ()
		{
			InitializeComponent ();

            BindingContext = new ChangeSectorViewModel(this.Navigation);
        }
	}
}

