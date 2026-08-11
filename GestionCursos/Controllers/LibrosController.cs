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
}