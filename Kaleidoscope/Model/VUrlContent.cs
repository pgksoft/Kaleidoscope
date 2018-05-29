using Kaleidoscope.Helpers;
using SQLite.Net.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Kaleidoscope.Model {
  public class VUrlContent : TableReference {

    #region Fields
    private int _nPathway;
    private string _sPathway = string.Empty;
    private int? _nImageType;
    private string _sImageType = string.Empty;
    private int? _nVideoType;
    private string _sVideoType = string.Empty;
    private int? _nRefComment;
    private string _sRefComment = string.Empty;
    private string _description = string.Empty;
    private int? _nProperty;
    private int? _nRotate;
    #endregion

    #region Properties
    public int NPathway {
      get { return _nPathway; }
      set { this.MutateVerbose(ref _nPathway, value, RaisePropertyChanged()); }
    }
    public string SPathway {
      get { return _sPathway; }
      set { this.MutateVerbose(ref _sPathway, value, RaisePropertyChanged()); }
    }
    public int? NImageType {
      get { return _nImageType; }
      set { this.MutateVerbose(ref _nImageType, value, RaisePropertyChanged()); }
    }
    public string SImageType {
      get { return _sImageType; }
      set { this.MutateVerbose(ref _sImageType, value, RaisePropertyChanged()); }
    }
    public int? NVideoType {
      get { return _nVideoType; }
      set { this.MutateVerbose(ref _nVideoType, value, RaisePropertyChanged()); }
    }
    public string SVideoType {
      get { return _sVideoType; }
      set { this.MutateVerbose(ref _sVideoType, value, RaisePropertyChanged()); }
    }
    public int? NRefComment {
      get { return _nRefComment; }
      set { this.MutateVerbose(ref _nRefComment, value, RaisePropertyChanged()); }
    }
    public string SRefComment {
      get { return _sRefComment; }
      set { this.MutateVerbose(ref _sRefComment, value, RaisePropertyChanged()); }
    }
    public string Description {
      get { return _description; }
      set { this.MutateVerbose(ref _description, value, RaisePropertyChanged()); }
    }
    public int? NProperty {
      get { return _nProperty; }
      set { this.MutateVerbose(ref _nProperty, value, RaisePropertyChanged()); }
    }
    public int? NRotate {
      get { return _nRotate; }
      set { this.MutateVerbose(ref _nRotate, value, RaisePropertyChanged()); }
    }
    #endregion

  }
}
