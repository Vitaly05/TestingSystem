using System.IO;
using System.Windows;
using TestingSystem.Data;

namespace TestingSystem.Views
{
    /// <summary>
    /// Логика взаимодействия для AuthorizationWindow.xaml
    /// </summary>
    public partial class AuthorizationWindow : Window
    {
        public AuthorizationWindow()
        {
            InitializeComponent();

            LoadTheme();
        }

        private void LoadTheme()
        {
            try
            {
                using (BinaryReader reader = new(File.Open("Theme.dat", FileMode.Open)))
                {
                    string theme = reader.ReadString();
                    ThemeSwitcher.SetTheme(theme);
                }
            }
            catch { }
        }
    }
}
