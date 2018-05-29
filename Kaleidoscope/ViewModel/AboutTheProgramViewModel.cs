using Kaleidoscope.Helpers;
using Kaleidoscope.Localization;
using Kaleidoscope.Model;
using LocalizatorHelper;
using MaterialDesignColors;
using RandomFill;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Kaleidoscope.ViewModel {
  class AboutTheProgramViewModel : ViewModelBase {
    public AboutTheProgramViewModel(UserControl headerControl) {
      _headerControl = headerControl;
      TestCommand = new DelegateCommand(o => TestSwatches());
      TestButtonVisibility = Settings.Same().ButtonTestVisibility; // ТЕСТОВАЯ КНОПКА
      ResourceManagerService.RegisterManager("AboutTheProgramRes", AboutTheProgramRes.ResourceManager, true);
    }
    #region Fields
    private RandomFills _reandomFill = RandomFills.Instance();
    private Visibility _testButtonVisibility = Visibility.Hidden;
    #endregion

    #region Properties
    public Visibility TestButtonVisibility {
      get { return _testButtonVisibility; }
      set { this.MutateVerbose(ref _testButtonVisibility, value, RaisePropertyChanged()); }
    }
    #endregion

    #region Commands
    public DelegateCommand TestCommand { get; set; }
    #endregion

    #region Commands implementation
    private void TestSwatches() {
      using (ContentDB db = new ContentDB()) {
        //var query = db.Database.SqlQuery<VRefPathway>("SELECT * FROM VRefPathways");
        //db.VRefPathways.Load();
        string temp = string.Empty;
        MessageConfirmBox.Show("",
                               $"NormalizedSizeMain {Settings.Instance.GetNormalizedImageWidth(ContentWorkMode.Main)}" +
                               $"\nNormalizedSizeFolders {Settings.Instance.GetNormalizedImageWidth(ContentWorkMode.Folders)}" +
                               $"\nSizePageMain {Settings.Instance.GetSizePageImageLoad(ContentWorkMode.Main)}" +
                               $"\nSizePageFolders {Settings.Instance.GetSizePageImageLoad(ContentWorkMode.Folders)}");
      }
      //MessageConfirmBox.Show(Settings.Instance.AppImageUrl,$"{ModelUtilities.Instance.GetId<RefPathway>(@"C:\PgkPrivate\Private\ФОТО РОДНЫЕ1")}", "Test");
    }
    #endregion

  }
}
