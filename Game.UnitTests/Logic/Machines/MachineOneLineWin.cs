using FluentAssertions;
using Game.Logic.Machines;
using Game.Logic.Reels;
using Game.Logic.ResultGenerators;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Game.UnitTests.Logic.Machines
{
    /// <summary>
    /// Testy klasy <see cref="MachineOneLineWin{T}"/>
    /// </summary>
    [TestClass]
    public class MachineOneLineWin
    {
        /// <summary>
        /// Testy <see cref="MachineOneLineWin{T}.MachineOneLineWin(uint, IEnumerable{IReel{T}}, IResultGenerator{T}, IDictionary{List{T}, uint})"/> - walidacja danych
        /// </summary>
        [TestMethod]
        public void ResultGeneratorRandom_Constructor_ValidateInput()
        {
            this.Invoking(t => new MachineOneLineWin<DayOfWeek>(0, new IReel<DayOfWeek>[] { new Mock<IReel<DayOfWeek>>().Object, new Mock<IReel<DayOfWeek>>().Object }, new Mock<IResultGenerator<DayOfWeek>>().Object, null))
                .Should().Throw<ArgumentNullException>().And.ParamName.Should().ContainEquivalentOf("winingTable");

            this.Invoking(t => new MachineOneLineWin<DayOfWeek>(0, new IReel<DayOfWeek>[] { new Mock<IReel<DayOfWeek>>().Object, new Mock<IReel<DayOfWeek>>().Object }, null, new Dictionary<List<DayOfWeek>, uint>()))
                .Should().Throw<ArgumentNullException>().And.ParamName.Should().ContainEquivalentOf("resultGenerator");

            this.Invoking(t => new MachineOneLineWin<DayOfWeek>(0, null, new Mock<IResultGenerator<DayOfWeek>>().Object, new Dictionary<List<DayOfWeek>, uint>()))
                .Should().Throw<ArgumentException>().And.Message.Should().ContainEquivalentOf("At least 1 reesl must be provided");

            this.Invoking(t => new MachineOneLineWin<DayOfWeek>(0, new IReel<DayOfWeek>[] { }, new Mock<IResultGenerator<DayOfWeek>>().Object, new Dictionary<List<DayOfWeek>, uint>()))
                .Should().Throw<ArgumentException>().And.Message.Should().ContainEquivalentOf("At least 1 reesl must be provided");
        }

        /// <summary>
        /// Testy <see cref="MachineOneLineWin{T}.MachineOneLineWin(uint, IEnumerable{IReel{T}}, IResultGenerator{T}, IDictionary{List{T}, uint})"/> - poprawne ustawienie początkowych wartości
        /// </summary>
        [TestMethod]
        public void ResultGeneratorRandom_Constructor_SetProperValues()
        {
            MachineOneLineWin<DayOfWeek> machine = new MachineOneLineWin<DayOfWeek>(110, new IReel<DayOfWeek>[] { new Mock<IReel<DayOfWeek>>().Object }, new Mock<IResultGenerator<DayOfWeek>>().Object, new Dictionary<List<DayOfWeek>, uint>());
            machine.Credits.Should().Be(110);
            machine.Bid.Should().Be(1);
        }

        /// <summary>
        /// Testy <see cref="MachineOneLineWin{T}.AddCredits(uint)"/>
        /// </summary>
        [TestMethod]
        public void ResultGeneratorRandom_AddCredits_WorksAsExpected()
        {
            MachineOneLineWin<DayOfWeek> machine = new MachineOneLineWin<DayOfWeek>(110, new IReel<DayOfWeek>[] { new Mock<IReel<DayOfWeek>>().Object }, new Mock<IResultGenerator<DayOfWeek>>().Object, new Dictionary<List<DayOfWeek>, uint>());
            machine.Credits.Should().Be(110);
            machine.AddCredits(15);
            machine.Credits.Should().Be(125);
            machine.AddCredits(7);
            machine.Credits.Should().Be(132);
        }

        /// <summary>
        /// Testy <see cref="MachineOneLineWin{T}.ChangeBid(uint)"/>
        /// </summary>
        [TestMethod]
        public void ResultGeneratorRandom_ChangeBid_WorksAsExpected()
        {
            MachineOneLineWin<DayOfWeek> machine = new MachineOneLineWin<DayOfWeek>(10, new IReel<DayOfWeek>[] { new Mock<IReel<DayOfWeek>>().Object }, new Mock<IResultGenerator<DayOfWeek>>().Object, new Dictionary<List<DayOfWeek>, uint>());

            //Akceptowalna stawka to tylko 1 i 2,  dla innych wyjątek
            this.Invoking(t => machine.ChangeBid(0)).Should().Throw<ArgumentException>().And.ParamName.Should().BeEquivalentTo("nNewBid");
            this.Invoking(t => machine.ChangeBid(3)).Should().Throw<ArgumentException>().And.ParamName.Should().BeEquivalentTo("nNewBid");
            this.Invoking(t => machine.ChangeBid(4)).Should().Throw<ArgumentException>().And.ParamName.Should().BeEquivalentTo("nNewBid");
            this.Invoking(t => machine.ChangeBid(100)).Should().Throw<ArgumentException>().And.ParamName.Should().BeEquivalentTo("nNewBid");

            machine.ChangeBid(2);
            machine.Bid.Should().Be(2);

            machine.ChangeBid(1);
            machine.Bid.Should().Be(1);

            machine.ChangeBid(2);
            machine.Bid.Should().Be(2);
        }

        /// <summary>
        /// Testy <see cref="MachineOneLineWin{T}.End"/>
        /// </summary>
        [TestMethod]
        public void ResultGeneratorRandom_End_WorksAsExpected()
        {
            MachineOneLineWin<DayOfWeek> machine = new MachineOneLineWin<DayOfWeek>(111, new IReel<DayOfWeek>[] { new Mock<IReel<DayOfWeek>>().Object }, new Mock<IResultGenerator<DayOfWeek>>().Object, new Dictionary<List<DayOfWeek>, uint>());
            machine.Credits.Should().Be(111);
            machine.End().Should().Be(111);
            machine.Credits.Should().Be(0);
        }

        /// <summary>
        /// Testy <see cref="MachineOneLineWin{T}.GetCurrentPossition"/>
        /// </summary>
        [TestMethod]
        public void ResultGeneratorRandom_GetCurrentPossition_WorksAsExpected()
        {
            Mock<IReel<DayOfWeek>> mockReel1 = new Mock<IReel<DayOfWeek>>();
            ReelPosition<DayOfWeek> reel1Position = new ReelPosition<DayOfWeek>(DayOfWeek.Monday, DayOfWeek.Sunday, DayOfWeek.Tuesday);
            mockReel1.Setup(r => r.GetCurrentPossition()).Returns(() => { return reel1Position; });
            Mock<IReel<DayOfWeek>> mockReel2 = new Mock<IReel<DayOfWeek>>();
            ReelPosition<DayOfWeek> reel2Position = new ReelPosition<DayOfWeek>(DayOfWeek.Tuesday, DayOfWeek.Thursday, DayOfWeek.Wednesday);
            mockReel2.Setup(r => r.GetCurrentPossition()).Returns(() => { return reel2Position; });

            MachineOneLineWin<DayOfWeek> machine = new MachineOneLineWin<DayOfWeek>(111, new IReel<DayOfWeek>[] { mockReel1.Object, mockReel2.Object }, new Mock<IResultGenerator<DayOfWeek>>().Object, new Dictionary<List<DayOfWeek>, uint>());

            IEnumerable<IReelPosition<DayOfWeek>> reelPositions = machine.GetCurrentPossition();
            reelPositions.Should().HaveCount(2);
            reelPositions.ElementAt(0).Should().Be(reel1Position);
            reelPositions.ElementAt(1).Should().Be(reel2Position);
        }

        /// <summary>
        /// Testy <see cref="MachineOneLineWin{T}.Spin"/> - sprawdza walidację wystarczającej liczby kredytów
        /// </summary>
        [TestMethod]
        public void ResultGeneratorRandom_Spin_ValidateCredits()
        {
            Mock<IReel<DayOfWeek>> mockReel1 = new Mock<IReel<DayOfWeek>>();
            Mock<IResultGenerator<DayOfWeek>> mockResultGenerator = new Mock<IResultGenerator<DayOfWeek>>();

            MachineOneLineWin<DayOfWeek> machine = new MachineOneLineWin<DayOfWeek>(0, new IReel<DayOfWeek>[] { mockReel1.Object }, mockResultGenerator.Object, new Dictionary<List<DayOfWeek>, uint>());
            this.Invoking(t => machine.Spin()).Should().Throw<InvalidOperationException>().And.Message.Should().ContainEquivalentOf("Insufficient Credits");
            mockReel1.Verify(m => m.SetNextValue(It.IsAny<DayOfWeek>()), Times.Never);
            mockResultGenerator.Verify(m => m.GetNextResult(), Times.Never);
        }

        /// <summary>
        /// Testy <see cref="MachineOneLineWin{T}.Spin"/> - sprawdza walidację czy wynik liczby wyniku 
        /// </summary>
        [TestMethod]
        public void ResultGeneratorRandom_Spin_ValidateResultFromResultGenerator()
        {
            Mock<IReel<DayOfWeek>> mockReel1 = new Mock<IReel<DayOfWeek>>();
            Mock<IResultGenerator<DayOfWeek>> mockResultGenerator = new Mock<IResultGenerator<DayOfWeek>>();
            mockResultGenerator.Setup(m => m.GetNextResult()).Returns(() => { return new DayOfWeek[] { DayOfWeek.Monday, DayOfWeek.Thursday }; });

            MachineOneLineWin<DayOfWeek> machine = new MachineOneLineWin<DayOfWeek>(10, new IReel<DayOfWeek>[] { mockReel1.Object }, mockResultGenerator.Object, new Dictionary<List<DayOfWeek>, uint>());
            this.Invoking(t => machine.Spin()).Should().Throw<InvalidOperationException>().And.Message.Should().ContainEquivalentOf("didn't return number values exactly");
            mockReel1.Verify(m => m.SetNextValue(It.IsAny<DayOfWeek>()), Times.Never);
            mockResultGenerator.Verify(m => m.GetNextResult(), Times.Once);
            machine.Credits.Should().Be(10, "Błąd nie powinien spowodować pobrania kredytów.");

            mockResultGenerator.Setup(m => m.GetNextResult()).Returns(() => { return new DayOfWeek[] { }; });
            this.Invoking(t => machine.Spin()).Should().Throw<InvalidOperationException>().And.Message.Should().ContainEquivalentOf("didn't return number values exactly");
            mockReel1.Verify(m => m.SetNextValue(It.IsAny<DayOfWeek>()), Times.Never);
            mockResultGenerator.Verify(m => m.GetNextResult(), Times.Exactly(2));
            machine.Credits.Should().Be(10, "Błąd nie powinien spowodować pobrania kredytów.");

            mockResultGenerator.Setup(m => m.GetNextResult()).Returns(() => { return null; });
            this.Invoking(t => machine.Spin()).Should().Throw<InvalidOperationException>().And.Message.Should().ContainEquivalentOf("didn't return number values exactly");
            mockReel1.Verify(m => m.SetNextValue(It.IsAny<DayOfWeek>()), Times.Never);
            mockResultGenerator.Verify(m => m.GetNextResult(), Times.Exactly(3));
            machine.Credits.Should().Be(10, "Błąd nie powinien spowodować pobrania kredytów.");
        }

        /// <summary>
        /// Testy <see cref="MachineOneLineWin{T}.Spin"/> - sprawdza walidację, czy wynik jest możliwy do osiągnięcia
        /// </summary>
        [TestMethod]
        public void ResultGeneratorRandom_Spin_ValidateIfResultIsPossible()
        {
            Mock<IReel<DayOfWeek>> mockReel1 = new Mock<IReel<DayOfWeek>>();
            Mock<IResultGenerator<DayOfWeek>> mockResultGenerator = new Mock<IResultGenerator<DayOfWeek>>();
            mockResultGenerator.Setup(m => m.GetNextResult()).Returns(() => { return new DayOfWeek[] { DayOfWeek.Monday }; });

            MachineOneLineWin<DayOfWeek> machine = new MachineOneLineWin<DayOfWeek>(10, new IReel<DayOfWeek>[] { mockReel1.Object }, mockResultGenerator.Object, new Dictionary<List<DayOfWeek>, uint>());
            this.Invoking(t => machine.Spin()).Should().Throw<InvalidOperationException>().And.Message.Should().ContainEquivalentOf("Returned result is nott possible to set");
            mockReel1.Verify(m => m.SetNextValue(It.IsAny<DayOfWeek>()), Times.Never);
            mockResultGenerator.Verify(m => m.GetNextResult(), Times.Once);
            machine.Credits.Should().Be(10, "Błąd nie powinien spowodować pobrania kredytów.");
        }

        /// <summary>
        /// Testy <see cref="MachineOneLineWin{T}.Spin"/> - sprawdza czy przy otrzymaniu wyniku nastąpiło ustawienie wartości na każdym z bębnów
        /// </summary>
        [TestMethod]
        public void ResultGeneratorRandom_Spin_SetValuesForEachReel()
        {
            Mock<IReel<DayOfWeek>> mockReel1 = new Mock<IReel<DayOfWeek>>();
            mockReel1.Setup(m => m.CheckIfExist(It.IsAny<DayOfWeek>())).Returns(true);
            mockReel1.Setup(m => m.SetNextValue(It.IsAny<DayOfWeek>())).Returns(() => { return new DayOfWeek[] { DayOfWeek.Monday}; });

            Mock<IReel<DayOfWeek>> mockReel2 = new Mock<IReel<DayOfWeek>>();
            mockReel2.Setup(m => m.CheckIfExist(It.IsAny<DayOfWeek>())).Returns(true);
            mockReel2.Setup(m => m.SetNextValue(It.IsAny<DayOfWeek>())).Returns(() => { return new DayOfWeek[] { DayOfWeek.Wednesday }; });

            Mock<IResultGenerator<DayOfWeek>> mockResultGenerator = new Mock<IResultGenerator<DayOfWeek>>();
            mockResultGenerator.Setup(m => m.GetNextResult()).Returns(() => { return new DayOfWeek[] { DayOfWeek.Monday, DayOfWeek.Thursday }; });

            MachineOneLineWin<DayOfWeek> machine = new MachineOneLineWin<DayOfWeek>(10, new IReel<DayOfWeek>[] { mockReel1.Object, mockReel2.Object }, mockResultGenerator.Object, new Dictionary<List<DayOfWeek>, uint>());
            machine.ChangeBid(2);
            machine.Spin();
            mockReel1.Verify(m => m.SetNextValue(DayOfWeek.Monday), Times.Once);
            mockReel2.Verify(m => m.SetNextValue(DayOfWeek.Thursday), Times.Once);
            mockResultGenerator.Verify(m => m.GetNextResult(), Times.Once);
            machine.Credits.Should().Be(8, "Powinno pobrać kredyty");
        }

        /// <summary>
        /// Testy <see cref="MachineOneLineWin{T}.Spin"/> - sprawdza zwracane są wyniki dla każdego  z obrotów bębnów
        /// </summary>
        [TestMethod]
        public void ResultGeneratorRandom_Spin_ReturnProperSpinResult()
        {
            Mock<IReel<DayOfWeek>> mockReel1 = new Mock<IReel<DayOfWeek>>();
            mockReel1.Setup(m => m.CheckIfExist(It.IsAny<DayOfWeek>())).Returns(true);
            DayOfWeek[] resultReel1 = new DayOfWeek[] { DayOfWeek.Monday, DayOfWeek.Sunday, DayOfWeek.Tuesday };
            mockReel1.Setup(m => m.SetNextValue(It.IsAny<DayOfWeek>())).Returns(() => { return resultReel1; });

            Mock<IReel<DayOfWeek>> mockReel2 = new Mock<IReel<DayOfWeek>>();
            mockReel2.Setup(m => m.CheckIfExist(It.IsAny<DayOfWeek>())).Returns(true);
            DayOfWeek[] resultReel2 = new DayOfWeek[] { DayOfWeek.Wednesday, DayOfWeek.Saturday, DayOfWeek.Monday };
            mockReel2.Setup(m => m.SetNextValue(It.IsAny<DayOfWeek>())).Returns(() => { return resultReel2; });

            Mock<IResultGenerator<DayOfWeek>> mockResultGenerator = new Mock<IResultGenerator<DayOfWeek>>();
            mockResultGenerator.Setup(m => m.GetNextResult()).Returns(() => { return new DayOfWeek[] { DayOfWeek.Monday, DayOfWeek.Thursday }; });

            MachineOneLineWin<DayOfWeek> machine = new MachineOneLineWin<DayOfWeek>(10, new IReel<DayOfWeek>[] { mockReel1.Object, mockReel2.Object }, mockResultGenerator.Object, new Dictionary<List<DayOfWeek>, uint>() { { new List<DayOfWeek>() { DayOfWeek.Friday, DayOfWeek.Friday }, 23 } });
            machine.ChangeBid(2);
            (IEnumerable<IEnumerable<DayOfWeek>> reelsSpinHistory, uint? nWinValue) spinResult = machine.Spin();
            mockReel1.Verify(m => m.SetNextValue(DayOfWeek.Monday), Times.Once);
            mockReel2.Verify(m => m.SetNextValue(DayOfWeek.Thursday), Times.Once);
            mockResultGenerator.Verify(m => m.GetNextResult(), Times.Once);
            machine.Credits.Should().Be(8, "Powinno pobrać kredyty");

            spinResult.reelsSpinHistory.Count().Should().Be(2);

            spinResult.reelsSpinHistory.ElementAt(0).Should().BeEquivalentTo(resultReel1, option => option.WithStrictOrdering());
            spinResult.reelsSpinHistory.ElementAt(1).Should().BeEquivalentTo(resultReel2, option => option.WithStrictOrdering());

            spinResult.nWinValue.Should().BeNull();
            machine.Wins.Should().Be(0);
        }

        /// <summary>
        /// Testy <see cref="MachineOneLineWin{T}.Spin"/> - poprawnie się zachowuje dla zwycieństwa
        /// </summary>
        [TestMethod]
        public void ResultGeneratorRandom_Spin_ActProperForWin()
        {
            Mock<IReel<DayOfWeek>> mockReel1 = new Mock<IReel<DayOfWeek>>();
            mockReel1.Setup(m => m.CheckIfExist(It.IsAny<DayOfWeek>())).Returns(true);
            DayOfWeek[] resultReel1 = new DayOfWeek[] { DayOfWeek.Monday, DayOfWeek.Sunday, DayOfWeek.Monday };
            mockReel1.Setup(m => m.SetNextValue(It.IsAny<DayOfWeek>())).Returns(() => { return resultReel1; });

            Mock<IReel<DayOfWeek>> mockReel2 = new Mock<IReel<DayOfWeek>>();
            mockReel2.Setup(m => m.CheckIfExist(It.IsAny<DayOfWeek>())).Returns(true);
            DayOfWeek[] resultReel2 = new DayOfWeek[] { DayOfWeek.Wednesday, DayOfWeek.Saturday, DayOfWeek.Thursday };
            mockReel2.Setup(m => m.SetNextValue(It.IsAny<DayOfWeek>())).Returns(() => { return resultReel2; });

            Mock<IResultGenerator<DayOfWeek>> mockResultGenerator = new Mock<IResultGenerator<DayOfWeek>>();
            mockResultGenerator.Setup(m => m.GetNextResult()).Returns(() => { return new DayOfWeek[] { DayOfWeek.Monday, DayOfWeek.Thursday }; });

            MachineOneLineWin<DayOfWeek> machine = new MachineOneLineWin<DayOfWeek>(10, new IReel<DayOfWeek>[] { mockReel1.Object, mockReel2.Object }, mockResultGenerator.Object, new Dictionary<List<DayOfWeek>, uint>() { { new List<DayOfWeek>() { DayOfWeek.Monday, DayOfWeek.Thursday }, 23 } });
            machine.ChangeBid(2);
            (IEnumerable<IEnumerable<DayOfWeek>> reelsSpinHistory, uint? nWinValue) spinResult = machine.Spin();
            mockReel1.Verify(m => m.SetNextValue(DayOfWeek.Monday), Times.Once);
            mockReel2.Verify(m => m.SetNextValue(DayOfWeek.Thursday), Times.Once);
            mockResultGenerator.Verify(m => m.GetNextResult(), Times.Once);
            machine.Credits.Should().Be(10-2+23*2, "Powinno pobrać kredyty i dać wygraną");

            spinResult.reelsSpinHistory.Count().Should().Be(2);

            spinResult.reelsSpinHistory.ElementAt(0).Should().BeEquivalentTo(resultReel1, option => option.WithStrictOrdering());
            spinResult.reelsSpinHistory.ElementAt(1).Should().BeEquivalentTo(resultReel2, option => option.WithStrictOrdering());

            spinResult.nWinValue.Should().Be(46);
            machine.Wins.Should().Be(1);
        }
    }
}