using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Parking.Mobile.Common;
using Parking.Mobile.ViewModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Parking.Mobile.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TabbedInitialPage : TabbedPage
    {
        public TabbedInitialPage ()
        {
            InitializeComponent();

            this.Title = AppContextGeneral.userInfo.Name;

            BindingContext = new TabbedInitialViewModel();
        }
    }
}
