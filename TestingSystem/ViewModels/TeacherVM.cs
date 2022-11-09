using System;
using System.Windows;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

using TestingSystem.Data;
using TestingSystem.Data.DataBase;
using TestingSystem.Data.Structs;
using TestingSystem.Models;
using TestingSystem.Exceptions;


namespace TestingSystem.ViewModels
{
    internal class TeacherVM : WindowCommands, INotifyPropertyChanged
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

        private List<Test> tests;
        public List<Test> Tests
        {
            get { return tests ?? DataBaseReader.GetAllTestsCreatedThisTeacher(); }
            set
            {
                tests = value;
                OnPropertyChanged(nameof(Tests));
            }
        }

        private Test selectedTest;
        public Test SelectedTest
        {
            get { return selectedTest; }
            set
            {
                selectedTest = value;
                if (value is not null)
                    ButtonsWorkWithTestEnabled = true;
                OnPropertyChanged(nameof(SelectedTest));
            }
        }



        #region VISIBILITY AND ENABLES

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

        private string createTestGridVisibility = Visibilities.Collapsed;
        public string CreateTestGridVisibility
        {
            get { return createTestGridVisibility; }
            set
            {
                createTestGridVisibility = value;
                OnPropertyChanged(nameof(CreateTestGridVisibility));
            }
        }

        private string addQuestionGridVisibility = Visibilities.Collapsed;
        public string AddQuestionGridVisibility
        {
            get { return addQuestionGridVisibility; }
            set
            {
                addQuestionGridVisibility = value;
                OnPropertyChanged(nameof(AddQuestionGridVisibility));
            }
        }

        private string questionTypeWithEnteringAnswerGridVisibility = Visibilities.Collapsed;
        public string QuestionTypeWithEnteringAnswerGridVisibility
        {
            get { return questionTypeWithEnteringAnswerGridVisibility; }
            set
            {
                questionTypeWithEnteringAnswerGridVisibility = value;
                OnPropertyChanged(nameof(QuestionTypeWithEnteringAnswerGridVisibility));
            }
        }

        private string questionTypeWithSelectingAnswerGridVisibility = Visibilities.Collapsed;
        public string QuestionTypeWithSelectingAnswerGridVisibility
        {
            get { return questionTypeWithSelectingAnswerGridVisibility; }
            set
            {
                questionTypeWithSelectingAnswerGridVisibility = value;
                OnPropertyChanged(nameof(QuestionTypeWithSelectingAnswerGridVisibility));
            }
        }


        private string addStudentToTestGridVisibility = Visibilities.Collapsed;
        public string AddStudentToTestGridVisibility
        {
            get { return addStudentToTestGridVisibility; }
            set
            {
                addStudentToTestGridVisibility = value;
                OnPropertyChanged(nameof(AddStudentToTestGridVisibility));
            }
        }

        private string addGroupsToTestGridVisibility = Visibilities.Collapsed;
        public string AddGroupsToTestGridVisibility
        {
            get { return addGroupsToTestGridVisibility; }
            set
            {
                addGroupsToTestGridVisibility = value;
                OnPropertyChanged(nameof(AddGroupsToTestGridVisibility));
            }
        }

        private string resultsGridVisibility = Visibilities.Collapsed;
        public string ResultsGridVisibility
        {
            get { return resultsGridVisibility; }
            set
            {
                resultsGridVisibility = value;
                OnPropertyChanged(nameof(ResultsGridVisibility));
            }
        }

        private string editTestGridVisibility = Visibilities.Collapsed;
        public string EditTestGridVisibility
        {
            get { return editTestGridVisibility; }
            set
            {
                editTestGridVisibility = value;
                OnPropertyChanged(nameof(EditTestGridVisibility));
            }
        }

        private string editQuestionGridVisibility = Visibilities.Collapsed;
        public string EditQuestionGridVisibility
        {
            get { return editQuestionGridVisibility; }
            set
            {
                editQuestionGridVisibility = value;
                OnPropertyChanged(nameof(EditQuestionGridVisibility));
            }
        }




