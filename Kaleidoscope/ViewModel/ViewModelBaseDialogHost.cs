using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kaleidoscope.ViewModel {
  public abstract class ViewModelBaseDialogHost : ViewModelBase {
    protected DialogSession _sessionMainDialogHost;
    public DialogSession SessionMainDialogHost => _sessionMainDialogHost;
  }
}
