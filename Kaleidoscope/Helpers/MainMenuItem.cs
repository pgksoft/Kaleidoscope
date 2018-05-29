using Kaleidoscope.ViewModel;
using System.Windows;
using System.Windows.Controls;

namespace Kaleidoscope.Helpers {
  public class MainMenuItem : ViewModelBase {
    protected string _name;
    protected object _content;
    protected ScrollBarVisibility _horizontalScrollBarVisibilityRequirement = ScrollBarVisibility.Disabled;
    protected ScrollBarVisibility _verticalScrollBarVisibilityRequirement = ScrollBarVisibility.Disabled;
    protected Thickness _marginRequirement = new Thickness(1,5,1,1);

    public MainMenuItem(string name, object content) {
      _name = name;
      Content = content;
    }
    public string Name {
      get { return _name; }
      set {
        this.MutateVerbose(ref _name, value, RaisePropertyChanged());
      }
    }
    public object Content {
      get { return _content; }
      set { this.MutateVerbose(ref _content, value, RaisePropertyChanged()); }
    }
    public ScrollBarVisibility HorizontalScrollBarVisibilityRequirement {
      get { return _horizontalScrollBarVisibilityRequirement; }
      set { this.MutateVerbose(ref _horizontalScrollBarVisibilityRequirement, value, RaisePropertyChanged()); }
    }
    public ScrollBarVisibility VerticalScrollBarVisibilityRequirement {
      get { return _verticalScrollBarVisibilityRequirement; }
      set { this.MutateVerbose(ref _verticalScrollBarVisibilityRequirement, value, RaisePropertyChanged()); }
    }
    public Thickness MarginRequirement {
      get { return _marginRequirement; }
      set { this.MutateVerbose(ref _marginRequirement, value, RaisePropertyChanged()); }
    }
  }
}
