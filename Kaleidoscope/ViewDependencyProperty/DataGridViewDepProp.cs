using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Kaleidoscope.ViewDependencyProperty {
  public class DataGridViewDepProp : DependencyObject {
    static DataGridViewDepProp() {
      Instance = new DataGridViewDepProp();
    }

    public static DataGridViewDepProp Instance { get; private set; }

    public static readonly DependencyProperty WidthColumnNameProperty =
      DependencyProperty.Register("WidthColumnName", typeof(double), typeof(DataGridViewDepProp));

    public static readonly DependencyProperty HeaderColumnNameProperty =
      DependencyProperty.Register("HeaderColumnName", typeof(string), typeof(DataGridViewDepProp));

    public static readonly DependencyProperty SortDirectionColumnNameProperty =
      DependencyProperty.Register("SortDirectionColumnName", typeof(ListSortDirection?), typeof(DataGridViewDepProp));

    public static readonly DependencyProperty HeaderColumnNCountImageTypeProperty =
      DependencyProperty.Register("HeaderColumnNCountImageType", typeof(string), typeof(DataGridViewDepProp));

    public static readonly DependencyProperty HeaderColumnNCountVideoTypeProperty =
      DependencyProperty.Register("HeaderColumnNCountVideoType", typeof(string), typeof(DataGridViewDepProp));

    public double WidthColumnName {
      get { return (double)GetValue(WidthColumnNameProperty); }
      set { SetValue(WidthColumnNameProperty, value); }
    }
    public string HeaderColumnName {
      get { return (string)GetValue(HeaderColumnNameProperty); }
      set { SetValue(HeaderColumnNameProperty, value); }
    }
    public ListSortDirection? SortDirectionColumnName {
      get { return (ListSortDirection?)GetValue(SortDirectionColumnNameProperty); }
      set { SetValue(SortDirectionColumnNameProperty, value); }
    }
    public string HeaderColumnNCountImageType {
      get { return (string)GetValue(HeaderColumnNCountImageTypeProperty); }
      set { SetValue(HeaderColumnNCountImageTypeProperty, value); }
    }
    public string HeaderColumnNCountVideoType {
      get { return (string)GetValue(HeaderColumnNCountVideoTypeProperty); }
      set { SetValue(HeaderColumnNCountVideoTypeProperty, value); }
    }
  }
}

