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

namespace Game
{
    /// <summary>
    /// ViewModel dla głównego okna
    /// </summary>
    public class GameViewModel<T> : INotifyPropertyChanged, IGameViewModel where T : Enum
    {
        private readonly IDictionary<T, ImageSource> m_symbolsImageSources;
        private readonly IInformationPresenter m_informationPresenter;
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
        private IMachine<T> m_machine;
        private uint m_uCredits;

        #region ViewModelBindingsProperty

        ///  <inheritdoc />
        public ICommand AddCreditsCommand { get; }

        ///  <inheritdoc />
        public uint Bid { get { return m_machine.Bid; } }

        ///  <inheritdoc />
        public uint Credits
        {
            get { return m_uCredits; }

            private set
            {
                m_uCredits = value;
                OnPropertyChanged(nameof(Credits));
            }
        }

        ///  <inheritdoc />
        public ICommand DecreasBidCommand { get; }

        ///  <inheritdoc />
        public ICommand IncreasBidCommand { get; }

        /// <inheritdoc />
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

        /// <inheritdoc />
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

        /// <inheritdoc />
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

        /// <inheritdoc />
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

        /// <inheritdoc />
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

        /// <inheritdoc />
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

        /// <inheritdoc />
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

        /// <inheritdoc />
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

        /// <inheritdoc />
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

        /// <inheritdoc />
        public bool ShowWinningLine
        {
            get { return m_bShowWiningLine; }

            set
            {
                m_bShowWiningLine = value;
                OnPropertyChanged(nameof(ShowWinningLine));
            }
        }

        /// <inheritdoc />
        public ICommand SpinCommand { get; }

        /// <inheritdoc />
        public uint Wins { get { return m_machine.Wins; } }

        #endregion ViewModelBindingsProperty

        /// <summary>
        /// Konstruktor
        /// </summary>
        /// <param name="machine">Konkretna implementacja maszyny</param>
        public GameViewModel(IMachine<T> machine, IDictionary<T, ImageSource> symbolsImageSources, IInformationPresenter informationPresenter)
        {
            m_machine = machine ?? throw new ArgumentNullException(nameof(machine));
            m_symbolsImageSources = symbolsImageSources ?? throw new ArgumentNullException(nameof(symbolsImageSources));
            m_informationPresenter = informationPresenter ?? throw new ArgumentNullException(nameof(informationPresenter));

            SpinCommand = new AsyncCommandHandler(Spin);
            DecreasBidCommand = new CommandHandler(() => ChangeBid(-1));
            IncreasBidCommand = new CommandHandler(() => ChangeBid(1));
            AddCreditsCommand = new CommandHandler(() => AddCredits(10));

            Credits = m_machine.Credits;

            SetCurrentPossitionsOnReels();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void AddCredits(uint nCredits)
        {
            try
            {
                m_machine.AddCredits(nCredits);
            }
            catch(Exception ex)
            {
                m_informationPresenter.ShowError("Wystąpił niedpodziewany błąd, spróbuj ponownie.", ex);
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
                m_informationPresenter.ShowWarning("Stawka może być równa tylko 1 lub 2.");
                return;
            }

            try
            {
                m_machine.ChangeBid((uint)nNewBid);
            }
            catch(Exception ex)
            {
                m_informationPresenter.ShowError("Wystąpił niedpodziewany błąd, spróbuj ponownie.", ex);
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
            IEnumerable<IReelPosition<T>> machineState = m_machine.GetCurrentPossition();
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
                    m_informationPresenter.ShowWarning("Brak funduszy na dany zakład.");
                    return;
                }

                Application.Current.Dispatcher.Invoke(() =>
                {
                    ShowWinningLine = false;
                    //Kredyty same "w sobie" odświeżą się dopiero po zakręceniu, więc na UI symulujemy zabranie
                    Credits -= Bid;
                }
                );

                (IEnumerable<IEnumerable<T>> reelsSpinHistory, uint? nWinValue) spinResult = m_machine.Spin();
                IEnumerable<IReelPosition<T>> currentState = m_machine.GetCurrentPossition();

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
                    m_informationPresenter.ShowSuccess($"Wygrana w ilości: {spinResult.nWinValue}!");
                }
            }
            catch (Exception ex)
            {
                m_informationPresenter.ShowError("Wystąpił niedpodziewany błąd, spróbuj ponownie.", ex);
                //W przypadku błędu ustawiamy obecne pozycje, już bez animacji, bo nie wiadomo w którym momencie coś się stało. Być może jeden z bębnów  się nie zakręcił.
                SetCurrentPossitionsOnReels();
            }
            finally
            {
                Credits = m_machine.Credits;
                OnPropertyChanged(nameof(Wins));
            }
        }

        private async Task<bool> SpinReel1(IEnumerable<T> mainSymbolsHistory, T nextSymbol)
        {
            //Ta gra działa tak, że potrzebujemy znać kolejny symbol, maszyna zwraca nam wartości głównych elementów na węźle
            //Tak więc na podstawie przekazanych danych tworzymy kolekcję "kolejnych" symboli

            //Pomijamy pierwszy symbol, bo jest to "następny" symbol który mamy na UI
            List<T> nextSymbolsHistory = mainSymbolsHistory.Skip(1).ToList();
            //Dodajemy na koniec kolejny symbol już po dojściu do końca "kręcenia"
            nextSymbolsHistory.Add(nextSymbol);

            foreach (T reelNextSymbols in nextSymbolsHistory)
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

        private async Task SpinReel2(IEnumerable<T> mainSymbolsHistory, T nextSymbol)
        {
            List<T> nextSymbolsHistory = mainSymbolsHistory.Skip(1).ToList();
            nextSymbolsHistory.Add(nextSymbol);

            foreach (T reelNextSymbols in nextSymbolsHistory)
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

        private async Task SpinReel3(IEnumerable<T> mainSymbolsHistory, T nextSymbol)
        {
            List<T> nextSymbolsHistory = mainSymbolsHistory.Skip(1).ToList();
            nextSymbolsHistory.Add(nextSymbol);

            foreach (T reelNextSymbols in nextSymbolsHistory)
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