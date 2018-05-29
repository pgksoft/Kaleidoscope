using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kaleidoscope.Model {
  public class ConditionShowContent {
    public ConditionShowContent(List<int> folders) {
      _folders = folders;
    }

    #region Fields
    private List<int> _folders;
    #endregion

    #region Properties
    public List<int> Follders => _folders;
    #endregion
  }
}