        private bool buttonsWorkWithTestEnabled;
        public bool ButtonsWorkWithTestEnabled
        {
            get { return buttonsWorkWithTestEnabled; }
            set
            {
                buttonsWorkWithTestEnabled = value;
                OnPropertyChanged(nameof(ButtonsWorkWithTestEnabled));
            }
        }

        private bool removeStudentButtonEnabled;
        public bool RemoveStudentButtonEnabled
        {
            get { return removeStudentButtonEnabled; }
            set
            {
                removeStudentButtonEnabled = value;
                OnPropertyChanged(nameof(RemoveStudentButtonEnabled));
            }
        }

        private bool addStudentButtonEnabled;
        public bool AddStudentButtonEnabled
        {
            get { return addStudentButtonEnabled; }
            set
            {
                addStudentButtonEnabled = value;
                OnPropertyChanged(nameof(AddStudentButtonEnabled));
            }
        }

        #endregion


        #region GRIDS METHODS

        private enum Grids
        {
            Main,
            CreateTest,
            AddQuestion,
            AddStudentsToTest,
            AddGroupsToTest,
            Results,
            EditTest,
            EditQuestion
        }

        private void UpdateTestsList()
        {
            Tests = DataBaseReader.GetAllTestsCreatedThisTeacher();
        }

        private void ShowGrid(Grids grid, string questionType = null)
        {
            HideAllGrids();
            EnableGrid(grid, questionType);
        }

        private void HideAllGrids()
        {
            MainGridVisibility = Visibilities.Collapsed;
            CreateTestGridVisibility = Visibilities.Collapsed;
            AddQuestionGridVisibility = Visibilities.Collapsed;
            AddStudentToTestGridVisibility = Visibilities.Collapsed;
            AddGroupsToTestGridVisibility = Visibilities.Collapsed;
            ResultsGridVisibility = Visibilities.Collapsed;
            EditTestGridVisibility = Visibilities.Collapsed;
            EditQuestionGridVisibility = Visibilities.Collapsed;

            QuestionTypeWithEnteringAnswerGridVisibility = Visibilities.Collapsed;
            QuestionTypeWithSelectingAnswerGridVisibility = Visibilities.Collapsed;
        }

        private void EnableGrid(Grids grid, string questionType = null)
        {
            switch (grid)
            {
                case Grids.Main:
                    MainGridVisibility = Visibilities.Visibile;
                    ButtonsWorkWithTestEnabled = false;
                    UpdateTestsList();
                    break;
                case Grids.CreateTest:
                    CreateTestGridVisibility = Visibilities.Visibile;
                    break;
                case Grids.AddQuestion:
                    AddQuestionGridVisibility = Visibilities.Visibile;
                    ShowQuestionTypeGrid(questionType);
                    break;
                case Grids.AddStudentsToTest:
                    AddStudentToTestGridVisibility = Visibilities.Visibile;
                    break;
                case Grids.AddGroupsToTest:
                    AddGroupsToTestGridVisibility = Visibilities.Visibile;
                    break;
                case Grids.Results:
                    ResultsGridVisibility = Visibilities.Visibile;
                    break;
                case Grids.EditTest:
                    EditTestGridVisibility = Visibilities.Visibile;
                    break;
                case Grids.EditQuestion:
                    EditQuestionGridVisibility = Visibilities.Visibile;
                    ShowQuestionTypeGrid(questionType);
                    break;
            }
        }

        private void ShowQuestionTypeGrid(string questionType)
        {
            switch (questionType)
            {
                case QuestionsTypes.WithEnteringAnswer:
                    QuestionTypeWithEnteringAnswerGridVisibility = Visibilities.Visibile;
                    break;
                case QuestionsTypes.WithSelectringAnswers:
                    QuestionTypeWithSelectingAnswerGridVisibility = Visibilities.Visibile;
                    break;
            }
        }

        #endregion


        #region ResetParameters

        private void ResetAllParameters()
        {
            ResetAllQuestionParameters();
            ResetAllTestParameters();

            ErrorVisibitity = Visibilities.Hidden;
        }

        private void ResetAllTestParameters()
        {
            TestName = null;
            TestDescription = null;
            TestMaxScore = null;
            createdQuestions.Clear();
            createdAnswers.Clear();
            TimerOn = false;

            IsSelectQuestionTypeWithEnteringAnswer = true;
        }

