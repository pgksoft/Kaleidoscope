using Kaleidoscope.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Kaleidoscope.Helpers {
  public enum ContentMenuItemsCode { Content, Folders, Catalog, CatalogItems, ImageTypes, VideoTypes, Comments };

  public class ContentMenuItem : MainMenuItem {
    public ContentMenuItem(ContentMenuItemsCode code, string name, object content) : base(name, content) {
      _code = code;
      _marginRequirement = new Thickness(0, 0, 0, 0);
      _horizontalScrollBarVisibilityRequirement = ScrollBarVisibility.Disabled;
      _verticalScrollBarVisibilityRequirement = ScrollBarVisibility.Disabled;
    }

    #region Fields
    private ContentMenuItemsCode _code;
    #endregion

    #region Properties
    public ContentMenuItemsCode Code { get { return _code; } }
    #endregion

    #region IDispose
    protected override void OnDispose() {
      base.OnDispose();
      if (Content is UserControl && (Content as UserControl).DataContext is ViewModelBase) {
        ((Content as UserControl).DataContext as ViewModelBase).Dispose();
        Content = null;
      }
    }
    #endregion
  }
}
