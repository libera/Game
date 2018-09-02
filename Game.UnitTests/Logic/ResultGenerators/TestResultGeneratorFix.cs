using FluentAssertions;
using Game.Logic.ResultGenerators;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace Game.UnitTests.Logic.ResultGenerators
{
    /// <summary>
    /// Testy klasy <see cref="ResultGeneratorFix{T}"/>
    /// </summary>
    [TestClass]
    public class TestResultGeneratorFix
    {
        /// <summary>
        /// Testy <see cref="ResultGeneratorFix{T}.ResultGeneratorFix(IEnumerable{IEnumerable{T}})"/>
        /// </summary>
        [TestMethod]
        public void ResultGeneratorFix_Constructor_ValidateInput()
        {
            this.Invoking(t => new ResultGeneratorFix<DayOfWeek>(null))
                 .Should().Throw<ArgumentException>("Null powinien dać wyjątek.");

            this.Invoking(t => new ResultGeneratorFix<DayOfWeek>(new DayOfWeek[][] { }))
                .Should().Throw<ArgumentException>("Pusta kolekcja wyników powinna dać wyjątek.");
        }

        /// <summary>
        /// Testy <see cref="ResultGeneratorFix{T}.GetNextResult"/> wraz z <see cref="ResultGeneratorFix{T}.Reset"/>
        /// </summary>
        [TestMethod]
        public void ResultGeneratorFix_GetNextResult_And_Reset_WorksAsExpected()
        {
            List<List<DayOfWeek>> fixedResults = new List<List<DayOfWeek>>
            {
                new List<DayOfWeek> {DayOfWeek.Friday, DayOfWeek.Saturday },
                new List<DayOfWeek> {DayOfWeek.Thursday, DayOfWeek.Monday },
                new List<DayOfWeek> {DayOfWeek.Wednesday, DayOfWeek.Saturday },
                //Generator zwracający ustalone wyniki nie patrzy na wyniki jakie ma zwracać, więc jak wszystkie wyniki są 2 elementowe a jeden 3, to tak powinien zwrócić
                new List<DayOfWeek> {DayOfWeek.Tuesday, DayOfWeek.Tuesday, DayOfWeek.Tuesday },
            };

            ResultGeneratorFix<DayOfWeek> fixResultGenerator = new ResultGeneratorFix<DayOfWeek>(fixedResults);
            fixResultGenerator.GetNextResult().Should().BeEquivalentTo(new[] { DayOfWeek.Friday, DayOfWeek.Saturday }, option => option.WithStrictOrdering());
            fixResultGenerator.GetNextResult().Should().BeEquivalentTo(new[] { DayOfWeek.Thursday, DayOfWeek.Monday }, option => option.WithStrictOrdering());
            fixResultGenerator.GetNextResult().Should().BeEquivalentTo(new[] { DayOfWeek.Wednesday, DayOfWeek.Saturday }, option => option.WithStrictOrdering());
            fixResultGenerator.GetNextResult().Should().BeEquivalentTo(new[] { DayOfWeek.Tuesday, DayOfWeek.Tuesday, DayOfWeek.Tuesday }, option => option.WithStrictOrdering());
            //Powinno być zapętlenie
            fixResultGenerator.GetNextResult().Should().BeEquivalentTo(new[] { DayOfWeek.Friday, DayOfWeek.Saturday }, option => option.WithStrictOrdering());
            fixResultGenerator.GetNextResult().Should().BeEquivalentTo(new[] { DayOfWeek.Thursday, DayOfWeek.Monday }, option => option.WithStrictOrdering());

            //Po resecie generator powinien zwracać wyniki od początku
            fixResultGenerator.Reset();
            fixResultGenerator.GetNextResult().Should().BeEquivalentTo(new[] { DayOfWeek.Friday, DayOfWeek.Saturday }, option => option.WithStrictOrdering());
            fixResultGenerator.GetNextResult().Should().BeEquivalentTo(new[] { DayOfWeek.Thursday, DayOfWeek.Monday }, option => option.WithStrictOrdering());
        }
    }
}