        private void ResetAllQuestionParameters()
        {
            Question = null;
            Answer = null;
            IncorrectAnswer1 = null;
            IncorrectAnswer2 = null;
            IncorrectAnswer3 = null;
        }

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

        private bool FieldsIsEmpty(Grids grid)
        {
            try
            {
                CheckFieldsForEmptiness(grid);
                ErrorVisibitity = Visibilities.Hidden;
                return false;
            }
            catch (EmptyFieldException ex)
            {
                ShowError(ex.Message);
                return true;
            }
        }

        private void CheckFieldsForEmptiness(Grids grid)
        {
            if (grid is Grids.CreateTest || grid is Grids.EditTest)
            {
                if ((TestName is null || TestName?.Trim() == "") ||
                    TestMaxScore is null)
                {
                    throw new EmptyFieldException("Заполните все обязательные поля");
                }

                if (TestMaxScore <= 0)
                    throw new EmptyFieldException("Неверное значение для максимальной оценки");

                if (TimerOn && 
                    (minutes == 0 && seconds == 0))
                    throw new EmptyFieldException("Неверно введено время");
            }

            if ((grid is Grids.AddQuestion || grid is Grids.EditQuestion)
                && questionType == QuestionsTypes.WithEnteringAnswer)
            {
                if ((Question is null || Question?.Trim() == "") ||
                (Answer is null || Answer?.Trim() == ""))
                    throw new EmptyFieldException("Заполните все поля");
            }

            if ((grid is Grids.AddQuestion || grid is Grids.EditQuestion)
                && questionType == QuestionsTypes.WithSelectringAnswers)
            {
                if ((Question is null || Question?.Trim() == "") ||
                (Answer is null || Answer?.Trim() == "") ||
                (IncorrectAnswer1 is null || IncorrectAnswer1?.Trim() == ""))
                    throw new EmptyFieldException("Заполните все поля");
            }
        }

        private void ShowError(string errorText)
        {
            ErrorText = errorText;
            ErrorVisibitity = Visibilities.Visibile;
        }

        #endregion


        #region ADD TEST/QUESTION PARAMETERS

        private string testName;
        public string TestName
        {
            get { return testName; }
            set
            {
                testName = value;
                OnPropertyChanged(nameof(TestName));
            }
        }

        private string testDescription;
        public string TestDescription
        {
            get { return testDescription; }
            set
            {
                testDescription = value;
                OnPropertyChanged(nameof(TestDescription));
            }
        }

        private int? testMaxScore;
        public int? TestMaxScore
        {
            get { return testMaxScore; }
            set
            {
                testMaxScore = value;
                OnPropertyChanged(nameof(TestMaxScore));
            }
        }

        private string questionType = QuestionsTypes.WithEnteringAnswer;

        private string question;
        public string Question
        {
            get { return question; }
            set
            {
                question = value;
                OnPropertyChanged(nameof(Question));
            }
        }

        private string answer;
        public string Answer
        {
            get { return answer; }
            set
            {
                answer = value;
                OnPropertyChanged(nameof(Answer));
            }
        }

        private string incorrectAnswer1;
        public string IncorrectAnswer1
        {
            get { return incorrectAnswer1; }
            set
            {
                incorrectAnswer1 = value;
                OnPropertyChanged(nameof(IncorrectAnswer1));
            }
        }

        private string incorrectAnswer2;
        public string IncorrectAnswer2
        {
            get { return incorrectAnswer2; }
            set
            {
                incorrectAnswer2 = value;
                OnPropertyChanged(nameof(IncorrectAnswer2));
            }
        }

        private string incorrectAnswer3;
        public string IncorrectAnswer3
        {
            get { return incorrectAnswer3; }
            set
            {
                incorrectAnswer3 = value;
                OnPropertyChanged(nameof(IncorrectAnswer3));
            }
        }


        private bool timerOn;
        public bool TimerOn
        {
            get { return timerOn; }
            set
            {
                timerOn = value;
                OnPropertyChanged(nameof(TimerOn));
            }
        }

        private int minutes = 0;
        public int Minutes
        {
            get { return minutes; }
            set
            {
                if (value > 60)
                    value = 60;

                if (value < 0)
                    value = 0;

                minutes = value;
                OnPropertyChanged(nameof(Minutes));
            }
        }

