using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using TestingSystem.Data.DataBase;

namespace TestingSystem.Models
{
    internal class TestResults
    {
        [Key]
        public int id { get; set; }

        [Required]
        public int test_id { get; set; }
        [Required]
        public int user_id { get; set; }

        public double? mark { get; set; }

        [NotMapped]
        public Test test
        {
            get
            {
                return DataBaseReader.GetTestById(test_id);

            }
        }

        [NotMapped]
        public User student
        {
            get
            {
                return DataBaseReader.GetUserById(user_id);
            }
        }
    }
}
