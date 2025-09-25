using System;
using SQLite;

namespace Parking.Mobile.Entity
{
    [Table("ConfigurationApp")]

    public class ConfigurationApp
    {
        [PrimaryKey, MaxLength(36)]
        public string IDConfigurationApp { get; set; }

        [MaxLength(10)]
        public string ParkingCode { get; set; }

        public Int32 IDDevice { get; set; }

        [MaxLength(100)]
        public string UrlWebApi { get; set; }

    }
}

