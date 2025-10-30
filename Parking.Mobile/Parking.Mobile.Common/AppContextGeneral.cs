using System;
using System.Collections.Generic;
using System.Data;
using System.Net.NetworkInformation;
using Parking.Mobile.Data.Model;
using Parking.Mobile.Data.Repository;
using Parking.Mobile.DependencyService.Interfaces;
using Parking.Mobile.Entity;
using Parking.Mobile.Interface.Message.Response;

namespace Parking.Mobile.Common
{
    public static class AppContextGeneral
    {
        public static Database databaseInstance;
        //public static ConfigurationApp configurationApp;
        public static string Model;
        public static string Version;
        public static string SerialNumber;

        public static ICamScanner scannerDep;

        private static ConfigurationApp _configurationApp;

        public static ConfigurationApp configurationApp
        {
            get
            {
                return _configurationApp;
            }

            set
            {
                _configurationApp = value;

                if(SignalRClient!=null)
                {
                    SignalRClient.StopAsync();
                    SignalRClient = null;
                }

                if (value != null)
                {
                    SignalRClient = new SignalRClientService(value.UrlWebApi);

                    SignalRClient.StartAsync();
                }
            }
        }
        

        public static UserInfoModel userInfo;
        public static ParkingInfoModel parkingInfo;
        public static DeviceInfoModel deviceInfo;
        public static CashierInfoModel cashierInfo;
        public static List<VehicleModelInfo> vehicles;
        public static List<ColorInfo> colors;

        public static SignalRClientService SignalRClient { get; set; }
    }
}

