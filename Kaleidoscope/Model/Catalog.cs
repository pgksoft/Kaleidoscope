using Kaleidoscope.Helpers;
using SQLite.Net.Attributes;

namespace Kaleidoscope.Model {
  public class Catalog : ModelBase {
    #region Fileds
    private int _node;
    private int? _parentId;
    #endregion

    #region Properties
    public int Node {
      get { return _node; }
      set { this.MutateVerbose(ref _node, value, RaisePropertyChanged()); }
    }
    public int? ParentID {
      get { return _parentId; }
      set { this.MutateVerbose(ref _parentId, value, RaisePropertyChanged()); }
    }
    #endregion
  }
}
