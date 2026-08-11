using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BibliotecaMonolito.Models;

namespace BibliotecaMonolito.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LibrosController : ControllerBase
{
    private readonly LibraryDbContext _context;

    public LibrosController(LibraryDbContext context)
    {
        _context = context;
    }

    //controller get
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var libros = await _context.Libros.Include(l => l.Autor).ToListAsync();
        return Ok(libros);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var libro = await _context.Libros.Include(l => l.Autor)
            .FirstOrDefaultAsync(l => l.Id == id);
        if (libro == null)
            return NotFound();

        return Ok(libro);
    }

    //controller create
     [HttpPost]
    public async Task<IActionResult> Create(Libro libro)
    {
        var autorExiste = await _context.Autores.AnyAsync(a => a.Id == libro.AutorId);
        if (!autorExiste)
            return BadRequest($"No existe un autor con id {libro.AutorId}.");

        _context.Libros.Add(libro);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = libro.Id }, libro);
    }

    //controller update
     [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, Libro libro)
    {
        if (id != libro.Id)
            return BadRequest("El id de la ruta no coincide con el id del cuerpo.");

        var existente = await _context.Libros.FindAsync(id);
        if (existente == null)
            return NotFound();

        existente.Titulo = libro.Titulo;
        existente.AnioPublicacion = libro.AnioPublicacion;
        existente.AutorId = libro.AutorId;

        await _context.SaveChangesAsync();

        return NoContent();
    }
}