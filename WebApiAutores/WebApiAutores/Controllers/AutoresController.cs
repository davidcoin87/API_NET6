using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiAutores.Entidades;

namespace WebApiAutores.Controllers
{
    [ApiController]
    [Route("api/autores")]
    public class AutoresController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public AutoresController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]//ruta por defecto como metodo Get
        [HttpGet("listado")]//ruta modificada api/autores/listado
        [HttpGet("/listado")]//ruta directa /listado
        public async Task<ActionResult<List<Autor>>> Get()
        {
            //return new List<Autor>() {
            //    new Autor() { Id = 1, Nombre = "Felipe" },
            //    new Autor() { Id = 2, Nombre = "Daniel" },
            //    new Autor() { Id = 3, Nombre = "Sandra" }
            //};

            return await _context.Autores.Include(x => x.Libros).ToListAsync();

        }

        [HttpGet("primero")]
        public async Task<ActionResult<Autor>> PrimerAutor()
        {
            return await _context.Autores.FirstOrDefaultAsync();
        }

        //Prueba de parametro opcional mas abajo
        //[HttpGet("{id:int}")]
        //public async Task<ActionResult<Autor>> GetById(int id)
        //{
        //    var autor = await _context.Autores.FirstOrDefaultAsync(x => x.Id == id);

        //    if(autor == null)
        //    {
        //        return NotFound("Id no existe");
        //    }

        //    return autor;
        //}

        [HttpGet("{nombre}")]
        public async Task<ActionResult<Autor>> GetByName(string nombre)
        {
            var autor = await _context.Autores.FirstOrDefaultAsync(x => x.Nombre.Contains(nombre));

            if (autor == null)
            {
                return NotFound("Id no existe");
            }

            return autor;
        }

        [HttpGet("{id:int}/{param2=persona}")]//puedo convertir un parametro como opcional agregando ? al final
        public async Task<ActionResult<Autor>> GetByData(int id, string param2)
        {
            var autor = await _context.Autores.FirstOrDefaultAsync(x => x.Id == id);

            if (autor == null)
            {
                return NotFound("Id no existe");
            }

            return autor;
        }

        [HttpPost]
        public async Task<ActionResult> Post(Autor autor)
        {

            _context.Add(autor);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPut("{id:int}")] // api/autores/id
        public async Task<ActionResult> Put (Autor autor, int id)
        {
            var existe = await _context.Autores.AnyAsync(x => x.Id == id);

            if (!existe)
            {
                return NotFound();
            }

            if (autor.Id != id)
            {
                return BadRequest("El id del autor no coincide con el id de la URL");
            }

            _context.Update(autor);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete("{id:int}")] // api/autores/id
        public async Task<ActionResult> Delete (int id)
        {
            var existe = await _context.Autores.AnyAsync(x => x.Id == id);

            if(!existe)
            {
                return NotFound();
            }

            _context.Remove(new Autor() { Id = id });
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
