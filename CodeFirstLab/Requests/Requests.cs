using System.ComponentModel.DataAnnotations;

namespace CodeFirstLab.Requests;

// ---------- CURSO ----------
public class InsertCourseRequest
{
    [Required, MaxLength(100)] public string Name { get; set; } = string.Empty;
    [Range(1, 10)] public int Credit { get; set; }
}

public class DeleteCourseRequest
{
    public int Id { get; set; }
}

public class DeleteCourseListRequest
{
    [Required, MinLength(1)] public List<int> CourseIds { get; set; } = new();
}

// ---------- GRADO ----------
public class InsertGradeRequest
{
    [Required, MaxLength(100)] public string Name { get; set; } = string.Empty;
    [MaxLength(300)] public string Description { get; set; } = string.Empty;
}

public class DeleteGradeRequest
{
    public int Id { get; set; }
}

// ---------- ESTUDIANTE ----------
public class InsertStudentRequest
{
    [Required] public int GradeId { get; set; }
    [Required, MaxLength(80)] public string FirstName { get; set; } = string.Empty;
    [Required, MaxLength(80)] public string LastName { get; set; } = string.Empty;
    [MaxLength(20)] public string Phone { get; set; } = string.Empty;
    [Required, EmailAddress] public string Email { get; set; } = string.Empty;
}

public class UpdateContactRequest
{
    [Required] public int Id { get; set; }
    [MaxLength(20)] public string Phone { get; set; } = string.Empty;
    [Required, EmailAddress] public string Email { get; set; } = string.Empty;
}

public class UpdatePersonalRequest
{
    [Required] public int Id { get; set; }
    [Required, MaxLength(80)] public string FirstName { get; set; } = string.Empty;
    [Required, MaxLength(80)] public string LastName { get; set; } = string.Empty;
}

public class StudentItem
{
    [Required, MaxLength(80)] public string FirstName { get; set; } = string.Empty;
    [Required, MaxLength(80)] public string LastName { get; set; } = string.Empty;
    [MaxLength(20)] public string Phone { get; set; } = string.Empty;
    [Required, EmailAddress] public string Email { get; set; } = string.Empty;
}

public class InsertStudentListRequest
{
    [Required] public int IdGrade { get; set; }
    [Required, MinLength(1)] public List<StudentItem> Students { get; set; } = new();
}

// ---------- MATRÍCULA ----------
public class EnrollRequest
{
    [Required] public int IdStudent { get; set; }
    [Required, MinLength(1)] public List<int> CourseIds { get; set; } = new();
}