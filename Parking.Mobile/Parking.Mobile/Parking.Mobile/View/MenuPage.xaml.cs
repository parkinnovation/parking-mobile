using System;
using System.Collections.Generic;
using Parking.Mobile.Model;
using Parking.Mobile.ViewModel;
using Xamarin.Forms;

namespace Parking.Mobile.View
{	
	public partial class MenuPage : MasterDetailPage
    {
        private MenuViewModel _menuViewModel;

        public MenuPage()
        {
            InitializeComponent();

            _menuViewModel = new MenuViewModel(false, this.Navigation);

            BindingContext = _menuViewModel;

            lstMenu.ItemTapped += LstMenu_ItemTapped;
        }

        private void LstMenu_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            MenuModel menuViewModel = (MenuModel)((ListView)sender).SelectedItem;

            _menuViewModel.ActionOpenPage.Execute(menuViewModel.Action);
        }
    }
}

