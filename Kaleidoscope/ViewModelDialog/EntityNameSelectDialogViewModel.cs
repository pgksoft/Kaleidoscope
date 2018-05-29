using Kaleidoscope.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;

namespace Kaleidoscope.ViewModelDialog {
  public class EntityNameSelectDialogViewModel : TableReferenceAddUpdDialogViewModel {
    public EntityNameSelectDialogViewModel(string headerUrlImage, 
                                           string headerCaption, 
                                           string name, 
                                           string imageButtonSelect, 
                                           string fieldNameHint,
                                           double fieldNameWidth = 400) 
      : base(headerUrlImage, headerCaption, name, fieldNameHint, fieldNameWidth) {
      ImageButtonSelect = imageButtonSelect;
      EntitySelectedCommand = new DelegateCommand(o => FolderSelected());
    }

    #region Fields
    private bool _isProgress = false;
    private bool _isCompletedProgress = false;
    private string _imageButtonSelect = string.Empty;
    private Visibility _progressBarVisibility = Visibility.Collapsed;
    private Visibility _progressTextBlockVisibility = Visibility.Collapsed;
    private string _captionProgress = string.Empty;
    #endregion

    #region Properties
    public bool IsProgress {
      get { return _isProgress; }
      set { this.MutateVerbose(ref _isProgress, value, RaisePropertyChanged()); }
    }
    public bool IsCompletedProgress {
      get { return _isCompletedProgress; }
      set { this.MutateVerbose(ref _isCompletedProgress, value, RaisePropertyChanged()); }
    }
    public string ImageButtonSelect {
      get { return _imageButtonSelect; }
      set { this.MutateVerbose(ref _imageButtonSelect, value, RaisePropertyChanged()); }
    }
    public Visibility ProgressBarVisibility {
      get { return _progressBarVisibility; }
      set { this.MutateVerbose(ref _progressBarVisibility, value, RaisePropertyChanged()); }
    }
    public Visibility ProgressTextBlockVisibility {
      get { return _progressTextBlockVisibility; }
      set { this.MutateVerbose(ref _progressTextBlockVisibility, value, RaisePropertyChanged()); }
    }
    public string CaptionProgress {
      get { return _captionProgress; }
      set { this.MutateVerbose(ref _captionProgress, value, RaisePropertyChanged()); }
    }
    #endregion

    #region Commands
    public DelegateCommand EntitySelectedCommand { get; }
    #endregion

    #region Helpers
    private void FolderSelected() {
      using (FolderBrowserDialog dlg = new FolderBrowserDialog()) {
        dlg.Description = Settings.Instance.LocalisationHelper["ContentFoldersRes.MsgDescriptionFolderBrowserDialog"];
        dlg.SelectedPath = Name;
        dlg.ShowNewFolderButton = true;
        DialogResult result = dlg.ShowDialog();
        if (result == DialogResult.OK) {
          Name = dlg.SelectedPath;
        }
      }
    }
    public void ProgressTextBlock(string caption) {
      CaptionProgress = caption;
    }
    public void TrackCompletedProgress() {
      IsCompletedProgress = true;
    }
    #endregion

  }
}
