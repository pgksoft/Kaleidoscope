using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Kaleidoscope.HelpersView {
  /// <summary>
  /// Логика взаимодействия для MessageConfirm.xaml
  /// </summary>
  public partial class MessageConfirm : Window {
    public MessageConfirm() {
      InitializeComponent();
      if (Application.Current.MainWindow?.IsLoaded == true) {
        this.Owner = Application.Current.MainWindow;
      }
    }
  }
}
