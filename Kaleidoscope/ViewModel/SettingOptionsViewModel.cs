using Kaleidoscope.Helpers;
using MaterialDesignColors;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Kaleidoscope.ViewModel {
  public class SettingOptionsViewModel : ViewModelBase {
    public SettingOptionsViewModel(UserControl headerControl) {
      _swatches = new SwatchesProvider().Swatches;
      _headerControl = headerControl;
      ApplyPrimaryCommand = new DelegateCommand(o => ApplyPrimary((Swatch)o));
      ApplyAccentCommand = new DelegateCommand(o => ApplyAccent((Swatch)o));
    }
    #region Fields
    private IEnumerable<Swatch> _swatches;
    #endregion

    #region Properties
    public IEnumerable<Swatch> Swatches { get { return _swatches; } }
    #endregion

    #region Events
    public static Action ApplyPrimaryChanged;
    public static Action ApplyAccentChanged;
    #endregion

    #region Commands
    public DelegateCommand ApplyPrimaryCommand { get; }
    public DelegateCommand ApplyAccentCommand { get; }
    #endregion

    #region Commands implementation
    private static void ApplyPrimary(Swatch swatch) {
      new PaletteHelper().ReplacePrimaryColor(swatch);
      Settings.Same().AppPalettePrimaryName = swatch.Name;
      ApplyPrimaryChanged?.Invoke();
    }
    private static void ApplyAccent(Swatch swatch) {
      new PaletteHelper().ReplaceAccentColor(swatch);
      Settings.Same().AppPaletteAccentName = swatch.Name;
      ApplyAccentChanged?.Invoke();
    }
    #endregion

  }
}
