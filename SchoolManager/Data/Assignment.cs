using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolManager.Data
{
    public class Assignment
    {
        public int AssignmentId { get; set; }
        public int TeacherId { get; set; }
        public int ModuleId { get; set; }

        [ForeignKey(nameof(TeacherId))] //specifica che TeacherId è una chiave esterna che fa riferimento alla tabella Teacher
        public Teacher? Teacher { get; set; }

        [ForeignKey(nameof(ModuleId))] //specifica che ModuleId è una chiave esterna che fa riferimento alla tabella Module
        public Module? Module { get; set; }
    }
}
