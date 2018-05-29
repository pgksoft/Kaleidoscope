using Kaleidoscope.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Kaleidoscope.ViewModel {
  public abstract class ViewModelBase : ViewModelPrimary {
    protected ViewModelBase() {
    }
    #region Fields
    protected UserControl _headerControl;
    protected HorizontalAlignment _headerControlHorizontalAlignment = HorizontalAlignment.Center;
    #endregion

    #region Properties
    public UserControl HeaderControl {
      get { return _headerControl; }
      set { this.MutateVerbose(ref _headerControl, value, RaisePropertyChanged()); }
    }
    public HorizontalAlignment HeaderControlHorizontalAlignment {
      get { return _headerControlHorizontalAlignment; }
      set { this.MutateVerbose(ref _headerControlHorizontalAlignment, value, RaisePropertyChanged()); }
    }
    #endregion

  }
}
