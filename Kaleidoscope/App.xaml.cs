using Kaleidoscope.Helpers;
using Kaleidoscope.Localization;
using LocalizatorHelper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace Kaleidoscope {
  /// <summary>
  /// Логика взаимодействия для App.xaml
  /// </summary>
  public partial class App : Application {
    public App() : base() {
      // Регистрируем ресурсы локализации
      ResourceManagerService.RegisterManager("MainWindowRes", MainWindowRes.ResourceManager, false);
      ResourceManagerService.RegisterManager("MessageConfirmRes", MessageConfirmRes.ResourceManager, false);
      ResourceManagerService.RegisterManager("ImagesRes", ImagesRes.ResourceManager, false);
      ResourceManagerService.RegisterManager("ContentFoldersRes", ContentFoldersRes.ResourceManager, false);
      ResourceManagerService.RegisterManager("ContentCatalogRes", ContentCatalogRes.ResourceManager, false);
      ResourceManagerService.RegisterManager("ContentCatalogItemsRes", ContentCatalogItemsRes.ResourceManager, false);
      ResourceManagerService.RegisterManager("ContentImageTypesRes", ContentImageTypesRes.ResourceManager, false);
      ResourceManagerService.RegisterManager("ContentVideoTypesRes", ContentVideoTypesRes.ResourceManager, false);
      ResourceManagerService.RegisterManager("ContentCommentsRes", ContentCommentsRes.ResourceManager, false);
      ResourceManagerService.RegisterManager("SettingOptionsRes", SettingOptionsRes.ResourceManager, false);
      ResourceManagerService.RegisterManager("ColorsRes", ColorsRes.ResourceManager, false);
      ResourceManagerService.RegisterManager("TicTacToeRes", TicTacToeRes.ResourceManager, false);
      ResourceManagerService.RegisterManager("DMLRes", DMLRes.ResourceManager, false);
      
      // Current status
      Settings.Same().AppStatus = TaskStatus.Running;

      this.DispatcherUnhandledException += new DispatcherUnhandledExceptionEventHandler(App_DispatcherUnhandledException);
    }
    #region Fields
    private LocalisationHelper _localisationHelper = new LocalisationHelper();
    #endregion

    #region AppEvents copmlementation
    void App_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e) {
      try {
        //Debug.WriteLine("------> DispatcherUnhandledException");
        ErrorProcessing.Show(e.Exception, _localisationHelper["MainWindowRes.AppDispatcherUnhandledExceptionTitleError"]);
        e.Handled = true; // помечаем необработанное исключение, как обработанное.
        Settings.Same().AppStatus = TaskStatus.Faulted;
        Shutdown();
      } catch (Exception eLocal) {
        ErrorProcessing.Show(eLocal);
      }
    }
    #endregion
  }
}
