using Kaleidoscope.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kaleidoscope.Model {
  public class ContentProperty : ModelBase {

    #region Fields
    private int _content;
    private int _rotate;
    #endregion

    #region Properties
    public int Content {
      get { return _content; }
      set { this.MutateVerbose(ref _content, value, RaisePropertyChanged()); }
    }
    public int Rotate {
      get { return _rotate; }
      set { this.MutateVerbose(ref _rotate, value, RaisePropertyChanged()); }
    }
    #endregion
  }
}
