using System;
using System.Diagnostics;
using System.Windows;

namespace Game
{
    /// <summary>
    /// Bazowy informator pokazujący informacje w postaci MessageBoxów
    /// </summary>
    internal class InformationPresenter : IInformationPresenter
    {
        ///<inheritdoc />
        public void ShowError(string strErrorMessage, Exception ex)
        {
            MessageBox.Show(strErrorMessage, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            Debug.WriteLine(ex);
        }

        ///<inheritdoc />
        public void ShowSuccess(string strWarningMessage)
        {
            MessageBox.Show(strWarningMessage, "Wygrana", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        ///<inheritdoc />
        public void ShowWarning(string strWarningMessage)
        {
            MessageBox.Show(strWarningMessage, "Uwaga", MessageBoxButton.OK, MessageBoxImage.Warning);
        }
    }
}