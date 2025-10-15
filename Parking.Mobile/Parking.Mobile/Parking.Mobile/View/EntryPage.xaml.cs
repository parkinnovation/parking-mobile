using System;
using System.Collections.Generic;
using Parking.Mobile.ViewModel;
using Xamarin.Forms;

namespace Parking.Mobile.View
{	
	public partial class EntryPage : ContentPage
	{	
		public EntryPage ()
		{
			InitializeComponent ();

            BindingContext = new EntryViewModel(this.Navigation);
        }
	}
}

