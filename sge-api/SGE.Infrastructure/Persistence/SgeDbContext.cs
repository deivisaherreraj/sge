using Microsoft.EntityFrameworkCore;
using SGE.Domain.Entities;

namespace SGE.Infrastructure.Persistence;

public class SgeDbContext : DbContext
{
    public SgeDbContext(DbContextOptions<SgeDbContext> options) : base(options) { }
    
    public DbSet<Employee> Employees => Set<Employee>();
    public DbSet<Department> Departments => Set<Department>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Entity<Department>(e =>
        {
            e.ToTable("Departments");
            e.HasKey(x => x.Id);
            e.Property(x => x.Name).IsRequired().HasMaxLength(80);
            e.HasIndex(x => x.Name).IsUnique();
        });
        
        modelBuilder.Entity<Employee>(e =>
        {
            e.ToTable("Employees", t =>
            {
                t.HasCheckConstraint("CK_Employees_Salary", "[Salary] >= 0");
            });
            e.HasKey(x => x.Id);
            e.Property(x => x.FullName).IsRequired().HasMaxLength(120);
            e.Property(x => x.Role).IsRequired().HasMaxLength(80);
            e.Property(x => x.HireDate).IsRequired();
            e.Property(x => x.Salary).HasColumnType("decimal(18,2)");
            e.HasOne(x => x.Department)
                .WithMany(d => d.Employees)
                .HasForeignKey(x => x.DepartmentId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }
}