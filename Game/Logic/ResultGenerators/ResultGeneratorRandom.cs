using System;
using System.Collections.Generic;

namespace Game.Logic.ResultGenerators
{
    /// <summary>
    /// Losowy generator wyników,  które  mają  się pojawić na bębnach
    /// </summary>
    /// <typeparam name="T">Enum, na którym ma operować generator</typeparam>
    internal class ResultGeneratorRandom<T> : IResultGenerator<T> where T : Enum
    {
        private readonly uint m_nNumberOfReels;
        private Random m_random;

        /// <summary>
        /// Konstruktor inicjalizujący klasę
        /// </summary>
        /// <param name="nNumberOfReels">Liczba będnów - wymagana do zwrócenia odpowiedniego wyniku</param>
        public ResultGeneratorRandom(uint nNumberOfReels)
        {
            if (nNumberOfReels < 1)
            {
                throw new ArgumentException("There must be at least 1 reel.", nameof(nNumberOfReels));
            }
            m_random = new Random();

            m_nNumberOfReels = nNumberOfReels;
        }

        /// <summary>
        /// Metoda zwracająca losowe wyniki jakie mają się znaleźć na bębnach
        /// </summary>
        /// <returns>Odpowiednio uszeregowany zbiór, z wartościami, jakie mają się znaleźć na kolejnych bębnach</returns>
        public IEnumerable<T> GetNextResult()
        {
            T[] result = new T[m_nNumberOfReels];

            //Gdyby wartości w enumie miały być nie pokolei (1, 3, 7), to na wszelki wypadek pobieramy ich wartości
            Array enumValues = Enum.GetValues(typeof(T));
            for (int i = 0; i < m_nNumberOfReels; i++)
            {
                //Losujemy liczbę do wielkości tablicy z wartościami enuma
                int nRandomValue = m_random.Next(enumValues.Length);
                //W tablicy pobieramy element na danym losowo wybranym miejscu i dajemy go do tablicy wynikowej
                result[i] = (T)enumValues.GetValue(nRandomValue);
            }

            return result;
        }
    }
}