using System.Collections.Generic;
using System.Linq;

using TestingSystem.Models;

namespace TestingSystem.Data.DataBase
{
    internal class DataBaseWriter
    {
        #region USERS

        public static void AddUser(User newUser)
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                db.Add(newUser);
                db.SaveChanges();
            }
        }

        public static void RemoveUser(User removableUser)
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                db.Remove(removableUser);
                db.SaveChanges();
            }
        }

        #endregion



        #region TESTS

        public static void AddTest(Test newTest)
        {
            using (var db = new ApplicationContext())
            {
                db.Tests.Add(newTest);
                db.SaveChanges();
            }
        }

        public static void RemoveTest(Test removeTest)
        {
            using (var db = new ApplicationContext())
            {
                db.Tests.Remove(removeTest);
                db.SaveChanges();
            }
        }

        public static void UpdateTest(Test Test)
        {
            using (var db = new ApplicationContext())
            {
                db.Tests.Update(Test);
                db.SaveChanges();
            }
        }

        #endregion


        public static void WriteMark(double mark, Test test)
        {
            using (var db = new ApplicationContext())
            {
                var testResults = db.Tests_Results.FirstOrDefault(p => p.test_id == test.id && p.user_id == Authorization.LoggedUser.id);
                testResults.mark = mark;

                db.Tests_Results.Update(testResults);
                db.SaveChanges();
            }
        }


        public static void AddGroupToTest(string group, Test test)
        {
            using (var db = new ApplicationContext())
            {
                List<User> usersWithThisGroup = DataBaseReader.GetAllUsersWithGroup(group);

                foreach (var user in usersWithThisGroup)
                {
                    var testInfo = db.Tests_Results.FirstOrDefault(p => p.user_id == user.id);

                    if (testInfo is null)
                        AddStudentToTest(user, test);
                }


            }
        }

        public static void RemoveGroupFromTest(string group, Test test)
        {
            using (var db = new ApplicationContext())
            {
                List<User> usersWithThisGroup = DataBaseReader.GetAllUsersWithGroup(group);

                foreach (var user in usersWithThisGroup)
                {
                    var testInfo = db.Tests_Results.FirstOrDefault(p => p.user_id == user.id && p.test_id == test.id);

                    if (testInfo is not null)
                        RemoveStudentFromTest(user, test);
                }


            }
        }


        public static void AddStudentToTest(User student, Test test)
        {
            using (var db = new ApplicationContext())
            {
                var testInfo = new TestResults
                {
                    test_id = test.id,
                    user_id = student.id
                };

                db.Tests_Results.Add(testInfo);
                db.SaveChanges();
            }
        }

        public static void RemoveStudentFromTest(User student, Test test)
        {
            using (var db = new ApplicationContext())
            {
                var removeableTestInfo = db.Tests_Results.Single(p => p.user_id == student.id && p.test_id == test.id);
                db.Tests_Results.Remove(removeableTestInfo);
                db.SaveChanges();
            }
        }
    }
}
