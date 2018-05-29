using Kaleidoscope.Model;
using Kaleidoscope.ViewModel;
using LocalizatorHelper;
using MaterialDesignColors;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Serialization;

namespace Kaleidoscope.Helpers {
  public class Settings : ViewModelBase {
    public Settings() {
      _instance = this;
    }
    public static Settings Same() {
      if (_instance == null) {
        _instance = new Settings();
      }
      return _instance;
    }

    #region Fields
    private static Settings _instance;
    #endregion

    #region Properties
    public static Settings Instance => Same();
    public ContentDB ContentDB = new ContentDB();
    public LocalisationHelper LocalisationHelper => new LocalisationHelper();
    public TaskStatus AppStatus {
      get { return Properties.Settings.Default.MainTaskStatus; }
      set {
        Properties.Settings.Default.MainTaskStatus = value;
        Properties.Settings.Default.Save();
        OnPropertyChanged();
      }
    }
    public bool AppIsDark {
      get { return Properties.Settings.Default.IsDark; }
      set {
        Properties.Settings.Default.IsDark = value;
        Properties.Settings.Default.Save();
        OnPropertyChanged();
      }
    }
    public string AppPalettePrimaryName {
      get { return Properties.Settings.Default.AppPalettePrimaryName; }
      set {
        Properties.Settings.Default.AppPalettePrimaryName = value;
        Properties.Settings.Default.Save();
        OnPropertyChanged();
      }
    }
    public string AppPaletteAccentName {
      get { return Properties.Settings.Default.AppPaletteAccentName; }
      set {
        Properties.Settings.Default.AppPaletteAccentName = value;
        Properties.Settings.Default.Save();
        OnPropertyChanged();
      }
    }
    public string Localization {
      get { return Properties.Settings.Default.Localization; }
      set {
        Properties.Settings.Default.Localization = value;
        Properties.Settings.Default.Save();
        OnPropertyChanged();
      }
    }
    public int SelectedIndexFillType {
      get { return Properties.Settings.Default.SelectedIndexFillTypes; }
      set {
        Properties.Settings.Default.SelectedIndexFillTypes = value;
        Properties.Settings.Default.Save();
        OnPropertyChanged();
      }
    }
    public Visibility ButtonTestVisibility {
      get { return Properties.Settings.Default.ButtonTestVisibility; }
      set {
        Properties.Settings.Default.ButtonTestVisibility = value;
        Properties.Settings.Default.Save();
        OnPropertyChanged();
      }
    }

    #region Properties: Content parameters 
    public int ContentMainImageSelectedWidthItem {
      get { return Properties.Settings.Default.ContentMainImageSelectedWidthItem; }
      set {
        Properties.Settings.Default.ContentMainImageSelectedWidthItem = value;
        Properties.Settings.Default.Save();
        OnPropertyChanged();
      }
    }

    public int ContentFoldersImageSelectedWidthItem {
      get { return Properties.Settings.Default.ContentFoldersImageSelectedWidthItem; }
      set {
        Properties.Settings.Default.ContentFoldersImageSelectedWidthItem = value;
        Properties.Settings.Default.Save();
        OnPropertyChanged();
      }
    }

    public int ContentCatalogsImageSelectedWidthItem {
      get { return Properties.Settings.Default.ContentCatalogsImageSelectedWidthItem; }
      set {
        Properties.Settings.Default.ContentCatalogsImageSelectedWidthItem = value;
        Properties.Settings.Default.Save();
        OnPropertyChanged();
      }
    }
    public int ContentMainImageSelectedHeightItem {
      get { return Properties.Settings.Default.ContentMainImageSelectedHeightItem; }
      set {
        Properties.Settings.Default.ContentMainImageSelectedHeightItem = value;
        Properties.Settings.Default.Save();
        OnPropertyChanged();
      }
    }

