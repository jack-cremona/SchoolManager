namespace SchoolManager.Data
{
    public class Teacher
    {
        public int TeacherId { get; set; }
        public required string Name { get; set; }
        public required string Surname { get; set; }
        public List<Module>? Modules { get; set; }
        public List<Subject>? Subjects { get; set; }
    }
}
