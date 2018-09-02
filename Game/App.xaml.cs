using Game.Logic.Machines;
using Game.Logic.Reels;
using Game.Logic.ResultGenerators;
using System.Collections.Generic;
using System.Windows;

namespace Game
{
    /// <summary>
    /// Logika interakcji dla klasy App.xaml
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// Konstruktor
        /// </summary>
        public App()
        {
            MainWindow window = new MainWindow(GenerateModel());
            window.Show();
        }

        /// <summary>
        /// Metoda zwracająca ViewModel dla głównego okna
        /// </summary>
        /// <returns></returns>
        private GameViewModel GenerateModel()
        {
            //Ustalamy 3 rolki z danymi wartościami
            List<IReel<EnumSymbols>> reels = new List<IReel<EnumSymbols>>
            {
                new ReelFixRotations<EnumSymbols>(new List<EnumSymbols> { EnumSymbols.Circle, EnumSymbols.Triangle, EnumSymbols.Square, EnumSymbols.Triangle, EnumSymbols.Circle }, EnumSymbols.Circle, 3),
                new ReelFixRotations<EnumSymbols>(new List<EnumSymbols> { EnumSymbols.Triangle, EnumSymbols.Circle, EnumSymbols.Circle, EnumSymbols.Square }, EnumSymbols.Triangle, 5),
                new ReelFixRotations<EnumSymbols>(new List<EnumSymbols> { EnumSymbols.Square, EnumSymbols.Triangle, EnumSymbols.Square, EnumSymbols.Circle }, EnumSymbols.Square, 1)
            };

            //Tworzymy listę wyników
            Dictionary<List<EnumSymbols>, uint> winingTable = new Dictionary<List<EnumSymbols>, uint>
            {
                { new List<EnumSymbols>() { EnumSymbols.Circle, EnumSymbols.Circle, EnumSymbols.Circle }, 2 },
                { new List<EnumSymbols>() { EnumSymbols.Square, EnumSymbols.Square, EnumSymbols.Square }, 3 },
                { new List<EnumSymbols>() { EnumSymbols.Triangle, EnumSymbols.Triangle, EnumSymbols.Triangle }, 1 }
            };

            //Tworzymy samą maszynę
            MachineOneLineWin<EnumSymbols> gameMachine = new MachineOneLineWin<EnumSymbols>(100, reels, new ResultGeneratorRandom<EnumSymbols>(3), winingTable);

            return new GameViewModel(gameMachine);
        }
    }
}