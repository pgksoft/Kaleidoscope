using Kaleidoscope.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kaleidoscope.Helpers {
  public class FillTypeColors : ViewModelBase {
    public FillTypeColors() {
        
    }
    public FillTypeColors(string name) {
      Name = name;
    }
    private string _name;
    public string Name {
      get { return _name; }
      set { this.MutateVerbose(ref _name, value, RaisePropertyChanged()); }
    }
  }
}
