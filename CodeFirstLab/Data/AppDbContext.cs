using CodeFirstLab.Models;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace CodeFirstLab.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Course> Courses => Set<Course>();
    public DbSet<Grade> Grades => Set<Grade>();
    public DbSet<Student> Students => Set<Student>();
    public DbSet<Enrollment> Enrollments => Set<Enrollment>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Course>(e =>
        {
            e.Property(x => x.Name).IsRequired().HasMaxLength(100);
        });

        modelBuilder.Entity<Grade>(e =>
        {
            e.Property(x => x.Name).IsRequired().HasMaxLength(100);
            e.Property(x => x.Description).HasMaxLength(300);
        });

        modelBuilder.Entity<Student>(e =>
        {
            e.Property(x => x.FirstName).IsRequired().HasMaxLength(80);
            e.Property(x => x.LastName).IsRequired().HasMaxLength(80);
            e.Property(x => x.Phone).HasMaxLength(20);
            e.Property(x => x.Email).IsRequired().HasMaxLength(120);
            e.HasIndex(x => x.Email).IsUnique();

            e.HasOne(x => x.Grade)
             .WithMany(g => g.Students)
             .HasForeignKey(x => x.GradeId)
             .OnDelete(DeleteBehavior.Restrict); 
        });

        modelBuilder.Entity<Enrollment>(e =>
        {
            e.HasKey(x => new { x.StudentId, x.CourseId });

            e.HasOne(x => x.Student)
             .WithMany(s => s.Enrollments)
             .HasForeignKey(x => x.StudentId);

            e.HasOne(x => x.Course)
             .WithMany(c => c.Enrollments)
             .HasForeignKey(x => x.CourseId);
        });
    }
}