using FluentAssertions;
using Game.Logic.Reels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace Game.UnitTests.Logic.Reels
{
    /// <summary>
    /// Testy klasy będącej implementacją bębna z zadaną ilością  obrotów
    /// </summary>
    [TestClass]
    public class TestReelFixRotations
    {
        /// <summary>
        /// Konstruktor waliduje wejście
        /// </summary>
        [TestMethod]
        public void ReelFixRotations_ValidateInput()
        {
            this.Invoking(t => new ReelFixRotations<DayOfWeek>(null, DayOfWeek.Friday, 3))
                .Should().Throw<ArgumentException>().And.ParamName.Should().ContainEquivalentOf("elements",  "Null jako lista elementów powinno dać wyjątek");

            this.Invoking(t => new ReelFixRotations<DayOfWeek>(new DayOfWeek[] { DayOfWeek.Friday }, DayOfWeek.Friday, 3))
                .Should().Throw<ArgumentException>().And.ParamName.Should().ContainEquivalentOf("elements", "Jedno elementowa lista elementów powinno spowodować rzucenie wyjątku.");

            this.Invoking(t => new ReelFixRotations<DayOfWeek>(new DayOfWeek[] { DayOfWeek.Friday, DayOfWeek.Saturday, DayOfWeek.Sunday }, DayOfWeek.Monday, 3))
                .Should().Throw<ArgumentException>().And.ParamName.Should().ContainEquivalentOf("startElement", "Na liście nie ma wymaganego elementu startowego - wyjątek.");
        }

        /// <summary>
        /// <see cref="ReelFixRotations{T}.GetCurrentPossition"/> działa zgodnie z założeniem
        /// </summary>
        [TestMethod]
        public void ReelFixRotations_GetCurrentPossition_WorksAsExpected()
        {
            DayOfWeek[] days = new DayOfWeek[] { DayOfWeek.Monday, DayOfWeek.Thursday, DayOfWeek.Wednesday, DayOfWeek.Monday, DayOfWeek.Sunday };
            ReelFixRotations<DayOfWeek> test = new ReelFixRotations<DayOfWeek>(days, DayOfWeek.Monday, 3);

            IReelPosition<DayOfWeek> position = test.GetCurrentPossition();

            //Powinien zostać wybrany najdalszy wskazany symbol, startujemy od 0, więc powinien być drugi poniedziałek na liście
            position.Current.Should().Be(DayOfWeek.Monday);
            position.Next.Should().Be(DayOfWeek.Sunday);
            position.Previous.Should().Be(DayOfWeek.Wednesday);
        }

        /// <summary>
        /// <see cref="ReelFixRotations{T}.SetNextValue(T)"/> działa zgodnie z założeniem
        /// </summary>
        [TestMethod]
        public void ReelFixRotations_SetNextValue_WorksAsExpected()
        {
            DayOfWeek[] days = new DayOfWeek[] { DayOfWeek.Monday, DayOfWeek.Thursday, DayOfWeek.Wednesday, DayOfWeek.Monday, DayOfWeek.Sunday };
            ReelFixRotations<DayOfWeek> test = new ReelFixRotations<DayOfWeek>(days, DayOfWeek.Monday, 2);

            //Początkowo maszyna powinna się zatrzymać na drugim poniedziałku, dlatego jak jeszcze raz będziemy chcieli go ustawić, to powinien być to "pierwszy" poniedziałek.
            IEnumerable<DayOfWeek> spinHistory = test.SetNextValue(DayOfWeek.Monday);

            //Sprawdzamy najpierw pozycję, czy w ogóle  jest dobra
            IReelPosition<DayOfWeek> position = test.GetCurrentPossition();
            position.Current.Should().Be(DayOfWeek.Monday);
            position.Next.Should().Be(DayOfWeek.Thursday);
            position.Previous.Should().Be(DayOfWeek.Sunday);

            DayOfWeek[] properSpinHistory = new DayOfWeek[] {
                //Pierwszy pełen obrót
                DayOfWeek.Sunday, DayOfWeek.Monday, DayOfWeek.Thursday, DayOfWeek.Wednesday, DayOfWeek.Monday,
                //Drugi pełen obrót
                DayOfWeek.Sunday, DayOfWeek.Monday, DayOfWeek.Thursday, DayOfWeek.Wednesday, DayOfWeek.Monday,
                //Normalne dojście do zadanego elementu
                DayOfWeek.Sunday, DayOfWeek.Monday
            };

            spinHistory.Should().BeEquivalentTo(properSpinHistory, options => options.WithStrictOrdering(), "Kolekcje powinny być takie same.");

            this.Invoking(t => test.SetNextValue(DayOfWeek.Friday)).Should().Throw<InvalidOperationException>().And.Message.Should().ContainEquivalentOf($"does not contains");
        }
    }
}