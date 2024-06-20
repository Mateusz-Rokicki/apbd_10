using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Context;
using WebApplication1.DTO;
using WebApplication1.Models;

namespace WebApplication1.Controller;
[Microsoft.AspNetCore.Components.Route("api/doctors")]
[ApiController]
public class DoctorsCotroller : ControllerBase
{
    private readonly AddDbContext _context;

    public DoctorsCotroller(AddDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Doctor>>>getDocc()
    {
        return await _context.Doctors.ToListAsync();
    }
    

    [HttpGet("{id}")]
    public async Task<ActionResult<Doctor>> getDoc(int id)
    {
        var doc = await _context.Doctors.FindAsync(id);

        if (doc != null)
        {
            return doc;
        }

        return NotFound();
    }

    [HttpPost]
    public async Task<ActionResult<Doctor>> postDoc(Doctor doc)
    {
        _context.Doctors.Add(doc);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(getDoc), new { id = doc.idDoctor }, doc);

    }

    [HttpPut]
    public async Task<IActionResult> putDoc(int id,Doctor doc)
    {
        if (id != doc.idDoctor)
        {
            return NotFound();
        }

        _context.Entry(doc).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete]
    public async Task<IActionResult> delDoc(int id)
    {
        var doc = await _context.Doctors.FindAsync(id);
        if (doc == null)
        {
            return NotFound();
        }

        _context.Doctors.Remove(doc);
        await _context.SaveChangesAsync();
        return NoContent();
    }
    
    
}