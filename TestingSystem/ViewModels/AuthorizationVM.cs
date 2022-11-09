using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;

using TestingSystem.Data;
using TestingSystem.Data.Structs;
using TestingSystem.Exceptions;
using TestingSystem.Models;

namespace TestingSystem.ViewModels
{
    internal class AuthorizationVM : WindowCommands, INotifyPropertyChanged
    {
        #region PROPERTY_CHANGED METHOD

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

        #endregion



        private Window thisWindow;

        private RelayCommand windowLoaded;
        public RelayCommand WindowLoaded
        {
            get
            {
                return windowLoaded ??
                    (windowLoaded = new RelayCommand((obj) =>
                    {
                        thisWindow = obj as Window;
                    }));
            }
        }


        private string login;
        public string Login
        {
            get { return login; }
            set
            {
                login = value;
                OnPropertyChanged(nameof(Login));
            }
        }

        private string password;
        public string Password
        {
            private get { return password; }
            set
            {
                password = value;
                OnPropertyChanged(nameof(Password));
            }
        }

        private User loggedUser;


        private RelayCommand loggin;
        public RelayCommand Loggin
        {
            get
            {
                return loggin ??
                    (loggin = new RelayCommand((obj) =>
                {
                    Password = ((PasswordBox)obj).Password;

                    if (FieldsIsEmpty())
                        return;

                    loggedUser = ReturnLoggedUserIfVerificationIsValid();

                    if (loggedUser is null)
                        return;

                    ShowNewWindowAndCloseThis();
                }));
            }
        }



        #region ERROR METHODS

        private string errorText;
        public string ErrorText
        {
            get { return errorText; }
            set
            {
                errorText = value;
                OnPropertyChanged(nameof(ErrorText));
            }
        }

        private string errorVisibitity = Visibilities.Hidden;
        public string ErrorVisibitity
        {
            get { return errorVisibitity; }
            set
            {
                errorVisibitity = value;
                OnPropertyChanged(nameof(ErrorVisibitity));
            }
        }

        private bool FieldsIsEmpty()
        {
            try
            {
                CheckFieldsForEmptiness();
                return false;
            }
            catch (EmptyFieldException ex)
            {
                ShowError(ex.Message);
                return true;
            }
        }

        private void CheckFieldsForEmptiness()
        {
            if ((Login is null || Login.Replace(" ", "") == "") &&
                (Password is null || Password.Replace(" ", "") == ""))
                throw new EmptyFieldException("Заполните все поля");

            if (Login is null || Login.Replace(" ", "") == "")
                throw new EmptyFieldException("Введите логин");

            if (Password is null || Password.Replace(" ", "") == "")
                throw new EmptyFieldException("Введите пароль");
        }

        #endregion



        private User ReturnLoggedUserIfVerificationIsValid()
        {
            try
            {
                return Authorization.ReturnLoggedUser(GetAuthorizationData());
            }
            catch (VerificationException ex)
            {
                ShowError(ex.Message);
                return null;
            }
        }
        
        private AuthorizationData GetAuthorizationData()
        {
            AuthorizationData authorizationData = new AuthorizationData();
            authorizationData.Login = login;
            authorizationData.Password = password;

            return authorizationData;
        }

        private void ShowError(string errorText)
        {
            ErrorText = errorText;
            ErrorVisibitity = Visibilities.Visibile;
        }


        private void ShowNewWindowAndCloseThis()
        {
            CreateNewWindow().ShowNewWindow();

            WindowMethods.closeThisWindow(thisWindow);
        }

        private WindowCreator CreateNewWindow()
        {
            WindowCreator WC = new WindowCreator();
            WC.WindowType = loggedUser.type;

            return WC;
        }
    }
}
