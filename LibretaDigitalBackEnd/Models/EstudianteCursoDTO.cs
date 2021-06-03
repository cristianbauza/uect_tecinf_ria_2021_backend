using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibretaDigitalBackEnd.Models
{
    public class EstudianteCursoDTO
    {
        public long id { get; set; }
        public long EstudianteId { get; set; }
        public long CursoId { get; set; }
    }
}
