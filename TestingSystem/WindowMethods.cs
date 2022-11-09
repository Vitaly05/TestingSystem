using System.Windows;

namespace TestingSystem
{
    internal static class WindowMethods
    {
        public static void closeThisWindow(object window)
        {
            ((Window)window).Close();
        }

        public static void minimizeThisWindow(object window)
        {
            ((Window)window).WindowState = WindowState.Minimized;
        }

        public static void dragMoveThisWindow(object window)
        {
            ((Window)window).DragMove();
        }

        public static void CloseAllWindowsExcept(Window exceptedWindow)
        {
            var windows = Application.Current.Windows;
            foreach (Window window in windows)
                if (window != exceptedWindow)
                    window.Close();
        }
    }
}