        private int seconds = 0;
        public int Seconds
        {
            get { return seconds; }
            set
            {
                if (value > 60)
                    value = 60;

                if (value < 0)
                    value = 0;

                seconds = value;
                OnPropertyChanged(nameof(Seconds));
            }
        }


        private List<List<string>> createdIncorrectAnswers = new();

        private List<string> createdQuestions = new();

        private List<string> createdAnswers = new();

        private List<string> questionTypes = new();

        #endregion



        private RelayCommand showHomePanel;
        public RelayCommand ShowHomePanel
        {
            get
            {
                return showHomePanel ??
                    (showHomePanel = new RelayCommand((obj) =>
                    {
                        ShowGrid(Grids.Main);
                    }));
            }
        }

        private RelayCommand discardChangesAndShowHomePanel;
        public RelayCommand DiscardChangesAndShowHomePanel
        {
            get
            {
                return discardChangesAndShowHomePanel ??
                    (discardChangesAndShowHomePanel = new RelayCommand((obj) =>
                    {
                        MessageBoxResult messageBoxResult = Views.MessageBox.Show(
                            "Вы действительно хотите отменить изменения?",
                            "Данные теста не были сохранены",
                            MessageBoxButton.YesNo);
                        if (messageBoxResult == MessageBoxResult.Yes)
                        {
                            ShowGrid(Grids.Main);
                        }
                    }));
            }
        }



        #region CREATE TEST

        private RelayCommand showCreateTestPanel;
        public RelayCommand ShowCreateTestPanel
        {
            get
            {
                return showCreateTestPanel ??
                    (showCreateTestPanel = new RelayCommand((obj) =>
                    {
                        ResetAllParameters();
                        ShowGrid(Grids.CreateTest);
                    }));
            }
        }



        private RelayCommand showAddQuestionPanel;
        public RelayCommand ShowAddQuestionPanel
        {
            get
            {
                return showAddQuestionPanel ??
                    (showAddQuestionPanel = new RelayCommand((obj) =>
                    {
                        if (!FieldsIsEmpty(Grids.CreateTest))
                        {
                            ShowGrid(Grids.AddQuestion, questionType);
                            ErrorVisibitity = Visibilities.Hidden;
                        }
                    }));
            }
        }


        private bool isSelectQuestionTypeWithEnteringAnswer = true;
        public bool IsSelectQuestionTypeWithEnteringAnswer
        {
            get { return isSelectQuestionTypeWithEnteringAnswer; }
            set
            {
                isSelectQuestionTypeWithEnteringAnswer = value;

                if (value == true)
                {
                    questionType = QuestionsTypes.WithEnteringAnswer;
                    if (AddQuestionGridVisibility == Visibilities.Visibile)
                    {
                        ResetAllQuestionParameters();
                        ShowGrid(Grids.AddQuestion, questionType);
                    }
                    if (EditQuestionGridVisibility == Visibilities.Visibile)
                    {
                        ShowGrid(Grids.EditQuestion, questionType);
                    }
                }

                OnPropertyChanged(nameof(IsSelectQuestionTypeWithEnteringAnswer));
            }
        }

        private bool isSelectQuestionTypeWithSelectingAnswer = false;
        public bool IsSelectQuestionTypeWithSelectingAnswer
        {
            get { return isSelectQuestionTypeWithSelectingAnswer; }
            set
            {
                isSelectQuestionTypeWithSelectingAnswer = value;

                if (value == true)
                {
                    questionType = QuestionsTypes.WithSelectringAnswers;
                    if (AddQuestionGridVisibility == Visibilities.Visibile)
                    {
                        ResetAllQuestionParameters();
                        ShowGrid(Grids.AddQuestion, questionType);
                    }
                    if (EditQuestionGridVisibility == Visibilities.Visibile)
                    {
                        ShowGrid(Grids.EditQuestion, questionType);
                    }
                }

                OnPropertyChanged(nameof(IsSelectQuestionTypeWithSelectingAnswer));
            }
        }


