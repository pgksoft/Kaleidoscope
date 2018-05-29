using Kaleidoscope.Helpers;
using Kaleidoscope.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Kaleidoscope.ViewModel {
  public enum ModeShowContent { None, List, OneFromList }
  public class ContentHeaderViewModel : ViewModelBase {
    public ContentHeaderViewModel(ContentWorkMode workMode,
                                  Predicate<object> canExecutePageFirst,
                                  Predicate<object> canExecutePageStepBackward,
                                  Predicate<object> canExecutePageStepForward,
                                  Predicate<object> canExecutePageLast) {
      _workMode = workMode;

      // Специальный параметры 
      OptionsMediaUserControl = new OptionsMedia() { DataContext = new OptionsMediaViewModel(workMode) };

      // Картинки на соответсвующих кнопаках в зависимости от цветовой темы
      SetImageChangeSize();

      // Подписка на события
      PaletteThemeViewModel.ThemeChanged += new Action(SetImageChangeSize);

      // Инициализация значений сохраненных параметров 
      // Последовательность инициализации может быть важной
      IsEnableRefreshContent = GetIsEnableRefreshContent();
      IsKindContent = GetIsKindContent();

      // Инициализация соответсвующих элементов управления
      if (WorkMode == ContentWorkMode.Main) {
        ButtonIsEnableRefreshContentVisibility = Visibility.Collapsed;
      }

      // Init commands
      RepeatLoadingContentCommand = new DelegateCommand(o => ExecuteRepeatLoadingContent(), o => CanExecuteExecuteRepeatLoadingContent());
      PageFirstCommand = new DelegateCommand(o => ExecutePageFirst(), canExecutePageFirst);
      PageStepBackwardCommand = new DelegateCommand(o => ExecutePageStepBackward(), canExecutePageStepBackward);
      PageStepForwardCommand = new DelegateCommand(o => ExecutePageStepForward(), canExecutePageStepForward);
      PageLastCommand = new DelegateCommand(o => ExecutePageLast(), canExecutePageLast);

      // Ready
      _isCreated = true;
    }

    #region Fields
    private bool _isCreated;
    private ContentWorkMode _workMode = ContentWorkMode.None;
    private int _contentItemSizeMin;
    private int _contentItemSizeMax;
    private int _selectedSizeWidthItem;
    private int _selectedSizeHeightItem;
    private bool _isEnableRefreshContent;
    private Visibility _buttonIsEnableRefreshContentVisibility = Visibility.Visible;
    private bool _isKindContent;
    private bool _isModeShowContent;
    private Visibility _pagesManagerPanelVisibility = Visibility.Visible;
    private bool _isChangeSizeItemSeparately = false;
    private string _imageChangeSizeWidth;
    private string _imageChangeSizeHeight;
    private Visibility _separatelyVisibility = Visibility.Visible;
    private string _captionRatioSizeItem = string.Empty;
    private double _ratioProportionally;
    private UserControl _optionsMediaUserControl;
    private string _pagesManagerCurrentStateCaption = string.Empty;
    private bool _isBeginningChangeKindModeContent = false;
    #endregion

    #region Properties
    public ContentWorkMode WorkMode => _workMode;
    public bool IsBeginningChangeKindModeContent => _isBeginningChangeKindModeContent;
    public int ContentItemSizeMin {
      get { return _contentItemSizeMin; }
      set { this.MutateVerbose(ref _contentItemSizeMin, value, RaisePropertyChanged()); }
    }
    public int ContentItemSizeMax {
      get { return _contentItemSizeMax; }
      set { this.MutateVerbose(ref _contentItemSizeMax, value, RaisePropertyChanged()); }
    }
    public UserControl OptionsMediaUserControl {
      get { return _optionsMediaUserControl; }
      set { this.MutateVerbose(ref _optionsMediaUserControl, value, RaisePropertyChanged()); }
    }
    public int SelectedSizeWidthItem {
      get { return _selectedSizeWidthItem; }
      set {
        this.MutateVerbose(ref _selectedSizeWidthItem, value, RaisePropertyChanged());
        ChangedWidthItemEvents?.Invoke(SelectedSizeWidthItem);
        CalculateRatio();
        UserSettingsSave();
      }
    }
    public int SelectedSizeHeightItem {
      get { return _selectedSizeHeightItem; }
      set {
        this.MutateVerbose(ref _selectedSizeHeightItem, value, RaisePropertyChanged());
        ChangedHeightItemEvents?.Invoke(SelectedSizeHeightItem);
        // Обязательное условие, иначе будет рекурсивное зацикливание
        if (SeparatelyVisibility == Visibility.Visible) {
          CalculateRatio();
        }
        UserSettingsSave();
      }
    }
    public string ImageChangeSizeWidth {
      get { return _imageChangeSizeWidth; }
      set { this.MutateVerbose(ref _imageChangeSizeWidth, value, RaisePropertyChanged()); }
    }
    public string ImageChangeSizeHeight {
      get { return _imageChangeSizeHeight; }
      set { this.MutateVerbose(ref _imageChangeSizeHeight, value, RaisePropertyChanged()); }
    }
    public bool IsEnableRefreshContent {
      get { return _isEnableRefreshContent; }
      set {
        this.MutateVerbose(ref _isEnableRefreshContent, value, RaisePropertyChanged());
        ChangedIsEnableRefreshContentEvent?.Invoke(IsEnableRefreshContent);
        UserSettingsSave();
      }
    }
    public Visibility ButtonIsEnableRefreshContentVisibility {
      get { return _buttonIsEnableRefreshContentVisibility; }
      set { this.MutateVerbose(ref _buttonIsEnableRefreshContentVisibility, value, RaisePropertyChanged()); }
    }
    public bool IsKindContent {
      get { return _isKindContent; }
      set {
        this.MutateVerbose(ref _isKindContent, value, RaisePropertyChanged());
        ChangeKindContentEvent?.Invoke(GetCurrentKindMediaContent());
        // Последовательность установки параметров является важной
        IsModeShowContent = GetIsModeShowContent();
      }
    }
    public bool IsModeShowContent {
      get { return _isModeShowContent; }
      set {
        this.MutateVerbose(ref _isModeShowContent, value, RaisePropertyChanged());
        ChangedModeShowContentEvent?.Invoke(GetCurrentModeShowContent());
        // Последовательность установки параметров является важной
        _isBeginningChangeKindModeContent = true;
        ContentItemSizeMin = GetContentItemSizeMin();
        ContentItemSizeMax = GetContentItemSizeMax();
        SelectedSizeWidthItem = GetSizeWidthItem();
        SelectedSizeHeightItem = GetSizeHeightItem();
        IsChangeSizeItemSeparately = GetIsChangeSizeItemSeparately();
        RatioProportionally = GetRatioProportionally();
        _isBeginningChangeKindModeContent = false;
        CalculateRatio();
        UserSettingsSave();
      }
    }
    public Visibility PagesManagerPanelVisibility {
      get { return _pagesManagerPanelVisibility; }
      set { this.MutateVerbose(ref _pagesManagerPanelVisibility, value, RaisePropertyChanged()); }
    }
    public bool IsChangeSizeItemSeparately {
      get { return _isChangeSizeItemSeparately; }
      set {
        this.MutateVerbose(ref _isChangeSizeItemSeparately, value, RaisePropertyChanged());
        SeparatelyVisibility = IsChangeSizeItemSeparately ? Visibility.Visible : Visibility.Collapsed;
        CalculateRatio();
        UserSettingsSave();
      }
    }
    public Visibility SeparatelyVisibility {
      get { return _separatelyVisibility; }
      set { this.MutateVerbose(ref _separatelyVisibility, value, RaisePropertyChanged()); }
    }
    public string CaptionRatioSizeItem {
      get { return _captionRatioSizeItem; }
      set { this.MutateVerbose(ref _captionRatioSizeItem, value, RaisePropertyChanged()); }
    }
    public double RatioProportionally {
      get { return _ratioProportionally; }
      set {
        this.MutateVerbose(ref _ratioProportionally, value, RaisePropertyChanged());
        UserSettingsSave();
      }
    }
    public string PagesManagerCurrentStateCaption {
      get { return _pagesManagerCurrentStateCaption; }
      set { this.MutateVerbose(ref _pagesManagerCurrentStateCaption, value, RaisePropertyChanged()); }
    }
    #endregion

    #region Commands
    public DelegateCommand RepeatLoadingContentCommand { get; protected set; }
    public DelegateCommand PageFirstCommand { get; private set; }
    public DelegateCommand PageStepBackwardCommand { get; private set; }
    public DelegateCommand PageStepForwardCommand { get; private set; }
    public DelegateCommand PageLastCommand { get; private set; }
    #endregion

    #region Commands implementation
    private void ExecuteRepeatLoadingContent() {
      RepeatLoadingContentCommandEvent?.Invoke();
    }
    private bool CanExecuteExecuteRepeatLoadingContent() {
      return IsEnableRefreshContent;
    }

    private void ExecutePageFirst() => PageFirstEvent?.Invoke();
    private void ExecutePageStepBackward() => PageStepBackwardEvent?.Invoke();
    private void ExecutePageStepForward() => PageStepForwardEvent?.Invoke();
    private void ExecutePageLast() => PageLastEvent?.Invoke();
    #endregion

    #region Events
    public event Action<int> ChangedWidthItemEvents;
    public event Action<int> ChangedHeightItemEvents;
    public event Action<ModeShowContent> ChangedModeShowContentEvent;
    public event Action<bool> ChangedIsEnableRefreshContentEvent;
    public event Action<KindMediaContent> ChangeKindContentEvent;
    public event Action RepeatLoadingContentCommandEvent;
    public event Action PageFirstEvent;
    public event Action PageStepBackwardEvent;
    public event Action PageStepForwardEvent;
    public event Action PageLastEvent;
    #endregion

    #region Methods
    public KindMediaContent GetCurrentKindMediaContent() => IsKindContent ? KindMediaContent.Videos : KindMediaContent.Images;
    public ModeShowContent GetCurrentModeShowContent() => IsModeShowContent ? ModeShowContent.OneFromList : ModeShowContent.List;
    #endregion

    #region Dispose
    protected override void OnDispose() {
      base.OnDispose();
      PaletteThemeViewModel.ThemeChanged -= new Action(SetImageChangeSize);
    }
    #endregion

    #region Helpers
    private void CalculateRatio() {
      if (IsBeginningChangeKindModeContent == true) return;
      if (SeparatelyVisibility == Visibility.Collapsed) {
        // Включен режим пропорционального изменения элемента контента
        // Расчет высоты элемента по ratio-коэффициенту
        SelectedSizeHeightItem = (int)(SelectedSizeWidthItem / RatioProportionally);
        CaptionRatioSizeItem = $"w{SelectedSizeWidthItem}/h{SelectedSizeHeightItem}";
      } else {
        // Включен режим настройки размера элемента контента отдельно по высоте и ширине
        // Считаем ratio-коэффициент
        if (SelectedSizeWidthItem >= GetContentItemSizeMin() && SelectedSizeHeightItem >= GetContentItemSizeMin()) {
          RatioProportionally = SelectedSizeWidthItem * 1.0 / SelectedSizeHeightItem * 1.0;
          CaptionRatioSizeItem = $"{SelectedSizeWidthItem}";
        }
      }
    }
    private void SetImageChangeSize() {
      if (Settings.Instance.AppIsDark) {
        ImageChangeSizeHeight = Settings.Instance.ImageSizeHeightDark;
        ImageChangeSizeWidth = Settings.Instance.ImageSizeWidthDark;
      } else {
        ImageChangeSizeHeight = Settings.Instance.ImageSizeHeight;
        ImageChangeSizeWidth = Settings.Instance.ImageSizeWidth;
      }
    }
    private void UserSettingsSave() {
      if (_isCreated) {
        if (IsBeginningChangeKindModeContent == true) return;
        // For the MAIN SECTION
        if (WorkMode == ContentWorkMode.Main) {
          Properties.Settings.Default.IsKindContentMain = IsKindContent;
          if (GetCurrentKindMediaContent() == KindMediaContent.Images) {
            Properties.Settings.Default.IsModeShowMainImage = IsModeShowContent;
            if (GetCurrentModeShowContent() == ModeShowContent.List) {
              Properties.Settings.Default.ContentMainImageSelectedWidthItem = SelectedSizeWidthItem;
              Properties.Settings.Default.ContentMainImageSelectedHeightItem = SelectedSizeHeightItem;
              Properties.Settings.Default.IsChangeSizeItemSeparatelyMainImage = IsChangeSizeItemSeparately;
              Properties.Settings.Default.RatioMainImageChangeSizeProportionally = RatioProportionally;
            } else if (GetCurrentModeShowContent() == ModeShowContent.OneFromList) {
              Properties.Settings.Default.ContentMainImageOneToListSelectedWidth = SelectedSizeWidthItem;
              Properties.Settings.Default.ContentMainImageOneToListSelectedHeight = SelectedSizeHeightItem;
              Properties.Settings.Default.IsChangeSizeItemSeparatelyMainOneToListImage = IsChangeSizeItemSeparately;
              Properties.Settings.Default.RatioMainImageOneToListChangeSizeProportionally = RatioProportionally;
            }
          } else if (GetCurrentKindMediaContent() == KindMediaContent.Videos) {
            Properties.Settings.Default.IsModeShowMainVideo = IsModeShowContent;
            if (GetCurrentModeShowContent() == ModeShowContent.List) {
              Properties.Settings.Default.ContentMainVideoSelectedWidthItem = SelectedSizeWidthItem;
              Properties.Settings.Default.ContentMainVideoSelectedHeightItem = SelectedSizeHeightItem;
              Properties.Settings.Default.IsChangeSizeItemSeparatelyMainVideo = IsChangeSizeItemSeparately;
              Properties.Settings.Default.RatioMainVideoChangeSizeProportionally = RatioProportionally;
            } else if (GetCurrentModeShowContent() == ModeShowContent.OneFromList) {
              Properties.Settings.Default.ContentMainVideoOneToListSelectedWidth = SelectedSizeWidthItem;
              Properties.Settings.Default.ContentMainVideoOneToListSelectedHeight= SelectedSizeHeightItem;
              Properties.Settings.Default.IsChangeSizeItemSeparatelyMainOneToListVideo = IsChangeSizeItemSeparately;
              Properties.Settings.Default.RatioMainVideoOneToListChangeSizeProportionally = RatioProportionally;
            }          
          }
          // For the FOLDER SECTION
        } else if (WorkMode == ContentWorkMode.Folders) {
          Properties.Settings.Default.IsKindContentFolders = IsKindContent;
          Properties.Settings.Default.IsEnableRefreshContentFolders = IsEnableRefreshContent;
          if (GetCurrentKindMediaContent() == KindMediaContent.Images) {
            Properties.Settings.Default.IsModeShowFoldersImage = IsModeShowContent;
            if (GetCurrentModeShowContent() == ModeShowContent.List) {
              Properties.Settings.Default.ContentFoldersImageSelectedWidthItem = SelectedSizeWidthItem;
              Properties.Settings.Default.ContentFoldersImageSelectedHeightItem = SelectedSizeHeightItem;
              Properties.Settings.Default.IsChangeSizeItemSeparatelyFoldersImage = IsChangeSizeItemSeparately;
              Properties.Settings.Default.RatioFoldersImageChangeSizeProportionally = RatioProportionally;
            } else if (GetCurrentModeShowContent() == ModeShowContent.OneFromList) {
              Properties.Settings.Default.ContentFoldersImageOneToListSelectedWidth = SelectedSizeWidthItem;
              Properties.Settings.Default.ContentFoldersImageOneToListSelectedHeight = SelectedSizeHeightItem;
              Properties.Settings.Default.IsChangeSizeItemSeparatelyFoldersOneToListImage = IsChangeSizeItemSeparately;
              Properties.Settings.Default.RatioFoldersImageOneToListChangeSizeProportionally = RatioProportionally;
            }
          } else if (GetCurrentKindMediaContent() == KindMediaContent.Videos) {
            Properties.Settings.Default.IsModeShowFoldersVideo = IsModeShowContent;
            if (GetCurrentModeShowContent() == ModeShowContent.List) {
              Properties.Settings.Default.ContentFoldersVideoSelectedWidthItem = SelectedSizeWidthItem;
              Properties.Settings.Default.ContentFoldersVideoSelectedHeightItem = SelectedSizeHeightItem;
              Properties.Settings.Default.IsChangeSizeItemSeparatelyFoldersVideo = IsChangeSizeItemSeparately;
              Properties.Settings.Default.RatioFoldersVideoChangeSizeProportionally = RatioProportionally;
            } else if (GetCurrentModeShowContent() == ModeShowContent.OneFromList) {
              Properties.Settings.Default.ContentFoldersVideoOneToListSelectedWidth = SelectedSizeWidthItem;
              Properties.Settings.Default.ContentFoldersVideoOneToListSelectedHeight = SelectedSizeHeightItem;
              Properties.Settings.Default.IsChangeSizeItemSeparatelyFoldersOneToListVideo = IsChangeSizeItemSeparately;
              Properties.Settings.Default.RatioFoldersVideoOneToListChangeSizeProportionally = RatioProportionally;
            }
          }
          // For the CATALOG SECTION
        } else if (WorkMode == ContentWorkMode.Catalogs) {
          Properties.Settings.Default.IsKindContentCatalogs = IsKindContent;
          Properties.Settings.Default.IsEnableRefreshContentCatalogs = IsEnableRefreshContent;
          if (GetCurrentKindMediaContent() == KindMediaContent.Images) {
            Properties.Settings.Default.IsModeShowCatalogsImage = IsModeShowContent;
            if (GetCurrentModeShowContent() == ModeShowContent.List) {
              Properties.Settings.Default.ContentCatalogsImageSelectedWidthItem = SelectedSizeWidthItem;
              Properties.Settings.Default.ContentCatalogsImageSelectedHeightItem = SelectedSizeHeightItem;
              Properties.Settings.Default.IsChangeSizeItemSeparatelyCatalogImage = IsChangeSizeItemSeparately;
              Properties.Settings.Default.RatioCatalogImageChangeSizeProportionally = RatioProportionally;
            } else if (GetCurrentModeShowContent() == ModeShowContent.OneFromList) {
              Properties.Settings.Default.ContentCatalogImageOneToListSelectedWidth = SelectedSizeWidthItem;
              Properties.Settings.Default.ContentCatalogImageOneToListSelectedHeight = SelectedSizeHeightItem;
              Properties.Settings.Default.IsChangeSizeItemSeparatelyCatalogOneToListImage = IsChangeSizeItemSeparately;
              Properties.Settings.Default.RatioCatalogsImageOneToListChangeSizeProportionally = RatioProportionally;
            }
          } else if (GetCurrentKindMediaContent() == KindMediaContent.Videos) {
            Properties.Settings.Default.IsModeShowCatalogsVideo = IsModeShowContent;
            if (GetCurrentModeShowContent() == ModeShowContent.List) {
              Properties.Settings.Default.ContentCatalogsVideoSelectedWidthItem = SelectedSizeWidthItem;
              Properties.Settings.Default.ContentCatalogsVideoSelectedHeightItem = SelectedSizeHeightItem;
              Properties.Settings.Default.IsChangeSizeItemSeparatelyCatalogVideo = IsChangeSizeItemSeparately;
              Properties.Settings.Default.RatioCatalogVideoChangeSizeProportionally = RatioProportionally;
            } else if (GetCurrentModeShowContent() == ModeShowContent.OneFromList) {
              Properties.Settings.Default.ContentCatalogsVideoOneToListSelectedWidth = SelectedSizeWidthItem;
              Properties.Settings.Default.ContentCatalogsVideoOneToListSelectedHeight = SelectedSizeHeightItem;
              Properties.Settings.Default.IsChangeSizeItemSeparatelyCatalogOneToListVideo = IsChangeSizeItemSeparately;
              Properties.Settings.Default.RatioCatalogsVideoOneToListChangeSizeProportionally = RatioProportionally;
            }
          }
        }
        Properties.Settings.Default.Save();
      }
    }
    private int GetContentItemSizeMin() {
      return GetCurrentKindMediaContent() == KindMediaContent.Images && 
             GetCurrentModeShowContent() == ModeShowContent.List ? Settings.Instance.ImageItemSizeInListMin 
             :
             GetCurrentKindMediaContent() == KindMediaContent.Images && 
             WorkMode == ContentWorkMode.Main &&
             GetCurrentModeShowContent() == ModeShowContent.OneFromList ? Settings.Instance.ImageMainItemSizeOneFromListMin
             :
             GetCurrentKindMediaContent() == KindMediaContent.Images &&
             (WorkMode == ContentWorkMode.Catalogs || WorkMode == ContentWorkMode.Folders) &&
             GetCurrentModeShowContent() == ModeShowContent.OneFromList ? Settings.Instance.ImageDicItemSizeOneFromListMin
             :
             GetCurrentKindMediaContent() == KindMediaContent.Videos && 
             GetCurrentModeShowContent() == ModeShowContent.List ? Settings.Instance.VideoItemSizeInListMin 
             :
             GetCurrentKindMediaContent() == KindMediaContent.Videos &&
             WorkMode == ContentWorkMode.Main &&
             GetCurrentModeShowContent() == ModeShowContent.OneFromList ? Settings.Instance.VideoMainItemSizeOneFromListMin 
             :
             GetCurrentKindMediaContent() == KindMediaContent.Videos &&
             (WorkMode == ContentWorkMode.Catalogs || WorkMode == ContentWorkMode.Folders) &&
             GetCurrentModeShowContent() == ModeShowContent.OneFromList ? Settings.Instance.VideoDicItemSizeOneFromListMin
             :
             GetCurrentKindMediaContent() == KindMediaContent.Images ? Settings.Instance.ImageItemSizeInListMin :
             GetCurrentKindMediaContent() == KindMediaContent.Videos ? Settings.Instance.VideoItemSizeInListMin :
             Settings.Instance.ImageItemSizeInListMin;
    }
    private int GetContentItemSizeMax() {
      return GetCurrentKindMediaContent() == KindMediaContent.Images && 
             GetCurrentModeShowContent() == ModeShowContent.List ? Settings.Instance.ImageItemSizeInListMax 
             :
             GetCurrentKindMediaContent() == KindMediaContent.Images &&
             WorkMode == ContentWorkMode.Main &&
             GetCurrentModeShowContent() == ModeShowContent.OneFromList ? Settings.Instance.ImageMainItemSizeOneFromListMax 
             :
             GetCurrentKindMediaContent() == KindMediaContent.Images &&
             (WorkMode == ContentWorkMode.Catalogs || WorkMode == ContentWorkMode.Folders) &&
             GetCurrentModeShowContent() == ModeShowContent.OneFromList ? Settings.Instance.ImageDicItemSizeOneFromListMax
             :
             GetCurrentKindMediaContent() == KindMediaContent.Videos && 
             GetCurrentModeShowContent() == ModeShowContent.List ? Settings.Instance.VideoItemSizeInListMax 
             :
             GetCurrentKindMediaContent() == KindMediaContent.Videos &&
             WorkMode == ContentWorkMode.Main &&
             GetCurrentModeShowContent() == ModeShowContent.OneFromList ? Settings.Instance.VideoMainItemSizeOneFromListtMax 
             :
             GetCurrentKindMediaContent() == KindMediaContent.Videos &&
             (WorkMode == ContentWorkMode.Catalogs || WorkMode == ContentWorkMode.Folders) &&
             GetCurrentModeShowContent() == ModeShowContent.OneFromList ? Settings.Instance.VideoDicItemSizeOneFromListtMax
             :
             GetCurrentKindMediaContent() == KindMediaContent.Images ? Settings.Instance.ImageItemSizeInListMax :
             GetCurrentKindMediaContent() == KindMediaContent.Videos ? Settings.Instance.VideoItemSizeInListMax :
             Settings.Instance.ImageItemSizeInListMax;
    }
    private int GetSizeWidthItem() {
      if (GetCurrentKindMediaContent() == KindMediaContent.Images) {
        if (GetCurrentModeShowContent() == ModeShowContent.List) {
          return WorkMode == ContentWorkMode.Main ? Settings.Instance.ContentMainImageSelectedWidthItem :
                 WorkMode == ContentWorkMode.Folders ? Settings.Instance.ContentFoldersImageSelectedWidthItem :
                 WorkMode == ContentWorkMode.Catalogs ? Settings.Instance.ContentCatalogsImageSelectedWidthItem :
                 Settings.Instance.ImageItemSizeInListMin;
        } else if (GetCurrentModeShowContent() == ModeShowContent.OneFromList) {
          return WorkMode == ContentWorkMode.Main ? Settings.Instance.ContentMainImageOneToListSelectedWidth :
                 WorkMode == ContentWorkMode.Folders ? Settings.Instance.ContentFoldersImageOneToListSelectedWidth :
                 WorkMode == ContentWorkMode.Catalogs ? Settings.Instance.ContentCatalogImageOneToListSelectedWidth :
                 Settings.Instance.ImageDicItemSizeOneFromListMin;
        } else {
          return Settings.Instance.ImageItemSizeInListMin;
        }
      } else if (GetCurrentKindMediaContent() == KindMediaContent.Videos) {
        if (GetCurrentModeShowContent() == ModeShowContent.List) {
          return WorkMode == ContentWorkMode.Main ? Settings.Instance.ContentMainVideoSelectedWidthItem :
                 WorkMode == ContentWorkMode.Folders ? Settings.Instance.ContentFoldersVideoSelectedWidthItem :
                 WorkMode == ContentWorkMode.Catalogs ? Settings.Instance.ContentCatalogsVideoSelectedWidthItem :
                 Settings.Instance.VideoItemSizeInListMin;
        } else if (GetCurrentModeShowContent() == ModeShowContent.OneFromList) {
          return WorkMode == ContentWorkMode.Main ? Settings.Instance.ContentMainVideoOneToListSelectedWidth :
                 WorkMode == ContentWorkMode.Folders ? Settings.Instance.ContentFoldersVideoOneToListSelectedWidth :
                 WorkMode == ContentWorkMode.Catalogs ? Settings.Instance.ContentCatalogsVideoOneToListSelectedWidth :
                 Settings.Instance.VideoDicItemSizeOneFromListMin;
        } else {
          return Settings.Instance.VideoItemSizeInListMin;
        }
      } else {
        return Settings.Instance.VideoItemSizeInListMin;
      }
    }
    private int GetSizeHeightItem() {
      if (GetCurrentKindMediaContent() == KindMediaContent.Images) {
        if (GetCurrentModeShowContent() == ModeShowContent.List) {
          return WorkMode == ContentWorkMode.Main ? Settings.Instance.ContentMainImageSelectedHeightItem :
                 WorkMode == ContentWorkMode.Folders ? Settings.Instance.ContentFoldersImageSelectedHeightItem :
                 WorkMode == ContentWorkMode.Catalogs ? Settings.Instance.ContentCatalogsImageSelectedHeightItem :
                 Settings.Instance.ImageItemSizeInListMin;
        } else if (GetCurrentModeShowContent() == ModeShowContent.OneFromList) {
          return WorkMode == ContentWorkMode.Main ? Settings.Instance.ContentMainImageOneToListSelectedHeight :
                 WorkMode == ContentWorkMode.Folders ? Settings.Instance.ContentFoldersImageOneToListSelectedHeight :
                 WorkMode == ContentWorkMode.Catalogs ? Settings.Instance.ContentCatalogImageOneToListSelectedHeight :
                 Settings.Instance.ImageDicItemSizeOneFromListMin;
        } else {
          return Settings.Instance.ImageItemSizeInListMin;
        }
      } else if (GetCurrentKindMediaContent() == KindMediaContent.Videos) {
        if (GetCurrentModeShowContent() == ModeShowContent.List) {
          return WorkMode == ContentWorkMode.Main ? Settings.Instance.ContentMainVideoSelectedHeightItem :
                 WorkMode == ContentWorkMode.Folders ? Settings.Instance.ContentFoldersVideoSelectedHeightItem :
                 WorkMode == ContentWorkMode.Catalogs ? Settings.Instance.ContentCatalogsVideoSelectedHeightItem :
                 Settings.Instance.VideoItemSizeInListMin;
        } else if (GetCurrentModeShowContent() == ModeShowContent.OneFromList) {
          return WorkMode == ContentWorkMode.Main ? Settings.Instance.ContentMainVideoOneToListSelectedHeight :
                 WorkMode == ContentWorkMode.Folders ? Settings.Instance.ContentFoldersVideoOneToListSelectedHeight :
                 WorkMode == ContentWorkMode.Catalogs ? Settings.Instance.ContentCatalogsVideoOneToListSelectedHeight :
                 Settings.Instance.VideoDicItemSizeOneFromListMin;
        } else {
          return Settings.Instance.VideoDicItemSizeOneFromListMin;
        }
      } else {
        return Settings.Instance.VideoItemSizeInListMin;
      }
    }
    private bool GetIsChangeSizeItemSeparately() {
      if (GetCurrentKindMediaContent() == KindMediaContent.Images) {
        if (GetCurrentModeShowContent() == ModeShowContent.List) {
          return WorkMode == ContentWorkMode.Main ? Settings.Instance.IsChangeSizeItemSeparatelyMainImage :
                 WorkMode == ContentWorkMode.Folders ? Settings.Instance.IsChangeSizeItemSeparatelyFoldersImage :
                 WorkMode == ContentWorkMode.Catalogs ? Settings.Instance.IsChangeSizeItemSeparatelyCatalogImage :
                 false;
        } else if (GetCurrentModeShowContent() == ModeShowContent.OneFromList) {
          return WorkMode == ContentWorkMode.Main ? Settings.Instance.IsChangeSizeItemSeparatelyMainOneToListImage :
                 WorkMode == ContentWorkMode.Folders ? Settings.Instance.IsChangeSizeItemSeparatelyFoldersOneToListImage :
                 WorkMode == ContentWorkMode.Catalogs ? Settings.Instance.IsChangeSizeItemSeparatelyCatalogOneToListImage :
                 false;
        } else {
          return false;
        }
      } else if (GetCurrentKindMediaContent() == KindMediaContent.Videos) {
        if (GetCurrentModeShowContent() == ModeShowContent.List) {
          return WorkMode == ContentWorkMode.Main ? Settings.Instance.IsChangeSizeItemSeparatelyMainVideo :
                 WorkMode == ContentWorkMode.Folders ? Settings.Instance.IsChangeSizeItemSeparatelyFoldersVideo :
                 WorkMode == ContentWorkMode.Catalogs ? Settings.Instance.IsChangeSizeItemSeparatelyCatalogVideo :
                 false;
        } else if (GetCurrentModeShowContent() == ModeShowContent.OneFromList) {
          return WorkMode == ContentWorkMode.Main ? Settings.Instance.IsChangeSizeItemSeparatelyMainOneToListVideo :
                 WorkMode == ContentWorkMode.Folders ? Settings.Instance.IsChangeSizeItemSeparatelyFoldersOneToListVideo :
                 WorkMode == ContentWorkMode.Catalogs ? Settings.Instance.IsChangeSizeItemSeparatelyCatalogOneToListVideo :
                 false;
        } else {
          return false;
        }
      } else {
        return false;
      }
    }
    private double GetRatioProportionally() {
      if (GetCurrentKindMediaContent() == KindMediaContent.Images) {
        if (GetCurrentModeShowContent() == ModeShowContent.List) {
          return WorkMode == ContentWorkMode.Main ? Settings.Instance.RatioMainImageChangeSizeProportionally :
                 WorkMode == ContentWorkMode.Folders ? Settings.Instance.RatioFoldersImageChangeSizeProportionally :
                 WorkMode == ContentWorkMode.Catalogs ? Settings.Instance.RatioCatalogImageChangeSizeProportionally :
                 1.0;
        } else if (GetCurrentModeShowContent() == ModeShowContent.OneFromList) {
          return WorkMode == ContentWorkMode.Main ? Settings.Instance.RatioMainImageOneToListChangeSizeProportionally :
                 WorkMode == ContentWorkMode.Folders ? Settings.Instance.RatioFoldersImageOneToListChangeSizeProportionally :
                 WorkMode == ContentWorkMode.Catalogs ? Settings.Instance.RatioCatalogsImageOneToListChangeSizeProportionally :
                 1.0;
        } else {
          return 1.0;
        }
      } else if (GetCurrentKindMediaContent() == KindMediaContent.Videos) {
        if (GetCurrentModeShowContent() == ModeShowContent.List) {
          return WorkMode == ContentWorkMode.Main ? Settings.Instance.RatioMainVideoChangeSizeProportionally :
                 WorkMode == ContentWorkMode.Folders ? Settings.Instance.RatioFoldersVideoChangeSizeProportionally :
                 WorkMode == ContentWorkMode.Catalogs ? Settings.Instance.RatioCatalogVideoChangeSizeProportionally :
                 1.0;
        } else if (GetCurrentModeShowContent() == ModeShowContent.OneFromList) {
          return WorkMode == ContentWorkMode.Main ? Settings.Instance.RatioMainVideoOneToListChangeSizeProportionally :
                 WorkMode == ContentWorkMode.Folders ? Settings.Instance.RatioFoldersVideoOneToListChangeSizeProportionally :
                 WorkMode == ContentWorkMode.Catalogs ? Settings.Instance.RatioCatalogsVideoOneToListChangeSizeProportionally :
                 1.0;
        } else {
          return 1.0;
        }
      } else {
        return 1.0;
      }
    }
    private bool GetIsEnableRefreshContent() {
      return WorkMode == ContentWorkMode.Main ? true :
             WorkMode == ContentWorkMode.Folders ? Settings.Instance.IsEnableRefreshContentFolders :
             WorkMode == ContentWorkMode.Catalogs ? Settings.Instance.IsEnableRefreshContentCatalogs :
             false;
    }
    private bool GetIsKindContent() {
      return WorkMode == ContentWorkMode.Main ? Settings.Instance.IsKindContentMain :
             WorkMode == ContentWorkMode.Folders ? Settings.Instance.IsKindContentFolders :
             WorkMode == ContentWorkMode.Catalogs ? Settings.Instance.IsKindContentCatalogs :
             false;
    }
    private bool GetIsModeShowContent() {
      if (GetCurrentKindMediaContent() == KindMediaContent.Images) {
        return WorkMode == ContentWorkMode.Main ? Settings.Instance.IsModeShowMainImage :
               WorkMode == ContentWorkMode.Folders ? Settings.Instance.IsModeShowFoldersImage :
               WorkMode == ContentWorkMode.Catalogs ? Settings.Instance.IsModeShowCatalogsImage :
               false;
      } else if (GetCurrentKindMediaContent() == KindMediaContent.Videos) {
        return WorkMode == ContentWorkMode.Main ? Settings.Instance.IsModeShowMainVideo :
               WorkMode == ContentWorkMode.Folders ? Settings.Instance.IsModeShowFoldersVideo :
               WorkMode == ContentWorkMode.Catalogs ? Settings.Instance.IsModeShowCatalogsVideo :
               false;
      } else {
        return false;
      }
    }

    #endregion

  }
}
