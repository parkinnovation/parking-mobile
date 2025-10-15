using System;
using System.Collections.Generic;
using Parking.Mobile.ViewModel;
using Xamarin.Forms;

namespace Parking.Mobile.View
{	
	public partial class ConfigurationPage : ContentPage
	{	
		public ConfigurationPage ()
		{
            InitializeComponent();

            BindingContext = new ConfigurationViewModel();
        }
	}
}

