using System.Windows.Input;
using System.Windows.Media;

namespace Game
{
    /// <summary>
    /// Interface ViewModelu głównego okna
    /// </summary>
    public interface IGameViewModel
    {
        /// <summary>
        /// Komenda to dodania kredytów
        /// </summary>
        ICommand AddCreditsCommand { get; }

        /// <summary>
        /// Stawka zakładu
        /// </summary>
        uint Bid { get; }

        /// <summary>
        /// Ilość punktów\kredytów
        /// </summary>
        uint Credits { get; }

        /// <summary>
        /// Komenda zmniejszenia stawki
        /// </summary>
        ICommand DecreasBidCommand { get; }

        /// <summary>
        /// Komenda zwiększenia stawki
        /// </summary>
        ICommand IncreasBidCommand { get; }

        /// <summary>
        /// Źródło głównego obrazka na bębnie 1
        /// </summary>
        ImageSource Reel1CurrentImagePath { get; }

        /// <summary>
        /// Źródło kolejnego obrazka na bębnie 1
        /// </summary>
        ImageSource Reel1NextImagePath { get; }

        /// <summary>
        /// Źródło poprzedniego obrazka na bębnie 1
        /// </summary>
        ImageSource Reel1PreviousImagePath { get; }

        /// <summary>
        /// Źródło obecnego obrazka na bębnie 2
        /// </summary>
        ImageSource Reel2CurrentImagePath { get; }

        /// <summary>
        /// Źródło następnego obrazka na bębnie 2
        /// </summary>
        ImageSource Reel2NextImagePath { get; }

        /// <summary>
        /// Źródło poprzedniego obrazka na bębnie 2
        /// </summary>
        ImageSource Reel2PreviousImagePath { get; }

        /// <summary>
        /// Źródło głównego obrazka na bębnie 3
        /// </summary>
        ImageSource Reel3CurrentImagePath { get; }

        /// <summary>
        /// Źródło następnego obrazka na bębnie 3
        /// </summary>
        ImageSource Reel3NextImagePath { get; }

        /// <summary>
        /// Źródło poprzedniego obrazka na bębnie 3
        /// </summary>
        ImageSource Reel3PreviousImagePath { get; }

        /// <summary>
        /// Property określające czy należy pokazać linię wygrywającą
        /// </summary>
        bool ShowWinningLine { get; }

        /// <summary>
        /// Komenda do "zakręcenia" grą
        /// </summary>
        ICommand SpinCommand { get; }

        /// <summary>
        /// Liczba zwycięstw
        /// </summary>
        uint Wins { get; }
    }
}