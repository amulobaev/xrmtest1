using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace XrmTest1.Data
{
    /// <summary>
    /// Сущность "Резюме" для Entity Framework (по сути DTO)
    /// </summary>
    [Table("Resume")]
    internal class ResumeEntity
    {
        [Key]
        public int Id { get; set; }

        public int ResumeId { get; set; }

        public string Name { get; set; }

        public int? Age { get; set; }

        public string Header { get; set; }

        public string Salary { get; set; }

        public string Info { get; set; }
    }
}
