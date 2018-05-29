using Kaleidoscope.Helpers;
using Kaleidoscope.HelpersViewModel;
using Kaleidoscope.Localization;
using Kaleidoscope.View;
using LocalizatorHelper;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace Kaleidoscope.ViewModel {
  public class MainWindowViewModel : ViewModelBase {
    public MainWindowViewModel() {
      try {
        // Позиционирование
        InitLocation();
        // Настройка событий
        _window.Activated += (o, e) => PositionSave(o, e);
        _window.Closing += (o, e) => Closing(_window, e);
        _window.ContentRendered += (o, e) => SetUpAppSettings(_window, e);
        // Apply localization
        ResourceManagerService.ChangeLocale(Settings.Same().Localization);
        // Инициализация списков культур
        _namesOfCultures = InitNamesOfCultures();
        ListOfLocalization = GetListOfLocalization();
        SelectedIndexLocalization = _namesOfCultures.IndexOf(Settings.Same().Localization);
        // Временно скрываем верхнюю линейку управления, что бы не мигало при инициализации цветово палитры 
        ColorZoneVisibility = Visibility.Hidden;
        // Инициализация главного меню
        MainMenuItemsInit();
        // Random ColorSet
        PaletteThemeViewModel.ThemeChanged += () => ColorSet.Create().RedefineColors();
        MainWindowViewModel.LocalizationChangedEvent += () => ColorSet.Create().RedefineColors();
        SettingOptionsViewModel.ApplyPrimaryChanged += () => ColorSet.Create().RedefineColors();
        SettingOptionsViewModel.ApplyAccentChanged += () => ColorSet.Create().RedefineColors();
        _dispatcherTimerShowBusyMemory = new DispatcherTimer(TimeSpan.FromMilliseconds(1000),
                                                             DispatcherPriority.Normal,
                                                             new EventHandler(SetBusyMemoryCaption),
                                                             Dispatcher.CurrentDispatcher);
        _dispatcherTimerShowBusyMemory.Stop();
        _dispatcherTimerShowBusyMemory.Start();
      } catch (Exception e) {
        Settings.Same().AppStatus = TaskStatus.Faulted;
        ErrorProcessing.Show(e);
        (Application.Current as App).Shutdown();
      }
    }

    #region Fields
    private DispatcherTimer _dispatcherTimerShowBusyMemory;
    private LocalisationHelper _localisationHelper = new LocalisationHelper();
    private MainWindow _window = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();
    private ObservableCollection<MainMenuItem> _mainMenuItems;
    private WindowStartupLocation _startupLocation = Properties.Settings.Default.MainWindowStartupLocation;
    private WindowState _state = Properties.Settings.Default.MainWindowState;
    private double _top = Properties.Settings.Default.MainWindowTop;
    private double _left = Properties.Settings.Default.MainWindowLeft;
    private double _height = Properties.Settings.Default.MainWindowHeight;
    private double _width = Properties.Settings.Default.MainWindowWidth;
    private Visibility _colorZoneVisibility;
    private IList<string> _listOfLocalization;
    private int _selectedIndexLocalization;
    private bool _switchSelectedIndexLocalization = false; // для недопущения рекурсии
    private bool _isDropDownOpenLocalization;
    private IList<string> _namesOfCultures;
    private string _busyMemoryCaption = string.Empty;
    #endregion

    #region Properties
    public string ProcessName => Process.GetCurrentProcess().ProcessName;
    public DialogSession SessionMainDialogHost { get; private set; }
    public ObservableCollection<MainMenuItem> MainMenuItems { get { return _mainMenuItems; } }
    public WindowStartupLocation StartupLocation {
      get { return _startupLocation; }
      set { this.MutateVerbose(ref _startupLocation, value, RaisePropertyChanged()); }
    }
    public WindowState State {
      get { return _state; }
      set { this.MutateVerbose(ref _state, value, RaisePropertyChanged()); }
    }
    public double Top {
      get { return _top; }
      set { this.MutateVerbose(ref _top, value, RaisePropertyChanged()); }
    }
    public double Left {
      get { return _left; }
      set { this.MutateVerbose(ref _left, value, RaisePropertyChanged()); }
    }
    public double Height {
      get { return _height; }
      set { this.MutateVerbose(ref _height, value, RaisePropertyChanged()); }
    }
    public double Width {
      get { return _width; }
      set { this.MutateVerbose(ref _width, value, RaisePropertyChanged()); }
    }
    public Visibility ColorZoneVisibility {
      get { return _colorZoneVisibility; }
      set { this.MutateVerbose(ref _colorZoneVisibility, value, RaisePropertyChanged()); }
    }
    public IList<string> ListOfLocalization {
      get { return _listOfLocalization; }
      set {
        this.MutateVerbose(ref _listOfLocalization, value, RaisePropertyChanged());
      }
    }
    public int SelectedIndexLocalization {
      get { return _selectedIndexLocalization; }
      set {
        this.MutateVerbose(ref _selectedIndexLocalization, value, RaisePropertyChanged());
        Settings.Same().Localization = _namesOfCultures[SelectedIndexLocalization];
        ResourceManagerService.ChangeLocale(Settings.Same().Localization);
        ListOfLocalization = GetListOfLocalization();
        if (!_switchSelectedIndexLocalization) {
          _switchSelectedIndexLocalization = true; // для недопущения рекурсии
          SelectedIndexLocalization = _namesOfCultures.IndexOf(Settings.Same().Localization);
          LocalizationChangedEvent?.Invoke();
        }
        MainMenuItemsLocalization();
      }
    }
    public bool IsDropDownOpenLocalization {
      get { return _isDropDownOpenLocalization; }
      set {
        _switchSelectedIndexLocalization = false; // для недопущения рекурсии
        this.MutateVerbose(ref _isDropDownOpenLocalization, value, RaisePropertyChanged());
      }
    }
    public string BusyMemoryCaption {
      get { return _busyMemoryCaption; }
      set { this.MutateVerbose(ref _busyMemoryCaption, value, RaisePropertyChanged()); }
    }
    #endregion

    #region Events
    public static event Action LocalizationChangedEvent;
    #endregion

    #region Commands
    #endregion

    #region Commands implementation
    #endregion

    #region Events implemetation
    private void SetUpAppSettings(MainWindow window, EventArgs e) {
      Settings.Same().SetPalette();
      ColorZoneVisibility = Visibility.Visible;
    }
    bool Exit() {
      return MessageConfirmBox.Show(Properties.Settings.Default.AppImage,
                                    _localisationHelper["MainWindowRes.ConfirmBoxMessageExit"],
                                    _localisationHelper["MainWindowRes.ConfirmBoxCaptionExit"],
                                    ConfirmBoxButtons.YesNo,
                                    ConfirmBoxImage.Question
                                   ) == MessageBoxResult.No ? true : false;
    }
    private void Closing(object sender, CancelEventArgs e) {
      if (Settings.Same().AppStatus == TaskStatus.Faulted || Exit()) {
        e.Cancel = true;
      }
    }
    private void MoveWindow() {
      StartupLocation = WindowStartupLocation.Manual;
      State = _window.WindowState;
      Top = _window.Top;
      Left = _window.Left;
      Height = _window.Height;
      Width = _window.Width;
      Properties.Settings.Default.MainWindowStartupLocation = StartupLocation;
      Properties.Settings.Default.MainWindowState = State;
      Properties.Settings.Default.MainWindowTop = Top;
      Properties.Settings.Default.MainWindowLeft = Left;
      Properties.Settings.Default.MainWindowHeight = Height;
      Properties.Settings.Default.MainWindowWidth = Width;
      Properties.Settings.Default.Save();
    }
    private void PositionSave(object sender, EventArgs e) {
      _window.LocationChanged += delegate { MoveWindow(); };
      _window.SizeChanged += delegate { MoveWindow(); };
    }
    #endregion

    #region Helpers
    private void InitLocation() {
      if (State == WindowState.Maximized) {
        _window.WindowState = State;
      } else {
        _window.WindowStartupLocation = StartupLocation;
        _window.WindowState = State;
        if (!(Top == 0)) _window.Top = Top;
        if (!(Left == 0)) _window.Left = Left;
        if (!(Height == 0)) _window.Height = Height;
        if (!(Width == 0)) _window.Width = Width;
      }
    }
    private IList<string> InitNamesOfCultures() {
      IList<string> list = new List<string>() {
        "en-US",
        "ru-RU",
        "uk-UA"
      };
      return list;
    }
    private IList<string> GetListOfLocalization() {
      IList<string> list;
      return list = new List<string>(3) {
          _localisationHelper["MainWindowRes.AddOptionsItemLocalizationEnUS"],
          _localisationHelper["MainWindowRes.AddOptionsItemLocalizationRuRU"],
          _localisationHelper["MainWindowRes.AddOptionsItemLocalizationUkUA"]
        };
    }
    private void MainMenuItemsLocalization() {
      if (MainMenuItems?.Count > 0) {
        MainMenuItems[0].Name = _localisationHelper["MainWindowRes.MenuItemAboutProgram"];
        MainMenuItems[1].Name = _localisationHelper["MainWindowRes.MenuItemKaleidoscopOfColors"];
        MainMenuItems[2].Name = _localisationHelper["MainWindowRes.MenuItemKaleidoscopeOfImages"];
        MainMenuItems[3].Name = _localisationHelper["TicTacToeRes.HeaderCaption"];
        MainMenuItems[4].Name = _localisationHelper["MainWindowRes.MenuItemSettingOptions"];
      }
    }
    private void MainMenuItemsInit() {
      _mainMenuItems = new ObservableCollection<MainMenuItem> {
        new MainMenuItem(
          _localisationHelper["MainWindowRes.MenuItemAboutProgram"],
          new AboutTheProgram() { DataContext = new AboutTheProgramViewModel(new AboutTheProgramHeader()) }
        ),
        new MainMenuItem(
          _localisationHelper["MainWindowRes.MenuItemKaleidoscopOfColors"],
          new View.KaleidoscopeOwn() {
            DataContext = new KaleidoscopeViewModel(
              new KaleidoscopeHeader() { DataContext = new KaleidoscopeHeaderViewModel() },
              HorizontalAlignment.Right
            )
          }
        ) {
          MarginRequirement = new Thickness(0,5,0,0)
        },
        new MainMenuItem(
          _localisationHelper["MainWindowRes.MenuItemKaleidoscopeOfImages"],
          new KaleidoscopeImages() { DataContext = new KaleidoscopeImagesViewModel(SessionMainDialogHost) }
        ),
        new MainMenuItem(
          _localisationHelper["TicTacToeRes.HeaderCaption"],
          new TicTacToe() {
            DataContext = new TicTacToeViewModel(
              new TicTacToeHeader() { DataContext = new TicTacToeHeaderViewModel() },
              HorizontalAlignment.Center
            )
          }
        ),
        new MainMenuItem(
          _localisationHelper["MainWindowRes.MenuItemSettingOptions"],
          new SettingOptions() { DataContext = new SettingOptionsViewModel(new SettingOptionsHeader()) }
        )
      };
    }
    private void SetBusyMemoryCaption(object sender, EventArgs e) {
      var counter = new PerformanceCounter("Process", "Working Set - Private", ProcessName);
      BusyMemoryCaption=$"{counter.RawValue / (1024*1024)}Mb";
    }
    #endregion
  }
}
