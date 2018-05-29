using Kaleidoscope.Helpers;
using Kaleidoscope.HelpersViewModel;
using Kaleidoscope.Model;
using Kaleidoscope.View;
using Kaleidoscope.ViewDependencyProperty;
using Kaleidoscope.ViewDialog;
using Kaleidoscope.ViewModelDialog;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.Entity.Infrastructure;
using System.Threading;
using System.Windows;
using System.Windows.Controls;

namespace Kaleidoscope.ViewModel {
  public class DirectoriesViewModel<T> : ViewModelBaseDialogHost where T : TableReference, new() {
    public DirectoriesViewModel(DialogSession sessionMainDialogHost, string identifierDialogHost, ContentDB db, double widthColumnName, DataGridSelectionMode selectionMode) {
      _identifierDialogHost = identifierDialogHost;
      _sessionMainDialogHost = sessionMainDialogHost;
      //SessionMainDialogHost?.UpdateContent(new SampleProgressDialog() { DataContext = new SampleProgressDialogViewModel(Settings.Same().LocalisationHelper["DMLRes.ConnectingDatabaseCaption"]) });
      HeaderControlHorizontalAlignment = HorizontalAlignment.Left;
      HeaderControlInit();
      SelectionChangedCommand = new DelegateCommand(SelectionChanged);
      MainWindowViewModel.LocalizationChangedEvent += new Action(SetColumNameCaption);
      // Init data
      _db = db;
      DataGridViewDepProp.Instance.WidthColumnName = widthColumnName;
      _tableName = typeof(T).Name + "s";
      _columnNameLocalizationIndex = GetColumnNameLocalizationIndex();
      SetColumNameCaption();
      Load();
      SelectionMode = selectionMode;
      SessionMainDialogHost?.Close(false);
    }
    #region Fields
    private BackgroundWorker _workerDeleteRecords;
    protected string _identifierDialogHost;
    protected TableReferenceHeaderControlViewModel _headerViewModel;
    protected ContentDB _db;
    protected string _tableName = string.Empty;
    protected string _columnNameLocalizationIndex = string.Empty;
    protected ObservableCollection<T> _directories;
    protected int _selectedIndex = -1;
    protected IList _selectedItems;
    protected DataGridSelectionMode _selectionMode = DataGridSelectionMode.Single;
    protected T _currentItem;
    protected DML _dmlCurrent = DML.Empty;
    private string _conditionName = string.Empty;
    #endregion

    #region Properties
    public virtual TableReferenceHeaderControlViewModel HeaderViewModel {
      get { return _headerViewModel; }
      set { this.MutateVerbose(ref _headerViewModel, value, RaisePropertyChanged()); }
    }
    public string TableName => _tableName;
    public string IdentifierDialogHost => _identifierDialogHost;
    //public PropertyInfo PropertyInfoEntity => _propertyInfoEntity;
    public string ColumnNameLocalizationIndex => _columnNameLocalizationIndex;
    public virtual ObservableCollection<T> Directories {
      get { return _directories; }
      set {
        this.MutateVerbose(ref _directories, value, RaisePropertyChanged());
      }
    }
    public int SelectedIndex {
      get { return _selectedIndex; }
      set {
        this.MutateVerbose(ref _selectedIndex, value, RaisePropertyChanged());
        OnSelectedIndex();
      }
    }
    public virtual IList SelectedItems {
      get { return _selectedItems; }
      set {
        this.MutateVerbose(ref _selectedItems, value, RaisePropertyChanged());
        SetColumNameCaption();
        if (SelectedItems.Count == 1) {
          CurrentItem = SelectedItems[0] as T;
        } else {
          CurrentItem = null;
        }
      }
    }
    public DataGridSelectionMode SelectionMode {
      get { return _selectionMode; }
      set { this.MutateVerbose(ref _selectionMode, value, RaisePropertyChanged()); }
    }
    public T CurrentItem {
      get { return _currentItem; }
      set { this.MutateVerbose(ref _currentItem, value, RaisePropertyChanged()); }
    }
    public string ConditionName {
      get { return _conditionName; }
      protected set { _conditionName = value; }
    }
    #endregion

    #region Events
    public event Action<string> SelectedIndexEvent;
    private event Action<string> DeleteProgressEvent;
    private event Action DeleteCompletedEvent;
    #endregion

    #region Events implementation
    public void OnSelectedIndex() {
      if (SelectedIndex < 0 || CurrentItem == null) {
        SelectedIndexEvent?.Invoke(string.Empty);
      } else {
        SelectedIndexEvent?.Invoke(CurrentItem.Name);
      }
    }
    #endregion

