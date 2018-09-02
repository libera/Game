using System;
using System.Collections.Generic;

namespace Game.Logic.Reels
{
    /// <summary>
    /// Interfejs pojedyczego bębna
    /// </summary>
    /// <typeparam name="T">Enum, na którym ma operować bęben</typeparam>
    internal interface IReel<T> where T : Enum
    {
        /// <summary>
        /// Metoda zwracająca obecne ułożenie rolki
        /// </summary>
        /// <returns>Obiekt zawierający informację o położeniu bębna</returns>
        IReelPosition<T> GetCurrentPossition();

        /// <summary>
        /// Metoda sprawdzająca czy na danym bębnie znajduje się dana wartość.
        /// Ma to zapobiedz ewentualnego rzuceniu wyjątka przy ustawianiu kolejnej wartości.
        /// </summary>
        /// <param name="value">Wartość</param>
        /// <returns>Czy dana wartość znajduje się na rolce</returns>
        bool CheckIfExist(T value);

        /// <summary>
        /// Metoda ustawiająca na bębnie daną wartość
        /// </summary>
        /// <param name="drewValue">Wylosowana nowa wartość</param>
        /// <returns>Wartości, jakie były kolejno ustawiane jako "główne" do czasu ustawienia wskazanej wartości</returns>
        IEnumerable<T> SetNextValue(T drewValue);
    }
}