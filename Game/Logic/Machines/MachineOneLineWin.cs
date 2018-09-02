using Game.Logic.Reels;
using Game.Logic.ResultGenerators;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Game.Logic.Machines
{
    /// <summary>
    /// Implementacja maszyny z jedną linią wygrywającą.
    /// </summary>
    /// <typeparam name="T">Enum, na którym ma operować cała gra</typeparam>
    internal class MachineOneLineWin<T> : IMachine<T> where T : Enum
    {
        private IEnumerable<IReel<T>> m_reels;
        private IResultGenerator<T> m_resultGenerator;
        private IDictionary<List<T>, uint> m_winingTable;

        /// <summary>
        /// Konstruktor
        /// </summary>
        /// <param name="nStartCredits">Początkowa ilość kredytów</param>
        /// <param name="reels">Kolekcja bębnów</param>
        /// <param name="resultGenerator">Generator kolejnych wartości</param>
        /// <param name="winingTable">Tabela wygranych</param>
        public MachineOneLineWin(uint nStartCredits, IEnumerable<IReel<T>> reels, IResultGenerator<T> resultGenerator, IDictionary<List<T>, uint> winingTable)
        {
            if (reels == null || reels.Count() < 1)
            {
                throw new ArgumentException("At least 1 reesl must be provided.", nameof(reels));
            }

            Credits = nStartCredits;
            m_reels = reels;
            m_resultGenerator = resultGenerator ?? throw new ArgumentNullException(nameof(resultGenerator));
            m_winingTable = winingTable ?? throw new ArgumentNullException(nameof(winingTable));
        }

        /// <inheritdoc />
        public uint Bid { get; private set; } = 1;

        /// <inheritdoc />
        public uint Credits { get; private set; }

        /// <inheritdoc />
        public uint Wins { get; private set; }

        /// <inheritdoc />
        public void AddCredits(uint nCredits)
        {
            Credits += nCredits;
        }

        /// <summary>
        /// Metoda ustawiająca nową wartość stawki.
        /// Wartość może być tylko 1 lub 2
        /// </summary>
        /// <param name="nNewBid">Nowa wartość stawki</param>
        public void ChangeBid(uint nNewBid)
        {
            if (nNewBid == 0 || nNewBid > 2)
            {
                throw new ArgumentException($"Invalid Bid value {nNewBid}", nameof(nNewBid));
            }

            Bid = nNewBid;
        }

        /// <inheritdoc />
        public uint End()
        {
            uint nCredits = Credits;
            Credits = 0;
            return nCredits;
        }

        /// <inheritdoc />
        public IEnumerable<IReelPosition<T>> GetCurrentPossition()
        {
            List<IReelPosition<T>> reelsState = new List<IReelPosition<T>>();

            foreach (IReel<T> reel in m_reels)
            {
                reelsState.Add(reel.GetCurrentPossition());
            }

            return reelsState;
        }

        /// <summary>
        /// Metoda wykonująca "zakręcenie" bębnami maszyny.
        /// Po zakończeniu aktualizowana jest ilość wygranych jeśli jest taka potrzeba oraz liczba punktów.
        ///
        /// Rzuca wyjątki jeśli jest niewystarczająca liczba kredytów lub generator wyników źle działa.
        /// </summary>
        /// <returns>Kolekcja historia kolejnych "głównych" elementów każego z węzłów wraz z wartością wygranej jeśli nastąpiła</returns>
        public (IEnumerable<IEnumerable<T>> reelsSpinHistory, uint? nWinValue) Spin()
        {
            //Zapamiętujemy ustawioną stawkę. Zapamiętujemy, żeby mieć oryginalną wartość jak ktoś potem by ją zmienił
            uint nSpinBid = Bid;

            if (this.Credits < nSpinBid)
            {
                throw new InvalidOperationException("Insufficient Credits.");
            }

            List<IEnumerable<T>> spinResult = new List<IEnumerable<T>>();
            List<T> currentCombination = new List<T>();

            IEnumerable<T> nextResult = m_resultGenerator.GetNextResult();

            //Nie powinno być tak, że ktoś dostarcza źle skonstruowany generator. Nie obsługujemy.
            if (nextResult == null || nextResult.Count() != m_reels.Count())
            {
                throw new InvalidOperationException("Result Generator didn't return number values exactly to number of reels.");
            }

            //Każdy z wyników powinien być osiągalny na bębnie. Nie obsługujemy błędów.
            bool bAreAllResultsPossible = true;
            for (int i = 0; i < m_reels.Count(); i++)
            {
                //Upewniamy się,  że możemy utworzyć dany wynik
                if (!m_reels.ElementAt(i).CheckIfExist(nextResult.ElementAt(i)))
                {
                    bAreAllResultsPossible = false;
                }
            }

            if (!bAreAllResultsPossible)
            {
                throw new InvalidOperationException("Returned result is nott possible to set.");
            }

            for (int i = 0; i < m_reels.Count(); i++)
            {
                IEnumerable<T> spinHistory = m_reels.ElementAt(i).SetNextValue(nextResult.ElementAt(i));
                spinResult.Add(spinHistory);

                //Zapamiętujemy też wynik obecnego położenia. Obecne położenie jest to ostatni element z historii.
                currentCombination.Add(spinHistory.LastOrDefault());
            }

            //Jak wszystko poszło, to odejmujemy od kredytów postawioną stawkę
            Credits -= nSpinBid;

            //Pobieramy wygraną dla danego ustawienia (będzie null przy braku wygranej)
            //Zakładamy że konieczne jest dokładnie takie ustawienie jak mamy.
            //Wersja bardziej rozszerzona to klasa wyników, do której przekazywalibyśmy obecny wynik i ona sama by zwracała wygraną.
            uint? nWin = m_winingTable.Where(kv => Enumerable.SequenceEqual(kv.Key, currentCombination)).Select(kv => kv.Value).Cast<uint?>().FirstOrDefault();
            //Jeżeli mamy wygraną, to mnożymy jej wartość razy stawkę i dodajemy do punktów oraz zwiększamy ilość wygranych
            if (nWin.HasValue)
            {
                nWin = nWin.Value * nSpinBid;
                Credits += nWin.Value;
                Wins++;
            }

            return (reelsSpinHistory: spinResult, nWin);
        }
    }
}