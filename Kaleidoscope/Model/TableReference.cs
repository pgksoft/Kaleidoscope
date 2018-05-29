using Kaleidoscope.Helpers;

namespace Kaleidoscope.Model {
  public abstract class TableReference : ModelBase {
    
    #region Fields
    private string _name;
    #endregion

    #region Properties
    public string Name {
      get { return _name; }
      set { this.MutateVerbose(ref _name, value, RaisePropertyChanged()); }
    }
    #endregion
  }
}
