using System;
using System.Collections.Generic;
using System.Linq;

namespace Game.Logic.ResultGenerators
{
    /// <summary>
    /// Generator zwracający wyniki wcześniej określone, w określonej kolejności
    /// </summary>
    /// <typeparam name="T">Enum, na którym ma operować generator</typeparam>
    internal class ResultGeneratorFix<T> : IResultGenerator<T> where T : Enum
    {
        private IEnumerable<IEnumerable<T>> m_fixResults;
        private int nGeneratorIteration = 0;

        /// <summary>
        /// Inicjalizuje listę wcześniej ustalonymi wartościami
        /// </summary>
        /// <param name="fixResults">Wyniki jakie mają być ustawione</param>
        public ResultGeneratorFix(IEnumerable<IEnumerable<T>> fixResults)
        {
            if (fixResults == null || fixResults.Count() == 0)
            {
                throw new ArgumentException("FixResyltsList must be passed with at least one result", nameof(fixResults));
            }

            m_fixResults = fixResults;
        }

        /// <summary>
        /// Zwraca kolejny wcześniej ustalony rezultat
        /// </summary>
        /// <returns>Rezultat ułożenia symboli na kolejnych bębnach</returns>
        public IEnumerable<T> GetNextResult()
        {
            return m_fixResults.ElementAt(nGeneratorIteration++ % m_fixResults.Count());
        }

        /// <summary>
        /// Metoda resetuje generator do wartości początkowej
        /// </summary>
        public void Reset()
        {
            nGeneratorIteration = 0;
        }
    }
}