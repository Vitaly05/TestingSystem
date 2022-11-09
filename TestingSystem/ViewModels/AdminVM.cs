using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

using TestingSystem.Data;
using TestingSystem.Data.DataBase;
using TestingSystem.Data.Structs;
using TestingSystem.Exceptions;
using TestingSystem.Models;

namespace TestingSystem.ViewModels
{
    internal class AdminVM : WindowCommands, INotifyPropertyChanged
    {
        #region PROPERTY_CHANGED METHOD

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

        #endregion



        public string UserName
        {
            get { return Authorization.LoggedUser.ToString(); }
        }



        #region VISIBILITY

        private string mainGridVisibility = Visibilities.Visibile;
        public string MainGridVisibility
        {
            get { return mainGridVisibility; }
            set
            {
                mainGridVisibility = value;
                OnPropertyChanged(nameof(MainGridVisibility));
            }
        }

        private string addUserGridVisibility = Visibilities.Collapsed;
        public string AddUserGridVisibility
        {
            get { return addUserGridVisibility; }
            set
            {
                addUserGridVisibility = value;
                OnPropertyChanged(nameof(AddUserGridVisibility));
            }
        }

        private string removeUserGridVisibility = Visibilities.Collapsed;
        public string RemoveUserGridVisibility
        {
            get { return removeUserGridVisibility; }
            set
            {
                removeUserGridVisibility = value;
                OnPropertyChanged(nameof(RemoveUserGridVisibility));
            }
        }

        #endregion



        private string functionName;
        public string FunctionName
        {
            get { return functionName; }
            set
            {
                functionName = value;
                OnPropertyChanged(nameof(FunctionName));
            }
        }

        private List<User> users = DataBaseReader.GetAllUsers();
        public List<User> Users
        {
            get
            {
                return users;
            }
            private set
            {
                users = value;
                OnPropertyChanged(nameof(Users));
            }
        }



        #region ADD USER PAREMETERS

        private string addUserLogin;
        public string AddUserLogin
        {
            get { return addUserLogin; }
            set
            {
                addUserLogin = value;
                OnPropertyChanged(nameof(AddUserLogin));
            }
        }

        private string addUserPassword;
        public string AddUserPassword
        {
            get { return addUserPassword; }
            set
            {
                addUserPassword = value;
                OnPropertyChanged(nameof(AddUserPassword));
            }
        }

        private string addUserName;
        public string AddUserName
        {
            get { return addUserName; }
            set
            {
                addUserName = value;
                OnPropertyChanged(nameof(AddUserName));
            }
        }

        private string addUserSurname;
        public string AddUserSurname
        {
            get { return addUserSurname; }
            set
            {
                addUserSurname = value;
                OnPropertyChanged(nameof(AddUserSurname));
            }
        }

        private string addUserPatronymic;
        public string AddUserPatronymic
        {
            get { return addUserPatronymic; }
            set
            {
                addUserPatronymic = value;
                OnPropertyChanged(nameof(AddUserPatronymic));
            }
        }

        private string addUserGroup;
        public string AddUserGroup
        {
            get { return addUserGroup; }
            set
            {
                addUserGroup = value;
                OnPropertyChanged(nameof(AddUserGroup));
            }
        }

        private string addUserAccountType;

        #endregion


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
            if ((AddUserLogin is null || AddUserLogin.Replace(" ", "") == "") ||
                (AddUserPassword is null || AddUserPassword.Replace(" ", "") == "") ||
                (AddUserName is null || AddUserName.Replace(" ", "") == "") ||
                (AddUserSurname is null || AddUserSurname.Replace(" ", "") == "") ||
                (AddUserPatronymic is null || AddUserPatronymic.Replace(" ", "") == ""))
            {
                throw new EmptyFieldException("Заполните все поля");
            }
        }

        private void ShowError(string errorText)
        {
            ErrorText = errorText;
            ErrorVisibitity = Visibilities.Visibile;
        }

        #endregion


        #region GRIDS CHANGE

        private enum Grids
        {
            Main,
            AddUser,
            RemoveUser
        }

        private void ShowGrid(Grids grid)
        {
            HideAllGrids();
            EnableGrid(grid);
        }

        private void HideAllGrids()
        {
            MainGridVisibility = Visibilities.Collapsed;
            AddUserGridVisibility = Visibilities.Collapsed;
            RemoveUserGridVisibility = Visibilities.Collapsed;
        }

        private void EnableGrid(Grids grid)
        {
            switch (grid)
            {
                case Grids.Main:
                    MainGridVisibility = Visibilities.Visibile;
                    break;
                case Grids.AddUser:
                    AddUserGridVisibility = Visibilities.Visibile;
                    ResetAddUserParameters();
                    break;
                case Grids.RemoveUser:
                    RemoveUserGridVisibility = Visibilities.Visibile;
                    break;
            }
        }

