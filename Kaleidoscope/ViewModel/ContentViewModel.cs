using Kaleidoscope.Helpers;
using Kaleidoscope.Model;
using Kaleidoscope.View;
using Kaleidoscope.ViewDependencyProperty;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace Kaleidoscope.ViewModel {
  public enum ContentWorkMode { None, Main, Folders, Catalogs }
  public enum KindMediaContent { None, Images, Videos }
  public class ContentViewModel : ViewModelBase {
    public ContentViewModel() { }
    public ContentViewModel(ContentWorkMode workMode) {
      _workMode = workMode;

      // Init UserControl
      HeaderControl = new ContentHeader() {
        DataContext = new ContentHeaderViewModel(workMode,
                                                 o => CanExecutePageFirst(),
                                                 o => CanExecutePageStepBackward(),
                                                 o => CanExecutePageStepForward(),
                                                 o => CanExecutePageLast())
      };
      HeaderControlHorizontalAlignment = HorizontalAlignment.Left;
      _headerViewModel = HeaderControl.DataContext as ContentHeaderViewModel;

      // Init properties
      ContentViewDepProp.Instance.FoldersImageBorderWidth = HeaderViewModel.SelectedSizeWidthItem;
      ContentViewDepProp.Instance.FoldersImageBorderHeight = HeaderViewModel.SelectedSizeHeightItem;

      // Subscribe to events
      HeaderViewModel.ChangedWidthItemEvents += new Action<int>(ChangeWidthItem);
      HeaderViewModel.ChangedHeightItemEvents += new Action<int>(ChangeHeightItem);
      HeaderViewModel.ChangedIsEnableRefreshContentEvent += new Action<bool>(TraceStateEnableRefreshContent);
      HeaderViewModel.RepeatLoadingContentCommandEvent += new Action(RepeatLoadingContent);
      HeaderViewModel.ChangeKindContentEvent += new Action<KindMediaContent>(TraceKindMediaContent);
      HeaderViewModel.ChangedModeShowContentEvent += new Action<ModeShowContent>(TraceModeShowContent);
      HeaderViewModel.PageFirstEvent += new Action(FirstPage);
      HeaderViewModel.PageStepBackwardEvent += new Action(StepBackwardPage);
      HeaderViewModel.PageStepForwardEvent += new Action(StepForwardPage);
      HeaderViewModel.PageLastEvent += new Action(LastPage);
      SetPagesStateCaptions();

      // Initiate fields that subscribe to events
      _stateEnableRefreshContent = HeaderViewModel.IsEnableRefreshContent;
      CurrentKindMediaContent = HeaderViewModel.GetCurrentKindMediaContent();
      CurrentModeShowContent = HeaderViewModel.GetCurrentModeShowContent();
      InitSizeItem();

      // Init lists content
      ImageContents = new ObservableCollection<ListContent>();
      VideoContents = new ObservableCollection<ListContent>();

      //Init Commands
      RotateRightCommand = new DelegateCommand(RotateRight);
      RotateLeftCommand = new DelegateCommand(RotateLeft);
      ImageMarkCommand = new DelegateCommand(ImageMark);
      ImagesListMouseDoubleClickCommand = new DelegateCommand(ImagesListMouseDoubleClick);
      ImagesListSelectionChangedCommand = new DelegateCommand(ImagesListSelectionChanged);
      MarkAllImageListCommand = new DelegateCommand(o => MarkAllImageList());
      CancelAllMarkImageListCommand = new DelegateCommand(o => CancelAllMarkImageList());
      InvertMarkImageListCommand = new DelegateCommand(o => InvertMarkImageList());
      RotateRightImageListMarksCommand = new DelegateCommand(o => RotateRightImageListMarks());
      RotateLeftImageListMarksCommand = new DelegateCommand(o => RotateLeftImageListMarks());
      VideoItemOneFromListOpenedCommand = new DelegateCommand(VideoItemOneFromListOpened);
      VideoItemOneFromListEndedCommand = new DelegateCommand(VideoItemOneFromListEnded);
      VideoItemOneFromListMouseLeftButtonDownCommand = new DelegateCommand(VideoItemOneFromListMouseLeftButtonDown);
      SliderOneFromListThumbDragStartedCommand = new DelegateCommand(SliderOneFromListThumbDragStarted);
      SliderOneFromListThumbDragCompletedCommand = new DelegateCommand(SliderOneFromListThumbDragCompleted);

      // All is Ready
        _isInitialization = true;
      SetMultimedia();
    }

    #region Fields
    private BackgroundWorker _worker;
    private int _workerCurrentHashCode;
    private bool _isInitialization = false;
    private bool _stateEnableRefreshContent;
    private ContentWorkMode _workMode = ContentWorkMode.None;
    private ConditionShowContent _conditionShow;
    private ContentHeaderViewModel _headerViewModel;
    private ObservableCollection<ListContent> _imageContents;
    private ObservableCollection<ListContent> _videoContents;
    private ListContent _contentItem;
    private Visibility _panelProgressVisibility = Visibility.Collapsed;
    private bool _isIndeterminateProgress = true;
    private int _itemsCount = 100;
    private int _valueProgress = 0;
    private string _progressCaption = string.Empty;
    private IOrderedEnumerable<VUrlContent> _query;
    private ManagerMediaPages _managerImagePages = new ManagerMediaPages(0, 0);
    private ManagerMediaPages _managerVideoPages = new ManagerMediaPages(0, 0);
    private KindMediaContent _currentKindMediaContent = KindMediaContent.None;
    private ModeShowContent _currentModeShowContent = ModeShowContent.None;
    private bool _isMoveToPage = false;
    private Visibility _panelImagesVisibility = Visibility.Collapsed;
    private Visibility _panelImageModeShowListVisibility = Visibility.Collapsed;
    private Visibility _panelImageModeShowOneFromListVisibility = Visibility.Collapsed;
    private Visibility _panelVideosVisibility = Visibility.Collapsed;
    private Visibility _panelVideoModeShowListVisibility = Visibility.Collapsed;
    private Visibility _panelVideoModeShowOneFromListVisibility = Visibility.Collapsed;
    private string _panelImagesHeight = "*";
    private string _panelVideoHeight = "*";
    private string _panelImagesModeShowListHeight = "*";
    private string _panelImagesModeShowOneFromListHeight = "*";
    private string _panelVideoModeShowListHeight = "*";
    private string _panelVideoModeShowOneFromListHeight = "*";
    private int _selectedIndexImageOneToList;
    private int _widthImageOneFromList;
    private int _heightImageOneFromList;
    private int _widthVideoOneFromList;
    private int _heightVideoOneFromList;
    private bool _isMarkingImage = false;
    private bool _isFullScreen = false;
    private IList _selectedImagesList;
    private ListContent _currentImageList;
    private int _countMarkedImagesList;
    private int _selectedIndexVideoOneToList = -1;
    private double _sliderPositionVideoOneToList;
    #endregion

    #region Properties
    public ContentWorkMode WorkMode => _workMode;
    public ContentHeaderViewModel HeaderViewModel => _headerViewModel;
    public bool StateEnableRefreshContent => _stateEnableRefreshContent;
    public bool IsFullScreen => _isFullScreen;
    public KindMediaContent CurrentKindMediaContent {
      get { return _currentKindMediaContent; }
      private set {
        _currentKindMediaContent = value;
        if (CurrentKindMediaContent == KindMediaContent.Images) {
          PanelVideosVisibility = Visibility.Collapsed;
          PanelVideoHeight = "Auto";
          PanelImagesVisibility = Visibility.Visible;
          PanelImagesHeight = "*";
        } else if (CurrentKindMediaContent == KindMediaContent.Videos) {
          PanelImagesVisibility = Visibility.Collapsed;
          PanelImagesHeight = "Auto";
          PanelVideosVisibility = Visibility.Visible;
          PanelVideoHeight = "*";
        } else {
          PanelImagesVisibility = Visibility.Collapsed;
          PanelImagesHeight = "Auto";
          PanelVideosVisibility = Visibility.Collapsed;
          PanelVideoHeight = "Auto";
        }
      }
    }
    public ModeShowContent CurrentModeShowContent {
      get { return _currentModeShowContent; }
      set {
        _currentModeShowContent = value;
        if (CurrentKindMediaContent == KindMediaContent.Images) {
          if (CurrentModeShowContent == ModeShowContent.List) {
            PanelImageModeShowOneFromListVisibility = Visibility.Collapsed;
            PanelImagesModeShowOneFromListHeight = "Auto";
            PanelImageModeShowListVisibility = Visibility.Visible;
            PanelImagesModeShowListHeight = "*";
          } else if (CurrentModeShowContent == ModeShowContent.OneFromList) {
            PanelImageModeShowListVisibility = Visibility.Collapsed;
            PanelImagesModeShowListHeight = "Auto";
            PanelImageModeShowOneFromListVisibility = Visibility.Visible;
            PanelImagesModeShowOneFromListHeight = "*";
          } else {
            PanelImageModeShowOneFromListVisibility = Visibility.Collapsed;
            PanelImageModeShowListVisibility = Visibility.Collapsed;
            PanelImagesModeShowListHeight = "Auto";
            PanelImagesModeShowOneFromListHeight = "Auto";
          }
        } else if (CurrentKindMediaContent == KindMediaContent.Videos) {
          if (CurrentModeShowContent == ModeShowContent.List) {
            PanelVideoModeShowOneFromListVisibility = Visibility.Collapsed;
            PanelVideoModeShowOneFromListHeight = "Auto";
            PanelVideoModeShowListVisibility = Visibility.Visible;
            PanelVideoModeShowListHeight = "*";
          } else if (CurrentModeShowContent == ModeShowContent.OneFromList) {
            PanelVideoModeShowListVisibility = Visibility.Collapsed;
            PanelVideoModeShowListHeight = "Auto";
            PanelVideoModeShowOneFromListVisibility = Visibility.Visible;
            PanelVideoModeShowOneFromListHeight = "*";
          } else {
            PanelVideoModeShowOneFromListVisibility = Visibility.Collapsed;
            PanelVideoModeShowOneFromListHeight = "Auto";
            PanelVideoModeShowListVisibility = Visibility.Collapsed;
            PanelVideoModeShowListHeight = "Auto";
          }
        } else {

        }
      }
    }
    public Visibility PanelImagesVisibility {
      get { return _panelImagesVisibility; }
      set { this.MutateVerbose(ref _panelImagesVisibility, value, RaisePropertyChanged()); }
    }
    public Visibility PanelImageModeShowListVisibility {
      get { return _panelImageModeShowListVisibility; }
      set { this.MutateVerbose(ref _panelImageModeShowListVisibility, value, RaisePropertyChanged()); }
    }
    public Visibility PanelImageModeShowOneFromListVisibility {
      get { return _panelImageModeShowOneFromListVisibility; }
      set { this.MutateVerbose(ref _panelImageModeShowOneFromListVisibility, value, RaisePropertyChanged()); }
    }
    public Visibility PanelVideosVisibility {
      get { return _panelVideosVisibility; }
      set { this.MutateVerbose(ref _panelVideosVisibility, value, RaisePropertyChanged()); }
    }
    public Visibility PanelVideoModeShowListVisibility {
      get { return _panelVideoModeShowListVisibility; }
      set { this.MutateVerbose(ref _panelVideoModeShowListVisibility, value, RaisePropertyChanged()); }
    }
    public Visibility PanelVideoModeShowOneFromListVisibility {
      get { return _panelVideoModeShowOneFromListVisibility; }
      set { this.MutateVerbose(ref _panelVideoModeShowOneFromListVisibility, value, RaisePropertyChanged()); }
    }
    public string PanelImagesHeight {
      get { return _panelImagesHeight; }
      set { this.MutateVerbose(ref _panelImagesHeight, value, RaisePropertyChanged()); }
    }
    public string PanelVideoHeight {
      get { return _panelVideoHeight; }
      set { this.MutateVerbose(ref _panelVideoHeight, value, RaisePropertyChanged()); }
    }
    public string PanelImagesModeShowListHeight {
      get { return _panelImagesModeShowListHeight; }
      set { this.MutateVerbose(ref _panelImagesModeShowListHeight, value, RaisePropertyChanged()); }
    }
    public string PanelImagesModeShowOneFromListHeight {
      get { return _panelImagesModeShowOneFromListHeight; }
      set { this.MutateVerbose(ref _panelImagesModeShowOneFromListHeight, value, RaisePropertyChanged()); }
    }
    public string PanelVideoModeShowListHeight {
      get { return _panelVideoModeShowListHeight; }
      set { this.MutateVerbose(ref _panelVideoModeShowListHeight, value, RaisePropertyChanged()); }
    }
    public string PanelVideoModeShowOneFromListHeight {
      get { return _panelVideoModeShowOneFromListHeight; }
      set { this.MutateVerbose(ref _panelVideoModeShowOneFromListHeight, value, RaisePropertyChanged()); }
    }
    public int SelectedIndexImageOneToList {
      get { return _selectedIndexImageOneToList; }
      set {
        this.MutateVerbose(ref _selectedIndexImageOneToList, value, RaisePropertyChanged());
      }
    }
    public ConditionShowContent ConditionShow {
      get { return _conditionShow; }
      set {
        _conditionShow = value;
        _isMoveToPage = false;
        SetMultimedia();
      }
    }
    public ObservableCollection<ListContent> ImageContents {
      get { return _imageContents; }
      set { this.MutateVerbose(ref _imageContents, value, RaisePropertyChanged()); }
    }
    public ObservableCollection<ListContent> VideoContents {
      get { return _videoContents; }
      set { this.MutateVerbose(ref _videoContents, value, RaisePropertyChanged()); }
    }
    public Visibility PanelProgressVisibility {
      get { return _panelProgressVisibility; }
      set { this.MutateVerbose(ref _panelProgressVisibility, value, RaisePropertyChanged()); }
    }
    public bool IsIndeterminateProgress {
      get { return _isIndeterminateProgress; }
      set { this.MutateVerbose(ref _isIndeterminateProgress, value, RaisePropertyChanged()); }
    }
    public int ItemsCount {
      get { return _itemsCount; }
      set { this.MutateVerbose(ref _itemsCount, value, RaisePropertyChanged()); }
    }
    public int ValueProgress {
      get { return _valueProgress; }
      set { this.MutateVerbose(ref _valueProgress, value, RaisePropertyChanged()); }
    }
    public string ProgressCaption {
      get { return _progressCaption; }
      set { this.MutateVerbose(ref _progressCaption, value, RaisePropertyChanged()); }
    }
    public int WidthImageOneFromList {
      get { return _widthImageOneFromList; }
      set { this.MutateVerbose(ref _widthImageOneFromList, value, RaisePropertyChanged()); }
    }
    public int HeightImageOneFromList {
      get { return _heightImageOneFromList; }
      set { this.MutateVerbose(ref _heightImageOneFromList, value, RaisePropertyChanged()); }
    }
    public int WidthVideoOneFromList {
      get { return _widthVideoOneFromList; }
      set { this.MutateVerbose(ref _widthVideoOneFromList, value, RaisePropertyChanged()); }
    }
    public int HeightVideoOneFromList {
      get { return _heightVideoOneFromList; }
      set { this.MutateVerbose(ref _heightVideoOneFromList, value, RaisePropertyChanged()); }
    }

    public bool IsMarkingImage {
      get { return _isMarkingImage; }
      set { this.MutateVerbose(ref _isMarkingImage, value, RaisePropertyChanged()); }
    }
    public ListContent CurrentImageList {
      get { return _currentImageList; }
      set {
        _currentImageList = value;
        if (CurrentImageList != null && IsMarkingImage) {
          CurrentImageList.IsMarked = !CurrentImageList.IsMarked;
          IsMarkingImage = CheckIsMarkedImageList();
        }
      }
    }
    public IList SelectedImagesList {
      get { return _selectedImagesList; }
      set {
        _selectedImagesList = value;
        if (SelectedImagesList.Count == 1) {
          CurrentImageList = SelectedImagesList[0] as ListContent;
        } else {
          CurrentImageList = null;
        }
      }
    }
    public int CountMarkedImagesList {
      get { return _countMarkedImagesList; }
      set { this.MutateVerbose(ref _countMarkedImagesList, value, RaisePropertyChanged()); }
    }
    public int SelectedIndexVideoOneToList {
      get { return _selectedIndexVideoOneToList; }
      set {
        int oldIndex = _selectedIndexVideoOneToList;
        this.MutateVerbose(ref _selectedIndexVideoOneToList, value, RaisePropertyChanged());
        if (oldIndex >= 0
            && oldIndex < VideoContents.Count
            && SelectedIndexVideoOneToList >= 0
            && oldIndex != SelectedIndexVideoOneToList) {
          if (VideoContents[oldIndex].TimerVideoDispatcher.IsEnabled) {
            VideoContents[oldIndex].TimerVideoDispatcher.Stop();
          }
          VideoContents[oldIndex].LoadedBehavior = MediaState.Stop;
          VideoContents[oldIndex].VideoMediaElement = null;
          VideoContents[oldIndex].PositionVideo = 0;
          VideoContents[oldIndex].IsPlay = false;
        }
        if (SelectedIndexVideoOneToList >= 0 && SelectedIndexVideoOneToList < VideoContents.Count && oldIndex != SelectedIndexVideoOneToList) {
          VideoContents[SelectedIndexVideoOneToList].LoadedBehavior = MediaState.Stop;
          VideoContents[SelectedIndexVideoOneToList].IsPlay = false;
          SliderPositionVideoOneToList = 0;
        }
      }
    }
    public double SliderPositionVideoOneToList {
      get { return _sliderPositionVideoOneToList; }
      set {
        this.MutateVerbose(ref _sliderPositionVideoOneToList, value, RaisePropertyChanged());
      }
    }
    #endregion

    #region Commands
    public DelegateCommand RotateRightCommand { get; set; }
    public DelegateCommand RotateLeftCommand { get; set; }
    public DelegateCommand ImageMarkCommand { get; set; }
    public DelegateCommand ImagesListMouseDoubleClickCommand { get; set; }
    public DelegateCommand ImagesListSelectionChangedCommand { get; set; }
    public DelegateCommand MarkAllImageListCommand { get; set; }
    public DelegateCommand CancelAllMarkImageListCommand { get; set; }
    public DelegateCommand InvertMarkImageListCommand { get; set; }
    public DelegateCommand RotateRightImageListMarksCommand { get; set; }
    public DelegateCommand RotateLeftImageListMarksCommand { get; set; }
    public DelegateCommand VideoItemOneFromListOpenedCommand { get; set; }
    public DelegateCommand VideoItemOneFromListEndedCommand { get; set; }
    public DelegateCommand VideoItemOneFromListMouseLeftButtonDownCommand { get; set; }
    public DelegateCommand SliderOneFromListThumbDragStartedCommand { get; set; }
    public DelegateCommand SliderOneFromListThumbDragCompletedCommand { get; set; }
    #endregion

    #region Implementation of subscriptions on events
    private void TraceStateEnableRefreshContent(bool state) {
      _stateEnableRefreshContent = state;
      if (!StateEnableRefreshContent) {
        ClearContents();
      }
    }
    private void TraceKindMediaContent(KindMediaContent kind) {
      CurrentKindMediaContent = kind;
    }
    private void TraceModeShowContent(ModeShowContent mode) {
      CurrentModeShowContent = mode;
    }
    private void FirstPage() {
      if (CurrentKindMediaContent == KindMediaContent.Images) {
        _managerImagePages.FirstPage();
        MoveToPage();
      } else if (CurrentKindMediaContent == KindMediaContent.Videos) {
        _managerVideoPages.FirstPage();
        MoveToPage();
      }
    }
    private void StepBackwardPage() {
      if (CurrentKindMediaContent == KindMediaContent.Images) {
        _managerImagePages.StepBackwardPage();
        MoveToPage();
      } else if (CurrentKindMediaContent == KindMediaContent.Videos) {
        _managerVideoPages.StepBackwardPage();
        MoveToPage();
      }
    }
    private void StepForwardPage() {
      if (CurrentKindMediaContent == KindMediaContent.Images) {
        _managerImagePages.StepForwardPage();
        MoveToPage();
      } else if (CurrentKindMediaContent == KindMediaContent.Videos) {
        _managerVideoPages.StepForwardPage();
        MoveToPage();
      }
    }
    private void LastPage() {
      if (CurrentKindMediaContent == KindMediaContent.Images) {
        _managerImagePages.LastPage();
        MoveToPage();
      } else if (CurrentKindMediaContent == KindMediaContent.Videos) {
        _managerVideoPages.LastPage();
        MoveToPage();
      }
    }
    private void SetPagesStateCaptions() {
      HeaderViewModel.PagesManagerCurrentStateCaption = CurrentKindMediaContent == KindMediaContent.Images ? _managerImagePages.CurrentStateCaption() :
                                                        CurrentKindMediaContent == KindMediaContent.Videos ? _managerVideoPages.CurrentStateCaption() :
                                                        string.Empty;
    }
    private void TraceSliderPositionVideoOneFromList(double totalSconds) {
      SliderPositionVideoOneToList = totalSconds;
    }
    #endregion

    #region Commands implementation
    private void RotateRight(object param) {
      ListContent item = param as ListContent;
      if (item != null) {
        ImageRotateRight(item);
      }
    }
    private void RotateLeft(object param) {
      ListContent item = param as ListContent;
      if (item != null) {
        ImageRotateLeft(item);
      }
    }
    private void ImageMark(object param) {
      if (param is ListContent item) {
        item.IsMarked = !item.IsMarked;
        IsMarkingImage = CheckIsMarkedImageList();
      }
    }
    private void MarkAllImageList() {
      foreach (var item in ImageContents) {
        item.IsMarked = true;
      }
      IsMarkingImage = CheckIsMarkedImageList();
    }
    private void CancelAllMarkImageList() {
      foreach (var item in ImageContents) {
        item.IsMarked = false;
      }
      IsMarkingImage = CheckIsMarkedImageList();
    }
    private void InvertMarkImageList() {
      foreach (var item in ImageContents) {
        item.IsMarked = !item.IsMarked;
      }
      IsMarkingImage = CheckIsMarkedImageList();
    }
    private void RotateRightImageListMarks() {
      foreach (var item in from list in ImageContents where list.IsMarked == true select list) {
        ImageRotateRight(item);
        item.IsMarked = false;
      }
      IsMarkingImage = CheckIsMarkedImageList();
    }
    private void RotateLeftImageListMarks() {
      foreach (var item in from list in ImageContents where list.IsMarked == true select list) {
        ImageRotateLeft(item);
        item.IsMarked = false;
      }
      IsMarkingImage = CheckIsMarkedImageList();
    }
    private bool CanExecutePageFirst() {
      if (CurrentKindMediaContent == KindMediaContent.Images) {
        return _managerImagePages.CanFirstPage();
      } else if (CurrentKindMediaContent == KindMediaContent.Videos) {
        return _managerVideoPages.CanFirstPage();
      }
      return false;
    }
    private bool CanExecutePageStepBackward() {
      if (CurrentKindMediaContent == KindMediaContent.Images) {
        return _managerImagePages.CanStepBackwardPage();
      } else if (CurrentKindMediaContent == KindMediaContent.Videos) {
        return _managerVideoPages.CanStepBackwardPage();
      }
      return false;
    }
    private bool CanExecutePageStepForward() {
      if (CurrentKindMediaContent == KindMediaContent.Images) {
        return _managerImagePages.CanStepForwardPage();
      } else if (CurrentKindMediaContent == KindMediaContent.Videos) {
        return _managerVideoPages.CanStepForwardPage();
      }
      return false;
    }
    private bool CanExecutePageLast() {
      if (CurrentKindMediaContent == KindMediaContent.Images) {
        return _managerImagePages.CanLastPage();
      } else if (CurrentKindMediaContent == KindMediaContent.Videos) {
        return _managerVideoPages.CanLastPage();
      }
      return false;
    }
    private void ImagesListSelectionChanged(object param) {
      if (param is IList item && item != null) {
        SelectedImagesList = param as IList;
      }
    }
    private void ImagesListMouseDoubleClick(object param) {
      if (param is ListContent item && item != null && IsMarkingImage) {
        item.IsMarked = !item.IsMarked;
        IsMarkingImage = CheckIsMarkedImageList();
      }
    }
    private void VideoItemOneFromListOpened(object param) {
      if (param is MediaElement videoItem) {
        if (!VideoContents[SelectedIndexVideoOneToList].IsPlayFromStateStop) {
          videoItem.IsMuted = true;
          if (VideoContents[SelectedIndexVideoOneToList].TimerVideoDispatcher.IsEnabled) {
            VideoContents[SelectedIndexVideoOneToList].TimerVideoDispatcher.Stop();
          }
          VideoContents[SelectedIndexVideoOneToList].VideoMediaElement = videoItem;
          VideoContents[SelectedIndexVideoOneToList].LoadedBehavior = MediaState.Stop;
          VideoContents[SelectedIndexVideoOneToList].IsPlay = false;
          videoItem.IsMuted = false;
          SliderPositionVideoOneToList = 0;
          if (videoItem.NaturalDuration.HasTimeSpan) {
            VideoContents[SelectedIndexVideoOneToList].VideoLengthSec = (int)videoItem.NaturalDuration.TimeSpan.TotalSeconds;
          }
        } else {
          VideoContents[SelectedIndexVideoOneToList].IsPlayFromStateStop = !VideoContents[SelectedIndexVideoOneToList].IsPlayFromStateStop;
        }
      }
    }
    private void VideoItemOneFromListEnded(object param) {
      if (param is MediaElement videoItem) {
        videoItem.Stop();
        VideoContents[SelectedIndexVideoOneToList].PositionVideo = 0;
        VideoContents[SelectedIndexVideoOneToList].LoadedBehavior = MediaState.Stop;
        VideoContents[SelectedIndexVideoOneToList].IsPlay = false;
        SliderPositionVideoOneToList = 0;
      }
    }
    private void VideoItemOneFromListMouseLeftButtonDown(object param) {
      if (param is MediaElement videoItem) {
        if (SelectedIndexVideoOneToList >= 0 && SelectedIndexVideoOneToList < VideoContents.Count) {
          VideoContents[SelectedIndexVideoOneToList].IsPlay = !VideoContents[SelectedIndexVideoOneToList].IsPlay;
        }
      }
    }
    private void SliderOneFromListThumbDragStarted(object param) {
      if (SelectedIndexVideoOneToList >= 0 && SelectedIndexVideoOneToList < VideoContents.Count) {
        VideoContents[SelectedIndexVideoOneToList].SliderDragStarted = true;
      }
    }
    private void SliderOneFromListThumbDragCompleted(object param) {
      if (SelectedIndexVideoOneToList >= 0 && SelectedIndexVideoOneToList < VideoContents.Count) {
        VideoContents[SelectedIndexVideoOneToList].SliderDragStarted = false;
      }
    }
    #endregion

    #region Helpers
    private void InitSizeItem() {
      if (CurrentKindMediaContent == KindMediaContent.Images && CurrentModeShowContent == ModeShowContent.List) {
        ContentViewDepProp.Instance.FoldersImageBorderWidth = HeaderViewModel.SelectedSizeWidthItem;
        ContentViewDepProp.Instance.FoldersImageBorderHeight = HeaderViewModel.SelectedSizeHeightItem;
      } else if (CurrentKindMediaContent == KindMediaContent.Images && CurrentModeShowContent == ModeShowContent.OneFromList) {
        WidthImageOneFromList = HeaderViewModel.SelectedSizeWidthItem;
        HeightImageOneFromList = HeaderViewModel.SelectedSizeHeightItem;
      } else if (CurrentKindMediaContent == KindMediaContent.Videos && CurrentModeShowContent == ModeShowContent.List) {

      } else if (CurrentKindMediaContent == KindMediaContent.Videos && CurrentModeShowContent == ModeShowContent.OneFromList) {
        WidthVideoOneFromList = HeaderViewModel.SelectedSizeWidthItem;
        HeightVideoOneFromList = HeaderViewModel.SelectedSizeHeightItem;
      }
    }
    private void ChangeWidthItem(int size) {
      if (CurrentKindMediaContent == KindMediaContent.Images && CurrentModeShowContent == ModeShowContent.List) {
        ContentViewDepProp.Instance.FoldersImageBorderWidth = size;
      } else if (CurrentKindMediaContent == KindMediaContent.Images && CurrentModeShowContent == ModeShowContent.OneFromList) {
        WidthImageOneFromList = size;
      } else if (CurrentKindMediaContent == KindMediaContent.Videos && CurrentModeShowContent == ModeShowContent.List) {

      } else if (CurrentKindMediaContent == KindMediaContent.Videos && CurrentModeShowContent == ModeShowContent.OneFromList) {
        WidthVideoOneFromList = size;
      }
    }
    private void ChangeHeightItem(int size) {
      if (CurrentKindMediaContent == KindMediaContent.Images && CurrentModeShowContent == ModeShowContent.List) {
        ContentViewDepProp.Instance.FoldersImageBorderHeight = size;
      } else if (CurrentKindMediaContent == KindMediaContent.Images && CurrentModeShowContent == ModeShowContent.OneFromList) {
        HeightImageOneFromList = size;
      } else if (CurrentKindMediaContent == KindMediaContent.Videos && CurrentModeShowContent == ModeShowContent.List) {

      } else if (CurrentKindMediaContent == KindMediaContent.Videos && CurrentModeShowContent == ModeShowContent.OneFromList) {
        HeightVideoOneFromList = size;
      }
    }
    private void RepeatLoadingContent() {
      _isMoveToPage = false;
      SetMultimedia();
    }
    private void MoveToPage() {
      _isMoveToPage = true;
      SetPagesStateCaptions();
      SetMultimedia();
    }
    private void SetMultimedia() {
      if (_isInitialization) {
        if (ConditionShow != null && StateEnableRefreshContent) {
          if (_worker != null) {
            if (_worker.WorkerSupportsCancellation) {
              _worker.CancelAsync();
            }
            _worker.Dispose();
            _worker = null;
            GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced, true);
          }
          _worker = new BackgroundWorker();
          _workerCurrentHashCode = _worker.GetHashCode();
          CreateListContents();
        }
      }
    }
    private void CreateListContents() {
      _query = null;
      IsMarkingImage = false;
      // Init properties
      ItemsCount = 100;
      ValueProgress = 0;
      ProgressCaption = string.Empty;
      IsIndeterminateProgress = true;
      PanelProgressVisibility = Visibility.Visible;
      // Get multimedia content
      if (WorkMode == ContentWorkMode.Folders && ConditionShow.Follders != null) {
        _query = from content in Settings.Instance.ContentDB.Database.SqlQuery<VUrlContent>("Select * FROM VUrlContents")
                 where ConditionShow.Follders.Contains<int>(content.NPathway)
                   && (!_isMoveToPage
                       ||
                       (_isMoveToPage &&
                        (
                         (CurrentKindMediaContent == KindMediaContent.Images &&
                          content.NImageType != null
                         )
                         ||
                         (CurrentKindMediaContent == KindMediaContent.Videos &&
                          content.NVideoType != null
                         )
                        )
                       )
                      )
                 orderby content.NPathway, content.Name
                 select content;
      }
      ClearContents();
      if (_query?.Count() > 0) {
        ItemsCount = _query.Count();
        ProgressCaption = GetProgressCaption();
        IsIndeterminateProgress = false;
        // Создаем менеджеры страниц
        if (!_isMoveToPage) {
          _managerImagePages = new ManagerMediaPages(
                                                     (
                                                      from content in Settings.Instance.ContentDB.Database.SqlQuery<VUrlContent>("Select * FROM VUrlContents")
                                                      where ConditionShow.Follders.Contains<int>(content.NPathway) && content.NImageType != null
                                                      select content
                                                     ).Count(),
                                                     Settings.Instance.GetSizePageImageLoad(WorkMode)
                                                    );
          _managerVideoPages = new ManagerMediaPages(
                                                     (
                                                      from content in Settings.Instance.ContentDB.Database.SqlQuery<VUrlContent>("Select * FROM VUrlContents")
                                                      where ConditionShow.Follders.Contains<int>(content.NPathway) && content.NVideoType != null
                                                      select content
                                                     ).Count(),
                                                     Settings.Instance.GetSizePageVideoLoad(WorkMode)
                                                    );
        }
        SetPagesStateCaptions();
      } else {
        _managerImagePages = new ManagerMediaPages(0, 0);
        _managerVideoPages = new ManagerMediaPages(0, 0);
        SetPagesStateCaptions();
        PanelProgressVisibility = Visibility.Collapsed;
        return;
      }
      // BackgroundWorker
      _worker.DoWork += new DoWorkEventHandler(Worker_DoWork);
      _worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(Worker_RunWorkerCompleted);
      _worker.ProgressChanged += new ProgressChangedEventHandler(Worker_ProgressChanged);
      _worker.WorkerReportsProgress = true;
      _worker.WorkerSupportsCancellation = true;
      _worker.RunWorkerAsync();
    }

    private void Worker_DoWork(object sender, DoWorkEventArgs e) {
      int normalizedSizeImage = Settings.Instance.GetNormalizedImageWidth(WorkMode);
      int workerCurrentHashCode = _workerCurrentHashCode;
      int iProgress = 0;
      int numberImage = 0;
      int numberVideo = 0;
      ObservableCollection<ListContent> imageContentsNew = new ObservableCollection<ListContent>();
      ObservableCollection<ListContent> videoContentsNew = new ObservableCollection<ListContent>();
      foreach (var item in _query) {
        if (_worker.CancellationPending) {
          e.Cancel = true;
          return;
        }
        iProgress++;
        // Загружаем выбранную страницу текущего вида контента, остальные пропускаем
        if (_isMoveToPage && GetIndexFirstItemPage() != 0 && GetIndexLastItemPage() != 0 &&
            (iProgress < GetIndexFirstItemPage() || iProgress > GetIndexLastItemPage())) {
          _worker.ReportProgress(iProgress, null);
          continue;
        }
        // Первичная загрузка контента по указанному условию
        // Для каждого вида контента контролируем загрузку 1-й страницы
        else if (!_isMoveToPage) {
          if (item.NImageType != null && item.NImageType > 0) {
            numberImage++;
            if (numberImage < _managerImagePages.IndexFirstItemPage || numberImage > _managerImagePages.IndexLastItemPage) {
              _worker.ReportProgress(iProgress, null);
              continue;
            }
          } else if (item.NVideoType != null && item.NVideoType > 0) {
            numberVideo++;
            if (numberVideo < _managerVideoPages.IndexFirstItemPage || numberVideo > _managerVideoPages.IndexLastItemPage) {
              _worker.ReportProgress(iProgress, null);
              continue;
            }
          } else {
            _worker.ReportProgress(iProgress, null);
            continue;
          }
        }
        _contentItem = new ListContent(item.Id,
                                       item.Name,
                                       item.NPathway,
                                       item.SPathway,
                                       item.NImageType,
                                       item.SImageType,
                                       item.NVideoType,
                                       item.SVideoType,
                                       item.NRefComment,
                                       item.SRefComment,
                                       item.Description,
                                       item.NProperty,
                                       item.NRotate,
                                       item.SPathway + @"\" + item.Name + "." + (string.IsNullOrEmpty(item.SImageType) ? item.SVideoType : item.SImageType)
                                      );
        if (item.NImageType != null && item.NImageType > 0) {

          _contentItem.ImageBitmap = new BitmapImage();
          var stream = File.OpenRead(_contentItem.FullName);
          _contentItem.ImageBitmap.BeginInit();
          _contentItem.ImageBitmap.CacheOption = BitmapCacheOption.OnLoad;
          _contentItem.ImageBitmap.DecodePixelWidth = normalizedSizeImage;
          _contentItem.ImageBitmap.StreamSource = stream;
          //contentItem.ImageBitmap.UriSource = new Uri(contentItem.FullName, UriKind.RelativeOrAbsolute);
          _contentItem.ImageBitmap.EndInit();
          stream.Close();
          stream.Dispose();
          _contentItem.ImageBitmap.Freeze();
          imageContentsNew.Add(_contentItem);

        } else if (item.NVideoType != null && item.NVideoType > 0) {

          MediaPlayer mediaPlayer = new MediaPlayer() { Volume = 0, ScrubbingEnabled = true };
          mediaPlayer.Open(new Uri(_contentItem.FullName, UriKind.RelativeOrAbsolute));
          mediaPlayer.Pause();
          mediaPlayer.Position = TimeSpan.FromSeconds(5);
          Thread.Sleep(1 * 1500);
          int imageWidh = Settings.Instance.GetNormalizedImageWidth(WorkMode);
          int imageHeight = (int)(imageWidh * ((mediaPlayer.NaturalVideoHeight * 1.0) / (mediaPlayer.NaturalVideoWidth * 1.0)));
          _contentItem.NaturalWidth = mediaPlayer.NaturalVideoWidth;
          _contentItem.NaturalHeight = mediaPlayer.NaturalVideoHeight;
          _contentItem.ShortDescription = $"{_contentItem.Name}.{_contentItem.SVideoType}\n{_contentItem.NaturalWidth}x{_contentItem.NaturalHeight}";

          DrawingVisual drawingVisual = new DrawingVisual();
          DrawingContext drawingContext = drawingVisual.RenderOpen();
          drawingContext.DrawVideo(mediaPlayer, new Rect(0, 0, imageWidh, imageHeight));
          drawingContext.Close();

          RenderTargetBitmap bmp = new RenderTargetBitmap(imageWidh, imageHeight, 96, 96, PixelFormats.Pbgra32);
          bmp.Render(drawingVisual);
          Duration duration = mediaPlayer.NaturalDuration;
          if (duration.HasTimeSpan) {
            _contentItem.VideoLengthSec = (int)duration.TimeSpan.TotalSeconds;
          }
          BitmapFrame frame = BitmapFrame.Create(bmp).GetCurrentValueAsFrozen() as BitmapFrame;
          BitmapEncoder bitmapEncoder = new PngBitmapEncoder();
          bitmapEncoder.Frames.Add(frame as BitmapFrame);
          MemoryStream stream = new MemoryStream();
          bitmapEncoder.Save(stream);
          CreateMediaItem(stream);
          mediaPlayer.Close();
          videoContentsNew.Add(_contentItem);
        }
        if (_worker.CancellationPending) {
          e.Cancel = true;
          return;
        }
        _worker.ReportProgress(iProgress, imageContentsNew);
        _worker.ReportProgress(-iProgress, videoContentsNew);
      }
    }
    private void CreateMediaItem(MemoryStream ms) {
      ms.Position = 0;
      BitmapImage bitmapImage = new BitmapImage();
      bitmapImage.BeginInit();
      bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
      bitmapImage.StreamSource = ms;
      bitmapImage.EndInit();
      _contentItem.ImageBitmap = bitmapImage;
      _contentItem.ImageBitmap.Freeze();
      ms.Close();
    }
    private void Worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e) {
      PanelProgressVisibility = Visibility.Collapsed;
      GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced, true);
    }
    private void Worker_ProgressChanged(object sender, ProgressChangedEventArgs e) {
      ValueProgress = Math.Abs(e.ProgressPercentage);
      if (PanelProgressVisibility == Visibility.Collapsed) {
        PanelProgressVisibility = Visibility.Visible;
      }
      ProgressCaption = GetProgressCaption();
      if (e.UserState != null) {
        if (e.ProgressPercentage > 0) {
          ObservableCollection<ListContent> images = e.UserState as ObservableCollection<ListContent>;
          for (int i = ImageContents.Count; i < images.Count; i++) {
            ImageContents.Add(images[i]);
          }
          if (SelectedIndexImageOneToList == -1) {
            SelectedIndexImageOneToList = 0;
          }
        } else {
          ObservableCollection<ListContent> movies = e.UserState as ObservableCollection<ListContent>;
          for (int i = VideoContents.Count; i < movies.Count; i++) {
            VideoContents.Add(movies[i]);
            VideoContents[VideoContents.Count - 1].CreateDispatcherTimer();
            VideoContents[VideoContents.Count - 1].PositionVideoChangedEvent += new Action<double>(TraceSliderPositionVideoOneFromList);
          }
          if (SelectedIndexVideoOneToList == -1) {
            SelectedIndexVideoOneToList = 0;
          }
        }
      }
    }
    private string GetProgressCaption() => $"{ValueProgress} / {ItemsCount}";
    private int GetIndexFirstItemPage() {
      return CurrentKindMediaContent == KindMediaContent.Images ? _managerImagePages.IndexFirstItemPage :
             CurrentKindMediaContent == KindMediaContent.Videos ? _managerVideoPages.IndexFirstItemPage :
             0;
    }
    private int GetIndexLastItemPage() {
      return CurrentKindMediaContent == KindMediaContent.Images ? _managerImagePages.IndexLastItemPage :
             CurrentKindMediaContent == KindMediaContent.Videos ? _managerVideoPages.IndexLastItemPage :
             0;
    }
    private void ClearContents() {
      foreach (var item in ImageContents) {
        item.ImageBitmap = null;
      }
      //PositionVideoChangedEvent
      foreach (var item in VideoContents) {
        item.ImageBitmap = null;
        item.VideoMediaElement = null;
        item.TimerVideoDispatcher.Stop();
        item.PositionVideoChangedEvent -= new Action<double>(TraceSliderPositionVideoOneFromList);
        item.TimerVideoDispatcher = null;
      }
      ImageContents.Clear();
      VideoContents.Clear();
      SelectedIndexImageOneToList = -1;
      SelectedIndexVideoOneToList = -1;
      GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced, true);
    }
    private bool CheckIsMarkedImageList() {
      CountMarkedImagesList = (from marked in ImageContents where marked.IsMarked == true select marked).Count();
      return CountMarkedImagesList > 0 ? true : false;
    }
    private void ImageRotateRight(ListContent item) {
      item.NRotate = item.NRotate == null ? 90 :
                     item.NRotate == 270 ? 0 :
                     item.NRotate + 90;
      ContentUtilities.Instance.SetImageRotate(item);
    }
    private void ImageRotateLeft(ListContent item) {
      item.NRotate = item.NRotate == null ? -90 :
               item.NRotate == -270 ? 0 :
               item.NRotate - 90;
      ContentUtilities.Instance.SetImageRotate(item);
    }
    #endregion

  }
}
