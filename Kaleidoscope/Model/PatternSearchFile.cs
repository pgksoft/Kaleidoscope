using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kaleidoscope.Model {
  public enum KindFile { None, Image, Video }
  public class PatternSearchFile {
    public PatternSearchFile(int id,string pattern, KindFile kind) {
      _id = id;
      _pattern = pattern;
      _kind = kind;
    }

    #region Fields
    private int _id;
    private string _pattern = string.Empty;
    private KindFile _kind = KindFile.None;
    #endregion
    public int Id => _id;
    public string Pattern => _pattern;
    public KindFile Kind => _kind;
  }
}
