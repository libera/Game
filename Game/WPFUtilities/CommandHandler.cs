using System;
using System.Windows.Input;

namespace Game.WPFUtilities
{
    /// <summary>
    /// Implementacja komendy, którą można Bindować przez ViewModel
    /// Opcja przyjmująca po prostu akcję do wykonania.
    /// </summary>
    public class CommandHandler : ICommand
    {
        private readonly Action m_action;

        /// <summary>
        /// Konstruktor
        /// </summary>
        /// <param name="action">Akcja do wykonania w ramach komendy</param>
        public CommandHandler(Action action)
        {
            m_action = action ?? throw new ArgumentNullException(nameof(action));
        }

        public event EventHandler CanExecuteChanged;

        /// <summary>
        /// Jest to prosty typ zakładający, że zawsze akcja może być wykonana.
        /// </summary>
        /// <param name="parameter">Parametr</param>
        /// <returns>True</returns>
        public bool CanExecute(object parameter)
        {
            return true;
        }

        /// <summary>
        /// Wykonuje wcześniej przekazaną akcję.
        /// </summary>
        /// <param name="parameter">parametr</param>
        public void Execute(object parameter)
        {
            m_action();
        }
    }
}