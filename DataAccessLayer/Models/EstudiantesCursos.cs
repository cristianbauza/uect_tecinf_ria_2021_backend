using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Models
{
    public class EstudiantesCursos
    {
        public long id { get; set; }
        public long CursosId { get; set; }
        public Cursos Curso { get; set; }
        public long EstudiantesId { get; set; }
        public Estudiantes Estudiante { get; set; }
    }
}
