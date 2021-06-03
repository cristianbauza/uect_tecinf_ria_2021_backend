using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Models
{
    public class Cursos
    {
        public long Id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        [Column(TypeName = "LONGTEXT")]
        public string Programa { get; set; }
        public string DocenteId { get; set; }
        public ApplicationUser Docente { get; set; }
    }
}
