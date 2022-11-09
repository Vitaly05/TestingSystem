using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Runtime.CompilerServices;
using System.Collections.ObjectModel;

using TestingSystem.Data;
using TestingSystem.Data.DataBase;
using TestingSystem.Data.Structs;
using TestingSystem.Models;
using TestingSystem.Exceptions;


namespace TestingSystem.ViewModels
{
    internal class StudentVM : WindowCommands, INotifyPropertyChanged
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

        private Timer testTimer;
        public Timer TestTimer
        {
            get { return testTimer; }
            set
            {
                testTimer = value;
                OnPropertyChanged(nameof(TestTimer));
            }
        }

        private List<Test> tests;
        public List<Test> Tests
        {
            get { return tests ?? DataBaseReader.GetAllTestsAvailableForThisStudent(); }
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
                ButtonsWorkWithTestEnabled = true;
                OnPropertyChanged(nameof(SelectedTest));
            }
        }

        private QuestionsReader QR;

        private QuestionData questionData;

        private int _questionNumber = 0;

        private int questionNumber
        {
            get { return _questionNumber; }
            set
            {
                _questionNumber = value;
            }
        }
        public int QuestionNumber
        {
            get
            {
                if (_questionNumber != questionCount)
                    return _questionNumber + 1;
                return _questionNumber;
            }
            set { }
        }

        private int questionCount;
        public int QuestionCount
        {
            get { return questionCount; }
            set { questionCount = value; OnPropertyChanged(nameof(QuestionCount)); }
        }

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

        private List<string> answers = new List<string>(4);
        public List<string> Answers
        {
            get { return answers; }
            set
            {
                answers = value;
                OnPropertyChanged(nameof(Answers));
            }
        }

        private string studentAnswer;
        public string StudentAnswer
        {
            get { return studentAnswer; }
            set
            {
                studentAnswer = value;
                OnPropertyChanged(nameof(StudentAnswer));
            }
        }

        private List<string> studentAnswers = new();



        private RelayCommand startSelectedTest;
        public RelayCommand StartSelectedTest
        {
            get
            {
                return startSelectedTest ??
                    (startSelectedTest = new RelayCommand((obj) =>
                    {
                        ResetTestParameters();

                        QR = new QuestionsReader();
                        QR.SelectedTest = selectedTest;

                        GetQuestionsData();

                        if (SelectedTest.testInfoForThisStudent.mark is not null)
                        {
                            Views.MessageBox.Show("Вы уже прошли этот тест", "Прохождение теста", MessageBoxButton.OK);
                            return;
                        }

                        ShowGrid(Grids.PassageTest);
                        questionNumber = 0;
                        studentAnswers.Clear();

                        tryToShowQuestion();

                        if (questionData.timerOn)
                        {
                            TimerVisibility = Visibilities.Visibile;

                            TestTimer = new Timer(questionData.minutes, questionData.seconds);
                            TestTimer.EndActions += FinishTest;
                            TestTimer.StartTimer();
                        }
                    }));
            }
        }

        private void GetQuestionsData()
        {
            questionData = QR.GetQuestionData();
            QuestionCount = questionData.questions.Count;
        }


        private RelayCommand nextQuestion;
        public RelayCommand NextQuestion
        {
            get
            {
                return nextQuestion ??
                    (nextQuestion = new RelayCommand((obj) =>
                    {
                        if (FieldsIsEmpty())
                        {
                            StudentAnswer = null;
                            return;
                        }


                        studentAnswers.Add(StudentAnswer);
                        ++questionNumber;
                        tryToShowQuestion();

                        StudentAnswer = null;
                        ErrorVisibitity = Visibilities.Hidden;
                    }));
            }
        }

        private void tryToShowQuestion()
        {
            try
            {
                OnPropertyChanged(nameof(QuestionNumber));
                EnableQuestionTypeGridAndShowQuestionData(questionData.questionTypes[questionNumber]);
            }
            catch
            {
                FinishTest();
            }
        }

        private void FinishTest()
        {
            try
            {
                TestTimer?.StopTimer();

                TestChecker testChecker = new(studentAnswers, questionData.answers, SelectedTest.max_mark);
                double mark = testChecker.GetTestMark();

                Views.MessageBox.Show($"Тест пройден. Оценка: {mark}.", "Проходжение теста", MessageBoxButton.OK);
                DataBaseWriter.WriteMark(mark, SelectedTest);

                ShowGrid(Grids.Main);
            }
            catch { }
        }

        private void EnableQuestionTypeGridAndShowQuestionData(string type)
        {
            HideAllQuestionTypeGrids();
            ShowQuestion();

            switch (type)
            {
                case (QuestionsTypes.WithEnteringAnswer):
                    QuestionWithEnteringAnswersGridVisibility = Visibilities.Visibile;
                    break;
                case (QuestionsTypes.WithSelectringAnswers):
                    QuestionWithSelectingAnswersGridVisibility = Visibilities.Visibile;
                    ShowAnswerChoises();
                    break;
            }
        }

        private void HideAllQuestionTypeGrids()
        {
            QuestionWithEnteringAnswersGridVisibility = Visibilities.Collapsed;
            QuestionWithSelectingAnswersGridVisibility = Visibilities.Collapsed;

            Answer3Visibility = Visibilities.Collapsed;
            Answer4Visibility = Visibilities.Collapsed;

            UncheckAnswers();
        }

        private void ShowQuestion()
        {
            Question = questionData.questions[questionNumber];
        }

        private void ShowAnswerChoises()
        {
            Answers = GetAndRefreshAnswers();
            ShowChoises();
        }

        private void ShowChoises()
        {
            Answer1Visibility = Visibilities.Visibile;
            Answer2Visibility = Visibilities.Visibile;
            if (Answers.Count == 3)
            {
                Answer3Visibility = Visibilities.Visibile;
                Answer3ColSpan = 2;
            }
                
            if (Answers.Count == 4)
            {
                Answer3Visibility = Visibilities.Visibile;
                Answer4Visibility = Visibilities.Visibile;
                Answer3ColSpan = 1;
            }
        }

        private List<string> GetAndRefreshAnswers()
        {
            List<string> refreshedAnswers = new();

            refreshedAnswers.Add(questionData.answers[questionNumber]);
            refreshedAnswers.AddRange(questionData.incorrectAnswers[questionNumber]);
            Shuffle(ref refreshedAnswers);
            refreshedAnswers.RemoveAll(answer => answer is null);

            return refreshedAnswers;
        }

        private void Shuffle(ref List<string> list)
        {
            Random rand = new();
            for (int iii = list.Count - 1; iii >= 1; iii--)
            {
                int jjj = rand.Next(iii + 1);

                string buf = list[jjj];
                list[jjj] = list[iii];
                list[iii] = buf;
            }
        }


        private RelayCommand setAnswer;
        public RelayCommand SetAnswer
        {
            get
            {
                return setAnswer ??
                    (setAnswer = new RelayCommand((obj) =>
                    {
                        StudentAnswer = (string)obj;
                    }));
            }
        }

        private RelayCommand selectAnswer;
        public RelayCommand SelectAnswer
        {
            get
            {
                return selectAnswer ??
                    (selectAnswer = new RelayCommand((obj) =>
                    {
                        StudentAnswer = obj.ToString();
                    }));
            }
        }

        private RelayCommand endTest;
        public RelayCommand EndTest
        {
            get
            {
                return endTest ??
                    (endTest = new RelayCommand((obj) =>
                    {
                        MessageBoxResult confirm = Views.MessageBox.Show("Вы ещё не ответили на все вопросы. Вы уверены, что хотите завершить тест и получить оценку?",
                            "Завершение теста",
                            MessageBoxButton.YesNo);
                        if (confirm == MessageBoxResult.Yes)
                            FinishTest();
                    }));
            }
        }

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



        #region VISIBILITIES & etc

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

        private string passageTestGridVisibility = Visibilities.Collapsed;
        public string PassageTestGridVisibility
        {
            get { return passageTestGridVisibility; }
            set
            {
                passageTestGridVisibility = value;
                OnPropertyChanged(nameof(PassageTestGridVisibility));
            }
        }

        private string questionWithEnteringAnswersGridVisibility = Visibilities.Collapsed;
        public string QuestionWithEnteringAnswersGridVisibility
        {
            get { return questionWithEnteringAnswersGridVisibility; }
            set
            {
                questionWithEnteringAnswersGridVisibility = value;
                OnPropertyChanged(nameof(QuestionWithEnteringAnswersGridVisibility));
            }
        }

        private string questionWithSelectingAnswersGridVisibility = Visibilities.Collapsed;
        public string QuestionWithSelectingAnswersGridVisibility
        {
            get { return questionWithSelectingAnswersGridVisibility; }
            set
            {
                questionWithSelectingAnswersGridVisibility = value;
                OnPropertyChanged(nameof(QuestionWithSelectingAnswersGridVisibility));
            }
        }


        private string timerVisibility;
        public string TimerVisibility
        {
            get { return timerVisibility; }
            set
            {
                timerVisibility = value;
                OnPropertyChanged(nameof(TimerVisibility));
            }
        }



        private string answer1Visibility = Visibilities.Visibile;
        public string Answer1Visibility
        {
            get { return answer1Visibility; }
            set
            {
                answer1Visibility = value;
                OnPropertyChanged(nameof(Answer1Visibility));
            }
        }

        private string answer2Visibility = Visibilities.Visibile;
        public string Answer2Visibility
        {
            get { return answer2Visibility; }
            set
            {
                answer2Visibility = value;
                OnPropertyChanged(nameof(Answer2Visibility));
            }
        }

        private string answer3Visibility = Visibilities.Collapsed;
        public string Answer3Visibility
        {
            get { return answer3Visibility; }
            set
            {
                answer3Visibility = value;
                OnPropertyChanged(nameof(Answer3Visibility));
            }
        }

        private string answer4Visibility = Visibilities.Collapsed;
        public string Answer4Visibility
        {
            get { return answer4Visibility; }
            set
            {
                answer4Visibility = value;
                OnPropertyChanged(nameof(Answer4Visibility));
            }
        }

        private int answer3ColSpan = 0;
        public int Answer3ColSpan
        {
            get { return answer3ColSpan; }
            set
            {
                answer3ColSpan = value;
                OnPropertyChanged(nameof(Answer3ColSpan));
            }
        }

        private bool answer1Checked;
        public bool Answer1Checked
        {
            get { return answer1Checked; }
            set
            {
                answer1Checked = value;
                OnPropertyChanged(nameof(Answer1Checked));
            }
        }

        private bool answer2Checked;
        public bool Answer2Checked
        {
            get { return answer2Checked; }
            set
            {
                answer2Checked = value;
                OnPropertyChanged(nameof(Answer2Checked));
            }
        }

        private bool answer3Checked;
        public bool Answer3Checked
        {
            get { return answer3Checked; }
            set
            {
                answer3Checked = value;
                OnPropertyChanged(nameof(Answer3Checked));
            }
        }

        private bool answer4Checked;
        public bool Answer4Checked
        {
            get { return answer4Checked; }
            set
            {
                answer4Checked = value;
                OnPropertyChanged(nameof(Answer4Checked));
            }
        }

        #endregion



        private void ResetTestParameters()
        {
            StudentAnswer = null;
            TimerVisibility = Visibilities.Collapsed;
            UncheckAnswers();
        }

        private void UncheckAnswers()
        {
            Answer1Checked = false;
            Answer2Checked = false;
            Answer3Checked = false;
            Answer4Checked = false;
        }



        #region GRIDS CHANGE

        private enum Grids
        {
            Main,
            PassageTest
        }

        private void ShowGrid(Grids grid)
        {
            HideAllGrids();
            EnableGrid(grid);
        }

        private void HideAllGrids()
        {
            MainGridVisibility = Visibilities.Collapsed;
            PassageTestGridVisibility = Visibilities.Collapsed;

            QuestionWithEnteringAnswersGridVisibility = Visibilities.Collapsed;
            QuestionWithSelectingAnswersGridVisibility= Visibilities.Collapsed;
        }

        private void EnableGrid(Grids grid)
        {
            switch (grid)
            {
                case Grids.Main:
                    MainGridVisibility = Visibilities.Visibile;
                    Tests = DataBaseReader.GetAllTestsAvailableForThisStudent();
                    ButtonsWorkWithTestEnabled = false;
                    break;
                case Grids.PassageTest:
                    PassageTestGridVisibility = Visibilities.Visibile;
                    break;
            }
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
            if (questionData.questionTypes[questionNumber] == QuestionsTypes.WithEnteringAnswer)
                if (StudentAnswer is null || StudentAnswer.Replace(" ", "") == "")
                    throw new EmptyFieldException("Введите ответ");
            if (questionData.questionTypes[questionNumber] == QuestionsTypes.WithSelectringAnswers)
                if (Answer1Checked == false &&
                    Answer2Checked == false &&
                    Answer3Checked == false &&
                    Answer4Checked == false)
                    throw new EmptyFieldException("Выберите ответ");
        }

        private void ShowError(string errorText)
        {
            ErrorText = errorText;
            ErrorVisibitity = Visibilities.Visibile;
        }

        #endregion
    }
}
