using System;
using System.Collections.Generic;

namespace Parking.Mobile.Interface.Message.Response
{
    public class ListClientResponse
    {
        public List<ClientInfo> Clients { get; set; }
    }

    public class ClientInfo
    {
        public int IDClient { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public bool Active { get; set; }
    }
}

