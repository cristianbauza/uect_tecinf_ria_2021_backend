using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DataAccessLayer;
using DataAccessLayer.Models;
using LibretaDigitalBackEnd.Models;

namespace LibretaDigitalBackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EstudiantesCursosController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public EstudiantesCursosController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/EstudiantesCursos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EstudiantesCursos>>> GetEstudiantesCursosPorCurso(long cursoId)
        {
            return await _context.EstudiantesCursos
                                .Include(x => x.Estudiante)
                                .Include(x => x.Curso)
                                .Include(x => x.Curso.Docente)
                                .Where(x => x.CursosId == cursoId)
                                .ToListAsync();
        }

        // GET: api/EstudiantesCursos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<EstudiantesCursos>> GetEstudiantesCursos(long id)
        {
            var estudiantesCursos = await _context.EstudiantesCursos
                                            .Include(x => x.Estudiante)
                                            .Include(x => x.Curso)
                                            .Include(x => x.Curso.Docente)
                                            .Where(x => x.id == id).FirstOrDefaultAsync();

            if (estudiantesCursos == null)
            {
                return NotFound();
            }

            return estudiantesCursos;
        }

        // PUT: api/EstudiantesCursos/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEstudiantesCursos(long id, EstudiantesCursos estudiantesCursos)
        {
            if (id != estudiantesCursos.id)
            {
                return BadRequest();
            }

            _context.Entry(estudiantesCursos).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EstudiantesCursosExists(id))
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

        // POST: api/EstudiantesCursos
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<EstudiantesCursos>> PostEstudiantesCursos(EstudianteCursoDTO estudiantesCursos)
        {
            EstudiantesCursos x = new EstudiantesCursos();
            x.Curso = _context.Cursos.Find(estudiantesCursos.CursoId);
            x.CursosId = estudiantesCursos.CursoId;
            x.Estudiante = _context.Estudiantes.Find(estudiantesCursos.EstudianteId);
            x.EstudiantesId = estudiantesCursos.EstudianteId;

            _context.EstudiantesCursos.Add(x);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetEstudiantesCursos", new { id = x.id }, x);
        }

        // DELETE: api/EstudiantesCursos/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEstudiantesCursos(long id)
        {
            var estudiantesCursos = await _context.EstudiantesCursos.FindAsync(id);
            if (estudiantesCursos == null)
            {
                return NotFound();
            }

            _context.EstudiantesCursos.Remove(estudiantesCursos);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool EstudiantesCursosExists(long id)
        {
            return _context.EstudiantesCursos.Any(e => e.id == id);
        }
    }
}
