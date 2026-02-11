namespace SchoolManager.DTO
{
    public class StudentDto
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Surname { get; set; }
        public Dictionary<int, string>? Courses { get; set; }
    }
}
