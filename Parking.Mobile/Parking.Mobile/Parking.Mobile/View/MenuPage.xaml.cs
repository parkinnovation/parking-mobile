using System;
using Parking.Mobile.ViewModel;
using Xamarin.Forms;

namespace Parking.Mobile.View
{
    public partial class MenuPage : MasterDetailPage
    {
        private readonly MenuViewModel _menuViewModel;

        public MenuPage()
        {
            InitializeComponent();

            _menuViewModel = new MenuViewModel(false, Navigation);
            BindingContext = _menuViewModel;

            // Adiciona ação de toque no botão "Sair"
            var tapGesture = new TapGestureRecognizer();
            tapGesture.Tapped += OnExitTapped;
            btnSair.GestureRecognizers.Add(tapGesture);
        }

        private async void OnExitTapped(object sender, EventArgs e)
        {
            bool confirm = await DisplayAlert("Confirmação", "Deseja realmente sair?", "Sim", "Não");
            if (confirm)
            {
                try
                {
                    _menuViewModel.ActionOpenPage?.Execute("Exit");
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Erro", $"Falha ao sair: {ex.Message}", "OK");
                }
            }
        }
    }
}
