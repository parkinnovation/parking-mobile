using System;
using Parking.Mobile.ApplicationCore;
using Parking.Mobile.Common;
using Parking.Mobile.Data.Repository;
using Parking.Mobile.Entity;
using Parking.Mobile.View;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Parking.Mobile
{
    public partial class App : Application
    {
        public App ()
        {
            InitializeComponent();

            MainPage = new LoginPage();
        }

        protected override void OnStart ()
        {
            AppContextGeneral.databaseInstance = new Database();

            AppConfiguration appConfiguration = new AppConfiguration();

            AppContextGeneral.configurationApp = appConfiguration.GetConfigurationApp();

            if (AppContextGeneral.configurationApp == null)
            {
                AppContextGeneral.configurationApp = new ConfigurationApp();

                AppContextGeneral.configurationApp.IDDevice = 0;
                AppContextGeneral.configurationApp.ParkingCode = "000000";
                AppContextGeneral.configurationApp.UrlWebApi = "http://177.155.199.151:5005/ctxgohub";
                
                appConfiguration.SaveConfigurationApp(AppContextGeneral.configurationApp);
            }

            


        }

        protected override void OnSleep ()
        {
        }

        protected override void OnResume ()
        {
        }
    }
}

