using Kaleidoscope.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kaleidoscope.ViewModel {
  public class ContentMainHeaderControlViewModel : ViewModelPrimary {
    public ContentMainHeaderControlViewModel(string localizationIndex) {
      _localizationIndex = localizationIndex;
      MainWindowViewModel.LocalizationChangedEvent += new Action(SetCaption);
      SetCaption();
    }

    #region Fields
    private string _localizationIndex;
    private string _caption;
    #endregion

    #region Properties
    public string Caption {
      get { return _caption; }
      set { this.MutateVerbose(ref _caption, value, RaisePropertyChanged()); }
    }
    #endregion

    #region Dispose
    protected override void OnDispose() {
      base.OnDispose();
      MainWindowViewModel.LocalizationChangedEvent -= new Action(SetCaption);
    }
    #endregion

    #region Helpers
    private void SetCaption() {
      Caption = Settings.Same().LocalisationHelper[_localizationIndex];
    }
    #endregion
  }
}
