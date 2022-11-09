using System;
using System.Linq;
using System.Windows;

using TestingSystem.Data.DataBase;
using TestingSystem.Exceptions;
using TestingSystem.Models;

namespace TestingSystem.Data
{
    internal static class Authorization
    {
        public static User LoggedUser;


        public static User ReturnLoggedUser(AuthorizationData authorizationData)
        {
            try
            {
                using (ApplicationContext db = new ApplicationContext())
                {
                    var result = db.Users.FirstOrDefault(user => user.login == authorizationData.Login &&
                        user.password == authorizationData.Password);

                    if (result is null)
                    {
                        throw NewVerificationException();
                    }

                    LoggedUser = result;
                    return LoggedUser;
                }
            }
            catch (InvalidOperationException)
            {
                Views.MessageBox.Show("Не удалось подключиться к серверу", "Ошибка", MessageBoxButton.OK);
                return null;
            }
        }

        private static VerificationException NewVerificationException()
        {
            string description = "Неверныйный логин или пароль";

            return new VerificationException(description);
        }
    }
}
