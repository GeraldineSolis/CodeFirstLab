using CodeFirstLab.Data;
using CodeFirstLab.Models;
using CodeFirstLab.Requests;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CodeFirstLab.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StudentController : ControllerBase
{
    private readonly AppDbContext _db;
    public StudentController(AppDbContext db) => _db = db;

    // Insertar Estudiante
    [HttpPost]
    public async Task<IActionResult> Insert(InsertStudentRequest request)
    {
        if (!await _db.Grades.AnyAsync(g => g.Id == request.GradeId))
            return NotFound(new { message = "El grado no existe." });

        if (await _db.Students.AnyAsync(s => s.Email == request.Email))
            return Conflict(new { message = "Ya existe un estudiante con ese email." });

        var student = new Student
        {
            GradeId = request.GradeId,
            FirstName = request.FirstName,
            LastName = request.LastName,
            Phone = request.Phone,
            Email = request.Email
        };

        _db.Students.Add(student);
        await _db.SaveChangesAsync();
        return Ok(new { student.Id, student.FirstName, student.LastName, student.GradeId });
    }

    // Actualizar Datos de Contacto
    [HttpPut("contact")]
    public async Task<IActionResult> UpdateContact(UpdateContactRequest request)
    {
        var student = await _db.Students.FindAsync(request.Id);
        if (student is null) return NotFound(new { message = "Estudiante no encontrado." });

        if (await _db.Students.AnyAsync(s => s.Email == request.Email && s.Id != request.Id))
            return Conflict(new { message = "Ese email ya está en uso." });

        student.Phone = request.Phone;
        student.Email = request.Email;
        await _db.SaveChangesAsync();
        return Ok(new { student.Id, student.Phone, student.Email });
    }

    // Actualizar Datos Personales
    [HttpPut("personal")]
    public async Task<IActionResult> UpdatePersonal(UpdatePersonalRequest request)
    {
        var student = await _db.Students.FindAsync(request.Id);
        if (student is null) return NotFound(new { message = "Estudiante no encontrado." });

        student.FirstName = request.FirstName;
        student.LastName = request.LastName;
        await _db.SaveChangesAsync();
        return Ok(new { student.Id, student.FirstName, student.LastName });
    }

    // Insertar Lista de Estudiantes por grado
    [HttpPost("by-grade")]
    public async Task<IActionResult> InsertList(InsertStudentListRequest request)
    {
        if (!await _db.Grades.AnyAsync(g => g.Id == request.IdGrade))
            return NotFound(new { message = "El grado no existe." });

        var emails = request.Students.Select(s => s.Email).ToList();

        if (emails.Distinct().Count() != emails.Count)
            return BadRequest(new { message = "Hay emails repetidos en la lista." });

        var existing = await _db.Students
            .Where(s => emails.Contains(s.Email))
            .Select(s => s.Email)
            .ToListAsync();

        if (existing.Count > 0)
            return Conflict(new { message = "Emails ya registrados.", emails = existing });

        var students = request.Students.Select(s => new Student
        {
            GradeId = request.IdGrade,
            FirstName = s.FirstName,
            LastName = s.LastName,
            Phone = s.Phone,
            Email = s.Email
        }).ToList();

        _db.Students.AddRange(students);
        await _db.SaveChangesAsync();
        return Ok(new { inserted = students.Count });
    }
}