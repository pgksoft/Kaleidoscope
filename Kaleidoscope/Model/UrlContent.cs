using Kaleidoscope.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kaleidoscope.Model {
  public class UrlContent : TableReference {

    #region Fields
    int _pathway;
    int? _imageType;
    int? _videoType;
    int? _refComment;
    string _description;
    #endregion

    #region Properties
    public int Pathway {
      get { return _pathway; }
      set { this.MutateVerbose(ref _pathway, value, RaisePropertyChanged()); }
    }
    public int? ImageType {
      get { return _imageType; }
      set { this.MutateVerbose(ref _imageType, value, RaisePropertyChanged()); }
    }
    public int? VideoType {
      get { return _videoType; }
      set { this.MutateVerbose(ref _videoType, value, RaisePropertyChanged()); }
    }
    public int? RefComment {
      get { return _refComment; }
      set { this.MutateVerbose(ref _refComment, value, RaisePropertyChanged()); }
    }
    public string Description {
      get { return _description; }
      set { this.MutateVerbose(ref _description, value, RaisePropertyChanged()); }
    }
    #endregion

  }
}
