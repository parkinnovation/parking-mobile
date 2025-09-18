using System;
using System.Collections.Generic;

namespace Parking.Mobile.Interface.Message.Response
{
    public class AuthenticateUserResponse
    {
        public int IdUser { get; set; }
        public string Name { get; set; }
        public string Profile { get; set; }
        public DateTime LastLogin { get; set; }
        public bool Disabled { get; set; }
        public bool ChangePasswordNextLogin { get; set; }
        public List<ProfilePermission> Permisions { get; set; }
    }

    public class ProfilePermission
    {
        public string Description { get; set; }
        public string Path { get; set; }
    }
}

