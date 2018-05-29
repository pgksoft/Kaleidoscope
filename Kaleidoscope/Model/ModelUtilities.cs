using Kaleidoscope.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kaleidoscope.Model {
  public sealed class ModelUtilities {
    private ModelUtilities() {
    }

    #region Fields
    private static ModelUtilities _instance;
    #endregion

    #region Properties
    public static ModelUtilities Instance {
      get { return _instance ?? (Instance = new ModelUtilities()); }
      private set { _instance = value; }
    }
    #endregion

    #region Methods
    public int GetId<T>(string name) where T : TableReference, new() {
      string tableName = typeof(T).Name + "s";
      return (
              from dic in Settings.Instance.ContentDB.Database.SqlQuery<T>("SELECT * FROM " + tableName)
              where dic.Name == name
              select dic.Id
             ).FirstOrDefault();
    }
    #endregion

  }
}
