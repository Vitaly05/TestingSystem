using TestingSystem.Data.Structs;
using TestingSystem.ViewModels;

namespace TestingSystem
{
    internal class WindowCreator
    {
        private string windowType;
        public string WindowType
        {
            private get { return windowType; }
            set { windowType = value; }
        }

        public void ShowNewWindow()
        {
            switch (WindowType)
            {
                case AccountTypes.Teacher:
                    TeacherWindow teacherWindow = new TeacherWindow();
                    teacherWindow.Show();
                    break;
                case AccountTypes.Student:
                    StudentWindow studentWindow = new StudentWindow();
                    studentWindow.Show();
                    break;
                case AccountTypes.Admin:
                    AdminWindow adminWindow = new AdminWindow();
                    adminWindow.Show();
                    break;
            }
        }
    }
}
