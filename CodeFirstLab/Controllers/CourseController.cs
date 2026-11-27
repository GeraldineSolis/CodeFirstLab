using CodeFirstLab.Data;
using CodeFirstLab.Models;
using CodeFirstLab.Requests;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CodeFirstLab.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CourseController : ControllerBase
{
    private readonly AppDbContext _db;
    public CourseController(AppDbContext db) => _db = db;

    // Insertar Curso
    [HttpPost]
    public async Task<IActionResult> Insert(InsertCourseRequest request)
    {
        var course = new Course { Name = request.Name, Credit = request.Credit };
        _db.Courses.Add(course);
        await _db.SaveChangesAsync();
        return Ok(new { course.Id, course.Name, course.Credit });
    }

    // Eliminar Curso
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete([FromRoute] DeleteCourseRequest request)
    {
        var course = await _db.Courses.FindAsync(request.Id);
        if (course is null) return NotFound(new { message = "Curso no encontrado." });

        _db.Courses.Remove(course);
        await _db.SaveChangesAsync();
        return NoContent();
    }

    // Eliminar Lista de Cursos
    [HttpPost("delete-list")]
    public async Task<IActionResult> DeleteList(DeleteCourseListRequest request)
    {
        var courses = await _db.Courses
            .Where(c => request.CourseIds.Contains(c.Id))
            .ToListAsync();

        if (courses.Count == 0)
            return NotFound(new { message = "Ningún curso encontrado." });

        _db.Courses.RemoveRange(courses);
        await _db.SaveChangesAsync();
        return Ok(new { deleted = courses.Count });
    }
}