    public int ContentFoldersImageSelectedHeightItem {
      get { return Properties.Settings.Default.ContentFoldersImageSelectedHeightItem; }
      set {
        Properties.Settings.Default.ContentFoldersImageSelectedHeightItem = value;
        Properties.Settings.Default.Save();
        OnPropertyChanged();
      }
    }
    public int ContentCatalogsImageSelectedHeightItem {
      get { return Properties.Settings.Default.ContentCatalogsImageSelectedHeightItem; }
      set {
        Properties.Settings.Default.ContentCatalogsImageSelectedHeightItem = value;
        Properties.Settings.Default.Save();
        OnPropertyChanged();
      }
    }
    public int ContentMainImageOneToListSelectedWidth {
      get { return Properties.Settings.Default.ContentMainImageOneToListSelectedWidth; }
      set {
        Properties.Settings.Default.ContentMainImageOneToListSelectedWidth = value;
        Properties.Settings.Default.Save();
        OnPropertyChanged();
      }
    }
    public int ContentFoldersImageOneToListSelectedWidth {
      get { return Properties.Settings.Default.ContentFoldersImageOneToListSelectedWidth; }
      set {
        Properties.Settings.Default.ContentFoldersImageOneToListSelectedWidth = value;
        Properties.Settings.Default.Save();
        OnPropertyChanged();
      }
    }
    public int ContentCatalogImageOneToListSelectedWidth {
      get { return Properties.Settings.Default.ContentCatalogImageOneToListSelectedWidth; }
      set {
        Properties.Settings.Default.ContentCatalogImageOneToListSelectedWidth = value;
        Properties.Settings.Default.Save();
        OnPropertyChanged();
      }
    }
    public int ContentMainImageOneToListSelectedHeight {
      get { return Properties.Settings.Default.ContentMainImageOneToListSelectedHeight; }
      set {
        Properties.Settings.Default.ContentMainImageOneToListSelectedHeight = value;
        Properties.Settings.Default.Save();
        OnPropertyChanged();
      }
    }
    public int ContentFoldersImageOneToListSelectedHeight {
      get { return Properties.Settings.Default.ContentFoldersImageOneToListSelectedHeight; }
      set {
        Properties.Settings.Default.ContentFoldersImageOneToListSelectedHeight = value;
        Properties.Settings.Default.Save();
        OnPropertyChanged();
      }
    }
    public int ContentCatalogImageOneToListSelectedHeight {
      get { return Properties.Settings.Default.ContentCatalogImageOneToListSelectedHeight; }
      set {
        Properties.Settings.Default.ContentCatalogImageOneToListSelectedHeight = value;
        Properties.Settings.Default.Save();
        OnPropertyChanged();
      }
    }
    public bool IsChangeSizeItemSeparatelyMainOneToListImage {
      get { return Properties.Settings.Default.IsChangeSizeItemSeparatelyMainOneToListImage; }
      set {
        Properties.Settings.Default.IsChangeSizeItemSeparatelyMainOneToListImage = value;
        Properties.Settings.Default.Save();
        OnPropertyChanged();
      }
    }
    public bool IsChangeSizeItemSeparatelyFoldersOneToListImage {
      get { return Properties.Settings.Default.IsChangeSizeItemSeparatelyFoldersOneToListImage; }
      set {
        Properties.Settings.Default.IsChangeSizeItemSeparatelyFoldersOneToListImage = value;
        Properties.Settings.Default.Save();
        OnPropertyChanged();
      }
    }
    public bool IsChangeSizeItemSeparatelyCatalogOneToListImage {
      get { return Properties.Settings.Default.IsChangeSizeItemSeparatelyCatalogOneToListImage; }
      set {
        Properties.Settings.Default.IsChangeSizeItemSeparatelyCatalogOneToListImage = value;
        Properties.Settings.Default.Save();
        OnPropertyChanged();
      }
    }
    public bool IsChangeSizeItemSeparatelyMainOneToListVideo {
      get { return Properties.Settings.Default.IsChangeSizeItemSeparatelyMainOneToListVideo; }
      set {
        Properties.Settings.Default.IsChangeSizeItemSeparatelyMainOneToListVideo = value;
        Properties.Settings.Default.Save();
        OnPropertyChanged();
      }
    }
    public bool IsChangeSizeItemSeparatelyFoldersOneToListVideo {
      get { return Properties.Settings.Default.IsChangeSizeItemSeparatelyFoldersOneToListVideo; }
      set {
        Properties.Settings.Default.IsChangeSizeItemSeparatelyFoldersOneToListVideo = value;
        Properties.Settings.Default.Save();
        OnPropertyChanged();
      }
    }
    public bool IsChangeSizeItemSeparatelyCatalogOneToListVideo {
      get { return Properties.Settings.Default.IsChangeSizeItemSeparatelyCatalogOneToListVideo; }
      set {
        Properties.Settings.Default.IsChangeSizeItemSeparatelyCatalogOneToListVideo = value;
        Properties.Settings.Default.Save();
        OnPropertyChanged();
      }
    }
    public double RatioMainImageOneToListChangeSizeProportionally {
      get { return Properties.Settings.Default.RatioMainImageOneToListChangeSizeProportionally; }
      set {
        Properties.Settings.Default.RatioMainImageOneToListChangeSizeProportionally = value;
        Properties.Settings.Default.Save();
        OnPropertyChanged();
      }
    }
    public double RatioFoldersImageOneToListChangeSizeProportionally {
      get { return Properties.Settings.Default.RatioFoldersImageOneToListChangeSizeProportionally; }
      set {
        Properties.Settings.Default.RatioFoldersImageOneToListChangeSizeProportionally = value;
        Properties.Settings.Default.Save();
        OnPropertyChanged();
      }
    }
    public double RatioCatalogsImageOneToListChangeSizeProportionally {
      get { return Properties.Settings.Default.RatioCatalogsImageOneToListChangeSizeProportionally; }
      set {
        Properties.Settings.Default.RatioCatalogsImageOneToListChangeSizeProportionally = value;
        Properties.Settings.Default.Save();
        OnPropertyChanged();
      }
    }
    public double RatioMainVideoOneToListChangeSizeProportionally {
      get { return Properties.Settings.Default.RatioMainVideoOneToListChangeSizeProportionally; }
      set {
        Properties.Settings.Default.RatioMainVideoOneToListChangeSizeProportionally = value;
        Properties.Settings.Default.Save();
        OnPropertyChanged();
      }
    }
    public double RatioFoldersVideoOneToListChangeSizeProportionally {
      get { return Properties.Settings.Default.RatioFoldersVideoOneToListChangeSizeProportionally; }
      set {
        Properties.Settings.Default.RatioFoldersVideoOneToListChangeSizeProportionally = value;
        Properties.Settings.Default.Save();
        OnPropertyChanged();
      }
    }
    public double RatioCatalogsVideoOneToListChangeSizeProportionally {
      get { return Properties.Settings.Default.RatioCatalogsVideoOneToListChangeSizeProportionally; }
      set {
        Properties.Settings.Default.RatioCatalogsVideoOneToListChangeSizeProportionally = value;
        Properties.Settings.Default.Save();
        OnPropertyChanged();
      }
    }
    public int ContentMainVideoSelectedWidthItem {
      get { return Properties.Settings.Default.ContentMainVideoSelectedWidthItem; }
      set {
        Properties.Settings.Default.ContentMainVideoSelectedWidthItem = value;
        Properties.Settings.Default.Save();
        OnPropertyChanged();
      }
    }
    public int ContentFoldersVideoSelectedWidthItem {
      get { return Properties.Settings.Default.ContentFoldersVideoSelectedWidthItem; }
      set {
        Properties.Settings.Default.ContentFoldersVideoSelectedWidthItem = value;
        Properties.Settings.Default.Save();
        OnPropertyChanged();
      }
    }
    public int ContentCatalogsVideoSelectedWidthItem {
      get { return Properties.Settings.Default.ContentCatalogsVideoSelectedWidthItem; }
      set {
        Properties.Settings.Default.ContentCatalogsVideoSelectedWidthItem = value;
        Properties.Settings.Default.Save();
        OnPropertyChanged();
      }
    }
    public int ContentMainVideoSelectedHeightItem {
      get { return Properties.Settings.Default.ContentMainVideoSelectedHeightItem; }
      set {
        Properties.Settings.Default.ContentMainVideoSelectedHeightItem = value;
        Properties.Settings.Default.Save();
        OnPropertyChanged();
      }
    }

