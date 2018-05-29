using Kaleidoscope.ViewModel;
using System.Windows.Controls;

namespace Kaleidoscope.View {
  /// <summary>
  /// Логика взаимодействия для PaletteTheme.xaml
  /// </summary>
  public partial class PaletteTheme : UserControl {
    public PaletteTheme() {
      InitializeComponent();
      DataContext = PaletteThemeViewModel.Create();
    }
  }
}
