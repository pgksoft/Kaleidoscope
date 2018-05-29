using Kaleidoscope.ViewModel;
using Kaleidoscope.Helpers;
using System.Linq;
using System.Windows;
using Kaleidoscope.HelpersView;
using LocalizatorHelper;

namespace Kaleidoscope.HelpersViewModel {
  public enum ConfirmBoxImage { None, Asterisk, Error, Exclamation, Hand, Information, Question, Stop, Warning }
  public enum ConfirmBoxButtons { OK, OKCancel, YesNo, Cancel }
  public class MessageConfirmViewModel : ViewModelBase {

    #region Constructurs
    public MessageConfirmViewModel(string message) {
      _window.MouseLeftButtonDown += (o, e) => _window.DragMove();
      Message = message;
      OkCommand = new DelegateCommand(o => Ok());
      CancelCommand = new DelegateCommand(o => Cancel());
      YesCommand = new DelegateCommand(o => Yes());
      NoCommand = new DelegateCommand(o => No());
    }
    public MessageConfirmViewModel(string message, string caption) : this(message) {
      Caption = caption;
    }
    public MessageConfirmViewModel(string message, string caption, ConfirmBoxButtons messageBoxButtons) : this(message, caption) {
      ConfirmBoxButtons = messageBoxButtons;
    }
    public MessageConfirmViewModel(string message, string caption, ConfirmBoxButtons confirmBoxButtons, ConfirmBoxImage confirmBoxImage) :
      this(message, caption, confirmBoxButtons) {
      ConfirmBoxImage = confirmBoxImage;
    }
    public MessageConfirmViewModel(string message, string caption, ConfirmBoxButtons confirmBoxButtons, ConfirmBoxImage confirmBoxImage,
                                   MessageBoxResult confirmBoxResult) : this(message, caption, confirmBoxButtons, confirmBoxImage) {
      ConfirmBoxResult = confirmBoxResult;
    }
    #endregion

    #region Fileds
    private MessageConfirm _window = Application.Current.Windows.OfType<MessageConfirm>().FirstOrDefault();
    private string _titleImageUrl;
    private Visibility _titleImageVisibility;
    private string _message;
    private HorizontalAlignment _messageHorizontalAlignment;
    private string _caption;
    private ConfirmBoxButtons _confirmBoxButtons;
    private ConfirmBoxImage _confirmBoxImage;
    private MessageBoxResult _confirmBoxResult;
    private string _imageUrl;
    private Visibility _imageVisibility;
    private double _imageSize;
    private Visibility _okVisibility;
    private Visibility _cancelVisibility;
    private Visibility _yesVisibility;
    private Visibility _noVisibility;
    #endregion

    #region Properties
    public string TitleImageUrl {
      get { return _titleImageUrl; }
      set {
        this.MutateVerbose(ref _titleImageUrl, value, RaisePropertyChanged());
        if (string.IsNullOrEmpty(TitleImageUrl)) {
          TitleImageVisibility = Visibility.Collapsed;
        } else {
          TitleImageVisibility = Visibility.Visible;
        }
      }
    }
    public Visibility TitleImageVisibility {
      get { return _titleImageVisibility; }
      set { this.MutateVerbose(ref _titleImageVisibility, value, RaisePropertyChanged()); }
    }
    public string Message {
      get { return _message; }
      set { this.MutateVerbose(ref _message, value, RaisePropertyChanged()); }
    }
    public HorizontalAlignment MessageHorizontalAlignment {
      get { return _messageHorizontalAlignment; }
      set { this.MutateVerbose(ref _messageHorizontalAlignment, value, RaisePropertyChanged()); }
    }
    public string Caption {
      get { return _caption; }
      set { this.MutateVerbose(ref _caption, value, RaisePropertyChanged()); }
    }
    public ConfirmBoxButtons ConfirmBoxButtons {
      get { return _confirmBoxButtons; }
      set {
        this.MutateVerbose(ref _confirmBoxButtons, value, RaisePropertyChanged());
        CheckVisibilityButtons();
      }
    }
    public ConfirmBoxImage ConfirmBoxImage {
      get { return _confirmBoxImage; }
      set {
        this.MutateVerbose(ref _confirmBoxImage, value, RaisePropertyChanged());
        if (ConfirmBoxImage == ConfirmBoxImage.None) {
          ImageVisibility = Visibility.Collapsed;
          ImageSize = 0;
          MessageHorizontalAlignment = HorizontalAlignment.Center;
        } else {
          ImageSize = 32;
          MessageHorizontalAlignment = HorizontalAlignment.Left;
        }
        ImageUrl = GetImageUrl();
      }
    }
    public MessageBoxResult ConfirmBoxResult {
      get { return _confirmBoxResult; }
      set { this.MutateVerbose(ref _confirmBoxResult, value, RaisePropertyChanged()); }
    }
    public Visibility ImageVisibility {
      get { return _imageVisibility; }
      set { this.MutateVerbose(ref _imageVisibility, value, RaisePropertyChanged()); }
    }
    public double ImageSize {
      get { return _imageSize; }
      set { this.MutateVerbose(ref _imageSize, value, RaisePropertyChanged()); }
    }
    public string ImageUrl {
      get { return _imageUrl; }
      set { this.MutateVerbose(ref _imageUrl, value, RaisePropertyChanged()); }
    }
    public Visibility OkVisibility {
      get { return _okVisibility; }
      set { this.MutateVerbose(ref _okVisibility, value, RaisePropertyChanged()); }
    }
    public Visibility CancelVisibility {
      get { return _cancelVisibility; }
      set { this.MutateVerbose(ref _cancelVisibility, value, RaisePropertyChanged()); }
    }
    public Visibility YesVisibility {
      get { return _yesVisibility; }
      set { this.MutateVerbose(ref _yesVisibility, value, RaisePropertyChanged()); }
    }
    public Visibility NoVisibility {
      get { return _noVisibility; }
      set { this.MutateVerbose(ref _noVisibility, value, RaisePropertyChanged()); ; }
    }
    #endregion

