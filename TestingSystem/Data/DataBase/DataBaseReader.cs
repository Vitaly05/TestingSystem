using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

using TestingSystem.Models;
using TestingSystem.Exceptions;
using TestingSystem.Data.Structs;

namespace TestingSystem.Data.DataBase
{
    internal class DataBaseReader
    {
        #region USERS

        public static List<User> GetAllUsers()
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                var result = db.Users.ToList();
                return result;
            }
        }

        public static List<User> GetAllUsersWithType(string type)
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                var result = db.Users.Where(user => user.type == type).ToList();
                return result;
            }
        }

        public static List<User> GetAllUsersWithGroup(string group)
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                var result = db.Users.Where(user => user.group == group).ToList();
                return result;
            }
        }

        public static bool IsUserInDataBase(User verifiableUser)
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                var result = db.Users.FirstOrDefault(user => user.login == verifiableUser.login);

                if (result is null)
                    return false;
                return true;
            }
        }

        public static User GetUserById(int id)
        {
            using (var db = new ApplicationContext())
            {
                return db.Users.Single(user => user.id == id);
            }
        }


        #region Find users methods

        public static List<User> FindByLogin(string login)
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                var result = db.Users.Where(user => EF.Functions.Like(user.login, $"%{login}%")).ToList();
                return result;
            }
        }

        public static List<User> FindBySurname(string surname)
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                var result = db.Users.Where(user => EF.Functions.Like(user.surname, $"%{surname}%")).ToList();
                return result;
            }
        }

        public static List<User> FindByGroup(string group)
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                var result = db.Users.Where(user => EF.Functions.Like(user.group, $"%{group}%")).ToList();
                return result;
            }
        }

        #endregion

        #endregion

        public static List<string> GetAllGroups()
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                var result = GetAllUsersWithType(AccountTypes.Student);


                List<string> allGroups = new List<string>();

                foreach (var student in result)
                {
                    if (!allGroups.Contains(student.group))
                    {
                        allGroups.Add(student.group);
                    }
                }
                
                return allGroups;
            }
        }



        #region TESTS

        public static List<Test> GetAllTestsCreatedThisTeacher()
        {
            using (var db = new ApplicationContext())
            {
                var result = db.Tests.Where(p => p.teacher_id == Authorization.LoggedUser.id).ToList();
                return result;
            }
        }

        public static List<Test> GetAllTestsAvailableForThisStudent()
        {
            using (var db = new ApplicationContext())
            {
                var availableTestsInfoList = db.Tests_Results.Where(p => p.user_id == Authorization.LoggedUser.id).ToList();

                List<Test> availableTests = new List<Test>();

                foreach (var availableTestInfo in availableTestsInfoList)
                {
                    var test = db.Tests.FirstOrDefault(p => p.id == availableTestInfo.test_id);
                    availableTests.Add(test);
                }

                return availableTests;
            }
        }

        public static Test GetTestById(int testId)
        {
            using (var db = new ApplicationContext())
            {
                return db.Tests.FirstOrDefault(p => p.id == testId);
            }
        }

        public static TestResults GetTestInfoForThisStudent(int testId, User student)
        {
            using (var db = new ApplicationContext())
            {
                return db.Tests_Results.FirstOrDefault(p => p.test_id == testId && p.user_id == student.id);
            }
        }

        public static List<TestResults> GetTestInfo(Test test)
        {
            using (var db = new ApplicationContext())
            {
                return db.Tests_Results.Where(p => p.test_id == test.id).ToList();
            }
        }

        public static List<TestResults> FindTestInfoByGroup(string group, Test test)
        {
            using (var db = new ApplicationContext())
            {
                var studentsWithThisGroup = FindByGroup(group);

                List<TestResults> result = new();

                foreach (var student in studentsWithThisGroup)
                {
                    var testInfo = db.Tests_Results.FirstOrDefault(p => p.user_id == student.id && p.test_id == test.id);
                    if (testInfo is not null)
                        result.Add(testInfo);
                }

                return result;
            }
        }

        public static List<TestResults> FindTestInfoBySurname(string surname, Test test)
        {
            using (var db = new ApplicationContext())
            {
                var studentsWithThisSurname = FindBySurname(surname);

                List<TestResults> result = new();

                foreach (var student in studentsWithThisSurname)
                {
                    var testInfo = db.Tests_Results.FirstOrDefault(p => p.user_id == student.id && p.test_id == test.id);
                    if (testInfo is not null)
                        result.Add(testInfo);
                }

                return result;
            }
        }

        #endregion



        private static List<User> addedStudents = new List<User>();

        public static List<User> GetAddedToTestUsers(Test test)
        {
            using (var db = new ApplicationContext())
            {
                addedStudents.Clear();

                List<TestResults> testInfo = GetTestInfo(test);

                if (testInfo.Count == 0)
                    throw new NoAddedStudentsException();

                GetAddedStudents(testInfo);

                return addedStudents;
            }
        }

        public static List<User> GetNotAddedToTestStudents(Test test)
        {
            using (var db = new ApplicationContext())
            {
                List<TestResults> testInfo = GetTestInfo(test);

                List<User> allStudents = GetAllUsersWithType(AccountTypes.Student);


                if (addedStudents is null)
                    return allStudents;


                List<User> notAddedStudents = RemoveAddedStudents();

                return notAddedStudents;
            }
        }

        private static void GetAddedStudents(List<TestResults> testInfo)
        {
            using (var db = new ApplicationContext())
            {
                foreach (var info in testInfo)
                {
                    User addedStudent = db.Users.Single(p => p.id == info.user_id);
                    addedStudents.Add(addedStudent);
                }
            }
        }

        private static List<User> RemoveAddedStudents()
        {
            List<User> allStudents = GetAllUsersWithType(AccountTypes.Student);
            List<User> notAddedStudents = new List<User>(allStudents);

            foreach (var addedStudent in addedStudents)
                foreach (var student in allStudents)
                {
                    if (student.id == addedStudent.id)
                        notAddedStudents.Remove(student);
                }

            return notAddedStudents;
        }



    }
}
