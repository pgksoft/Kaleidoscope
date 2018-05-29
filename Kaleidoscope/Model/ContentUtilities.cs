using Kaleidoscope.Helpers;
using Kaleidoscope.HelpersViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Kaleidoscope.Model {
  public sealed class ContentUtilities {

    private ContentUtilities() {
    }

    #region Fields
    private static ContentUtilities _instance;
    private string _folder;
    private DML _originalOperation;
    private Action<string> _insert;
    private Action<string> _update;
    private int _countAddFolders;
    private int _countAddFiles;
    private int _countCheckFiles;
    private int _countDelRegisteresFiles;
    private int _countEmptyFolders;
    private BackgroundWorker _workerSyncFolder;
    #endregion

    #region Properties
    public static ContentUtilities Instance {
      get { return _instance ?? (Instance = new ContentUtilities()); }
      private set { _instance = value; }
    }
    public IEnumerable<PatternSearchFile> ListSearchPatternImageFiles => GetListSearchPatternImageFiles();
    public IEnumerable<PatternSearchFile> ListSearchPatternVideoFiles => GetListSearchPatternVideoFiles();
    public IEnumerable<PatternSearchFile> ListSearchPatternContentFiles => GetListSearchPatternContentFiles();
    public BackgroundWorker WorkerSyncFolder => _workerSyncFolder;
    #endregion

    #region Events
    public event Action<string> SynchronizationFolderEvent;
    public event Action SynchronizationFolderCompletedEvent;
    #endregion

    #region Methods
    public void SetImageRotate(ListContent item) {
      try {
        if (item.NProperty != null) {
          // Update
          ContentProperty rotateProperty = Settings.Instance.ContentDB.ContentProperties.Find(item.NProperty);
          if (rotateProperty != null) {
            rotateProperty.Rotate = (int)item.NRotate;
            Settings.Instance.ContentDB.Entry(rotateProperty).State = EntityState.Modified;
            Settings.Instance.ContentDB.SaveChanges();
          }
        } else {
          // Insert
          ContentProperty rotateProperty = new ContentProperty {
            Content = item.Id,
            Rotate = (int)item.NRotate
          };
          Settings.Instance.ContentDB.ContentProperties.Add(rotateProperty);
          Settings.Instance.ContentDB.SaveChanges();
          item.NProperty = GetIdContentProperties(item.Id);
        }
      } catch (Exception e) {
        ErrorProcessing.Show(e);
      }
    }
    public void SynchronizationFolder(string folder, DML originalOperation, Action<string> insert, Action<string> update) {
      _folder = folder;
      _originalOperation = originalOperation;
      _insert = insert;
      _update = update;
      _countAddFolders = 0;
      _countAddFiles = 0;
      _countCheckFiles = 0;
      _countDelRegisteresFiles = 0;
      _countEmptyFolders = 0;
      _workerSyncFolder = new BackgroundWorker();
      _workerSyncFolder.DoWork += new DoWorkEventHandler(Worker_DoWorkSync);
      _workerSyncFolder.RunWorkerCompleted += new RunWorkerCompletedEventHandler(Worker_RunWorkerSyncCompleted);
      _workerSyncFolder.ProgressChanged += new ProgressChangedEventHandler(Worker_ProgressSyncChanged);
      _workerSyncFolder.WorkerReportsProgress = true;
      _workerSyncFolder.WorkerSupportsCancellation = true;
      _workerSyncFolder.RunWorkerAsync();
    }
    private void Worker_DoWorkSync(object sender, DoWorkEventArgs e) {
      try {
        Stack<DirectoryInfo> stackDir = new Stack<DirectoryInfo>();
        DirectoryInfo dir = new DirectoryInfo(_folder);
        int idPath;
        string nameFile = string.Empty;
        if (dir.Exists) {
          // Фиксируем выбранную папку
          if (_originalOperation == DML.Insert) {
            _workerSyncFolder.ReportProgress(0, _folder);
            _insert.Invoke(_folder);
          } else if (_originalOperation == DML.Update) {
            _update.Invoke(_folder);
            _workerSyncFolder.ReportProgress(0, _folder);
          }
          stackDir.Push(dir);
          while (stackDir.Count > 0) {
            if (_workerSyncFolder.CancellationPending) {
              e.Cancel = true;
              return;
            }
            dir = stackDir.Pop();
            _workerSyncFolder.ReportProgress(0, dir.FullName);
            // Формируем список вложенных папок
            foreach (var item in dir.EnumerateDirectories()) {
              stackDir.Push(item);
            }
            idPath = ModelUtilities.Instance.GetId<RefPathway>(dir.FullName);
            if (idPath == 0) {
              if (IsContainsFiles(dir)) {
                _insert.Invoke(dir.FullName);
                idPath = ModelUtilities.Instance.GetId<RefPathway>(dir.FullName);
                if (idPath == 0) {
                  throw new InvalidOperationException(dir.FullName);
                }
                _countAddFolders++;
              } else {
                _countEmptyFolders++;
                continue;
              }
            }
            // Синхронизация файлов
            // Проверка наличия зарегистрированных файлов
            CheckFolderRegisteredFiles(dir, idPath);
            // Регистрация новых файлов
            foreach (var pattern in ListSearchPatternContentFiles) {
              foreach (var fileInfo in dir.EnumerateFiles(pattern.Pattern)) {
                if (pattern.Kind == KindFile.Image) {
                  if (!ExistsUrlContentImage(Path.GetFileNameWithoutExtension(fileInfo.Name), idPath, pattern.Id)) {
                    UrlContent content = new UrlContent() {
                      Name = Path.GetFileNameWithoutExtension(fileInfo.Name),
                      Pathway = idPath,
                      ImageType = pattern.Id
                    };
                    Settings.Instance.ContentDB.UrlContents.Add(content);
                    Settings.Instance.ContentDB.SaveChanges();
                    _workerSyncFolder.ReportProgress(0, fileInfo.FullName);
                    _countAddFiles++;
                  }
                } else if (pattern.Kind == KindFile.Video) {
                  if (!ExistsUrlContentVideo(Path.GetFileNameWithoutExtension(fileInfo.Name), idPath, pattern.Id)) {
                    UrlContent content = new UrlContent() {
                      Name = Path.GetFileNameWithoutExtension(fileInfo.Name),
                      Pathway = idPath,
                      VideoType = pattern.Id
                    };
                    Settings.Instance.ContentDB.UrlContents.Add(content);
                    Settings.Instance.ContentDB.SaveChanges();
                    _workerSyncFolder.ReportProgress(0, fileInfo.FullName);
                    _countAddFiles++;
                  }
                }
              }
            }
            if (_workerSyncFolder.CancellationPending) {
              e.Cancel = true;
              return;
            }
          }
          if (_originalOperation == DML.Insert || _originalOperation == DML.Update || _originalOperation == DML.Synchronization) {
            _workerSyncFolder.ReportProgress(0, $"{Settings.Instance.LocalisationHelper["ContentFoldersRes.ReportSyncFoldersAddFolders"]}: {_countAddFolders}" +
                                                $"\n{Settings.Instance.LocalisationHelper["ContentFoldersRes.ReportSyncFoldersEmptyFolders"]}: {_countEmptyFolders}" +
                                                $"\n{Settings.Instance.LocalisationHelper["ContentFoldersRes.ReportSyncFoldersAddFiles"]}: {_countAddFiles}" +
                                                $"\n{Settings.Instance.LocalisationHelper["ContentFoldersRes.ReportSyncFoldersCheckFiles"]}: {_countCheckFiles}" +
                                                $"\n{Settings.Instance.LocalisationHelper["ContentFoldersRes.ReportSyncFoldersDelRegisteresFiles"]}: {_countDelRegisteresFiles}");
          }
        } else {
          _workerSyncFolder.ReportProgress(-1, Settings.Instance.LocalisationHelper["ContentFoldersRes.MsgDirectoryNotFound"]);
        }
      } catch (Exception eWorker) {
        _workerSyncFolder.ReportProgress(-1, ErrorProcessing.GetExeptionContent(eWorker));
      }
    }
    private void Worker_RunWorkerSyncCompleted(object sender, RunWorkerCompletedEventArgs e) {
      SynchronizationFolderCompletedEvent?.Invoke();
      _workerSyncFolder.Dispose();
      _workerSyncFolder = null;
      GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced, true);
    }
    private void Worker_ProgressSyncChanged(object sender, ProgressChangedEventArgs e) {
      if (e.ProgressPercentage == 0) {
        SynchronizationFolderEvent?.Invoke(e.UserState as string);
      } else if (e.ProgressPercentage == -1) {
        MessageConfirmBox.Show(Settings.Instance.AppImageUrl, e.UserState as string, "", ConfirmBoxButtons.OK, ConfirmBoxImage.Error);
      }
    }
    #endregion

    #region Helpers
    private void CheckFolderRegisteredFiles(DirectoryInfo dir, int id) {
      var listRegContent = from regContent in Settings.Instance.ContentDB.Database.SqlQuery<VUrlContent>("SELECT * FROM VUrlContents")
                           where regContent.NPathway == id
                           select regContent;
      string fileName = string.Empty;
      foreach (var item in listRegContent) {
        _countCheckFiles++;
        fileName = item.SPathway + @"\" + item.Name + "." + (string.IsNullOrEmpty(item.SImageType) ? item.SVideoType : item.SImageType);
        _workerSyncFolder.ReportProgress(0, fileName);
        if (!File.Exists(fileName)) {
          int deleted = Settings.Instance.ContentDB.Database.ExecuteSqlCommand("DELETE FROM UrlContents WHERE Id = " + item.Id);
          _countDelRegisteresFiles++;
        }
      }
    }
    private bool IsContainsFiles(DirectoryInfo dir) {
      bool isContains = false;
      foreach (var pattern in ListSearchPatternContentFiles) {
        foreach (var fileInfo in dir.EnumerateFiles(pattern.Pattern)) {
          isContains = true;
          break;
        }
      }
      return isContains;
    }
    private string GetSearchPatternImageFiles() {
      string pattern = string.Join(";", (
                                          from imageType in
                                            Settings.Instance.ContentDB.Database.SqlQuery<RefImageType>("SELECT * FROM RefImageTypes")
                                          select imageType.Name
                                         ).Select(s => $"*.{s}"));
      if (string.IsNullOrWhiteSpace(pattern)) {
        throw new ArgumentNullException("SearchPatternImageFile");
      }
      return pattern;
    }
    private IEnumerable<PatternSearchFile> GetListSearchPatternImageFiles() {
      return from imageType in
              Settings.Instance.ContentDB.Database.SqlQuery<RefImageType>("SELECT * FROM RefImageTypes")
             select new PatternSearchFile(imageType.Id, "*." + imageType.Name, KindFile.Image);
    }
    private string GetSearchPatternVideoFiles() {
      string pattern = string.Join(";", (
                                         from videoType in
                                           Settings.Instance.ContentDB.Database.SqlQuery<RefVideoType>("SELECT * FROM RefVideoTypes")
                                         select videoType.Name
                                         ).Select(s => $"*.{s}"));
      if (string.IsNullOrWhiteSpace(pattern)) {
        throw new ArgumentNullException("SearchPatternVideoFile");
      }
      return pattern;
    }
    private IEnumerable<PatternSearchFile> GetListSearchPatternVideoFiles() {
      return from videoType in
              Settings.Instance.ContentDB.Database.SqlQuery<RefVideoType>("SELECT * FROM RefVideoTypes")
             select new PatternSearchFile(videoType.Id, "*." + videoType.Name, KindFile.Video);

    }
    private IEnumerable<PatternSearchFile> GetListSearchPatternContentFiles() {
      return ListSearchPatternImageFiles.Union(ListSearchPatternVideoFiles, new PatternSearchFileComparer());
    }
    private int GetIdUrlContentImage(string name, int path, int image) {
      return (
        from dic in Settings.Instance.ContentDB.Database.SqlQuery<UrlContent>("SELECT * FROM UrlContents")
        where dic.Name == name && dic.Pathway == path && dic.ImageType == image
        select dic.Id
       ).FirstOrDefault();
    }
    private int GetIdUrlContentVideo(string name, int path, int video) {
      return (
        from dic in Settings.Instance.ContentDB.Database.SqlQuery<UrlContent>("SELECT * FROM UrlContents")
        where dic.Name == name && dic.Pathway == path && dic.VideoType == video
        select dic.Id
       ).FirstOrDefault();
    }
    private bool ExistsUrlContentImage(string name, int path, int image) {
      return GetIdUrlContentImage(name, path, image) == 0 ? false : true;
    }
    private bool ExistsUrlContentVideo(string name, int path, int video) {
      return GetIdUrlContentVideo(name, path, video) == 0 ? false : true;
    }
    //private bool ExistsIdContentProperties(int id) {
    //  return (
    //          from contProp in Settings.Instance.ContentDB.Database.SqlQuery<ContentProperty>("SELECT * FROM ContentProperties WHERE Id="+id)
    //          select contProp.Id
    //         ).FirstOrDefault() == id ? true : false;
    //}
    //private bool ExistsUrlContentProperties(int idUrl) {
    //  return (
    //          from contProp in Settings.Instance.ContentDB.Database.SqlQuery<ContentProperty>("SELECT * FROM ContentProperties WHERE Content=" + idUrl)
    //          select contProp.Content
    //         ).FirstOrDefault() == idUrl ? true : false;
    //}
    private int GetIdContentProperties(int idUrl) {
      return (
              from contProp in Settings.Instance.ContentDB.Database.SqlQuery<ContentProperty>("SELECT * FROM ContentProperties WHERE Content=" + idUrl)
              select contProp.Id
             ).FirstOrDefault();
    }
    #endregion

  }
}
