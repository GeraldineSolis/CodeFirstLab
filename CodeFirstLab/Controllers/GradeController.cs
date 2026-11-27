using CodeFirstLab.Data;
using CodeFirstLab.Models;
using CodeFirstLab.Requests;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CodeFirstLab.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GradeController : ControllerBase
{
    private readonly AppDbContext _db;
    public GradeController(AppDbContext db) => _db = db;

    // Insertar Grado
    [HttpPost]
    public async Task<IActionResult> Insert(InsertGradeRequest request)
    {
        var grade = new Grade { Name = request.Name, Description = request.Description };
        _db.Grades.Add(grade);
        await _db.SaveChangesAsync();
        return Ok(new { grade.Id, grade.Name, grade.Description });
    }

    // Eliminar Grado
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete([FromRoute] DeleteGradeRequest request)
    {
        var grade = await _db.Grades.FindAsync(request.Id);
        if (grade is null) return NotFound(new { message = "Grado no encontrado." });

        var hasStudents = await _db.Students.AnyAsync(s => s.GradeId == request.Id);
        if (hasStudents)
            return Conflict(new { message = "No se puede eliminar: el grado tiene estudiantes." });

        _db.Grades.Remove(grade);
        await _db.SaveChangesAsync();
        return NoContent();
    }
}