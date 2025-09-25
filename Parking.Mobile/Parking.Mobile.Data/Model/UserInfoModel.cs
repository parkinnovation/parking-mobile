using System;
using System.Collections.Generic;

namespace Parking.Mobile.Data.Model
{
    public class UserInfoModel
    {
        public int IdUser { get; set; }
        public string Name { get; set; }
        public string Profile { get; set; }
        public DateTime LastLogin { get; set; }
        public List<UserProfilePermissionModel> Permisions { get; set; }
        public bool ShowParkingLotCounter { get; set; }
        public bool ShowEntry { get; set; }
        public bool ShowPayment { get; set; }
        public bool ShowTicketLost { get; set; }
        public bool ShowFinancialReport { get; set; }
        public bool ShowOpenCloseCashier { get; set; }
        public bool ShowCancelTicket { get; set; }
        public bool ShowSearchCredential { get; set; }
    }

    public class UserProfilePermissionModel
    {
        public string Description { get; set; }
        public string Path { get; set; }
    }
}

