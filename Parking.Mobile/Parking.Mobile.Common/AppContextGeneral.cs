using System;
using System.Collections.Generic;
using System.Data;
using System.Net.NetworkInformation;
using Parking.Mobile.Data.Model;
using Parking.Mobile.DependencyService.Interfaces;
using Parking.Mobile.Entity;

namespace Parking.Mobile.Common
{
    public static class AppContextGeneral
    {
        //public static Database databaseInstance;
        //public static ConfigurationApp configurationApp;
        public static string Model;
        public static string Version;
        public static string SerialNumber;

        public static ICamScanner scannerDep;

        public static ConfigurationApp configurationApp;
        public static UserInfoModel userInfo;
        public static ParkingInfoModel parkingInfo;
        public static DeviceInfoModel deviceInfo;
        public static CashierInfoModel cashierInfo;
    }
}