        private void ResetAddUserParameters()
        {
            AddUserLogin = null;
            AddUserPassword = null;
            AddUserName = null;
            AddUserSurname = null;
            AddUserPatronymic = null;
            AddUserGroup = null;

            ErrorVisibitity = Visibilities.Hidden;
        }

        #endregion


        #region BUTTONS COMMANDS

        private RelayCommand showAddTeacherPanel;
        public RelayCommand ShowAddTeacherPanel
        {
            get
            {
                return showAddTeacherPanel ??
                    (showAddTeacherPanel = new RelayCommand((obj) =>
                    {
                        if (AddUserGridVisibility == Visibilities.Collapsed)
                            ShowGrid(Grids.AddUser);
                        addUserAccountType = AccountTypes.Teacher;
                        FunctionName = "Добавление преподавателя";
                    }));
            }
        }

        private RelayCommand showAddStudentPanel;
        public RelayCommand ShowAddStudentPanel
        {
            get
            {
                return showAddStudentPanel ??
                    (showAddStudentPanel = new RelayCommand((obj) =>
                    {
                        if (AddUserGridVisibility == Visibilities.Collapsed)
                            ShowGrid(Grids.AddUser);
                        addUserAccountType = AccountTypes.Student;
                        FunctionName = "Добавление ученика";
                    }));
            }
        }

        private RelayCommand showAddAdminPanel;
        public RelayCommand ShowAddAdminPanel
        {
            get
            {
                return showAddAdminPanel ??
                    (showAddAdminPanel = new RelayCommand((obj) =>
                    {
                        if (AddUserGridVisibility == Visibilities.Collapsed)
                            ShowGrid(Grids.AddUser);
                        addUserAccountType = AccountTypes.Admin;
                        FunctionName = "Добавление администратора";
                    }));
            }
        }

        private RelayCommand showRemoveUsersPanel;
        public RelayCommand ShowRemoveUsersPanel
        {
            get
            {
                return showRemoveUsersPanel ??
                    (showRemoveUsersPanel = new RelayCommand((obj) =>
                    {
                        if (RemoveUserGridVisibility == Visibilities.Collapsed)
                        {
                            ShowGrid(Grids.RemoveUser);
                            SearchText = "";
                            updateUsersList();
                        }
                    }));
            }
        }

        private RelayCommand toHome;
        public RelayCommand ToHome
        {
            get
            {
                return toHome ??
                    (toHome = new RelayCommand((obj) =>
                    {
                        if (MainGridVisibility == Visibilities.Collapsed)
                            ShowGrid(Grids.Main);
                    }));
            }
        }

        private RelayCommand addUser;
        public RelayCommand AddUser
        {
            get
            {
                return addUser ??
                    (addUser = new RelayCommand((obj) =>
                    {
                        if (FieldsIsEmpty())
                            return;

                        User newUser = new User
                        {
                            login = AddUserLogin,
                            password = AddUserPassword,
                            name = AddUserName,
                            surname = AddUserSurname,
                            patronymic = AddUserPatronymic,
                            group = AddUserGroup,
                            type = addUserAccountType
                        };

                        if (DataBaseReader.IsUserInDataBase(newUser))
                        {
                            ErrorText = "Пользователь с таким логином уже существует";
                            ErrorVisibitity = Visibilities.Visibile;
                            return;
                        }

                        DataBaseWriter.AddUser(newUser);
                        ShowGrid(Grids.Main);
                    }));
            }
        }

        #endregion


        #region REMOVE USERS

        private User selectedUser;
        public User SelectedUser
        {
            get { return selectedUser; }
            set
            {
                selectedUser = value;
                RemoveButtonEnabled = true;
                OnPropertyChanged(nameof(SelectedUser));
            }
        }


        private bool removeButtonEnabled;
        public bool RemoveButtonEnabled
        {
            get { return removeButtonEnabled; }
            set
            {
                removeButtonEnabled = value;
                OnPropertyChanged(nameof(RemoveButtonEnabled));
            }
        }


        private enum ShowUsersFilter
        {
            All,
            Students,
            Teachers,
            Admins
        }

        private ShowUsersFilter selectedShowUsersFilter = ShowUsersFilter.All;


        private RelayCommand changeFilterToAll;
        public RelayCommand ChangeFilterToAll
        {
            get
            {
                return changeFilterToAll ??
                    (changeFilterToAll = new RelayCommand((obj) =>
                    {
                        selectedShowUsersFilter = ShowUsersFilter.All;
                        updateUsersList();
                    }));
            }
        }

        private RelayCommand changeFilterToStudents;
        public RelayCommand ChangeFilterToStudents
        {
            get
            {
                return changeFilterToStudents ??
                    (changeFilterToStudents = new RelayCommand((obj) =>
                    {
                        selectedShowUsersFilter = ShowUsersFilter.Students;
                        updateUsersList();
                    }));
            }
        }

