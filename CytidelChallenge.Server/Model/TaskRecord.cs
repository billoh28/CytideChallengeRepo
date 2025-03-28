using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CytidelChallenge.Server.Model
{
    [Table("Tasks")]
    public class TaskRecord
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long TaskId { get; set; }
        [Required]
        public required string Title { get; set; }
        public string? Description { get; set; }
        public Priority Priority { get; set; }
        public DateTime DueDate { get; set; }
        public Status Status { get; set; }
    }
}
