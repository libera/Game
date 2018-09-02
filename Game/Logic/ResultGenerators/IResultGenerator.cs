using System;
using System.Collections.Generic;

namespace Game.Logic.ResultGenerators
{
    /// <summary>
    /// Interfejs generatora wyników
    /// </summary>
    /// <typeparam name="T">Enum, na którym ma operować generator</typeparam>
    internal interface IResultGenerator<T> where T : Enum
    {
        /// <summary>
        /// Metoda zwracająca kolejny rezultat jaki ma być na poszczególnych bębnach maszyny
        /// </summary>
        /// <returns>Uporządkowany zbiór symboli, jaki ma się znaleźć na poszczególnych bębnach</returns>
        IEnumerable<T> GetNextResult();
    }
}