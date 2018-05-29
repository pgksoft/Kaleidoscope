using Kaleidoscope.Helpers;
using Kaleidoscope.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kaleidoscope.ViewModelDialog {
  public class TableReferenceAddUpdDialogViewModel : ViewModelPrimary {
    public TableReferenceAddUpdDialogViewModel(string headerUrlImage, 
                                               string headerCaption, 
                                               string name,
                                               string fieldNameHint,
                                               double fieldNameWidth = 400) {
      _headerUrlImage = headerUrlImage;
      _headerCaption = headerCaption;
      _nameOld = name;
      FieldNameHint = fieldNameHint;
      FieldNameWidth = fieldNameWidth;
      Name = name;
    }

    #region Fields
    protected string _headerUrlImage;
    protected string _headerCaption;
    protected string _name = string.Empty;
    protected string _nameOld = string.Empty;
    protected bool _notIsEnabled;
    protected double _fieldNameWidth;
    protected string _fieldNameHint;
    #endregion

    #region Properties
    public string Name {
      get { return _name; }
      set {
        this.MutateVerbose(ref _name, value, RaisePropertyChanged());
        NotIsEnabled = NotCanAccept();
      }
    }
    public string HeaderUrlImage => _headerUrlImage;
    public string HeaderCaption => _headerCaption;
    public bool NotIsEnabled {
      get { return _notIsEnabled; }
      set { this.MutateVerbose(ref _notIsEnabled, value, RaisePropertyChanged()); }
    }
    public double FieldNameWidth {
      get { return _fieldNameWidth; }
      set { this.MutateVerbose(ref _fieldNameWidth, value, RaisePropertyChanged()); }
    }
    public string FieldNameHint {
      get { return _fieldNameHint; }
      set { this.MutateVerbose(ref _fieldNameHint, value, RaisePropertyChanged()); }
    }
    #endregion

    #region Command implementation
    public bool NotCanAccept() {
      return !string.IsNullOrWhiteSpace(_nameOld) && String.Compare(Name, _nameOld) == 0;
    }
    #endregion
  }
}
