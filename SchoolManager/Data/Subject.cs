namespace SchoolManager.Data
{
    public class Subject
    {
        public int SubjectId { get; set; }
        public required string Name { get; set; }
        public List<Teacher>? Teachers { get; set; }
        public List<Module>? Modules { get; set; }
    }
}
