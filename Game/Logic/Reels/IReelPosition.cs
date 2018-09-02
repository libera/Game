using System;

namespace Game.Logic.Reels
{
    /// <summary>
    /// Interface położenia bębna
    /// </summary>
    /// <typeparam name="T">Enum, na którym ma operować bęben</typeparam>
    public interface IReelPosition<T> where T : Enum
    {
        /// <summary>
        /// Pozycja główna na bębnie
        /// </summary>
        T Current { get; }

        /// <summary>
        /// Pozycja następna na bębnie
        /// </summary>
        T Next { get; }

        /// <summary>
        /// Pozycja poprzednia na bębnie
        /// </summary>
        T Previous { get; }
    }
}