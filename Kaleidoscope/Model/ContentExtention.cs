using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kaleidoscope.Model {
  public static class ContentExtention {
    public static VCatalogExt Init(this VCatalogExt nodeTreeView, VCatalog item) {
      nodeTreeView.Id = item.Id;
      nodeTreeView.Node = item.Node;
      nodeTreeView.SNode = item.SNode;
      nodeTreeView.SParent = item.SParent;
      nodeTreeView.ParentID = item.ParentID;
      return nodeTreeView;
    }
  }
}
