using Microsoft.EntityFrameworkCore;

using TestingSystem.Models;

namespace TestingSystem.Data.DataBase
{
    internal class ApplicationContext : DbContext
    {
        #region CONNECTION PARAMS

        private static string host = "localhost";
        private static string port = "3306";
        private static string userName = "root";
        private static string password = "root";
        private static string dataBase = "testing_system";

        #endregion

        public DbSet<User> Users { get; set; }

        public DbSet<Test> Tests { get; set; }
        public DbSet<TestResults> Tests_Results { get; set; }

        public DbSet<Question> Questions { get; set; }

        public ApplicationContext()
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
                optionsBuilder.UseMySQL(
                    $"server={host};" +
                    $"port={port};" +
                    $"username={userName};" +
                    $"password={password};" +
                    $"database={dataBase}");
        }
    }
}
