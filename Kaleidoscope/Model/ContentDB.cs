using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kaleidoscope.Model {
  public class ContentDB : DbContext {
    public ContentDB() : base("name=ContentDB") {

    }

    #region Tables
    public virtual DbSet<UrlContent> UrlContents { get; set; }
    public virtual DbSet<ContentProperty> ContentProperties { get; set; }
    public virtual DbSet<Catalog> Catalogs { get; set; }
    public virtual DbSet<RefPathway> RefPathways { get; set; }
    public virtual DbSet<RefNode> RefNodes { get; set; }
    public virtual DbSet<RefVideoType> RefVideoTypes { get; set; }
    public virtual DbSet<RefImageType> RefImageTypes { get; set; }
    public virtual DbSet<RefComment> RefComments { get; set; }
    #endregion

    #region View
    public virtual DbSet<VUrlContent> VUrlContents { get; set; }
    public virtual DbSet<VCatalog> VCatalogs { get; set; }
    public virtual DbSet<VTreeCatalog> VTreeCatalogs { get; set; }
    public virtual DbSet<VRefPathway> VRefPathways { get; set; }
    #endregion

  }
}
