using CodeFirstLab.Data;
using CodeFirstLab.Models;
using CodeFirstLab.Requests;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CodeFirstLab.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EnrollmentController : ControllerBase
{
    private readonly AppDbContext _db;
    public EnrollmentController(AppDbContext db) => _db = db;

    // Matricular
    [HttpPost]
    public async Task<IActionResult> Enroll(EnrollRequest request)
    {
        if (!await _db.Students.AnyAsync(s => s.Id == request.IdStudent))
            return NotFound(new { message = "Estudiante no encontrado." });

        var courseIds = request.CourseIds.Distinct().ToList();

        var validIds = await _db.Courses
            .Where(c => courseIds.Contains(c.Id))
            .Select(c => c.Id)
            .ToListAsync();

        var missing = courseIds.Except(validIds).ToList();
        if (missing.Count > 0)
            return NotFound(new { message = "Cursos inexistentes.", ids = missing });

        var alreadyEnrolled = await _db.Enrollments
            .Where(e => e.StudentId == request.IdStudent && courseIds.Contains(e.CourseId))
            .Select(e => e.CourseId)
            .ToListAsync();

        var newEnrollments = courseIds
            .Except(alreadyEnrolled)
            .Select(id => new Enrollment { StudentId = request.IdStudent, CourseId = id })
            .ToList();

        _db.Enrollments.AddRange(newEnrollments);
        await _db.SaveChangesAsync();

        return Ok(new { enrolled = newEnrollments.Count, skipped = alreadyEnrolled.Count });
    }
}