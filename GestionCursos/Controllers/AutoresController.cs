using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BibliotecaMonolito.Models;

namespace BibliotecaMonolito.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AutoresController : ControllerBase
{
    private readonly LibraryDbContext _context;

    public AutoresController(LibraryDbContext context)
    {
        _context = context;
    }

    //controller get
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var autores = await _context.Autores.ToListAsync();
        return Ok(autores);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var autor = await _context.Autores.FindAsync(id);
        if (autor == null)
            return NotFound();

        return Ok(autor);
    }

    //controller create
    [HttpPost]
    public async Task<IActionResult> Create(Autor autor)
    {
        _context.Autores.Add(autor);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = autor.Id }, autor);
    }

    //controller update
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, Autor autor)
    {
        if (id != autor.Id)
            return BadRequest("El id no coincide con el id del cuerpo.");

        var existente = await _context.Autores.FindAsync(id);
        if (existente == null)
            return NotFound();

        existente.Nombre = autor.Nombre;
        existente.Nacionalidad = autor.Nacionalidad;

        await _context.SaveChangesAsync();

        return NoContent();
    }
    
    //controller delete
     [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var autor = await _context.Autores.FindAsync(id);
        if (autor == null)
            return NotFound();

        _context.Autores.Remove(autor);
        await _context.SaveChangesAsync();

        return NoContent();
    }

}