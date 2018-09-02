using System;

namespace Game.Logic.Reels
{
    /// <summary>
    /// Klasa zwracająca dane o położeniu symboli na bębnie w obecnym stanie
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal class ReelPosition<T> : IReelPosition<T> where T : Enum
    {
        /// <summary>
        /// Konstruktor
        /// </summary>
        /// <param name="current">Pozycja główna na bębnie</param>
        /// <param name="next">Pozycja następna na bębnie</param>
        /// <param name="previous">Pozycja poprzednia na bębnie</param>
        public ReelPosition(T current, T next, T previous)
        {
            Current = current;
            Previous = previous;
            Next = next;
        }

        /// <inheritdoc />
        public T Current { get; }

        /// <inheritdoc />
        public T Next { get; }

        /// <inheritdoc />
        public T Previous { get; }
    }
}