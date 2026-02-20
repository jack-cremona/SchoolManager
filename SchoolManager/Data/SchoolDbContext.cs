using Microsoft.EntityFrameworkCore;

namespace SchoolManager.Data
{
    public class SchoolDbContext : DbContext
    {
        public SchoolDbContext() : base() { }

        public SchoolDbContext(DbContextOptions<SchoolDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Module>()
                .Property(m => m.ModuleId)
                .HasColumnName("Id");

            modelBuilder.Entity<Course>()
                .HasMany(c => c.Modules)
                .WithOne(m => m.Course)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Subject>()
                .HasMany(s => s.Modules)
                .WithOne(m => m.Subject)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Course>()
                .HasMany(c => c.Enrollments)
                .WithOne(m => m.Course)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Student>()
                .HasMany(s => s.Enrollments)
                .WithOne(m => m.Student)
                .OnDelete(DeleteBehavior.Restrict);

            //cambiare nomi alle tabelle di join per le relazioni many-to-many

            modelBuilder.Entity<Teacher>()
                .HasMany(t => t.Modules)
                .WithMany(m => m.Teachers)
                .UsingEntity(e => e.ToTable("Assignments"));

            modelBuilder.Entity<Teacher>()
                .HasMany(t => t.Subjects)
                .WithMany(m => m.Teachers)
                .UsingEntity(e => e.ToTable("Competences"));
        }

        public DbSet<Course> Courses { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; }
        public DbSet<Module> Modules { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        
    }
}
