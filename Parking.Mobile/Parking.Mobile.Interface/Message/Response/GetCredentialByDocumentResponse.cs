using System;
using System.Collections.Generic;

namespace Parking.Mobile.Interface.Message.Response
{
    public class GetCredentialByDocumentResponse
    {
        public List<CredentialItemInfo> Credentials { get; set; }
    }

    public class CredentialItemInfo
    {
        public string Code { get; set; }
        public bool Active { get; set; }
        public DateTime DateStart { get; set; }
        public DateTime DateEnd { get; set; }
        public string Name { get; set; }
        public int IDType { get; set; }
        public string TypeDescription { get; set; }
    }
}

