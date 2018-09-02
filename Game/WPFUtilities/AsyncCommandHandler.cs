using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Game.WPFUtilities
{
    /// <summary>
    /// Asynchroniczna wersja komendy, którą można bindować do ViewModelu
    /// </summary>
    public class AsyncCommandHandler : ICommand
    {
        private readonly Func<Task> m_function;
        private bool m_bIsInProgress;
        private readonly object m_lock = new Object();

        /// <summary>
        /// Konstruktor
        /// </summary>
        /// <param name="function">Funkcja zwracająca taska</param>
        public AsyncCommandHandler(Func<Task> function)
        {
            m_function = function ?? throw new ArgumentNullException(nameof(function));
        }

        /// <inheritdoc />
        public event EventHandler CanExecuteChanged;

        /// <summary>
        /// Metoda zwracająca informację, czy akcję można wykonać.
        /// </summary>
        /// <param name="parameter">Parametry</param>
        /// <returns>Akcję można wykonać jeśli nie jest wykonywana</returns>
        public bool CanExecute(object parameter)
        {
            return !m_bIsInProgress;
        }

        /// <summary>
        /// Odpala przekazaną wcześniej akcję. W czasie wykonywania ustawia że nie można wykonać akcji ponownie.
        /// </summary>
        /// <param name="parameter">Parametry</param>
        public async void Execute(object parameter)
        {
            lock (m_lock)
            {
                m_bIsInProgress = true;
            }
            CanExecuteChanged?.Invoke(this, new EventArgs());

            try
            {
                await m_function();
            }
            finally
            {
                lock (m_lock)
                {
                    m_bIsInProgress = false;
                }
                CanExecuteChanged?.Invoke(this, new EventArgs());
            }
        }
    }
}