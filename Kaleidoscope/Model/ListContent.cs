using Kaleidoscope.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace Kaleidoscope.Model {
  public enum TypeChangePositionVideo { None, DispatcherTimer, Slider }
  public class ListContent : TableReference {

    #region Constructors
    public ListContent() {
    }
    public ListContent(int id,
                       string name,
                       int nPathway,
                       string sPathway,
                       int? nImageType,
                       string sImageType,
                       int? nVideoType,
                       string sVideoType,
                       int? nRefComment,
                       string sRefComment,
                       string description,
                       int? nProperty,
                       int? nRotate,
                       string fullName
                      ) {
      _instance = this;
      Id = id;
      Name = name;
      SPathway = sPathway;
      NPathway = nPathway;
      NImageType = nImageType;
      SImageType = sImageType;
      NVideoType = nVideoType;
      SVideoType = sVideoType;
      NRefComment = nRefComment;
      SRefComment = sRefComment;
      Description = description;
      NProperty = nProperty;
      NRotate = nRotate;
      FullName = fullName;
    }
    public ListContent(int id,
                       string name,
                       int nPathway,
                       string sPathway,
                       int? nImageType,
                       string sImageType,
                       int? nVideoType,
                       string sVideoType,
                       int? nRefComment,
                       string sRefComment,
                       string description,
                       int? nProperty,
                       int? nRotate,
                       string fullName,
                       BitmapImage imageBitmap
                      ) : this(id,
                               name,
                               nPathway,
                               sPathway,
                               nImageType,
                               sImageType,
                               nVideoType,
                               sVideoType,
                               nRefComment,
                               sRefComment,
                               description,
                               nProperty,
                               nRotate,
                               fullName
                              ) {
      ImageBitmap = imageBitmap;
    }
    #endregion

    #region Fields
    private ListContent _instance;
    private int _nPathway;
    private string _sPathway = string.Empty;
    private int? _nImageType;
    private string _sImageType = string.Empty;
    private int? _nVideoType;
    private string _sVideoType = string.Empty;
    private int? _nRefComment;
    private string _sRefComment = string.Empty;
    private string _description = string.Empty;
    private string _fullName;
    private int? _nProperty;
    private int? _nRotate;
    private BitmapImage _imageBitmap;
    private int _videoLengthSec;
    private string _videoLength = string.Empty;
    private int _naturalWidth;
    private int _naturalHeight;
    private string _shortDescription;
    private MediaState _loadedBehavior = MediaState.Stop;
    private bool _isPlay = false;
    private string _captionVideoPlace = string.Empty;
    private double _positionVideo = 0;
    private MediaElement _videoMediaElement;
    private bool _isPlayFromStateStop = false;
    private TypeChangePositionVideo _typeChangingPositionVideo = TypeChangePositionVideo.None;
    private DispatcherTimer _timerVideoDispatcher;
    private bool _sliderDragStarted;
    //
    private bool _isMarked = false;
    #endregion

    #region Properties
    public ListContent Instance => _instance;
    public DispatcherTimer TimerVideoDispatcher {
      get { return _timerVideoDispatcher; }
      set { _timerVideoDispatcher = value; }
    }
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
    public string FullName {
      get { return _fullName; }
      set { this.MutateVerbose(ref _fullName, value, RaisePropertyChanged()); }
    }
    public int? NProperty {
      get { return _nProperty; }
      set { this.MutateVerbose(ref _nProperty, value, RaisePropertyChanged()); }
    }
    public int? NRotate {
      get { return _nRotate; }
      set { this.MutateVerbose(ref _nRotate, value, RaisePropertyChanged()); }
    }
    public BitmapImage ImageBitmap {
      get { return _imageBitmap; }
      set { this.MutateVerbose(ref _imageBitmap, value, RaisePropertyChanged()); }
    }
    public int VideoLengthSec {
      get { return _videoLengthSec; }
      set {
        this.MutateVerbose(ref _videoLengthSec, value, RaisePropertyChanged());
        VideoLength = VideoLengthSec < 60 ? $"{VideoLengthSec} секунд" : $"{(VideoLengthSec / 60.0):N1} минут";
      }
    }
    public string VideoLength {
      get { return _videoLength; }
      set { this.MutateVerbose(ref _videoLength, value, RaisePropertyChanged()); }
    }
    public int NaturalWidth {
      get { return _naturalWidth; }
      set { this.MutateVerbose(ref _naturalWidth, value, RaisePropertyChanged()); }
    }
    public int NaturalHeight {
      get { return _naturalHeight; }
      set { this.MutateVerbose(ref _naturalHeight, value, RaisePropertyChanged()); }
    }
    public string ShortDescription {
      get { return _shortDescription; }
      set { this.MutateVerbose(ref _shortDescription, value, RaisePropertyChanged()); }
    }
    public MediaState LoadedBehavior {
      get { return _loadedBehavior; }
      set { this.MutateVerbose(ref _loadedBehavior, value, RaisePropertyChanged()); }
    }
    public bool IsPlay {
      get { return _isPlay; }
      set {
        this.MutateVerbose(ref _isPlay, value, RaisePropertyChanged());
        if (VideoMediaElement != null) {
          if (VideoMediaElement.LoadedBehavior == MediaState.Stop && IsPlay) {
            if (!TimerVideoDispatcher.IsEnabled) {
              TimerVideoDispatcher.Start();
            }
            IsPlayFromStateStop = true;
            VideoMediaElement.LoadedBehavior = MediaState.Manual;
            VideoMediaElement.Close();
            VideoMediaElement.Position = TimeSpan.Zero;
            VideoMediaElement.Play();
          } else if (VideoMediaElement.LoadedBehavior == MediaState.Manual) {
            if (IsPlay) {
              if (!TimerVideoDispatcher.IsEnabled) {
                TimerVideoDispatcher.Start();
              }
              VideoMediaElement.Play();
            } else if (!IsPlay) {
              if (TimerVideoDispatcher.IsEnabled) {
                TimerVideoDispatcher.Stop();
              }
              VideoMediaElement.Pause();
            }
          }
        }
      }
    }
    public string CaptionVideoPlace {
      get { return _captionVideoPlace; }
      set { this.MutateVerbose(ref _captionVideoPlace, value, RaisePropertyChanged()); }
    }
    public double PositionVideo {
      get { return _positionVideo; }
      set {
        this.MutateVerbose(ref _positionVideo, value, RaisePropertyChanged());
        if (TypeChangingPositionVideo == TypeChangePositionVideo.DispatcherTimer) {
          PositionVideoChangedEvent?.Invoke(PositionVideo);
        } else if (TypeChangingPositionVideo == TypeChangePositionVideo.Slider) {
          if (VideoMediaElement != null) {
            VideoMediaElement.Position = TimeSpan.FromSeconds(PositionVideo);
          }
        }
        CaptionVideoPlace = GetCaptionVideoPlace();
        if (VideoMediaElement.NaturalDuration.HasTimeSpan) {

        } else {
          CaptionVideoPlace = "0 / 0";
        }
      }
    }
    public MediaElement VideoMediaElement {
      get { return _videoMediaElement; }
      set {
        this.MutateVerbose(ref _videoMediaElement, value, RaisePropertyChanged());
        CaptionVideoPlace = GetCaptionVideoPlace();
      }
    }
    public bool IsPlayFromStateStop {
      get { return _isPlayFromStateStop; }
      set { this.MutateVerbose(ref _isPlayFromStateStop, value, RaisePropertyChanged()); }
    }
    public TypeChangePositionVideo TypeChangingPositionVideo {
      get { return _typeChangingPositionVideo; }
      set { this.MutateVerbose(ref _typeChangingPositionVideo, value, RaisePropertyChanged()); }
    }
    public bool SliderDragStarted {
      get { return _sliderDragStarted; }
      set { this.MutateVerbose(ref _sliderDragStarted, value, RaisePropertyChanged()); }
    }
    public bool IsMarked {
      get { return _isMarked; }
      set { this.MutateVerbose(ref _isMarked, value, RaisePropertyChanged()); }
    }
    #endregion

    #region Events
    public Action<double> PositionVideoChangedEvent;
    #endregion

    #region Methods
    private void TimerTickVideo(object sender, EventArgs e) {
      if (VideoMediaElement != null) {
        TypeChangingPositionVideo = TypeChangePositionVideo.DispatcherTimer;
        PositionVideo = VideoMediaElement.Position.TotalSeconds;
      }
    }
    public void CreateDispatcherTimer() {
      _timerVideoDispatcher = new DispatcherTimer(TimeSpan.FromMilliseconds(100),
                                                  DispatcherPriority.Normal,
                                                  new EventHandler(TimerTickVideo),
                                                  Dispatcher.CurrentDispatcher);
      _timerVideoDispatcher.Stop();
    }
    public string GetCaptionVideoPlace() {
      if (VideoMediaElement != null && VideoMediaElement.NaturalDuration.HasTimeSpan) {
        if (VideoMediaElement.NaturalDuration.TimeSpan.TotalSeconds < 3600) {
          // Duration of video less than one hour 
          return $"{Math.Floor(PositionVideo / 60):0}:" +
                 $"{PositionVideo % 60:00} / " +
                 $"{Math.Floor(VideoMediaElement.NaturalDuration.TimeSpan.TotalSeconds / 60):0}:" +
                 $"{VideoMediaElement.NaturalDuration.TimeSpan.TotalSeconds % 60:00}";
        } else {
          // Duration of video more than one hour
          return $"{Math.Floor(PositionVideo / 60):0}:" +
                 $"{PositionVideo % 60:00} / " +
                 $"{Math.Floor(VideoMediaElement.NaturalDuration.TimeSpan.TotalSeconds / 60):0}:" +
                 $"{VideoMediaElement.NaturalDuration.TimeSpan.TotalSeconds % 60:00}";
        }
      } else {
        return "0:00 / 0:00";
      }
    }
    #endregion

  }
}
