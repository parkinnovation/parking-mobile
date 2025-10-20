using System;
using System.Collections.Generic;

namespace Parking.Mobile.Interface.Message.Response
{
    public class GetClientResponse
    {
        public int IDClient { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public DateTime DateStart { get; set; }
        public DateTime DateEnd { get; set; }
        public bool Active { get; set; }
        public string Document { get; set; }
        public string GroupName { get; set; }
        public int VacancyLimit { get; set; }
        public List<CredentialItemInfo> Credentials { get; set; }
        public List<PlateInfo> Plates { get; set; }
    }

    public class PlateInfo
    {
        public string Plate { get; set; }
        public bool Active { get; set; }
    }
}

