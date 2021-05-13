using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibretaDigitalBackEnd.Models
{
    public class LoginResponse
    {
        public string token { get; set; }
        public string email { get; set; }

        public LoginResponse(string token, string email)
        {
            this.token = token;
            this.email = email;
        }
    }
}
