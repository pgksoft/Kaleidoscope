using Kaleidoscope.Helpers;
using MaterialDesignThemes.Wpf;
using System;

namespace Kaleidoscope.ViewModel {
  public class PaletteThemeViewModel : ViewModelBase {
    public static PaletteThemeViewModel Create() {
      if (_instance ==  null) {
        _instance = new PaletteThemeViewModel();
      };
      return _instance;
    }
    protected PaletteThemeViewModel() {
      // Set up app settings
      IsDark = Settings.Same().AppIsDark;
    }

    #region Events
    public static Action ThemeChanged;
    #endregion

    #region Fields
    static private PaletteThemeViewModel _instance;
    private bool _isDark;
    #endregion

    #region Properties
    public bool IsDark {
      get { return _isDark; }
      set {
        this.MutateVerbose(ref _isDark, value, RaisePropertyChanged());
        new PaletteHelper().SetLightDark(IsDark);
        Settings.Same().AppIsDark = IsDark;
        ThemeChanged?.Invoke();
      }
    }
    #endregion
  }
}