    public int ContentFoldersVideoSelectedHeightItem {
      get { return Properties.Settings.Default.ContentFoldersVideoSelectedHeightItem; }
      set {
        Properties.Settings.Default.ContentFoldersVideoSelectedHeightItem = value;
        Properties.Settings.Default.Save();
        OnPropertyChanged();
      }
    }
    public int ContentCatalogsVideoSelectedHeightItem {
      get { return Properties.Settings.Default.ContentCatalogsVideoSelectedHeightItem; }
      set {
        Properties.Settings.Default.ContentCatalogsVideoSelectedHeightItem = value;
        Properties.Settings.Default.Save();
        OnPropertyChanged();
      }
    }
    public int ContentMainVideoOneToListSelectedWidth {
      get { return Properties.Settings.Default.ContentMainVideoOneToListSelectedWidth; }
      set {
        Properties.Settings.Default.ContentMainVideoOneToListSelectedWidth = value;
        Properties.Settings.Default.Save();
        OnPropertyChanged();
      }
    }
    public int ContentMainVideoOneToListSelectedHeight {
      get { return Properties.Settings.Default.ContentMainVideoOneToListSelectedHeight; }
      set {
        Properties.Settings.Default.ContentMainVideoOneToListSelectedHeight = value;
        Properties.Settings.Default.Save();
        OnPropertyChanged();
      }
    }
    public int ContentFoldersVideoOneToListSelectedWidth {
      get { return Properties.Settings.Default.ContentFoldersVideoOneToListSelectedWidth; }
      set {
        Properties.Settings.Default.ContentFoldersVideoOneToListSelectedWidth = value;
        Properties.Settings.Default.Save();
        OnPropertyChanged();
      }
    }
    public int ContentFoldersVideoOneToListSelectedHeight {
      get { return Properties.Settings.Default.ContentFoldersVideoOneToListSelectedHeight; }
      set {
        Properties.Settings.Default.ContentFoldersVideoOneToListSelectedHeight = value;
        Properties.Settings.Default.Save();
        OnPropertyChanged();
      }
    }
    public int ContentCatalogsVideoOneToListSelectedWidth {
      get { return Properties.Settings.Default.ContentCatalogsVideoOneToListSelectedWidth; }
      set {
        Properties.Settings.Default.ContentCatalogsVideoOneToListSelectedWidth = value;
        Properties.Settings.Default.Save();
        OnPropertyChanged();
      }
    }
    public int ContentCatalogsVideoOneToListSelectedHeight {
      get { return Properties.Settings.Default.ContentCatalogsVideoOneToListSelectedHeight; }
      set {
        Properties.Settings.Default.ContentCatalogsVideoOneToListSelectedHeight = value;
        Properties.Settings.Default.Save();
        OnPropertyChanged();
      }
    }
    public bool IsChangeSizeItemSeparatelyMainImage {
      get { return Properties.Settings.Default.IsChangeSizeItemSeparatelyMainImage; }
      set {
        Properties.Settings.Default.IsChangeSizeItemSeparatelyMainImage = value;
        Properties.Settings.Default.Save();
        OnPropertyChanged();
      }
    }
    public bool IsChangeSizeItemSeparatelyMainVideo {
      get { return Properties.Settings.Default.IsChangeSizeItemSeparatelyMainVideo; }
      set {
        Properties.Settings.Default.IsChangeSizeItemSeparatelyMainVideo = value;
        Properties.Settings.Default.Save();
        OnPropertyChanged();
      }
    }
    public bool IsChangeSizeItemSeparatelyFoldersImage {
      get { return Properties.Settings.Default.IsChangeSizeItemSeparatelyFoldersImage; }
      set {
        Properties.Settings.Default.IsChangeSizeItemSeparatelyFoldersImage = value;
        Properties.Settings.Default.Save();
        OnPropertyChanged();
      }
    }
    public bool IsChangeSizeItemSeparatelyFoldersVideo {
      get { return Properties.Settings.Default.IsChangeSizeItemSeparatelyFoldersVideo; }
      set {
        Properties.Settings.Default.IsChangeSizeItemSeparatelyFoldersVideo = value;
        Properties.Settings.Default.Save();
        OnPropertyChanged();
      }
    }
    public bool IsChangeSizeItemSeparatelyCatalogImage {
      get { return Properties.Settings.Default.IsChangeSizeItemSeparatelyCatalogImage; }
      set {
        Properties.Settings.Default.IsChangeSizeItemSeparatelyCatalogImage = value;
        Properties.Settings.Default.Save();
        OnPropertyChanged();
      }
    }
    public bool IsChangeSizeItemSeparatelyCatalogVideo {
      get { return Properties.Settings.Default.IsChangeSizeItemSeparatelyCatalogVideo; }
      set {
        Properties.Settings.Default.IsChangeSizeItemSeparatelyCatalogVideo = value;
        Properties.Settings.Default.Save();
        OnPropertyChanged();
      }
    }

