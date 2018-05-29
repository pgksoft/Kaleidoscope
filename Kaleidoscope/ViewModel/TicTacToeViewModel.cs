using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Kaleidoscope.ViewModel {
  class TicTacToeViewModel : ViewModelBase {
    public TicTacToeViewModel(UserControl headerControl, HorizontalAlignment headerControlHorizontalAlignment) {
      _headerControl = headerControl;
      _headerControlHorizontalAlignment = headerControlHorizontalAlignment;
    }
  }
}
