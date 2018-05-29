using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kaleidoscope.Model {
  public class PatternSearchFileComparer : IEqualityComparer<PatternSearchFile> {
    public bool Equals(PatternSearchFile x, PatternSearchFile y) {
      if (Object.ReferenceEquals(x, y)) return true;
      if (Object.ReferenceEquals(x, null) || Object.ReferenceEquals(y, null)) return false;
      return x.Id == y.Id && x.Pattern == y.Pattern && x.Kind == y.Kind;
    }

    public int GetHashCode(PatternSearchFile obj) {
      if (Object.ReferenceEquals(obj, null)) return 0;

      int hashID = obj.Id.GetHashCode();
      int hashPattern = obj.Pattern == null ? 0 : obj.Pattern.GetHashCode();
      int hashKind = obj.Kind.GetHashCode();

      //Calculate the hash code for the product.
      return hashID ^ hashPattern ^ hashKind;
    }
  }
}
