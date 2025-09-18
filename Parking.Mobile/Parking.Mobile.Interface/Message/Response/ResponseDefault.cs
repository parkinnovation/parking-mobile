using System;
namespace Parking.Mobile.Interface.Message.Response
{
    public class ResponseDefault<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }
        public bool ParkingOffline { get; set; }
    }
}

