using System.ComponentModel.DataAnnotations;

namespace TestingSystem.Models
{
    internal class Question
    {
        [Key]
        public int id { get; set; }

        [Required]
        public int test_id { get; set; }

        [Required]
        public string data { get; set; }
    }
}