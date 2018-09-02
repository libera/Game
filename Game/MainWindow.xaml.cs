using System;
using System.Windows;

namespace Game
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Konstruktor
        /// </summary>
        /// <param name="model">ViewModel okna</param>
        internal MainWindow(GameViewModel model)
        {
            InitializeComponent();
            DataContext = model ?? throw new ArgumentNullException(nameof(model));
        }
    }
}