    #region Commands
    public DelegateCommand OkCommand { get; }
    public DelegateCommand CancelCommand { get; }
    public DelegateCommand YesCommand { get; }
    public DelegateCommand NoCommand { get; }
    #endregion

    #region Commands implementation
    private void Ok() {
      _window.DialogResult = true;
    }
    private void Cancel() {
      _window.DialogResult = false;
    }
    private void Yes() {
      _window.DialogResult = true;
    }
    private void No() {
      _window.DialogResult = false;
    }
    #endregion

    #region Helpers
    private string GetImageUrl() {
      string tempUrl = null;
      if (ConfirmBoxImage == ConfirmBoxImage.Asterisk) {
        tempUrl = @"/Resourses/Regular/Png 32x32/asterisk-orange-icon.png";
      } else if (ConfirmBoxImage == ConfirmBoxImage.Error) {
        tempUrl = @"/Resourses/Regular/Png 32x32/Error.png";
      } else if (ConfirmBoxImage == ConfirmBoxImage.Exclamation) {
        tempUrl = @"/Resourses/Regular/Png 32x32/Badge Alt Exclamation.png";
      } else if (ConfirmBoxImage == ConfirmBoxImage.Hand) {
        tempUrl = @"/Resourses/Regular/Png 32x32/Symbol Stop.png";
      } else if (ConfirmBoxImage == ConfirmBoxImage.Information) {
        tempUrl = @"/Resourses/Regular/Png 32x32/Symbol Information 2.png";
      } else if (ConfirmBoxImage == ConfirmBoxImage.Question) {
        tempUrl = @"/Resourses/Regular/Png 32x32/Question.png";
      } else if (ConfirmBoxImage == ConfirmBoxImage.Stop) {
        tempUrl = @"/Resourses/Regular/Png 32x32/Symbol Restricted 2.png";
      } else if (ConfirmBoxImage == ConfirmBoxImage.Warning) { tempUrl = @"/Resourses/Regular/Png 32x32/Warning.png"; };
      return tempUrl;
    }
    private void CheckVisibilityButtons() {
      if (ConfirmBoxButtons == ConfirmBoxButtons.OK) {
        YesVisibility = Visibility.Collapsed;
        NoVisibility = Visibility.Collapsed;
        CancelVisibility = Visibility.Collapsed;
      } else if (ConfirmBoxButtons == ConfirmBoxButtons.OKCancel) {
        YesVisibility = Visibility.Collapsed;
        NoVisibility = Visibility.Collapsed;
      } else if (ConfirmBoxButtons == ConfirmBoxButtons.YesNo) {
        OkVisibility = Visibility.Collapsed;
        CancelVisibility = Visibility.Collapsed;
      }
    }
    #endregion
  }
}
