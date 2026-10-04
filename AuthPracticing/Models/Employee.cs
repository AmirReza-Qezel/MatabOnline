using System.ComponentModel.DataAnnotations;

namespace AuthPracticing.Models
{
    public class Employee
    {
        [Key]
        public int Id { get; set; }
        [MaxLength(100)]
        public string Name { get; set; }
        [MaxLength(100)]
        public string LastName { get; set; }
        [MaxLength(100)]
        public string City { get; set; }
        public string Gender { get; set; }
    }
}