    #region Implementation of subscriptions on events
    protected virtual void Add() {
      _dmlCurrent = DML.Insert;
      ExecuteRunDialog(Settings.Same().LocalisationHelper["DMLRes.Insert"], string.Empty);
    }
    protected virtual void Upd() {
      if (CurrentItem != null) {
        _dmlCurrent = DML.Update;
        ExecuteRunDialog(Settings.Same().LocalisationHelper["DMLRes.Update"], CurrentItem.Name);
      }
    }
    protected virtual void Del() {
      if (CurrentItem != null && SelectionMode == DataGridSelectionMode.Single || SelectionMode == DataGridSelectionMode.Extended) {
        Delete();
      }
    }
    protected virtual void ImplementConditions(string conditionName) {
      ConditionName = conditionName;
      Load();
    }
    #endregion

    #region Commands
    public DelegateCommand SelectionChangedCommand { get; }
    public DelegateCommand AddCommand { get { return HeaderViewModel.AddCommand; } }
    public DelegateCommand UpdCommand { get { return HeaderViewModel.UpdCommand; } }
    public DelegateCommand DelCommand { get { return HeaderViewModel.DelCommand; } }
    public DelegateCommand ClearConditionCommand { get { return HeaderViewModel.ClearConditionCommand; } }
    #endregion

    #region Command implementation
    protected virtual void SelectionChanged(object param) {
      if (param is IList item && item != null) {
        SelectedItems = param as IList;
      }
    }
    protected virtual async void ExecuteRunDialog(string headerCaption, string name) {
      var view = new TableReferenceAddUpdDialog {
        DataContext = new TableReferenceAddUpdDialogViewModel(Settings.Same().AppImageUrl,
                                                              headerCaption,
                                                              name,
                                                              Settings.Same().LocalisationHelper[GetColumnNameLocalizationIndex()])
      };
      var result = await DialogHost.Show(view, IdentifierDialogHost, ClosingEventHandler);
    }
    protected virtual void ClosingEventHandler(object sender, DialogClosingEventArgs eventArgs) {
      if ((bool)eventArgs.Parameter == false) return;
      // Получаем значение поля ввода
      string name = ((eventArgs.Session.Content as TableReferenceAddUpdDialog).DataContext as TableReferenceAddUpdDialogViewModel).Name;
      if (_dmlCurrent == DML.Insert) {
        Insert(name);
      } else if (_dmlCurrent == DML.Update) {
        Update(name);
      };
    }
    #endregion

    #region Dispose
    protected override void OnDispose() {
      base.OnDispose();
      MainWindowViewModel.LocalizationChangedEvent -= new Action(SetColumNameCaption);
    }
    #endregion

