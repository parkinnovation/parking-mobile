using System;
using System.Collections.Generic;

namespace Parking.Mobile.Data.Model
{
    public class ClientInfoDetailModel
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
        public List<CredentialInfoModel> Credentials { get; set; }
        public List<PlateInfoModel> Plates { get; set; }
        public string State
        {
            get
            {
                if (Active)
                {
                    return "Ativo";
                }
                else
                {
                    return "Inativo";
                }
            }
        }

        public string DateStartText
        {
            get
            {
                return DateStart.ToString("dd/MM/yyyy");
            }
        }

        public string DateEndText
        {
            get
            {
                return DateEnd.ToString("dd/MM/yyyy");
            }
        }
    }

    public class PlateInfoModel
    {
        public string Plate { get; set; }
        public bool Active { get; set; }
        public string State
        {
            get
            {
                if (Active)
                {
                    return "Ativo";
                }
                else
                {
                    return "Inativo";
                }
            }
        }
    }
}