    public double RatioMainImageChangeSizeProportionally {
      get { return Properties.Settings.Default.RatioMainImageChangeSizeProportionally; }
      set {
        Properties.Settings.Default.RatioMainImageChangeSizeProportionally = value;
        Properties.Settings.Default.Save();
        OnPropertyChanged();
      }
    }
    public double RatioMainVideoChangeSizeProportionally {
      get { return Properties.Settings.Default.RatioMainVideoChangeSizeProportionally; }
      set {
        Properties.Settings.Default.RatioMainVideoChangeSizeProportionally = value;
        Properties.Settings.Default.Save();
        OnPropertyChanged();
      }
    }
    public double RatioFoldersImageChangeSizeProportionally {
      get { return Properties.Settings.Default.RatioFoldersImageChangeSizeProportionally; }
      set {
        Properties.Settings.Default.RatioFoldersImageChangeSizeProportionally = value;
        Properties.Settings.Default.Save();
        OnPropertyChanged();
      }
    }
    public double RatioFoldersVideoChangeSizeProportionally {
      get { return Properties.Settings.Default.RatioFoldersVideoChangeSizeProportionally; }
      set {
        Properties.Settings.Default.RatioFoldersVideoChangeSizeProportionally = value;
        Properties.Settings.Default.Save();
        OnPropertyChanged();
      }
    }
    public double RatioCatalogImageChangeSizeProportionally {
      get { return Properties.Settings.Default.RatioCatalogImageChangeSizeProportionally; }
      set {
        Properties.Settings.Default.RatioCatalogImageChangeSizeProportionally = value;
        Properties.Settings.Default.Save();
        OnPropertyChanged();
      }
    }
    public double RatioCatalogVideoChangeSizeProportionally {
      get { return Properties.Settings.Default.RatioCatalogVideoChangeSizeProportionally; }
      set {
        Properties.Settings.Default.RatioCatalogVideoChangeSizeProportionally = value;
        Properties.Settings.Default.Save();
        OnPropertyChanged();
      }
    }
    public bool IsEnableRefreshContentFolders {
      get { return Properties.Settings.Default.IsEnableRefreshContentFolders; }
      set {
        Properties.Settings.Default.IsEnableRefreshContentFolders = value;
        Properties.Settings.Default.Save();
        OnPropertyChanged();
      }
    }
    public bool IsEnableRefreshContentCatalogs {
      get { return Properties.Settings.Default.IsEnableRefreshContentCatalogs; }
      set {
        Properties.Settings.Default.IsEnableRefreshContentCatalogs = value;
        Properties.Settings.Default.Save();
        OnPropertyChanged();
      }
    }
    public int ImageItemSizeInListMin => 50;
    public int ImageItemSizeInListMax => 1000;
    public int ImageMainItemSizeOneFromListMin => 800;
    public int ImageMainItemSizeOneFromListMax => 5000;
    public int ImageDicItemSizeOneFromListMin => 600;
    public int ImageDicItemSizeOneFromListMax => 1920;
    public int VideoItemSizeInListMin => 200;
    public int VideoItemSizeInListMax => 1280;
    public int VideoMainItemSizeOneFromListMin => 800;
    public int VideoMainItemSizeOneFromListtMax => 4096;
    public int VideoDicItemSizeOneFromListMin => 600;
    public int VideoDicItemSizeOneFromListtMax => 1920;
    public int NormalizedSizeImageIndexDefaul => 1;
    public int NormalizedSizeVideoIndexDefaul => 1;
    public int SizePageImageLoadIndexDefault => 1;
    public int SizePageVideoLoadIndexDefault => 1;
    public int SizePageImageLoadMin => 100;
    public int SizePageImageLoadMax => 1000;
    public int SizePageImageLoadStep => 50;
    public int SizePageVideoLoadMin => 10;
    public int SizePageVideoLoadMax => 100;
    public int SizePageVideoLoadStep => 5;
    public int NormalizedSizeImageMainIndex {
      get { return Properties.Settings.Default.NormalizedSizeImageMainIndex; }
      set {
        Properties.Settings.Default.NormalizedSizeImageMainIndex = value;
        Properties.Settings.Default.Save();
        OnPropertyChanged();
      }
    }
    public int NormalizedSizeImageFoldersIndex {
      get { return Properties.Settings.Default.NormalizedSizeImageFoldersIndex; }
      set {
        Properties.Settings.Default.NormalizedSizeImageFoldersIndex = value;
        Properties.Settings.Default.Save();
        OnPropertyChanged();
      }
    }
    public int NormalizedSizeImageCatalogsIndex {
      get { return Properties.Settings.Default.NormalizedSizeImageCatalogsIndex; }
      set {
        Properties.Settings.Default.NormalizedSizeImageCatalogsIndex = value;
        Properties.Settings.Default.Save();
        OnPropertyChanged();
      }
    }
    public int SizePageImageLoadMainIndex {
      get { return Properties.Settings.Default.SizePageImageLoadMainIndex; }
      set {
        Properties.Settings.Default.SizePageImageLoadMainIndex = value;
        Properties.Settings.Default.Save();
        OnPropertyChanged();
      }
    }
    public int SizePageImageLoadFoldersIndex {
      get { return Properties.Settings.Default.SizePageImageLoadFoldersIndex; }
      set {
        Properties.Settings.Default.SizePageImageLoadFoldersIndex = value;
        Properties.Settings.Default.Save();
        OnPropertyChanged();
      }
    }
    public int SizePageImageLoadCatalogsIndex {
      get { return Properties.Settings.Default.SizePageImageLoadCatalogsIndex; }
      set {
        Properties.Settings.Default.SizePageImageLoadCatalogsIndex = value;
        Properties.Settings.Default.Save();
        OnPropertyChanged();
      }
    }
    public int SizePageVideoLoadMainIndex {
      get { return Properties.Settings.Default.SizePageVideoLoadMainIndex; }
      set {
        Properties.Settings.Default.SizePageVideoLoadMainIndex = value;
        Properties.Settings.Default.Save();
        OnPropertyChanged();
      }
    }
    public int SizePageVideoLoadFoldersIndex {
      get { return Properties.Settings.Default.SizePageVideoLoadFoldersIndex; }
      set {
        Properties.Settings.Default.SizePageVideoLoadFoldersIndex = value;
        Properties.Settings.Default.Save();
        OnPropertyChanged();
      }
    }
    public int SizePageVideoLoadCatalogsIndex {
      get { return Properties.Settings.Default.SizePageVideoLoadCatalogsIndex; }
      set {
        Properties.Settings.Default.SizePageVideoLoadCatalogsIndex = value;
        Properties.Settings.Default.Save();
        OnPropertyChanged();
      }
    }
    public bool IsKindContentMain {
      get { return Properties.Settings.Default.IsKindContentMain; }
      set {
        Properties.Settings.Default.IsKindContentMain = value;
        Properties.Settings.Default.Save();
        OnPropertyChanged();
      }
    }
    public bool IsKindContentFolders {
      get { return Properties.Settings.Default.IsKindContentFolders; }
      set {
        Properties.Settings.Default.IsKindContentFolders = value;
        Properties.Settings.Default.Save();
        OnPropertyChanged();
      }
    }
    public bool IsKindContentCatalogs {
      get { return Properties.Settings.Default.IsKindContentCatalogs; }
      set {
        Properties.Settings.Default.IsKindContentCatalogs = value;
        Properties.Settings.Default.Save();
        OnPropertyChanged();
      }
    }
    public bool IsModeShowMainImage {
      get { return Properties.Settings.Default.IsModeShowMainImage; }
      set {
        Properties.Settings.Default.IsModeShowMainImage = value;
        Properties.Settings.Default.Save();
        OnPropertyChanged();
      }
    }
    public bool IsModeShowFoldersImage {
      get { return Properties.Settings.Default.IsModeShowFoldersImage; }
      set {
        Properties.Settings.Default.IsModeShowFoldersImage = value;
        Properties.Settings.Default.Save();
        OnPropertyChanged();
      }
    }
    public bool IsModeShowCatalogsImage {
      get { return Properties.Settings.Default.IsModeShowCatalogsImage; }
      set {
        Properties.Settings.Default.IsModeShowCatalogsImage = value;
        Properties.Settings.Default.Save();
        OnPropertyChanged();
      }
    }
    public bool IsModeShowMainVideo {
      get { return Properties.Settings.Default.IsModeShowMainVideo; }
      set {
        Properties.Settings.Default.IsModeShowMainVideo = value;
        Properties.Settings.Default.Save();
        OnPropertyChanged();
      }
    }
    public bool IsModeShowFoldersVideo {
      get { return Properties.Settings.Default.IsModeShowFoldersVideo; }
      set {
        Properties.Settings.Default.IsModeShowFoldersVideo = value;
        Properties.Settings.Default.Save();
        OnPropertyChanged();
      }
    }
    public bool IsModeShowCatalogsVideo {
      get { return Properties.Settings.Default.IsModeShowCatalogsVideo; }
      set {
        Properties.Settings.Default.IsModeShowCatalogsVideo = value;
        Properties.Settings.Default.Save();
        OnPropertyChanged();
      }
    }
    #endregion

