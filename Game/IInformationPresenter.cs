using System;

namespace Game
{
    /// <summary>
    /// Interface klasy przekazyjącej użytkownikowi informacje
    /// </summary>
    public interface IInformationPresenter
    {
        /// <summary>
        /// Metoda do pokazania błędu
        /// </summary>
        /// <param name="strErrorMessage">Treść błędu</param>
        /// <param name="ex">Ewentualny wyjątek</param>
        void ShowError(string strErrorMessage, Exception ex);

        /// <summary>
        /// Metoda pokazująca warning
        /// </summary>
        /// <param name="strWarningMessage">Treść ostrzeżenia</param>
        void ShowWarning(string strWarningMessage);

        /// <summary>
        /// Metoda pokazująca sukces
        /// </summary>
        /// <param name="strWarningMessage">Treść wiadomości o sukcesie</param>
        void ShowSuccess(string strWarningMessage);
    }
}