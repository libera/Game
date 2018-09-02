using Game.Logic.Machines;
using Game.Logic.Reels;
using Game.Logic.ResultGenerators;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

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
        private GameViewModel<EnumSymbols> GenerateModel()
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

            //Słownik wartości enuma na źródła obrazków
            Dictionary<EnumSymbols, ImageSource> symbolsImageSources = new Dictionary<EnumSymbols, ImageSource>()
            {
                { EnumSymbols.Circle, new BitmapImage(new Uri(@"Imagines\Circle.png", UriKind.RelativeOrAbsolute))},
                { EnumSymbols.Triangle, new BitmapImage(new Uri(@"Imagines\Triangle.png", UriKind.RelativeOrAbsolute))},
                { EnumSymbols.Square, new BitmapImage(new Uri(@"Imagines\Square.png", UriKind.RelativeOrAbsolute))}
            };
            symbolsImageSources[EnumSymbols.Circle].Freeze();
            symbolsImageSources[EnumSymbols.Triangle].Freeze();
            symbolsImageSources[EnumSymbols.Square].Freeze();

            //Tworzymy samą maszynę
            MachineOneLineWin<EnumSymbols> gameMachine = new MachineOneLineWin<EnumSymbols>(100, reels, new ResultGeneratorRandom<EnumSymbols>(3), winingTable);

            return new GameViewModel<EnumSymbols>(gameMachine, symbolsImageSources, new InformationPresenter());
        }

        /// <summary>
        /// Enum symboli występujących na bębnach
        /// </summary>
        private enum EnumSymbols
        {
            /// <summary>
            /// Koło
            /// </summary>
            Circle = 0,

            /// <summary>
            /// Trójkąt
            /// </summary>
            Triangle = 1,

            /// <summary>
            /// Kwadrat
            /// </summary>
            Square = 2
        }
    }
}