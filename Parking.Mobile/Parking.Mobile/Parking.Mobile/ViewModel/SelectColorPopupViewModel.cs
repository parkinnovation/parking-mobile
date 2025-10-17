using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.CommunityToolkit.UI.Views;
using Parking.Mobile.Common;
using System.Linq;

namespace Parking.Mobile.ViewModel
{
    public class SelectColorPopupViewModel : BindableObject
    {
        private readonly Popup popup;
        private string searchText;
        private string selectedColor;
        private bool showList;

        public ObservableCollection<string> Colors { get; set; }
        public ObservableCollection<string> FilteredList { get; set; }

        public ICommand CloseCommand { get; }

        public string SearchText
        {
            get => searchText;
            set
            {
                searchText = value;
                OnPropertyChanged();
                FilterColors();
            }
        }

        public string SelectedColor
        {
            get => selectedColor;
            set
            {
                if (value == null) return;

                selectedColor = value;
                OnPropertyChanged();

                // Fecha o popup passando o veículo selecionado
                popup.Dismiss(selectedColor);

                // Limpa a seleção
                selectedColor = null;
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

        public SelectColorPopupViewModel(Popup popupRef)
        {
            popup = popupRef;

            Colors = new ObservableCollection<string>(
                AppContextGeneral.colors.Select(l => l.Description)
            );

            FilteredList = new ObservableCollection<string>();

            CloseCommand = new Command(() => popup.Dismiss(null));

            ShowList = false;
        }

        private void FilterColors()
        {
            if (string.IsNullOrWhiteSpace(SearchText))
            {
                FilteredList.Clear();
                ShowList = false; // não mostrar lista
            }
            else
            {
                var filtered = Colors
                    .Where(v => v.ToLower().Contains(SearchText.ToLower()))
                    .ToList();

                FilteredList = new ObservableCollection<string>(filtered);
                ShowList = FilteredList.Any(); // mostrar apenas se tiver resultado
            }

            OnPropertyChanged(nameof(FilteredList));
        }
    }
}
