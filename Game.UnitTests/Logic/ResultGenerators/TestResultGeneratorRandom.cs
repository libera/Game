using FluentAssertions;
using Game.Logic.ResultGenerators;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Game.UnitTests.Logic.ResultGenerators
{
    /// <summary>
    /// Testy klasy <see cref="ResultGeneratorRandom{T}"/>
    /// </summary>
    [TestClass]
    public class TestResultGeneratorRandom
    {
        private enum TestEnum
        {
            Aaa = 5,

            Bbb = 9
        }

        /// <summary>
        /// Testy <see cref="ResultGeneratorRandom{T}.ResultGeneratorRandom(uint)"/>
        /// </summary>
        [TestMethod]
        public void ResultGeneratorRandom_Constructor_ValidateInput()
        {
            this.Invoking(t => new ResultGeneratorRandom<DayOfWeek>(0))
                .Should().Throw<ArgumentException>("Null powinien dać wyjątek.");

            //Informacja,  że jest jeden bęben jest poprawna
            this.Invoking(t => new ResultGeneratorRandom<DayOfWeek>(1))
                .Should().NotThrow();
        }

        /// <summary>
        /// Testy <see cref="ResultGeneratorRandom{T}.GetNextResult"/> - wyniki są losowe
        /// </summary>
        [TestMethod]
        public void ResultGeneratorRandom_GetNextResult_IsRandom()
        {
            ResultGeneratorRandom<DayOfWeek> generator = new ResultGeneratorRandom<DayOfWeek>(5);

            //Bardzo mało prawdopodobne a teoretycznie niemożliwe powinno być uzyskanie 3 kolejnych identycznych wyników
            IEnumerable<DayOfWeek> result1 = generator.GetNextResult();
            IEnumerable<DayOfWeek> result2 = generator.GetNextResult();
            IEnumerable<DayOfWeek> result3 = generator.GetNextResult();

            if (Enumerable.SequenceEqual(result1, result2))
            {
                if (Enumerable.SequenceEqual(result1, result3))
                {
                    result2.Should().NotBeEquivalentTo(result3, "3 kollejne wyniki są takie same, to mało prawdopodobne, należy się przyjrzeć.");
                }
            }

            //Również mało prawdopodobne by wszystkie wartości w wyniku były jednakowe
            result1.All(r => r == result1.ElementAt(0)).Should().BeFalse("Mało prawdopodobne by w jednym rezultacie wszystkie wyniki były takie same, należy się przyjrzeć.");

            //Również po ponownym utworzeniu generatora nie powinien on zwracać wyników w takiej samej kolejności jak przy pierwszym uruchomieniu
            generator = new ResultGeneratorRandom<DayOfWeek>(5);
            IEnumerable<DayOfWeek> newResult1 = generator.GetNextResult();

            result1.Should().NotBeEquivalentTo(newResult1, "Kolejne uruchomienie generatora dało pierwszy taki sam wynik jak poprzednio, należy się przyjrzeć.");
        }

        /// <summary>
        /// Testy <see cref="ResultGeneratorRandom{T}.GetNextResult"/> - zwraca wyniki będące w enumie
        /// </summary>
        [TestMethod]
        public void ResultGeneratorRandom_GetNextResult_ReturnDefinedValues()
        {
            ResultGeneratorRandom<TestEnum> generator = new ResultGeneratorRandom<TestEnum>(1);

            for (int i = 0; i < 1000; i++)
            {
                Enum.IsDefined(typeof(TestEnum), generator.GetNextResult().First()).Should().BeTrue("Zwrócony wynik powinien być zdefiniowany w enumie.");
            }
        }

        /// <summary>
        /// Testy <see cref="ResultGeneratorRandom{T}.GetNextResult"/> - wynik ma odpowiednią liczbę elementów
        /// </summary>
        [TestMethod]
        public void ResultGeneratorRandom_GetNextResult_ReturnsCorrestSizeResults()
        {
            ResultGeneratorRandom<DayOfWeek> generator;

            for (int nNumberOfReels = 1; nNumberOfReels < 7; nNumberOfReels++)
            {
                generator = new ResultGeneratorRandom<DayOfWeek>((uint)nNumberOfReels);

                for (int i = 0; i < 100; i++)
                {
                    generator.GetNextResult().Should().HaveCount(nNumberOfReels);
                }
            }
        }
    }
}