    #region Helpers
    protected virtual void HeaderControlInit() {
      HeaderControl = new TableReferenceHeaderControl() { DataContext = new TableReferenceHeaderControlViewModel(o => UpdCommandValidate(), o => DelCommandValidate()) };
      HeaderViewModel = HeaderControl.DataContext as TableReferenceHeaderControlViewModel;
      HeaderViewModel.AddCommandEvent += () => Add();
      HeaderViewModel.UpdCommandEvent += () => Upd();
      HeaderViewModel.DelCommandEvent += () => Del();
      HeaderViewModel.ConditionsEvent += (o) => ImplementConditions(o);
    }
    protected virtual string GetColumnNameLocalizationIndex() {
      string index = string.Empty;
      if (typeof(T).Name == typeof(RefPathway).Name) {
        index = "ContentFoldersRes.CaptionName";
      } else
      if (typeof(T).Name == typeof(RefNode).Name) {
        index = "ContentCatalogItemsRes.CaptionName";
      } else
      if (typeof(T).Name == typeof(RefVideoType).Name) {
        index = "ContentVideoTypesRes.CaptionName";
      } else
      if (typeof(T).Name == typeof(RefImageType).Name) {
        index = "ContentImageTypesRes.CaptionName";
      } else
      if (typeof(T).Name == typeof(RefComment).Name) {
        index = "ContentCommentsRes.CaptionName";
      };
      return index;
    }
    protected virtual void SetColumNameCaption() {
      string nameColumn = Settings.Same().LocalisationHelper[ColumnNameLocalizationIndex];
      if (SelectionMode == DataGridSelectionMode.Extended && SelectedItems?.Count > 1) {
        nameColumn += $" ({SelectedItems?.Count})";
      }
      DataGridViewDepProp.Instance.HeaderColumnName = nameColumn;
    }
    protected virtual bool UpdCommandValidate() {
      return SelectedIndex > -1 && SelectedItems?.Count == 1;
    }
    protected virtual bool DelCommandValidate() {
      return SelectionMode == DataGridSelectionMode.Single ? SelectedIndex > -1 && SelectedItems?.Count == 1 :
             SelectionMode == DataGridSelectionMode.Extended ? SelectedIndex > -1 : false;
    }
    protected virtual void Load() {
      DbRawSqlQuery<T> query;
      if (string.IsNullOrWhiteSpace(ConditionName)) {
        query = _db.Database.SqlQuery<T>("SELECT * FROM " + TableName);
      } else {
        query = _db.Database.SqlQuery<T>("SELECT * FROM " + TableName + " WHERE Name LIKE \"" + "%" + ConditionName + "%\"");
      }
      ObservableCollection<T> temp = new ObservableCollection<T>();
      foreach (var item in query) {
        temp.Add(item);
      }
      Directories = temp;
    }
    protected virtual void Insert(string name) {
      if (!string.IsNullOrWhiteSpace(name)) {
        try {
          //T entity = new T() { Name = name };
          //_db.RefNodes.Add(node);
          //_db.SaveChanges();
          int inserted = _db.Database.ExecuteSqlCommand("INSERT INTO " + TableName + " (Name) VALUES ('" + name + "')");
        } catch (Exception e) {
          ErrorProcessing.Show(e, name);
        }
        Load();
      }
    }
    protected virtual void Update(string name) {
      if (!string.IsNullOrWhiteSpace(name)) {
        try {
          //RefNode node = _db.RefNodes.Find(Directories[SelectedIndex].Id);
          //if (node != null) {
          //  node.Name = name;
          //  _db.Entry(node).State = EntityState.Modified;
          //  _db.SaveChanges();
          //} else {
          //  MessageConfirmBox.Show(Settings.Same().AppImageUrl, $"ID={Directories[SelectedIndex].Id}", "Данные не найдены.", ConfirmBoxButtons.Cancel, ConfirmBoxImage.Warning);
          //}
          int updated = _db.Database.ExecuteSqlCommand("UPDATE " + TableName + " SET Name = '" + name + "' WHERE Id = " + CurrentItem.Id);
        } catch (Exception e) {
          ErrorProcessing.Show(e, name);
        }
        Load();
      }
    }
    protected virtual void Delete() {
      if (SelectionMode == DataGridSelectionMode.Single || SelectionMode == DataGridSelectionMode.Extended && SelectedItems?.Count == 1) {
        if (MessageConfirmBox.Show(Settings.Same().AppImageUrl,
                                   $"{CurrentItem.Name}",
                                   Settings.Same().LocalisationHelper["DMLRes.ConfirmDeleteCaption"],
                                   ConfirmBoxButtons.OKCancel,
                                   ConfirmBoxImage.Question) == MessageBoxResult.OK) {
          try {
            //RefNode node = _db.RefNodes.Find(Directories[SelectedIndex].Id);
            //if (node != null) {
            //  _db.RefNodes.Remove(node);
            //  _db.SaveChanges();
            //} else {
            //  MessageConfirmBox.Show(Settings.Same().AppImageUrl, $"ID={Directories[SelectedIndex].Id}", "Данные не найдены.", ConfirmBoxButtons.Cancel, ConfirmBoxImage.Warning);
            //}
            int deleted = _db.Database.ExecuteSqlCommand("DELETE FROM " + TableName + " WHERE Id = " + CurrentItem.Id);
          } catch (Exception e) {
            ErrorProcessing.Show(e);
          }
          Load();
        }
      } else {
        ExecuteDeleteDialog();
      }
    }
    private async void ExecuteDeleteDialog() {
      var view = new SampleProgressDialog {
        DataContext = new SampleProgressDialogViewModel(Settings.Instance.ImageUidRegularSync,
                                                        Settings.Instance.LocalisationHelper["DMLRes.Delete"],
                                                        $"{SelectedItems?.Count} {GetCaptionRecords(SelectedItems.Count)}"
                                                       )
      };
      var result = await DialogHost.Show(view, IdentifierDialogHost, ClosingDeleteEventHandler);
    }
    private void ClosingDeleteEventHandler(object sender, DialogClosingEventArgs eventArgs) {
      SampleProgressDialogViewModel dialog = ((eventArgs.Session.Content as SampleProgressDialog).DataContext as SampleProgressDialogViewModel);
      if ((bool)eventArgs.Parameter == false && !dialog.IsProgress) {
        return;
      } else if ((bool)eventArgs.Parameter && !dialog.IsProgress) {
        if (MessageConfirmBox.Show(Settings.Same().AppImageUrl,
                                   $"{SelectedItems?.Count} {GetCaptionRecords(SelectedItems.Count)}",
                                   Settings.Instance.LocalisationHelper["DMLRes.ConfirmDeleteCaption"],
                                   ConfirmBoxButtons.YesNo,
                                   ConfirmBoxImage.Question) == MessageBoxResult.No) {
          return;
        }
      } else if ((bool)eventArgs.Parameter == false && dialog.IsProgress) {
        if (!dialog.IsCompletedProgress) {
          if (MessageConfirmBox.Show(Settings.Instance.AppImageUrl,
                                     Settings.Instance.LocalisationHelper["DMLRes.InterruptDeleteRecordsConfirm"],
                                     "",
                                     ConfirmBoxButtons.YesNo,
                                     ConfirmBoxImage.Question
                                    ) == MessageBoxResult.No) {
            eventArgs.Cancel();
            return;
          } else {
            _workerDeleteRecords.CancelAsync();
            eventArgs.Cancel();
            return;
          }
        } else {
          DeleteProgressEvent -= new Action<string>(dialog.ProgressTextBlock);
          DeleteCompletedEvent -= new Action(dialog.TrackCompletedProgress);
          Load();
          return;
        }
      }
      eventArgs.Cancel();
      dialog.IsProgress = true;
      DeleteProgressEvent += new Action<string>(dialog.ProgressTextBlock);
      DeleteCompletedEvent += new Action(dialog.TrackCompletedProgress);
      dialog.ProgressPanelVisibility = Visibility.Visible;
      try {
        _workerDeleteRecords = new BackgroundWorker();
        _workerDeleteRecords.DoWork += new DoWorkEventHandler(Worker_DoWorkDelete);
        _workerDeleteRecords.RunWorkerCompleted += new RunWorkerCompletedEventHandler(Worker_RunDeleteCompleted);
        _workerDeleteRecords.ProgressChanged += new ProgressChangedEventHandler(Worker_ProgressDeleteChanged);
        _workerDeleteRecords.WorkerReportsProgress = true;
        _workerDeleteRecords.WorkerSupportsCancellation = true;
        _workerDeleteRecords.RunWorkerAsync();
      } catch (Exception e) {
        DeleteProgressEvent -= new Action<string>(dialog.ProgressTextBlock);
        DeleteCompletedEvent -= new Action(dialog.TrackCompletedProgress);
        ErrorProcessing.Show(e);
      }
    }
    private void Worker_DoWorkDelete(object sender, DoWorkEventArgs e) {
      int index = 0;
      int countDeleted = 0;
      int countUnDeleted = 0;
      try {
        foreach (var item in SelectedItems) {
          if (_workerDeleteRecords.CancellationPending) {
            e.Cancel = true;
            break;
          }
          index++;
          T rec = item as T;
          try {
            _workerDeleteRecords.ReportProgress(0, $"{index} / {SelectedItems.Count} \n{rec.Name}");
            int deleted = _db.Database.ExecuteSqlCommand("DELETE FROM " + TableName + " WHERE Id = " + rec.Id);
            countDeleted++;
          } catch (Exception eDel) {
            _workerDeleteRecords.ReportProgress(0, $"{index} / {SelectedItems.Count} \n{rec.Name} \n{ErrorProcessing.GetExeptionContent(eDel.GetBaseException())}");
            countUnDeleted++;
          }
        }
      } catch (Exception eWorkerDelete) {
        _workerDeleteRecords.ReportProgress(-1, ErrorProcessing.GetExeptionContent(eWorkerDelete));
      } finally {
        _workerDeleteRecords.ReportProgress(0, $"Успешно удалено: {countDeleted}" +
                                               $"\nНе удалено: {countUnDeleted}");
      }
    }
    private void Worker_RunDeleteCompleted(object sender, RunWorkerCompletedEventArgs e) {
      DeleteCompletedEvent?.Invoke();
      _workerDeleteRecords.Dispose();
      _workerDeleteRecords = null;
      GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced, true);
    }
    private void Worker_ProgressDeleteChanged(object sender, ProgressChangedEventArgs e) {
      if (e.ProgressPercentage == 0) {
        DeleteProgressEvent?.Invoke(e.UserState as string);
      } else if (e.ProgressPercentage == -1) {
        MessageConfirmBox.Show(Settings.Instance.AppImageUrl, e.UserState as string, "", ConfirmBoxButtons.OK, ConfirmBoxImage.Error);
      }
    }
    private string GetCaptionRecords(int number) {
      return number % 10 == 1 && number % 100 != 11 ? Settings.Instance.LocalisationHelper["DMLRes.Records_N1"] :
             number % 100 >= 11 && number % 100 <= 20 || number % 10 >= 5 || number % 10 == 0 ? Settings.Instance.LocalisationHelper["DMLRes.Records_N5_N0"] :
             Settings.Instance.LocalisationHelper["DMLRes.Records_N2_N4"];
    }
    #endregion
  }
}
