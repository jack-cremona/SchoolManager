using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolManager.Data
{
    [PrimaryKey(nameof(TeacherId), nameof(SubjectId))] //primary key composta da TeacherId e SubjectId
    public class Competence
    {
        public int TeacherId { get; set; }
        public int SubjectId { get; set; }

        [ForeignKey(nameof(TeacherId))] //specifica che TeacherId è una chiave esterna che fa riferimento alla tabella Teacher
        public Teacher? Teacher { get; set; }

        [ForeignKey(nameof(SubjectId))]
        public Subject? Subject { get; set; }
    }
}
