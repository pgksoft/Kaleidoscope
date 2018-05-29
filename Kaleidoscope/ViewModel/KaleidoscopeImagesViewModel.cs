using Kaleidoscope.Helpers;
using Kaleidoscope.Model;
using Kaleidoscope.View;
using LocalizatorHelper;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Kaleidoscope.ViewModel {
  public class KaleidoscopeImagesViewModel : ViewModelBaseDialogHost {
    public KaleidoscopeImagesViewModel() {
      try {
        MainWindowViewModel.LocalizationChangedEvent += () => ContentMenuItemsLocalization();
        ContentMenuItemsInit();
        SelectedIndexContentMenu = 0;
        _db = Settings.Same().ContentDB;
      } catch (Exception e) {
        Settings.Same().AppStatus = TaskStatus.Faulted;
        ErrorProcessing.Show(e);
        (Application.Current as App).Shutdown();
      }
    }
    public KaleidoscopeImagesViewModel(DialogSession sessionMainDialogHost) : this() {
      _sessionMainDialogHost = sessionMainDialogHost;
    }
    public KaleidoscopeImagesViewModel(UserControl headerControl, HorizontalAlignment headerControlHorizontalAlignment) : this() {
      HeaderControl = headerControl;
      HeaderControlHorizontalAlignment = headerControlHorizontalAlignment;
    }

    #region Fields
    private ContentDB _db;
    private LocalisationHelper _localisationHelper = new LocalisationHelper();
    private ObservableCollection<ContentMenuItem> _contentMenuItems;
    private int _selectedIndexContentMenu;
    private int _selectedIndexContentMenuOld;
    #endregion

    #region Properties
    public ObservableCollection<ContentMenuItem> ContentMenuItems { get { return _contentMenuItems; } }
    public int SelectedIndexContentMenu {
      get { return _selectedIndexContentMenu; }
      set {
        _selectedIndexContentMenuOld = _selectedIndexContentMenu;
        this.MutateVerbose(ref _selectedIndexContentMenu, value, RaisePropertyChanged());
        RemovePreviousUserControl();
        OnSelectedIndexContentMenu();
      }
    }
    #endregion

    #region Event implementatiom
    #endregion

    #region Comands
    #endregion

    #region Commands implementation
    #endregion

    #region Helpers
    private void OnSelectedIndexContentMenu() {
      // Create new UserControl CONTENT 
      if (ContentMenuItems[SelectedIndexContentMenu].Code == ContentMenuItemsCode.Content) {
        HeaderControl = new KaleidoscopeImagesHeader() { DataContext = new KaleidoscopeImagesHeaderViewModel() };
        HeaderControlHorizontalAlignment = HorizontalAlignment.Center;
        if (ContentMenuItems[SelectedIndexContentMenu].Content == null) {
          ContentMenuItems[SelectedIndexContentMenu].Content = new Content() {
            DataContext = new ContentViewModel(ContentWorkMode.Main) 
          };
        }
        // Create new UserControl FOLDERS
      } else if (ContentMenuItems[SelectedIndexContentMenu].Code == ContentMenuItemsCode.Folders) {
        HeaderControl = new ContentMainHeaderControl() { DataContext = new ContentMainHeaderControlViewModel("ContentFoldersRes.MainHeaderTitle") };
        HeaderControlHorizontalAlignment = HorizontalAlignment.Center;
        if (ContentMenuItems[SelectedIndexContentMenu].Content == null) {
          ContentMenuItems[SelectedIndexContentMenu].Content = new Folders() {
            DataContext = new FoldersViewModel<RefPathway>(SessionMainDialogHost, "MainDialog", _db, 400, DataGridSelectionMode.Extended)
          };
        }
        // Create new UserControl CATALOG
      } else if (ContentMenuItems[SelectedIndexContentMenu].Code == ContentMenuItemsCode.Catalog) {
        HeaderControl = new ContentMainHeaderControl() { DataContext = new ContentMainHeaderControlViewModel("ContentCatalogRes.MainHeaderTitle") };
        HeaderControlHorizontalAlignment = HorizontalAlignment.Center;
        if (ContentMenuItems[SelectedIndexContentMenu].Content == null) {
          ContentMenuItems[SelectedIndexContentMenu].Content = new View.Catalog() {
            DataContext = new CatalogViewModel(SessionMainDialogHost, _db)
          };
        }
        // Create new UserControl CATALOG ITEMS
      } else if (ContentMenuItems[SelectedIndexContentMenu].Code == ContentMenuItemsCode.CatalogItems) {
        HeaderControl = new ContentMainHeaderControl() { DataContext = new ContentMainHeaderControlViewModel("ContentCatalogItemsRes.MainHeaderTitle") };
        HeaderControlHorizontalAlignment = HorizontalAlignment.Center;
        if (ContentMenuItems[SelectedIndexContentMenu].Content == null) {
          ContentMenuItems[SelectedIndexContentMenu].Content = new Directories() {
            DataContext = new DirectoriesViewModel<RefNode>(SessionMainDialogHost, "MainDialog", _db, 400, DataGridSelectionMode.Single)
          };
        }
        // Create new UserControl IMAGE TYPES
      } else if (ContentMenuItems[SelectedIndexContentMenu].Code == ContentMenuItemsCode.ImageTypes) {
        HeaderControl = new ContentMainHeaderControl() { DataContext = new ContentMainHeaderControlViewModel("ContentImageTypesRes.MainHeaderTitle") };
        HeaderControlHorizontalAlignment = HorizontalAlignment.Center;
        ContentMenuItems[SelectedIndexContentMenu].Content = new Directories() {
          DataContext = new DirectoriesViewModel<RefImageType>(SessionMainDialogHost, "MainDialog", _db, 200, DataGridSelectionMode.Single)
        };
        // Create new UserControl VIDEO TYPES
      } else if (ContentMenuItems[SelectedIndexContentMenu].Code == ContentMenuItemsCode.VideoTypes) {
        HeaderControl = new ContentMainHeaderControl() { DataContext = new ContentMainHeaderControlViewModel("ContentVideoTypesRes.MainHeaderTitle") };
        HeaderControlHorizontalAlignment = HorizontalAlignment.Center;
        ContentMenuItems[SelectedIndexContentMenu].Content = new Directories() {
          DataContext = new DirectoriesViewModel<RefVideoType>(SessionMainDialogHost, "MainDialog", _db, 200, DataGridSelectionMode.Single)
        };
        // Create new UserControl Comments
      } else if (ContentMenuItems[SelectedIndexContentMenu].Code == ContentMenuItemsCode.Comments) {
        HeaderControl = new ContentMainHeaderControl() { DataContext = new ContentMainHeaderControlViewModel("ContentCommentsRes.MainHeaderTitle") };
        HeaderControlHorizontalAlignment = HorizontalAlignment.Center;
        ContentMenuItems[SelectedIndexContentMenu].Content = new Directories() {
          DataContext = new DirectoriesViewModel<RefComment>(SessionMainDialogHost, "MainDialog", _db, 600, DataGridSelectionMode.Single)
        };
      }
    }

    private void RemovePreviousUserControl() {
      if (_selectedIndexContentMenuOld != _selectedIndexContentMenu && ContentMenuItems[_selectedIndexContentMenuOld].Code != ContentMenuItemsCode.Content) {
        ContentMenuItems[_selectedIndexContentMenuOld].Dispose();
        (HeaderControl.DataContext as ContentMainHeaderControlViewModel).Dispose();
        //  // Create new UserControl FOLDERS
        //if (ContentMenuItems[SelectedIndexContentMenu].Code == ContentMenuItemsCode.Folders) {
        //  // Create new UserControl CATALOG
        //} else if (ContentMenuItems[SelectedIndexContentMenu].Code == ContentMenuItemsCode.Catalog) {
        //  // Create new UserControl CATALOG ITEMS
        //} else if (ContentMenuItems[SelectedIndexContentMenu].Code == ContentMenuItemsCode.CatalogItems) {
        //  // Create new UserControl IMAGE TYPES
        //} else if (ContentMenuItems[SelectedIndexContentMenu].Code == ContentMenuItemsCode.ImageTypes) {
        //  // Create new UserControl VIDEO TYPES
        //} else if (ContentMenuItems[SelectedIndexContentMenu].Code == ContentMenuItemsCode.VideoTypes) {
        //};
      }
    }
    private void ContentMenuItemsLocalization() {
      if (ContentMenuItems?.Count > 0) {
        ContentMenuItems[0].Name = _localisationHelper["ImagesRes.ContentMenuItemContent"];
        ContentMenuItems[1].Name = _localisationHelper["ImagesRes.ContentMenuItemFolders"];
        ContentMenuItems[2].Name = _localisationHelper["ImagesRes.ContentMenuItemCatalog"];
        ContentMenuItems[3].Name = _localisationHelper["ImagesRes.ContentMenuItemCatalogItems"];
        ContentMenuItems[4].Name = _localisationHelper["ImagesRes.ContentMenuItemImageTypes"];
        ContentMenuItems[5].Name = _localisationHelper["ImagesRes.ContentMenuItemVideoTypes"];
        ContentMenuItems[6].Name = _localisationHelper["ImagesRes.ContentMenuItemComments"];
      }
    }
    private void ContentMenuItemsInit() {
      _contentMenuItems = new ObservableCollection<ContentMenuItem> {
        new ContentMenuItem(
          ContentMenuItemsCode.Content,
          _localisationHelper["ImagesRes.ContentMenuItemContent"],
          null
        ),
        new ContentMenuItem(
          ContentMenuItemsCode.Folders,
          _localisationHelper["ImagesRes.ContentMenuItemFolders"],
          null
        ),
        new ContentMenuItem(
          ContentMenuItemsCode.Catalog,
          _localisationHelper["ImagesRes.ContentMenuItemCatalog"],
          null
        ),
        new ContentMenuItem(
          ContentMenuItemsCode.CatalogItems,
          _localisationHelper["ImagesRes.ContentMenuItemCatalogItems"],
          null
        ),
        new ContentMenuItem(
          ContentMenuItemsCode.ImageTypes,
          _localisationHelper["ImagesRes.ContentMenuItemImageTypes"],
          null
        ),
        new ContentMenuItem(
          ContentMenuItemsCode.VideoTypes,
          _localisationHelper["ImagesRes.ContentMenuItemVideoTypes"],
          null
        ),
        new ContentMenuItem(
          ContentMenuItemsCode.Comments,
          _localisationHelper["ImagesRes.ContentMenuItemComments"],
          null
        ),
      };
    }
    protected override void OnDispose() {
      base.OnDispose();
      _db?.Dispose();
    }
    #endregion
  }
}
