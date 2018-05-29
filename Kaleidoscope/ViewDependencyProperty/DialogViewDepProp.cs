using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Kaleidoscope.ViewDependencyProperty {
  public class DialogViewDepProp : DependencyObject {
    static DialogViewDepProp() {
      Instance = new DialogViewDepProp();
    }
    public static DialogViewDepProp Instance { get; private set; }

    #region Fields Property
    public static readonly DependencyProperty WidthFieldNameProperty = 
      DependencyProperty.Register("WidthFieldName", typeof(double), typeof(DialogViewDepProp), new PropertyMetadata(400.0));
    #endregion

    #region Properties as implementation of fields of properties
    public double WidthFieldName {
      get { return (double)GetValue(WidthFieldNameProperty); }
      set { SetValue(WidthFieldNameProperty, value); }
    }
    #endregion

  }
}
