using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Kaleidoscope.ViewDependencyProperty {
  public class ContentViewDepProp : DependencyObject {
    static ContentViewDepProp() {
      Instance = new ContentViewDepProp();
    }

    public static ContentViewDepProp Instance { get; private set; }
    // FoldersImageBorderWidth
    public static readonly DependencyProperty FoldersImageBorderWidthProperty =
      DependencyProperty.Register("FoldersImageBorderWidth", typeof(double), typeof(ContentViewDepProp));
    public double FoldersImageBorderWidth {
      get { return (double)GetValue(FoldersImageBorderWidthProperty); }
      set { SetValue(FoldersImageBorderWidthProperty, value); }
    }
    // FoldersImageBorderHeightProperty
    public static readonly DependencyProperty FoldersImageBorderHeightProperty =
      DependencyProperty.Register("FoldersImageBorderHeight", typeof(double), typeof(ContentViewDepProp));
    public double FoldersImageBorderHeight {
      get { return (double)GetValue(FoldersImageBorderHeightProperty); }
      set { SetValue(FoldersImageBorderHeightProperty, value); }
    }
    //

  }
}