        private RelayCommand changeFilterToTeachers;
        public RelayCommand ChangeFilterToTeachers
        {
            get
            {
                return changeFilterToTeachers ??
                    (changeFilterToTeachers = new RelayCommand((obj) =>
                    {
                        selectedShowUsersFilter = ShowUsersFilter.Teachers;
                        updateUsersList();
                    }));
            }
        }

        private RelayCommand changeFilterToAdmins;
        public RelayCommand ChangeFilterToAdmins
        {
            get
            {
                return changeFilterToAdmins ??
                    (changeFilterToAdmins = new RelayCommand((obj) =>
                    {
                        selectedShowUsersFilter = ShowUsersFilter.Admins;
                        updateUsersList();
                    }));
            }
        }

        private RelayCommand removeUser;
        public RelayCommand RemoveUser
        {
            get
            {
                return removeUser ??
                    (removeUser = new RelayCommand((obj) =>
                    {
                        if (SelectedUser is not null)
                        {
                            DataBaseWriter.RemoveUser(SelectedUser);

                            updateUsersList();
                        }
                    }));
            }
        }

        private void updateUsersList()
        {
            useFilter();
            RemoveButtonEnabled = false;
        }

        private void useFilter()
        {
            switch (selectedShowUsersFilter)
            {
                case ShowUsersFilter.All:
                    Users = DataBaseReader.GetAllUsers();
                    break;
                case ShowUsersFilter.Students:
                    Users = DataBaseReader.GetAllUsersWithType(AccountTypes.Student);
                    break;
                case ShowUsersFilter.Teachers:
                    Users = DataBaseReader.GetAllUsersWithType(AccountTypes.Teacher);
                    break;
                case ShowUsersFilter.Admins:
                    Users = DataBaseReader.GetAllUsersWithType(AccountTypes.Admin);
                    break;
            }
        }

        #endregion


        #region  USERS SEARCH

        private enum SearchFilters
        {
            ByLogin,
            BySurname,
            ByGroup
        }

        private SearchFilters searchFilter = SearchFilters.ByLogin;

        private RelayCommand setFilterByLogin;
        public RelayCommand SetFilterByLogin
        {
            get
            {
                return setFilterByLogin ??
                    (setFilterByLogin = new RelayCommand((obj) =>
                    {
                        searchFilter = SearchFilters.ByLogin;
                    }));
            }
        }

        private RelayCommand setFilterBySurname;
        public RelayCommand SetFilterBySurname
        {
            get
            {
                return setFilterBySurname ??
                    (setFilterBySurname = new RelayCommand((obj) =>
                    {
                        searchFilter = SearchFilters.BySurname;
                    }));
            }
        }

        private RelayCommand setFilterByGroup;
        public RelayCommand SetFilterByGroup
        {
            get
            {
                return setFilterByGroup ??
                    (setFilterByGroup = new RelayCommand((obj) =>
                    {
                        searchFilter = SearchFilters.ByGroup;
                    }));
            }
        }


        private string searchText;
        public string SearchText
        {
            get { return searchText; }
            set
            {
                searchText = value;
                OnPropertyChanged(nameof(SearchText));
            }
        }

        private RelayCommand setSearchText;
        public RelayCommand SetSearchText
        {
            get
            {
                return setSearchText ??
                    (setSearchText = new RelayCommand((obj) =>
                    {
                        SearchText = obj as string;
                    }));
            }
        }

        private bool changeFilterToAllButton_checked = true;
        public bool ChangeFilterToAllButton_checked
        {
            get { return changeFilterToAllButton_checked; }
            set
            {
                changeFilterToAllButton_checked = value;
                OnPropertyChanged(nameof(ChangeFilterToAllButton_checked));
            }
        }

        private RelayCommand search;
        public RelayCommand Search
        {
            get
            {
                return search ??
                    (search = new RelayCommand((obj) =>
                    {
                        switch (searchFilter)
                        {
                            case SearchFilters.ByLogin:
                                Users = DataBaseReader.FindByLogin(SearchText);
                                break;
                            case SearchFilters.BySurname:
                                Users = DataBaseReader.FindBySurname(SearchText);
                                break;
                            case SearchFilters.ByGroup:
                                Users = DataBaseReader.FindByGroup(SearchText);
                                break;
                        }

                        ChangeFilterToAllButton_checked = true;
                        RemoveButtonEnabled = false;
                    }));
            }
        }

        private RelayCommand resetSearchResult;
        public RelayCommand ResetSearchResult
        {
            get
            {
                return resetSearchResult ??
                    (resetSearchResult = new RelayCommand((obj) =>
                    {
                        selectedShowUsersFilter = ShowUsersFilter.All;
                        ChangeFilterToAllButton_checked = true;
                        SearchText = null;
                        updateUsersList();
                    }));
            }
        }

        #endregion
    }
}
