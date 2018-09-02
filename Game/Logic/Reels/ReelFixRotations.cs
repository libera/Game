using System;
using System.Collections.Generic;
using System.Linq;

namespace Game.Logic.Reels
{
    /// <summary>
    /// Implementacja rolki, która przed zwróceniem kolejnego wyniku, wykonuje zadaną ilość obrotów.
    /// </summary>
    /// <typeparam name="T">Enum, na którym ma operować bęben</typeparam>
    internal class ReelFixRotations<T> : IReel<T> where T : Enum
    {
        private readonly ushort m_nNumberOfRotation;
        private readonly int m_nReelSize;
        private int m_nSelectedItemIntex;
        private List<T> m_reelElements;

        /// <summary>
        /// Konstruktor
        /// </summary>
        /// <param name="elemtns">Kolekcja elementów na bębnie, kolekcja musi zawierać co najmniej 2 elementy</param>
        /// <param name="startElement">Początkowy element</param>
        /// <param name="nNumberOfRotation">Liczba obrotów przez ustawieniem nowej wartości</param>
        public ReelFixRotations(IEnumerable<T> elemtns, T startElement, ushort nNumberOfRotation)
        {
            if (elemtns == null || elemtns.Count() < 2)
            {
                throw new ArgumentException("Elements list must be provided with at least 2 elements", nameof(elemtns));
            }

            if (!elemtns.Contains(startElement))
            {
                throw new ArgumentException("There is no start element on the elements list", nameof(startElement));
            }

            m_reelElements = elemtns.ToList();
            m_nReelSize = m_reelElements.Count();
            m_nNumberOfRotation = nNumberOfRotation;

            m_nSelectedItemIntex = GetItemIndex(startElement);
        }

        /// <inheritdoc />
        public bool CheckIfExist(T value)
        {
            return m_reelElements.Contains(value);
        }

        /// <inheritdoc />
        public IReelPosition<T> GetCurrentPossition()
        {
            return new ReelPosition<T>(
                m_reelElements[m_nSelectedItemIntex],
                m_reelElements[(m_nSelectedItemIntex + 1) % m_nReelSize],
                m_reelElements[(m_nSelectedItemIntex - 1 + m_nReelSize) % m_nReelSize]);
        }

        /// <summary>
        /// Metoda ustawiająca wskazaną wartość na bębnie.
        /// Bęben najpierw obraca się zadaną wcześniej liczbę obrotów, a dopiero póżniej ustawia wskazany element.
        /// Jeżeli na bębnie jest wiele elmentów danego typu, ustawiany jest ten najbardziej oddalony.
        /// </summary>
        /// <param name="drewValue">Wartość do ustawienia</param>
        /// <returns>Kolekcja "głównych" elementów do czasu ustawienia wskazanej wartości.</returns>
        public IEnumerable<T> SetNextValue(T drewValue)
        {
            List<T> positionsHistory = new List<T>();
            //Na początku zadana ilość obrotów
            for (int r = 0; r < m_nNumberOfRotation; r++)
            {
                for (int i = 1; i <= m_nReelSize; i++)
                {
                    positionsHistory.Add(m_reelElements[(m_nSelectedItemIntex + i) % m_nReelSize]);
                }
            }

            int nNextIndexPossition = GetItemIndex(drewValue);
            //Idziemy teraz do pozycji jaką nam wskazała metoda
            do
            {
                m_nSelectedItemIntex = (m_nSelectedItemIntex + 1) % m_nReelSize;
                positionsHistory.Add(m_reelElements[m_nSelectedItemIntex]);
            }
            while (m_nSelectedItemIntex != nNextIndexPossition);

            return positionsHistory;
        }

        /// <summary>
        /// Metoda zwracająca index ostatniego elementu (w zględem obecnego położenia), pod którym znajduje się wskazany element.
        /// </summary>
        /// <param name="element">Element, który ma zostać znaleziony</param>
        /// <returns>Ostatni indeks danego elementu, względem obecnej pozycji</returns>
        private int GetItemIndex(T element)
        {
            if (!m_reelElements.Contains(element))
            {
                throw new InvalidOperationException($"Reel does not contains element {element}");
            }

            for (int i = m_nSelectedItemIntex; true; i--)
            {
                //Modulo samo w sobie z wartości ujemnych nie przejdzie na dodatnie
                //Dlatego dla zabezpieczenia musimy dodać wielkość listy i dopiero zrobić modulo
                int nIndex = (i + m_nReelSize) % m_nReelSize;
                if (m_reelElements[nIndex].Equals(element))
                {
                    return nIndex;
                }
            }
        }
    }
}