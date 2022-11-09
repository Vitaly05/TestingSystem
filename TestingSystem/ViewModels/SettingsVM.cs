using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.IO;

using TestingSystem.Data;

namespace TestingSystem.ViewModels
{
    internal class SettingsVM : WindowCommands, INotifyPropertyChanged
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


        private RelayCommand switchToLightTheme;
        public RelayCommand SwitchToLightTheme
        {
            get
            {
                return switchToLightTheme ??
                    (switchToLightTheme = new RelayCommand((obj) =>
                    {
                        SwitchTheme(ThemeSwitcher.Themes.Light);
                    }));
            }
        }

        private RelayCommand switchToDarkTheme;
        public RelayCommand SwitchToDarkTheme
        {
            get
            {
                return switchToDarkTheme ??
                    (switchToDarkTheme = new RelayCommand((obj) =>
                    {
                        SwitchTheme(ThemeSwitcher.Themes.Dark);
                    }));
            }
        }


        private void SwitchTheme(string theme)
        {
            if (ConfirmSwitchTheme())
            {
                ThemeSwitcher.SetTheme(theme);

                SaveThemeInFile(theme);

                CloseAllWindowsAndShowAuthorization();
            }
        }

        private bool ConfirmSwitchTheme()
        {
            MessageBoxResult confirm = Views.MessageBox.Show("Для смены темы, необходимо перезайти. Все несохранённые данные будут потеряны. Выйти и применить тему?",
                "Изменение темы",
                MessageBoxButton.OKCancel);

            if (confirm == MessageBoxResult.OK)
                return true;
            else return false;
        }

        private void SaveThemeInFile(string theme)
        {
            using (BinaryWriter writer = new BinaryWriter(File.Open("Theme.dat", FileMode.OpenOrCreate)))
            {
                writer.Write(theme);
            }
        }

        private void CloseAllWindowsAndShowAuthorization()
        {
            var authWindow = new Views.AuthorizationWindow();
            authWindow.Show();

            WindowMethods.CloseAllWindowsExcept(authWindow);
        }
    }
}