        private RelayCommand saveQuestion;
        public RelayCommand SaveQuestion
        {
            get
            {
                return saveQuestion ??
                    (saveQuestion = new RelayCommand((obj) =>
                    {
                        if (FieldsIsEmpty(Grids.AddQuestion))
                            return;

                        createdQuestions.Add(Question);
                        createdAnswers.Add(Answer);
                        questionTypes.Add(questionType);

                        if (IncorrectAnswer1?.Trim() == "") IncorrectAnswer1 = null;
                        if (IncorrectAnswer2?.Trim() == "") IncorrectAnswer2 = null;
                        if (IncorrectAnswer3?.Trim() == "") IncorrectAnswer3 = null;
                        createdIncorrectAnswers.Add(new List<string>
                        {
                            IncorrectAnswer1,
                            IncorrectAnswer2,
                            IncorrectAnswer3
                        });

                        ResetAllQuestionParameters();
                    }));
            }
        }

        private RelayCommand saveTest;
        public RelayCommand SaveTest
        {
            get
            {
                return saveTest ??
                    (saveTest = new RelayCommand((obj) =>
                    {
                        if (createdQuestions.Count == 0)
                        {
                            Views.MessageBox.Show("В тесте должен быть как минимум 1 вопрос", "Создание теста", MessageBoxButton.OK);
                            return;
                        }

                        
                        var createDate = DateTime.Now.Date;

                        Test newTest = new Test
                        {
                            teacher_id = Authorization.LoggedUser.id,
                            name = TestName,
                            description = TestDescription,
                            max_mark = (int)TestMaxScore,
                            create_date = createDate
                        };

                        DataBaseWriter.AddTest(newTest);

                        QuestionData questionData = new QuestionData
                        {
                            questionTypes = questionTypes,
                            questions = createdQuestions,
                            answers = createdAnswers,
                            incorrectAnswers = createdIncorrectAnswers,
                            timerOn = TimerOn,
                            minutes = Minutes,
                            seconds = Seconds
                        };

                        string QuestionData = QuestionWriter.DataToString(questionData);


                        AddQuestionsInTest(newTest, QuestionData);

                        ShowGrid(Grids.Main);
                    }));
            }
        }

        private void AddQuestionsInTest(Test test, string questionData)
        {
            QuestionWriter QW = new QuestionWriter();

            QW.Data = questionData;
            QW.SelectedTest = test;

            QW.SaveQuestions();
        }

        #endregion



        private RelayCommand removeTest;
        public RelayCommand RemoveTest
        {
            get
            {
                return removeTest ??
                    (removeTest = new RelayCommand((obj) =>
                    {
                        MessageBoxResult deletionConfirmation = Views.MessageBox.Show(
                            $"Вы действительно хотите удалить тест \"{SelectedTest.name}\"",
                            "Удалить тест",
                            MessageBoxButton.YesNo);

                        if (deletionConfirmation is MessageBoxResult.No)
                            return;

                        DataBaseWriter.RemoveTest(SelectedTest);
                        UpdateTestsList();
                        ButtonsWorkWithTestEnabled = false;
                    }));
            }
        }



        #region ADD & REMOVE STUDENTS

        private List<User> addedToTestStudents;
        public List<User> AddedToTestStudents
        {
            get { return addedToTestStudents; }
            set
            {
                addedToTestStudents = value;
                OnPropertyChanged(nameof(AddedToTestStudents));
            }
        }

        private List<User> notAddedToTestStudents;
        public List<User> NotAddedToTestStudents
        {
            get { return notAddedToTestStudents; }
            set
            {
                notAddedToTestStudents = value;
                OnPropertyChanged(nameof(NotAddedToTestStudents));
            }
        }

        private string noStudentsAddedToTest_WarningVisibility = Visibilities.Collapsed;
        public string NoStudentsAddedToTest_WarningVisibility
        {
            get { return noStudentsAddedToTest_WarningVisibility; }
            set
            {
                noStudentsAddedToTest_WarningVisibility = value;
                OnPropertyChanged(nameof(NoStudentsAddedToTest_WarningVisibility));
            }
        }


