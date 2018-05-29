using Kaleidoscope.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Kaleidoscope.ViewModel {
  public class SampleProgressDialogViewModel : ViewModelPrimary {
    public SampleProgressDialogViewModel(string imageUrl, 
                                         string headerCaption, 
                                         double progressTextBlockWidth = 500) {
      HeaderUrlImage = imageUrl;
      HeaderCaption = headerCaption;
      ProgressTextBlockWidth = progressTextBlockWidth;
    }
    public SampleProgressDialogViewModel(string imageUrl,
                                         string headerCaption,
                                         string mainCaption,
                                         double progressTextBlockWidth = 500) 
      : this(imageUrl, headerCaption, progressTextBlockWidth) {
      _mainCaption = mainCaption;
    }
    public SampleProgressDialogViewModel(string imageUrl, 
                                         string headerCaption,
                                         string mainCaption,
                                         string progressCaption, 
                                         double progressTextBlockWidth = 500)
      : this(imageUrl, headerCaption, mainCaption, progressTextBlockWidth) {
      ProgressCaption = progressCaption;
    }

    #region Fields
    private bool _isProgress = false;
    private bool _isCompletedProgress = false;
    private string _headerUrlImage = string.Empty;
    private string _headerCaption = string.Empty;
    private string _mainCaption = string.Empty;
    private Visibility _mainCaptionVisibility = Visibility.Visible;
    private Visibility _progressPanelVisibility = Visibility.Collapsed;
    private Visibility _progressBarVisibility = Visibility.Collapsed;
    private double _progressTextBlockWidth;
    private string _progressCaption = string.Empty;
    private Visibility _buttonPanelVisibility = Visibility.Visible;
    #endregion

    #region Properties
    public bool IsProgress {
      get { return _isProgress; }
      set {
        this.MutateVerbose(ref _isProgress, value, RaisePropertyChanged());
        if (IsProgress) {
          ProgressBarVisibility = Visibility.Visible;
        }
      }
    }
    public bool IsCompletedProgress {
      get { return _isCompletedProgress; }
      set {
        this.MutateVerbose(ref _isCompletedProgress, value, RaisePropertyChanged());
        ProgressBarVisibility = Visibility.Collapsed;
      }
    }
    public string HeaderUrlImage {
      get { return _headerUrlImage; }
      set { this.MutateVerbose(ref _headerUrlImage, value, RaisePropertyChanged()); }
    }
    public string HeaderCaption {
      get { return _headerCaption; }
      set { this.MutateVerbose(ref _headerCaption, value, RaisePropertyChanged()); }
    }
    public string MainCaption {
      get { return _mainCaption; }
      set { this.MutateVerbose(ref _mainCaption, value, RaisePropertyChanged()); }
    }
    public Visibility MainCaptionVisibility {
      get { return _mainCaptionVisibility; }
      set { this.MutateVerbose(ref _mainCaptionVisibility, value, RaisePropertyChanged()); }
    }
    public Visibility ProgressPanelVisibility {
      get { return _progressPanelVisibility; }
      set { this.MutateVerbose(ref _progressPanelVisibility, value, RaisePropertyChanged()); }
    }
    public Visibility ProgressBarVisibility {
      get { return _progressBarVisibility; }
      set { this.MutateVerbose(ref _progressBarVisibility, value, RaisePropertyChanged()); }
    }
    public double ProgressTextBlockWidth {
      get { return _progressTextBlockWidth; }
      set { this.MutateVerbose(ref _progressTextBlockWidth, value, RaisePropertyChanged()); }
    }
    public string ProgressCaption {
      get { return _progressCaption; }
      set { this.MutateVerbose(ref _progressCaption, value, RaisePropertyChanged()); }
    }
    public Visibility ButtonPanelVisibility {
      get { return _buttonPanelVisibility; }
      set { this.MutateVerbose(ref _buttonPanelVisibility, value, RaisePropertyChanged()); }
    }
    #endregion

    #region Helpers
    public void ProgressTextBlock(string caption) {
      ProgressCaption = caption;
    }
    public void TrackCompletedProgress() {
      IsCompletedProgress = true;
    }
    #endregion
  }
}
