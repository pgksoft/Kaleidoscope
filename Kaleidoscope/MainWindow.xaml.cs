using Kaleidoscope.ViewModel;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Kaleidoscope {
  /// <summary>
  /// Логика взаимодействия для MainWindow.xaml
  /// </summary>
  public partial class MainWindow : Window {
    public MainWindow() {
      InitializeComponent();
      DataContext = new MainWindowViewModel();
    }
    private void UIElement_OnPreviewMouseButtonUp(object sender, MouseButtonEventArgs e) {
      //until we had a StaysOpen glag to Drawer, this will help with scroll bars
      var dependencyObject = Mouse.Captured as DependencyObject;
      while (dependencyObject != null) {
        if (dependencyObject is ScrollBar) return;
        dependencyObject = VisualTreeHelper.GetParent(dependencyObject);
      }

      MenuToggleButton.IsChecked = false;
    }

  }
}
