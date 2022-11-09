namespace TestingSystem
{
    internal class WindowCommands
    {
        private RelayCommand closeWindow;
        public RelayCommand CloseWindow
        {
            get
            {
                return closeWindow ??
                    (closeWindow = new RelayCommand((window) =>
                    {
                        WindowMethods.closeThisWindow(window);
                    }));
            }
        }

        private RelayCommand minimizeWindow;
        public RelayCommand MinimizeWindow
        {
            get
            {
                return minimizeWindow ??
                    (minimizeWindow = new RelayCommand((window) =>
                    {
                        WindowMethods.minimizeThisWindow(window);
                    }));
            }
        }

        private RelayCommand dragMoveWindow;
        public RelayCommand DragMoveWindow
        {
            get
            {
                return dragMoveWindow ??
                    (dragMoveWindow = new RelayCommand((window) =>
                    {
                        WindowMethods.dragMoveThisWindow(window);
                    }));
            }
        }

        private RelayCommand returnToAuthorizationWindow;
        public RelayCommand ReturnToAuthorizationWindow
        {
            get
            {
                return returnToAuthorizationWindow ??
                    (returnToAuthorizationWindow = new RelayCommand((window) =>
                    {
                        Views.AuthorizationWindow authWindow = new Views.AuthorizationWindow();
                        authWindow.Show();
                        WindowMethods.closeThisWindow(window);
                    }));
            }
        }

        private RelayCommand showSettingsWindow;
        public RelayCommand ShowSettingsWindow
        {
            get
            {
                return showSettingsWindow ??
                    (showSettingsWindow = new RelayCommand((window) =>
                    {
                        Views.SettingsWindow settingsWindow = new Views.SettingsWindow();
                        settingsWindow.ShowDialog();
                    }));
            }
        }
    }
}
