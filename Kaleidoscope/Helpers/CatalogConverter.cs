﻿using Kaleidoscope.Model;
using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Windows.Data;

namespace Kaleidoscope.Helpers {
  public class CatalogConverter : IValueConverter {
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
      if (value is VCatalogExt x) {
        var query = Settings.Same().ContentDB.Database.SqlQuery<VCatalog>("SELECT * FROM VCatalogs WHERE ParentID = " + x.Id + " ORDER BY SNode");
        ObservableCollection<VCatalogExt> temp = new ObservableCollection<VCatalogExt>();
        foreach (var item in query) {
          temp.Add(new VCatalogExt().Init(item));
        }
        return temp;
      } else {
        return null;
      }
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
      throw new NotImplementedException();
    }
  }
}
