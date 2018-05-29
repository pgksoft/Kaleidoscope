using Kaleidoscope.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kaleidoscope.Model {
  public class VTreeCatalog : ModelBase {
    #region Fields
    private int _node;
    private string _sNode;
    private int? _parentID;
    private string _sParent;
    private int _level;
    #endregion

    #region Properties
    public int Node {
      get { return _node; }
      set { this.MutateVerbose(ref _node, value, RaisePropertyChanged()); }
    }
    public string SNode {
      get { return _sNode; }
      set { this.MutateVerbose(ref _sNode, value, RaisePropertyChanged()); }
    }
    public int? ParentID {
      get { return _parentID; }
      set { this.MutateVerbose(ref _parentID, value, RaisePropertyChanged()); }
    }
    public string SParent {
      get { return _sParent; }
      set { this.MutateVerbose(ref _sParent, value, RaisePropertyChanged()); }
    }
    public int Level {
      get { return _level; }
      set { this.MutateVerbose(ref _level, value, RaisePropertyChanged()); }
    }
    #endregion
  }
}
