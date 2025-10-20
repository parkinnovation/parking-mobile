using System;
namespace Parking.Mobile.Data.Model
{
    public class CredentialInfoModel
    {
        public string Code { get; set; }
        public bool Active { get; set; }
        public DateTime DateStart { get; set; }
        public DateTime DateEnd { get; set; }
        public string Name { get; set; }
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
}

