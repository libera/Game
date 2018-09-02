using Game.Logic.Machines;
using Game.Logic.Reels;
using Game.WPFUtilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Game
{
    /// <summary>
    /// ViewModel dla głównego okna
    /// </summary>
    public class GameViewModel : INotifyPropertyChanged
    {
        private bool m_bShowWiningLine = false;
        private ImageSource m_imgReel1CurrentImagePath;
        private ImageSource m_imgReel1NextImagePath;
        private ImageSource m_imgReel1PreviousImagePath;
        private ImageSource m_imgReel2CurrentImagePath;
        private ImageSource m_imgReel2NextImagePath;
        private ImageSource m_imgReel2PreviousImagePath;
        private ImageSource m_imgReel3CurrentImagePath;
        private ImageSource m_imgReel3NextImagePath;
        private ImageSource m_imgReel3PreviousImagePath;
        private IMachine<EnumSymbols> m_machine;
        private Dictionary<EnumSymbols, ImageSource> m_symbolsImageSources = new Dictionary<EnumSymbols, ImageSource>();
        private uint m_uCredits;

        /// <summary>
        /// Konstruktor
        /// </summary>
        /// <param name="machine">Konkretna implementacja maszyny</param>
        public GameViewModel(IMachine<EnumSymbols> machine)
        {
            m_machine = machine ?? throw new ArgumentNullException(nameof(machine));
            SpinCommand = new AsyncCommandHandler(Spin);
            DecreasBidCommand = new CommandHandler(() => ChangeBid(-1));
            IncreasBidCommand = new CommandHandler(() => ChangeBid(1));
            AddCreditsCommand = new CommandHandler(() => AddCredits(10));

            Credits = m_machine.Credits;

            m_symbolsImageSources.Add(EnumSymbols.Circle, new BitmapImage(new Uri(@"Imagines\Circle.png", UriKind.RelativeOrAbsolute)));
            m_symbolsImageSources.Add(EnumSymbols.Triangle, new BitmapImage(new Uri(@"Imagines\Triangle.png", UriKind.RelativeOrAbsolute)));
            m_symbolsImageSources.Add(EnumSymbols.Square, new BitmapImage(new Uri(@"Imagines\Square.png", UriKind.RelativeOrAbsolute)));
            m_symbolsImageSources[EnumSymbols.Circle].Freeze();
            m_symbolsImageSources[EnumSymbols.Triangle].Freeze();
            m_symbolsImageSources[EnumSymbols.Square].Freeze();

            SetCurrentPossitionsOnReels();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void AddCredits(uint nCredits)
        {
            try
            {
                m_machine.AddCredits(nCredits);
            }
            catch
            {
                MessageBox.Show("Wystąpił niedpodziewany błąd, spróbuj ponownie.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                //Kredyty aktyalizujemy zawsze, bo może był błąd, i coś  się nie dodało
                Credits = m_machine.Credits;
            }
        }

        private void ChangeBid(int nChange)
        {
            long nNewBid = Bid + nChange;

            if (nNewBid < 1 || nNewBid > 2)
            {
                MessageBox.Show("Stawka może być równa tylko 1 lub 2.", "Uwaga", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                m_machine.ChangeBid((uint)nNewBid);
            }
            catch
            {
                MessageBox.Show("Wystąpił niedpodziewany błąd, spróbuj ponownie.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                OnPropertyChanged(nameof(Bid));
            }
        }

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void SetCurrentPossitionsOnReels()
        {
            IEnumerable<IReelPosition<EnumSymbols>> machineState = m_machine.GetCurrentPossition();
            Reel1CurrentImagePath = m_symbolsImageSources[machineState.ElementAt(0).Current];
            Reel1PreviousImagePath = m_symbolsImageSources[machineState.ElementAt(0).Previous];
            Reel1NextImagePath = m_symbolsImageSources[machineState.ElementAt(0).Next];

            Reel2CurrentImagePath = m_symbolsImageSources[machineState.ElementAt(1).Current];
            Reel2PreviousImagePath = m_symbolsImageSources[machineState.ElementAt(1).Previous];
            Reel2NextImagePath = m_symbolsImageSources[machineState.ElementAt(1).Next];

            Reel3CurrentImagePath = m_symbolsImageSources[machineState.ElementAt(2).Current];
            Reel3PreviousImagePath = m_symbolsImageSources[machineState.ElementAt(2).Previous];
            Reel3NextImagePath = m_symbolsImageSources[machineState.ElementAt(2).Next];
        }
        #region ViewModelBindingsProperty

        /// <summary>
        /// Komenda to dodania kredytów
        /// </summary>
        public ICommand AddCreditsCommand { get; }

        /// <summary>
        /// Stawka zakładu
        /// </summary>
        public uint Bid { get { return m_machine.Bid; } }

        /// <summary>
        /// Ilość punktów\kredytów
        /// </summary>
        public uint Credits
        {
            get { return m_uCredits; }

            set
            {
                m_uCredits = value;
                OnPropertyChanged(nameof(Credits));
            }
        }

        /// <summary>
        /// Komenda zmniejszenia stawki
        /// </summary>
        public ICommand DecreasBidCommand { get; }

        /// <summary>
        /// Komenda zwiększenia stawki
        /// </summary>
        public ICommand IncreasBidCommand { get; }

        /// <summary>
        /// Źródło głównego obrazka na bębnie 1
        /// </summary>
        public ImageSource Reel1CurrentImagePath
        {
            get
            { return m_imgReel1CurrentImagePath; }
            set
            {
                m_imgReel1CurrentImagePath = value;
                OnPropertyChanged(nameof(Reel1CurrentImagePath));
            }
        }

        /// <summary>
        /// Źródło kolejnego obrazka na bębnie 1
        /// </summary>
        public ImageSource Reel1NextImagePath
        {
            get
            { return m_imgReel1NextImagePath; }
            set
            {
                m_imgReel1NextImagePath = value;
                OnPropertyChanged(nameof(Reel1NextImagePath));
            }
        }

        /// <summary>
        /// Źródło poprzedniego obrazka na bębnie 1
        /// </summary>
        public ImageSource Reel1PreviousImagePath
        {
            get
            { return m_imgReel1PreviousImagePath; }
            set
            {
                m_imgReel1PreviousImagePath = value;
                OnPropertyChanged(nameof(Reel1PreviousImagePath));
            }
        }

        /// <summary>
        /// Źródło obecnego obrazka na bębnie 2
        /// </summary>
        public ImageSource Reel2CurrentImagePath
        {
            get
            { return m_imgReel2CurrentImagePath; }
            set
            {
                m_imgReel2CurrentImagePath = value;
                OnPropertyChanged(nameof(Reel2CurrentImagePath));
            }
        }

        /// <summary>
        /// Źródło następnego obrazka na bębnie 2
        /// </summary>
        public ImageSource Reel2NextImagePath
        {
            get
            { return m_imgReel2NextImagePath; }
            set
            {
                m_imgReel2NextImagePath = value;
                OnPropertyChanged(nameof(Reel2NextImagePath));
            }
        }

        /// <summary>
        /// Źródło poprzedniego obrazka na bębnie 2
        /// </summary>
        public ImageSource Reel2PreviousImagePath
        {
            get
            { return m_imgReel2PreviousImagePath; }
            set
            {
                m_imgReel2PreviousImagePath = value;
                OnPropertyChanged(nameof(Reel2PreviousImagePath));
            }
        }

        /// <summary>
        /// Źródło głównego obrazka na bębnie 3
        /// </summary>
        public ImageSource Reel3CurrentImagePath
        {
            get
            { return m_imgReel3CurrentImagePath; }
            set
            {
                m_imgReel3CurrentImagePath = value;
                OnPropertyChanged(nameof(Reel3CurrentImagePath));
            }
        }

        /// <summary>
        /// Źródło następnego obrazka na bębnie 3
        /// </summary>
        public ImageSource Reel3NextImagePath
        {
            get
            { return m_imgReel3NextImagePath; }
            set
            {
                m_imgReel3NextImagePath = value;
                OnPropertyChanged(nameof(Reel3NextImagePath));
            }
        }

        /// <summary>
        /// Źródło poprzedniego obrazka na bębnie 3
        /// </summary>
        public ImageSource Reel3PreviousImagePath
        {
            get
            { return m_imgReel3PreviousImagePath; }
            set
            {
                m_imgReel3PreviousImagePath = value;
                OnPropertyChanged(nameof(Reel3PreviousImagePath));
            }
        }

        /// <summary>
        /// Property określające czy należy pokazać linię wygrywającą
        /// </summary>
        public bool ShowWinningLine
        {
            get { return m_bShowWiningLine; }

            set
            {
                m_bShowWiningLine = value;
                OnPropertyChanged(nameof(ShowWinningLine));
            }
        }

        /// <summary>
        /// Komenda do "zakręcenia" grą
        /// </summary>
        public ICommand SpinCommand { get; }

        /// <summary>
        /// Liczba zwycięstw
        /// </summary>
        public uint Wins { get { return m_machine.Wins; } }

        #endregion ViewModelBindingsProperty
        /// <summary>
        /// Metoda odpowiedzialna za wykonanie kolejnego "zakręcenia" w automacie
        /// </summary>
        private async Task Spin()
        {
            await Task.Yield();
            try
            {
                if (Credits < Bid)
                {
                    MessageBox.Show("Brak funduszy na dany zakład.", "Uwaga", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                Application.Current.Dispatcher.Invoke(() =>
                {
                    ShowWinningLine = false;
                    //Kredyty same "w sobie" odświeżą się dopiero po zakręceniu, więc na UI symulujemy zabranie
                    Credits -= Bid;
                }
                );

                (IEnumerable<IEnumerable<EnumSymbols>> reelsSpinHistory, uint? nWinValue) spinResult = m_machine.Spin();
                IEnumerable<IReelPosition<EnumSymbols>> currentState = m_machine.GetCurrentPossition();

                //Odpalamy wszystkie taski bez awaita, żeby leciały
                Task[] spiningTasks = new Task[]
                {
                    SpinReel1(spinResult.reelsSpinHistory.ElementAt(0),  currentState.ElementAt(0).Next),
                    SpinReel2(spinResult.reelsSpinHistory.ElementAt(1),  currentState.ElementAt(1).Next),
                    SpinReel3(spinResult.reelsSpinHistory.ElementAt(2),  currentState.ElementAt(2).Next)
                };

                //Czekamy aż wszystkie taski się skończą.
                while (!spiningTasks.All(t => t.Status == TaskStatus.RanToCompletion || t.Status == TaskStatus.Faulted))
                {
                    await Task.Delay(10);
                }

                ShowWinningLine = spinResult.nWinValue.HasValue;
                if (spinResult.nWinValue.HasValue)
                {
                    MessageBox.Show($"Wygrana w ilości: {spinResult.nWinValue}!", "Wygrana", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch
            {
                MessageBox.Show("Wystąpił niedpodziewany błąd, spróbuj ponownie.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                //W przypadku błędu ustawiamy obecne pozycje, już bez animacji, bo nie wiadomo w którym momencie coś się stało. Być może jeden z bębnów  się nie zakręcił.
                this.SetCurrentPossitionsOnReels();
            }
            finally
            {
                Credits = m_machine.Credits;
                OnPropertyChanged(nameof(Wins));
            }
        }

        private async Task<bool> SpinReel1(IEnumerable<EnumSymbols> mainSymbolsHistory, EnumSymbols nextSymbol)
        {
            //Ta gra działa tak, że potrzebujemy znać kolejny symbol, maszyna zwraca nam wartości głównych elementów na węźle
            //Tak więc na podstawie przekazanych danych tworzymy kolekcję "kolejnych" symboli

            //Pomijamy pierwszy symbol, bo jest to "następny" symbol który mamy na UI
            List<EnumSymbols> nextSymbolsHistory = mainSymbolsHistory.Skip(1).ToList();
            //Dodajemy na koniec kolejny symbol już po dojściu do końca "kręcenia"
            nextSymbolsHistory.Add(nextSymbol);

            foreach (EnumSymbols reelNextSymbols in nextSymbolsHistory)
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    Reel1PreviousImagePath = Reel1CurrentImagePath;
                    Reel1CurrentImagePath = Reel1NextImagePath;
                    Reel1NextImagePath = m_symbolsImageSources[reelNextSymbols];
                });
                await Task.Delay(100);
            }

            return true;
        }

        private async Task SpinReel2(IEnumerable<EnumSymbols> mainSymbolsHistory, EnumSymbols nextSymbol)
        {
            List<EnumSymbols> nextSymbolsHistory = mainSymbolsHistory.Skip(1).ToList();
            nextSymbolsHistory.Add(nextSymbol);

            foreach (EnumSymbols reelNextSymbols in nextSymbolsHistory)
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    Reel2PreviousImagePath = Reel2CurrentImagePath;
                    Reel2CurrentImagePath = Reel2NextImagePath;
                    Reel2NextImagePath = m_symbolsImageSources[reelNextSymbols];
                });
                await Task.Delay(150);
            }
        }

        private async Task SpinReel3(IEnumerable<EnumSymbols> mainSymbolsHistory, EnumSymbols nextSymbol)
        {
            List<EnumSymbols> nextSymbolsHistory = mainSymbolsHistory.Skip(1).ToList();
            nextSymbolsHistory.Add(nextSymbol);

            foreach (EnumSymbols reelNextSymbols in nextSymbolsHistory)
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    Reel3PreviousImagePath = Reel3CurrentImagePath;
                    Reel3CurrentImagePath = Reel3NextImagePath;
                    Reel3NextImagePath = m_symbolsImageSources[reelNextSymbols];
                });
                await Task.Delay(250);
            }
        }
    }
}