    public string AppImageUrl => Properties.Settings.Default.AppImage;
    public string ImageUidRegularAddRoot => "/Kaleidoscope;component/Resourses/UID/Regular/Symbol Add 2 root.png";
    public string ImageUidRegularAdd => "/Kaleidoscope;component/Resourses/UID/Regular/Symbol Add 2.png";
    public string ImageUidRegularDel => "/Kaleidoscope;component/Resourses/UID/Regular/Symbol Delete 2.png";
    public string ImageUidRegularUpd => "/Kaleidoscope;component/Resourses/UID/Regular/Symbol Edit 2.png";
    public string ImageUidRegularReplace => "/Kaleidoscope;component/Resourses/UID/Regular/replace.png";
    public string ImageUidRegularSync => "/Kaleidoscope;component/Resourses/UID/Regular/Sync-icon.png";
    public string ImageCommandsFindDel => "/Kaleidoscope;component/Resourses/Commands/Find Delete.png";
    public string ImageFolderContent => "/Kaleidoscope;component/Resourses/Folder-Content.png";
    public string ImagePhotos => "/Kaleidoscope;component/Resourses/Photos-icon.png";
    public string ImageMovies => "/Kaleidoscope;component/Resourses/Movies.png";
    public string ImageSizeBoth => "/Kaleidoscope;component/Resourses/size_both_32.png";
    public string ImageSizeSeparately => "/Kaleidoscope;component/Resourses/size_separately_32.png";
    public string ImageSizeHeight => "/Kaleidoscope;component/Resourses/size_height_32.png";
    public string ImageSizeWidth => "/Kaleidoscope;component/Resourses/size_width_32.png";
    public string ImageSizeHeightDark => "/Kaleidoscope;component/Resourses/size_height_dark32.png";
    public string ImageSizeWidthDark => "/Kaleidoscope;component/Resourses/size_width_dark32.png";
    public string ImageModeShowContentList => "/Kaleidoscope;component/Resourses/modeShowContent_List_32.png";
    public string ImageModeShowContentOneFromList => "/Kaleidoscope;component/Resourses/modeShowContent_OneFromListl_32.png";
    public string ImageEnableRefreshContent => "/Kaleidoscope;component/Resourses/reload_refresh.png";
    public string ImageDisableRefreshContent => "/Kaleidoscope;component/Resourses/dont refresh.png";
    public string ImageContentImages => "/Kaleidoscope;component/Resourses/images.png";
    public string ImageContentMovies => "/Kaleidoscope;component/Resourses/movie.png";
    #endregion

