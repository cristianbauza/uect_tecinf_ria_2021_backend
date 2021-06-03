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
using Microsoft.AspNetCore.Identity;
using LibretaDigitalBackEnd.Models;

namespace LibretaDigitalBackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "ADMIN")]
    public class CursosController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public CursosController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: api/Cursos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Cursos>>> GetCursos()
        {
            return await _context.Cursos.ToListAsync();
        }

        // GET: api/Cursos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Cursos>> GetCursos(long id)
        {
            var cursos = await _context.Cursos.FindAsync(id);

            if (cursos == null)
            {
                return NotFound();
            }

            return cursos;
        }

        // PUT: api/Cursos/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCursos(long id, Cursos cursos)
        {
            if (id != cursos.Id)
            {
                return BadRequest();
            }

            _context.Entry(cursos).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CursosExists(id))
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

        // POST: api/Cursos
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Cursos>> PostCursos(CursoDTO cursos)
        {
            ApplicationUser user = (ApplicationUser)await _userManager.FindByIdAsync(cursos.UserId);
            Cursos curso = new Cursos() 
            {
                Descripcion = cursos.Descripcion,
                Docente = user, 
                Id = cursos.Id,
                Nombre = cursos.Nombre,
                Programa = cursos.Programa,
                DocenteId = user.Id
            };
            _context.Cursos.Add(curso);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCursos", new { id = curso.Id }, curso);
        }

        // DELETE: api/Cursos/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCursos(long id)
        {
            var cursos = await _context.Cursos.FindAsync(id);
            if (cursos == null)
            {
                return NotFound();
            }

            _context.Cursos.Remove(cursos);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CursosExists(long id)
        {
            return _context.Cursos.Any(e => e.Id == id);
        }
    }
}
