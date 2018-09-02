using Game.Logic.Reels;
using System;
using System.Collections.Generic;

namespace Game.Logic.Machines
{
    /// <summary>
    /// Interfejs maszyny do grania.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IMachine<T> where T : Enum
    {
        /// <summary>
        /// Stawka obecnej gry
        /// </summary>
        uint Bid { get; }

        /// <summary>
        /// Posiadane punkty\kredyty
        /// </summary>
        uint Credits { get; }

        /// <summary>
        /// Liczba wygranych od uruchomienia maszyny
        /// </summary>
        uint Wins { get; }

        /// <summary>
        /// Metoda pozwalająca dodać kredyty\punkty
        /// </summary>
        /// <param name="nCredits">Liczba kredytów do dodania</param>
        void AddCredits(uint nCredits);

        /// <summary>
        /// Metoda do zmiany stawki gry
        /// </summary>
        /// <param name="nNewBid">Nowa stawka gry</param>
        void ChangeBid(uint nNewBid);

        /// <summary>
        /// Metoda kończąca grę, zeruje liczbę kredytów i zwraca ją
        /// </summary>
        /// <returns>Zwraca liczbę pozostałych kredytów</returns>
        uint End();

        /// <summary>
        /// Metoda zwracająca obecne położenie każdego z bębnów
        /// </summary>
        /// <returns>Kolekcja położenia każdego z bębnów ("główny" stan, poprzedni oraz następny)</returns>
        IEnumerable<IReelPosition<T>> GetCurrentPossition();

        /// <summary>
        /// Metoda wykonująca "zakręcenie" bębnami maszyny.
        /// Po zakończeniu aktualizowana jest ilość wygranych jeśli jest taka potrzeba oraz liczba punktów.
        /// </summary>
        /// <returns>Kolekcja historia kolejnych "głównych" elementów każego z węzłów wraz z wartością wygranej jeśli nastąpiła</returns>
        (IEnumerable<IEnumerable<T>> reelsSpinHistory, uint? nWinValue) Spin();
    }
}