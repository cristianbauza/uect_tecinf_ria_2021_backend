using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DataAccessLayer;
using DataAccessLayer.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;

namespace LibretaDigitalBackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "ADMIN")]
    public class EstudiantesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public EstudiantesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Estudiantes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Estudiantes>>> GetEstudiantes()
        {
            return await _context.Estudiantes.ToListAsync();
        }

        // GET: api/Estudiantes/Paging
        [HttpGet("api/Estudiantes/Paging")]
        public async Task<ActionResult<IEnumerable<Estudiantes>>> GetEstudiantesPaging(int size, int index, string nombre = "", string apellido = "", string documento = "")
        {
            Models.EstudiantesPagingDTO result = new Models.EstudiantesPagingDTO();
            result.Lista = await _context.Estudiantes
                .Where(e => e.Documento.Contains(documento) && 
                            (e.PrimerNombre.Contains(nombre) || e.SegundoNombre.Contains(nombre)) &&
                            (e.PrimerApellido.Contains(apellido) || e.SegundoApellido.Contains(apellido)))
                .Skip((index-1) * size)
                .Take(size)                
                .ToListAsync();
            result.Size = _context.Estudiantes
                .Where(e => e.Documento.Contains(documento) &&
                            (e.PrimerNombre.Contains(nombre) || e.SegundoNombre.Contains(nombre)) &&
                            (e.PrimerApellido.Contains(apellido) || e.SegundoApellido.Contains(apellido)))
                .Count();

            return Ok(result);
        }

        // GET: api/Estudiantes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Estudiantes>> GetEstudiantes(long id)
        {
            var estudiantes = await _context.Estudiantes.FindAsync(id);

            if (estudiantes == null)
            {
                return NotFound();
            }

            return estudiantes;
        }

        // PUT: api/Estudiantes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEstudiantes(long id, Estudiantes estudiantes)
        {
            if (id != estudiantes.Id)
            {
                return BadRequest();
            }

            _context.Entry(estudiantes).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EstudiantesExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Estudiantes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Estudiantes>> PostEstudiantes(Estudiantes estudiantes)
        {
            _context.Estudiantes.Add(estudiantes);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetEstudiantes", new { id = estudiantes.Id }, estudiantes);
        }

        // DELETE: api/Estudiantes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEstudiantes(long id)
        {
            var estudiantes = await _context.Estudiantes.FindAsync(id);
            if (estudiantes == null)
            {
                return NotFound();
            }

            _context.Estudiantes.Remove(estudiantes);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool EstudiantesExists(long id)
        {
            return _context.Estudiantes.Any(e => e.Id == id);
        }
    }
}
