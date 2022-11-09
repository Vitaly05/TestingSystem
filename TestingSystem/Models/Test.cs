using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using TestingSystem.Data;
using TestingSystem.Data.DataBase;

namespace TestingSystem.Models
{
    internal class Test
    {
        [Key]
        public int id { get; set; }

        [Required]
        public int teacher_id { get; set; }

        [Required]
        [MaxLength(45)]
        public string name { get; set; }

        [MaxLength(100)]
        public string description { get; set; }

        [Required]
        public int max_mark { get; set; }

        [Required]
        public DateTime create_date { get; set; }

        [NotMapped]
        public TestResults testInfoForThisStudent
        {
            get
            {
                return DataBaseReader.GetTestInfoForThisStudent(id, Authorization.LoggedUser);
            }
        }
    }
}
