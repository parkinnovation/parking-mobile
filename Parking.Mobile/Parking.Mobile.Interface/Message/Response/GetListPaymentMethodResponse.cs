using System;
using System.Collections.Generic;

namespace Parking.Mobile.Interface.Message.Response
{
    public class GetListPaymentMethodResponse
    {
        public List<PaymentMethodInfo> Methods { get; set; }
    }

    public class PaymentMethodInfo
    {
        public int IDPaymentMethod { get; set; }
        public string Description { get; set; }
        public int Type { get; set; }
        public bool Osa { get; set; }
        public bool DigitalWallet { get; set; }
        public string DigitalWalletCode { get; set; }
    }
}

