using Kaleidoscope.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kaleidoscope.Model {
  public class VRefPathway : RefPathway {

    #region Fields
    private int? _nCountImageType;
    private int? _nCountVideoType;
    #endregion

    #region Properties
    public int? NCountImageType {
      get { return _nCountImageType; }
      set { this.MutateVerbose(ref _nCountImageType, value, RaisePropertyChanged()); }
    }
    public int? NCountVideoType {
      get { return _nCountVideoType; }
      set { this.MutateVerbose(ref _nCountVideoType, value, RaisePropertyChanged()); }
    }
    #endregion

  }
}
