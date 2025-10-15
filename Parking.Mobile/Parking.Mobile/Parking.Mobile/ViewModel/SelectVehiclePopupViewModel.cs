using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.CommunityToolkit.UI.Views;

namespace Parking.Mobile.ViewModel
{
    public class SelectVehiclePopupViewModel : BindableObject
    {
        private readonly Popup popup;
        private string searchText;
        private string selectedVehicle;
        private bool showList;

        public ObservableCollection<string> Vehicles { get; set; }
        public ObservableCollection<string> FilteredList { get; set; }

        public ICommand CloseCommand { get; }

        public string SearchText
        {
            get => searchText;
            set
            {
                searchText = value;
                OnPropertyChanged();
                FilterVehicles();
            }
        }

        public string SelectedVehicle
        {
            get => selectedVehicle;
            set
            {
                if (value == null) return;

                selectedVehicle = value;
                OnPropertyChanged();

                // Fecha o popup passando o veículo selecionado
                popup.Dismiss(selectedVehicle);

                // Limpa a seleção
                SelectedVehicle = null;
            }
        }

        // Propriedade para controlar a visibilidade da lista
        public bool ShowList
        {
            get => showList;
            set
            {
                showList = value;
                OnPropertyChanged();
            }
        }

        public SelectVehiclePopupViewModel(Popup popupRef)
        {
            popup = popupRef;

            Vehicles = new ObservableCollection<string>
            {
                "Fiat Uno", "Honda Civic", "Toyota Corolla",
                "Chevrolet Onix", "VW Gol", "Hyundai HB20"
            };

            FilteredList = new ObservableCollection<string>();

            CloseCommand = new Command(() => popup.Dismiss(null));

            ShowList = false;
        }

        private void FilterVehicles()
        {
            if (string.IsNullOrWhiteSpace(SearchText))
            {
                FilteredList.Clear();
                ShowList = false; // não mostrar lista
            }
            else
            {
                var filtered = Vehicles
                    .Where(v => v.ToLower().Contains(SearchText.ToLower()))
                    .ToList();

                FilteredList = new ObservableCollection<string>(filtered);
                ShowList = FilteredList.Any(); // mostrar apenas se tiver resultado
            }

            OnPropertyChanged(nameof(FilteredList));
        }
    }
}
