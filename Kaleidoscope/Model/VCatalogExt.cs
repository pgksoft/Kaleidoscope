using Kaleidoscope.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kaleidoscope.Model {
  public class VCatalogExt : VCatalog {
    // Для поддержки операций TreeView
    #region Fields
    private bool _selected;
    #endregion

    #region Properties
    public bool Selected {
      get { return _selected; }
      set { this.MutateVerbose(ref _selected, value, RaisePropertyChanged()); }
    }
    #endregion
  }
}
