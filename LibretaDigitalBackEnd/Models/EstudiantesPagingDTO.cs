using DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibretaDigitalBackEnd.Models
{
    public class EstudiantesPagingDTO
    {
        public List<Estudiantes> Lista { get; set; }
        public int Size { get; set; }
    }
}