        private RelayCommand showAddStudentToTestPanel;
        public RelayCommand ShowAddStudentToTestPanel
        {
            get
            {
                return showAddStudentToTestPanel ??
                    (showAddStudentToTestPanel = new RelayCommand((obj) =>
                    {
                        UpdateAndShowStudentsLists();

                        ShowGrid(Grids.AddStudentsToTest);
                    }));
            }
        }

        private void UpdateAndShowStudentsLists()
        {
            AddedToTestStudents = new List<User>();
            TryGetAddedToTestStudents();
            TryGetNotAddedToTestStudents();

            RemoveStudentButtonEnabled = false;
            AddStudentButtonEnabled = false;
        }

        private void TryGetAddedToTestStudents()
        {
            try
            {
                AddedToTestStudents = DataBaseReader.GetAddedToTestUsers(SelectedTest);
                NoStudentsAddedToTest_WarningVisibility = Visibilities.Collapsed;
            }
            catch (NoAddedStudentsException)
            {
                NoStudentsAddedToTest_WarningVisibility = Visibilities.Visibile;
            }
        }

        private void TryGetNotAddedToTestStudents()
        {
            try
            {
                NotAddedToTestStudents = DataBaseReader.GetNotAddedToTestStudents(SelectedTest);
            }
            catch
            {
                NotAddedToTestStudents.Clear();
            }
        }



        private User selectedNotAddedStudent;
        public User SelectedNotAddedStudent
        {
            get { return selectedNotAddedStudent; }
            set
            {
                selectedNotAddedStudent = value;
                AddStudentButtonEnabled = true;
                OnPropertyChanged(nameof(SelectedNotAddedStudent));
            }
        }

        private RelayCommand addStudentToTest;
        public RelayCommand AddStudentToTest
        {
            get
            {
                return addStudentToTest ??
                    (addStudentToTest = new RelayCommand(obj =>
                    {
                        DataBaseWriter.AddStudentToTest(SelectedNotAddedStudent, SelectedTest);
                        UpdateAndShowStudentsLists();
                    }));
            }
        }


        private User selectedAddedStudent;
        public User SelectedAddedStudent
        {
            get { return selectedAddedStudent; }
            set
            {
                selectedAddedStudent = value;
                RemoveStudentButtonEnabled = true;
                OnPropertyChanged(nameof(SelectedAddedStudent));
            }
        }


        private RelayCommand removeStudentFromTest;
        public RelayCommand RemoveStudentFromTest
        {
            get
            {
                return removeStudentFromTest ??
                    (removeStudentFromTest = new RelayCommand(obj =>
                    {
                        DataBaseWriter.RemoveStudentFromTest(SelectedAddedStudent, SelectedTest);
                        UpdateAndShowStudentsLists();
                    }));
            }
        }



        private List<string> allGroups = DataBaseReader.GetAllGroups();
        public List<string> AllGroups
        {
            get { return allGroups; }
            set
            {
                allGroups = value;
            }
        }

        private string selectedGroup;
        public string SelectedGroup
        {
            get { return selectedGroup; }
            set
            {
                selectedGroup = value;
                OnPropertyChanged(nameof(SelectedGroup));
            }
        }


        private RelayCommand addGroupToTest;
        public RelayCommand AddGroupToTest
        {
            get
            {
                return addGroupToTest ??
                    (addGroupToTest = new RelayCommand((obj) =>
                {
                    DataBaseWriter.AddGroupToTest(SelectedGroup, SelectedTest);
                    UpdateAndShowStudentsLists();
                }));
            }
        }

        private RelayCommand removeGroupFromTest;
        public RelayCommand RemoveGroupFromTest
        {
            get
            {
                return removeGroupFromTest ??
                    (removeGroupFromTest = new RelayCommand((obj) =>
                    {
                        DataBaseWriter.RemoveGroupFromTest(SelectedGroup, SelectedTest);
                        UpdateAndShowStudentsLists();
                    }));
            }
        }

        #endregion


        #region TEST RESULTS

        private List<TestResults> testResults;
        public List<TestResults> TestResults
        {
            get { return testResults; }
            set
            {
                testResults = value;
                OnPropertyChanged(nameof(TestResults));
            }
        }

