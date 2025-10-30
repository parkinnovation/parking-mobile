using System;
using Parking.Mobile.ApplicationCore;
using Parking.Mobile.Common;
using Parking.Mobile.Entity;
using Parking.Mobile.View;
using System.Collections.Generic;
using System.ComponentModel;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Parking.Mobile.ViewModel
{
    public class ConfigurationViewModel : INotifyPropertyChanged
    {

        private AppConfiguration appConfiguration = new AppConfiguration();
        private ConfigurationApp configuration;
        private string parkingCode;
        private int idDevice;
        private string urlService;
        private bool enableButton;
        
        public Command<string> ActionPage { get; set; }

        public string AndroidVersion
        {
            get
            {
                return DeviceInfo.Platform.ToString() + " " + DeviceInfo.VersionString;
            }
        }
        
        public bool EnableButton
        {
            set
            {
                enableButton = value;

                OnPropertyChanged("EnableButton");
            }

            get
            {
                return enableButton;
            }
        }

        public string ParkingCode
        {
            get
            {
                return this.parkingCode;
            }

            set
            {
                this.parkingCode = value;

                if (!String.IsNullOrEmpty(this.UrlService) && !String.IsNullOrEmpty(this.ParkingCode) && this.ParkingCode.Length > 0 && this.IDDevice > 0 && this.UrlService.Length >= 10)
                {
                    this.EnableButton = true;
                }
                else
                {
                    this.EnableButton = false;
                }

                OnPropertyChanged("ParkingCode");
            }
        }

        public int IDDevice
        {
            get
            {
                return this.idDevice;
            }

            set
            {
                this.idDevice = value;

                if (!String.IsNullOrEmpty(this.UrlService) && !String.IsNullOrEmpty(this.ParkingCode) && this.ParkingCode.Length > 0 && this.IDDevice > 0 && this.UrlService.Length >= 10)
                {
                    this.EnableButton = true;
                }
                else
                {
                    this.EnableButton = false;
                }

                OnPropertyChanged("IDDevice");
            }
        }

        public string UrlService
        {
            get
            {
                return this.urlService;
            }

            set
            {
                this.urlService = value;

                if (!String.IsNullOrEmpty(this.UrlService) && !String.IsNullOrEmpty(this.ParkingCode) && this.ParkingCode.Length > 0 && this.IDDevice > 0 && this.UrlService.Length >= 10)
                {
                    this.EnableButton = true;
                }
                else
                {
                    this.EnableButton = false;
                }

                OnPropertyChanged("UrlService");
            }
        }

        public ConfigurationViewModel()
        {
            this.configuration = appConfiguration.GetConfigurationApp();

            this.ParkingCode = this.configuration.ParkingCode;
            this.IDDevice = this.configuration.IDDevice;
            this.UrlService = this.configuration.UrlWebApi;
            
            ActionPage = new Command<string>(ActionButton);
        }

        private void ActionButton(string parameter)
        {
            if (parameter == "Confirm")
            {
                this.configuration.ParkingCode = this.ParkingCode;
                this.configuration.IDDevice = this.IDDevice;
                this.configuration.UrlWebApi = this.UrlService;
                
                appConfiguration.SaveConfigurationApp(configuration);

                AppContextGeneral.configurationApp = Util.Clone<ConfigurationApp>(configuration);

                Application.Current.MainPage = new LoginPage();
            }
            else
            {
                Application.Current.MainPage = new LoginPage();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string nameProperty)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(nameProperty));
            }
        }
    }
}

