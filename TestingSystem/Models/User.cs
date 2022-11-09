using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;

namespace TestingSystem.Models
{
    [Index("login", IsUnique = true)]
    internal class User
    {
        [Key]
        public int id { get; set; }

        [Required]
        [MaxLength(20)]
        public string login { get; set; }

        [Required]
        [MaxLength(20)]
        public string password { get; set; }

        [Required]
        [MaxLength(45)]
        public string name { get; set; }

        [Required]
        [MaxLength(45)]
        public string surname { get; set; }

        [Required]
        [MaxLength(45)]
        public string patronymic { get; set; }

        [Required]
        [MaxLength(7)]
        public string type { get; set; }

        [MaxLength(20)]
        public string group { get; set; }



        public override string ToString()
        {
            return $"{surname} {name} {patronymic}";
        }
    }
}