    #region Methods
    public void SetPalette() {
      Swatch primary = null;
      Swatch accent = null;
      IEnumerable<Swatch> swatches = new SwatchesProvider().Swatches;
      foreach (var item in swatches) {
        if (item.Name == AppPalettePrimaryName) primary = item;
        if (item.Name == AppPaletteAccentName) accent = item;
      }
      if (primary != null) new PaletteHelper().ReplacePrimaryColor(primary);
      if (accent != null) new PaletteHelper().ReplaceAccentColor(accent); 
    }
    public int GetNormalizedSizeImageIndex(ContentWorkMode mode) {
      return mode == ContentWorkMode.Main ? Settings.Instance.NormalizedSizeImageMainIndex :
             mode == ContentWorkMode.Folders ? Settings.Instance.NormalizedSizeImageFoldersIndex :
             mode == ContentWorkMode.Catalogs ? Settings.Instance.NormalizedSizeImageCatalogsIndex :
             NormalizedSizeImageIndexDefaul;
    }
    public int GetSizePageImageLoadIndex(ContentWorkMode mode) {
      return mode == ContentWorkMode.Main ? Settings.Instance.SizePageImageLoadMainIndex :
             mode == ContentWorkMode.Folders ? Settings.Instance.SizePageImageLoadFoldersIndex :
             mode == ContentWorkMode.Catalogs ? Settings.Instance.SizePageImageLoadCatalogsIndex :
             SizePageImageLoadIndexDefault;
    }
    public int GetSizePageVideoLoadIndex(ContentWorkMode mode) {
      return mode == ContentWorkMode.Main ? Settings.Instance.SizePageVideoLoadMainIndex :
             mode == ContentWorkMode.Folders ? Settings.Instance.SizePageVideoLoadFoldersIndex :
             mode == ContentWorkMode.Catalogs ? Settings.Instance.SizePageVideoLoadCatalogsIndex :
             SizePageVideoLoadIndexDefault;
    }
    public int GetNormalizedImageWidth(ContentWorkMode mode) {
      int size = 160;
      NormalizeSize enumSizeImage = (NormalizeSize)Enum.GetValues(typeof(NormalizeSize)).GetValue(GetNormalizedSizeImageIndex(mode));
      size = enumSizeImage == NormalizeSize.S160x120 ? 160 :
             enumSizeImage == NormalizeSize.S314x235 ? 314 :
             enumSizeImage == NormalizeSize.S448x336 ? 448 :
             enumSizeImage == NormalizeSize.S640x480 ? 640 :
             enumSizeImage == NormalizeSize.S800x600 ? 800 :
             enumSizeImage == NormalizeSize.S1024x768 ? 1024 : size;
      return size;
    }
    public int GetNormalizedImageHeight(ContentWorkMode mode) {
      int size = 160;
      NormalizeSize enumSizeImage = (NormalizeSize)Enum.GetValues(typeof(NormalizeSize)).GetValue(GetNormalizedSizeImageIndex(mode));
      size = enumSizeImage == NormalizeSize.S160x120 ? 120 :
             enumSizeImage == NormalizeSize.S314x235 ? 235 :
             enumSizeImage == NormalizeSize.S448x336 ? 336 :
             enumSizeImage == NormalizeSize.S640x480 ? 480 :
             enumSizeImage == NormalizeSize.S800x600 ? 600 :
             enumSizeImage == NormalizeSize.S1024x768 ? 768 : size;
      return size;
    }
    public int GetSizePageImageLoad(ContentWorkMode mode) {
      return SizePageImageLoadMin + GetSizePageImageLoadIndex(mode) * SizePageImageLoadStep;
    }
    public int GetSizePageVideoLoad(ContentWorkMode mode) {
      return SizePageVideoLoadMin + GetSizePageVideoLoadIndex(mode) * SizePageVideoLoadStep;
    }

    #endregion

  }
}
