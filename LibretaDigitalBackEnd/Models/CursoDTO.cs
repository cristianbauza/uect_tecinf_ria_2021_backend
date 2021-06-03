using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibretaDigitalBackEnd.Models
{
    public class CursoDTO
    {
        public long Id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public string Programa { get; set; }
        public string UserId { get; set; }
    }
}
