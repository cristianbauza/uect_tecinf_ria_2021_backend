using DataAccessLayer.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser> 
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        //protected override void OnModelCreating(ModelBuilder builder)
        //{
        //    builder.Entity<Estudiantes>()
        //        .HasIndex(u => u.Documento)
        //        .IsUnique();
        //}

        public virtual DbSet<Personas> Personas { get; set; }
        public virtual DbSet<Estudiantes> Estudiantes { get; set; }
        public virtual DbSet<Cursos> Cursos{ get; set; }
        public virtual DbSet<EstudiantesCursos> EstudiantesCursos { get; set; }
    }
}
