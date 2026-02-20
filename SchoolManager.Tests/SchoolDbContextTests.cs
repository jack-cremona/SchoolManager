using Microsoft.EntityFrameworkCore;
using SchoolManager.Data;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace SchoolManager.Tests
{
    public class SchoolDbContextTests
    {
        private SchoolDbContext CreateContext()
        {
            var options = new DbContextOptionsBuilder<SchoolDbContext>()
                .UseInMemoryDatabase(databaseName: System.Guid.NewGuid().ToString()) // Unique DB per test
                .Options;

            return new SchoolDbContext(options);
        }

        [Fact]
        public void Can_Add_Teacher_With_Modules_And_Subjects()
        {
            using var context = CreateContext();

            // Arrange
            var subject = new Subject { Name = "Mathematics" };
            var course = new Course { Title = "High School 101" };

            var module = new Module 
            { 
                Title = "Algebra I", 
                Subject = subject, 
                Course = course 
            };

            var teacher = new Teacher 
            { 
                Name = "Maria", 
                Surname = "Rossi",
                Modules = new List<Module> { module },
                Subjects = new List<Subject> { subject }
            };

            // Act
            context.Teachers.Add(teacher);
            context.SaveChanges();

            // Assert
            // Verify data is saved and relationships are preserved
            var savedTeacher = context.Teachers
                .Include(t => t.Modules)
                .Include(t => t.Subjects)
                .FirstOrDefault(t => t.Name == "Maria");

            Assert.NotNull(savedTeacher);
            Assert.Equal("Rossi", savedTeacher.Surname);

            Assert.NotNull(savedTeacher.Modules);
            Assert.Single(savedTeacher.Modules);
            Assert.Equal("Algebra I", savedTeacher.Modules.First().Title);

            Assert.NotNull(savedTeacher.Subjects);
            Assert.Single(savedTeacher.Subjects);
            Assert.Equal("Mathematics", savedTeacher.Subjects.First().Name);
        }

        [Fact]
        public void Module_Should_Have_Column_Name_Id_Mapping()
        {
            // This test verifies the context builds the model correctly, specifically:
            // modelBuilder.Entity<Module>().Property(m => m.ModuleId).HasColumnName("Id");
            // However, InMemory provider might not fully respect relational column mappings in the same way SQLite/SQLServer do for inspection,
            // but the Model metadata should reflect it.

            using var context = CreateContext();
            var entityType = context.Model.FindEntityType(typeof(Module));
            var property = entityType?.FindProperty(nameof(Module.ModuleId));

            var columnName = property?.GetColumnName();

            Assert.Equal("Id", columnName);
        }

        [Fact]
        public void Confirm_ManyToMany_Relational_Table_Names()
        {
            using var context = CreateContext();

            // Locate the "Assignments" join table for Teacher <-> Modules
            var teacherEntity = context.Model.FindEntityType(typeof(Teacher));
            var modulesNav = teacherEntity?.FindSkipNavigation(nameof(Teacher.Modules));
            var joinEntityModules = modulesNav?.JoinEntityType;

            Assert.NotNull(joinEntityModules);
            // Verify table name is "Assignments"
            Assert.Equal("Assignments", joinEntityModules.GetTableName());

            // Locate the "Competences" join table for Teacher <-> Subjects
            var subjectsNav = teacherEntity?.FindSkipNavigation(nameof(Teacher.Subjects));
            var joinEntitySubjects = subjectsNav?.JoinEntityType;

            Assert.NotNull(joinEntitySubjects);
            // Verify table name is "Competences"
            Assert.Equal("Competences", joinEntitySubjects.GetTableName());
        }
    }
}
