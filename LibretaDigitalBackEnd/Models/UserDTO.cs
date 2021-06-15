using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibretaDigitalBackEnd.Models
{
    public class UserDTO
    {
        public string Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Nombre { get; set; }
        public string Fotos { get; set; }
        public List<string> Roles { get; set; }

        public UserDTO()
        {
            Roles = new List<string>();
        }
    }
}
