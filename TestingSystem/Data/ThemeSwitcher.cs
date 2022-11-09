using System;
using System.Windows;

namespace TestingSystem.Data
{
    internal class ThemeSwitcher
    {
        public struct Themes
        {
            readonly static string path = "Styles/Colors/";
            public readonly static string Light = path + "LightStyle.xaml";
            public readonly static string Dark = path + "DarkStyle.xaml";
        }

        static public void SetTheme(string theme)
        {
            var uri = new Uri(theme, UriKind.Relative);
            ResourceDictionary resourceDict = Application.LoadComponent(uri) as ResourceDictionary;

            Application.Current.Resources.Clear();
            Application.Current.Resources.MergedDictionaries.Add(resourceDict);
        }
    }
}
