using Kaleidoscope.Helpers;
using Kaleidoscope.Model;
using Kaleidoscope.ViewModel;
using System;
using System.Windows.Controls;

namespace Kaleidoscope.ViewModelDialog {
  public class ChoiceDialogDictionaryViewModel<T> : ViewModelPrimary where T : TableReference, new() {
    public ChoiceDialogDictionaryViewModel(string headerUrlImage, string headerCaption, string name) {
      _headerUrlImage = headerUrlImage;
      _headerCaption = headerCaption + (string.IsNullOrWhiteSpace(name) ? string.Empty : ": " + name);
      _nameOld = name;
    }

    #region Fields
    private UserControl _headerContent;
    private UserControl _dictionaryContent;
    private string _headerUrlImage;
    private string _headerCaption;
    private string _name = string.Empty;
    private string _nameOld = string.Empty;
    private bool _notIsEnabled;
    #endregion

    #region Properties
    public UserControl HeaderContent {
      get { return _headerContent; }
      set { this.MutateVerbose(ref _headerContent, value, RaisePropertyChanged()); }
    }
    public UserControl DictionaryContent {
      get { return _dictionaryContent; }
      set {
        this.MutateVerbose(ref _dictionaryContent, value, RaisePropertyChanged());
        (DictionaryContent.DataContext as DirectoriesViewModel<T>).SelectedIndexEvent += (o) => SelectedIndexDictionary(o);
        HeaderContent = (DictionaryContent.DataContext as DirectoriesViewModel<T>).HeaderControl;
        (DictionaryContent.DataContext as DirectoriesViewModel<T>).OnSelectedIndex();
      }
    }
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
    #endregion

    #region Implementation of subscriptions on events
    private void SelectedIndexDictionary(string name) {
      Name = name;
    }
    #endregion

    #region Command implementation
    public bool NotCanAccept() {
      return string.IsNullOrWhiteSpace(Name) || (!string.IsNullOrWhiteSpace(_nameOld) && String.Compare(Name, _nameOld) == 0);
    }
    #endregion

  }
}