        private RelayCommand showResultsPanel;
        public RelayCommand ShowResultsPanel
        {
            get
            {
                return showResultsPanel ??
                    (showResultsPanel = new RelayCommand((obj) =>
                    {
                        TestResults = DataBaseReader.GetTestInfo(SelectedTest);
                        ShowGrid(Grids.Results);
                    }));
            }
        }


        private enum SearchFilters
        {
            BySurname,
            ByGroup
        }

        private SearchFilters searchFilter = SearchFilters.BySurname;

        

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

        private bool changeFilterToSurname_checked = true;
        public bool ChangeFilterToSurname_checked
        {
            get { return changeFilterToSurname_checked; }
            set
            {
                changeFilterToSurname_checked = value;
                OnPropertyChanged(nameof(ChangeFilterToSurname_checked));
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
                            case SearchFilters.BySurname:
                                TestResults = DataBaseReader.FindTestInfoBySurname(SearchText, SelectedTest);
                                break;
                            case SearchFilters.ByGroup:
                                TestResults = DataBaseReader.FindTestInfoByGroup(SearchText, SelectedTest);
                                break;
                        }
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
                        TestResults = DataBaseReader.GetTestInfo(SelectedTest);
                        ChangeFilterToSurname_checked = true;
                        searchFilter = SearchFilters.BySurname;
                        SearchText = null;
                    }));
            }
        }

        #endregion


        #region EDIT TEST

        private RelayCommand showEditTestPanel;
        public RelayCommand ShowEditTestPanel
        {
            get
            {
                return showEditTestPanel ??
                    (showEditTestPanel = new RelayCommand((obj) =>
                    {
                        QR = new QuestionsReader();
                        QR.SelectedTest = selectedTest;

                        GetQuestionData();

                        TestName = SelectedTest.name;
                        TestDescription = SelectedTest.description;
                        TestMaxScore = SelectedTest.max_mark;

                        TimerOn = questionData.timerOn;
                        Minutes = questionData.minutes;
                        Seconds = questionData.seconds;

                        ResetAllEditTestParamiters();

                        ShowGrid(Grids.EditTest);
                    }));
            }
        }

        private void ResetAllEditTestParamiters()
        {
            QuestionNumber = 0;
            createdQuestions.Clear();
            createdAnswers.Clear();
            createdIncorrectAnswers.Clear();
            questionTypes.Clear();
            questionsOver = false;
            needToAddNewQuesion = false;
        }


        private QuestionsReader QR = new QuestionsReader();

        private int QuestionNumber = 0;


        private RelayCommand showEditQuestionPanel;
        public RelayCommand ShowEditQuestionPanel
        {
            get
            {
                return showEditQuestionPanel ??
                    (showEditQuestionPanel = new RelayCommand((obj) =>
                    {
                        if (FieldsIsEmpty(Grids.EditTest))
                            return;

                        ResetAllQuestionParameters();

                        ShowGrid(Grids.EditQuestion, questionData.questionTypes[QuestionNumber]);
                        tryToShowQuestion();
                    }));
            }
        }

        private QuestionData questionData;

        private void GetQuestionData()
        {
            questionData = QR.GetQuestionData();
        }

        private RelayCommand nextQuestion;
        public RelayCommand NextQuestion
        {
            get
            {
                return nextQuestion ??
                    (nextQuestion = new RelayCommand((obj) =>
                    {
                        if (!questionsOver)
                        {
                            if (FieldsIsEmpty(Grids.EditQuestion))
                                return;

                            saveEditQuestion();
                        }

                        ++QuestionNumber;
                        tryToShowQuestion();
                    }));
            }
        }

        private RelayCommand removeQuestion;
        public RelayCommand RemoveQuestion
        {
            get
            {
                return removeQuestion ??
                    (removeQuestion = new RelayCommand((obj) =>
                    {
                        ++QuestionNumber;
                        tryToShowQuestion();
                    }));
            }
        }

        private RelayCommand addQuestion;
        public RelayCommand AddQuestion
        {
            get
            {
                return addQuestion ??
                    (addQuestion = new RelayCommand((obj) =>
                    {
                        if (!questionsOver)
                        {
                            if (FieldsIsEmpty(Grids.EditQuestion))
                                return;

                            saveEditQuestion();
                        }

                        needToAddNewQuesion = true;
                        tryToShowQuestion();
                    }));
            }
        }


        private bool questionsOver = false;
        private bool needToAddNewQuesion = false;

        private void tryToShowQuestion()
        {
            try
            {
                ResetAllQuestionParameters();

                if (needToAddNewQuesion && QuestionNumber == questionData.questionTypes.Count)
                {
                    --QuestionNumber;
                    questionsOver = false;
                }
                    
                EnableQuestionTypeGridAndShowQuestionData(questionData.questionTypes[QuestionNumber]);
            }
            catch
            {
                questionsOver = true;

                MessageBoxResult saveConfirm = Views.MessageBox.Show("Это был последний вопрос. Сохранить изменения?",
                    "Редактирование теста",
                    MessageBoxButton.YesNo);

                if (saveConfirm == MessageBoxResult.No)
                    return;


                SelectedTest.name = TestName;
                SelectedTest.description = TestDescription;
                SelectedTest.max_mark = testMaxScore ?? 0;


                QuestionData newQuestionData = new QuestionData
                {
                    questions = createdQuestions,
                    answers = createdAnswers,
                    questionTypes = questionTypes,
                    incorrectAnswers = createdIncorrectAnswers,
                    timerOn = TimerOn,
                    minutes = Minutes,
                    seconds = Seconds
                };

                string QuestionData = QuestionWriter.DataToString(newQuestionData);



                DataBaseWriter.UpdateTest(SelectedTest);
                UpdateQuestionsInTest(SelectedTest, QuestionData);



                ResetAllParameters();
                ShowGrid(Grids.Main);
            }
        }

        private void EnableQuestionTypeGridAndShowQuestionData(string type)
        {
            HideAllQuestionTypeGrids();

            switch (type)
            {
                case (QuestionsTypes.WithEnteringAnswer):
                    IsSelectQuestionTypeWithEnteringAnswer = true;
                    QuestionTypeWithEnteringAnswerGridVisibility = Visibilities.Visibile;
                    break;
                case (QuestionsTypes.WithSelectringAnswers):
                    IsSelectQuestionTypeWithSelectingAnswer = true;
                    QuestionTypeWithSelectingAnswerGridVisibility = Visibilities.Visibile;
                    break;
            }


            if (needToAddNewQuesion)
            {
                ResetAllQuestionParameters();
                needToAddNewQuesion = false;
                return;
            }

            Question = questionData.questions[QuestionNumber];
            Answer = questionData.answers[QuestionNumber];

            if (type == QuestionsTypes.WithSelectringAnswers)
            {
                List<string> incorrectAnswers = questionData.incorrectAnswers[QuestionNumber];
                IncorrectAnswer1 = incorrectAnswers[0];

                if (incorrectAnswers.Count >= 2)
                    IncorrectAnswer2 = incorrectAnswers[1];
                else IncorrectAnswer2 = null;

                if (incorrectAnswers.Count == 3)
                    IncorrectAnswer3 = incorrectAnswers[2];
                else IncorrectAnswer3 = null;
            }
        }

        private void HideAllQuestionTypeGrids()
        {
            QuestionTypeWithEnteringAnswerGridVisibility = Visibilities.Collapsed;
            QuestionTypeWithSelectingAnswerGridVisibility = Visibilities.Collapsed;
        }

        
        public void saveEditQuestion()
        {
            if (IncorrectAnswer1?.Trim() == "") IncorrectAnswer1 = null;
            if (IncorrectAnswer2?.Trim() == "") IncorrectAnswer2 = null;
            if (IncorrectAnswer3?.Trim() == "") IncorrectAnswer3 = null;

            createdQuestions.Add(Question);
            createdAnswers.Add(Answer);
            questionTypes.Add(questionType);
            createdIncorrectAnswers.Add(new List<string>
            {
                IncorrectAnswer1,
                IncorrectAnswer2,
                IncorrectAnswer3
            });
        }


        private void UpdateQuestionsInTest(Test test, string QuestionData)
        {
            QuestionWriter QW = new QuestionWriter();

            QW.Data = QuestionData;
            QW.SelectedTest = test;
            QW.questionId = questionData.id;

            QW.UpdateQuestions();
        }

        #endregion
    }
}
