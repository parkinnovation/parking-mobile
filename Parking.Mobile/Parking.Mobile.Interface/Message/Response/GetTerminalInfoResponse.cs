using System;
using Parking.Mobile.Interface.Message.Enum;

namespace Parking.Mobile.Interface.Message.Response
{
    public class GetTerminalInfoResponse
    {
        public int IDDevice { get; set; }
        public string Description { get; set; }
        public bool PaymentInEntry { get; set; }
        public string TEFId { get; set; }
        public string TEFMobileId { get; set; }
        public TerminalTypeEnum Type { get; set; }
        public string TypeDescription { get; set; }
        public int? RPSSeries { get; set; }
        public bool NfceEnable { get; set; }
    }
}

