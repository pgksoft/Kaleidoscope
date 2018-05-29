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
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity.Infrastructure;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Kaleidoscope.ViewModel {
  public class FoldersViewModel<T> : DirectoriesViewModel<T> where T : TableReference, new() {
    public FoldersViewModel(DialogSession sessionMainDialogHost, string identifierDialogHost, ContentDB db, double widthColumnName, DataGridSelectionMode selectionMode)
      : base(sessionMainDialogHost, identifierDialogHost, db, widthColumnName, selectionMode) {
      MultimediaСontent = new Content() {
        DataContext = new ContentViewModel(ContentWorkMode.Folders) 
      };
      _contentControlViewModel = MultimediaСontent?.DataContext as ContentViewModel;
      HeaderViewModel.ContentControlHeader = ContentControlViewModel?.HeaderControl as ContentHeader;
    }

    #region Fields
    protected new FoldersHeaderViewModel _headerViewModel;
    protected ObservableCollection<VRefPathway> _vDirectories;
    private string _viewName = "VRefPathways";
    private Content _multimediaСontent;
    private ContentViewModel _contentControlViewModel;
    #endregion

    #region Properties
    public override IList SelectedItems {
      get => base.SelectedItems;
      set {
        base.SelectedItems = value;
        List<int> idDic = new List<int>();
        foreach (VRefPathway item in SelectedItems) {
          idDic.Add(item.Id);
        }
        ContentControlViewModel.ConditionShow = new ConditionShowContent(idDic);
      }
    }
    public string ViewName => _viewName;
    public new FoldersHeaderViewModel HeaderViewModel {
      get { return _headerViewModel; }
      set { this.MutateVerbose(ref _headerViewModel, value, RaisePropertyChanged()); }
    }
    public ObservableCollection<VRefPathway> VDirectories {
      get { return _vDirectories; }
      set {
        this.MutateVerbose(ref _vDirectories, value, RaisePropertyChanged());
      }
    }
    public Content MultimediaСontent {
      get { return _multimediaСontent; }
      set { this.MutateVerbose(ref _multimediaСontent, value, RaisePropertyChanged()); }
    }
    public ContentViewModel ContentControlViewModel => _contentControlViewModel;
    #endregion

    #region Commands
    public new DelegateCommand AddCommand { get { return HeaderViewModel.AddCommand; } }
    public new DelegateCommand UpdCommand { get { return HeaderViewModel.UpdCommand; } }
    public new DelegateCommand DelCommand { get { return HeaderViewModel.DelCommand; } }
    public DelegateCommand SyncCommand { get { return HeaderViewModel.SyncCommand; } }
    public new DelegateCommand ClearConditionCommand { get { return HeaderViewModel.ClearConditionCommand; } }
    #endregion

    #region Implementation of subscriptions on events
    private void Sync() {
      _dmlCurrent = DML.Synchronization;
      ExecuteSyncDialog();
    }
    #endregion

    #region Command implementation
    private async void ExecuteSyncDialog() {
      var view = new SampleProgressDialog {
        DataContext = new SampleProgressDialogViewModel(Settings.Instance.ImageUidRegularSync,
                                                        Settings.Instance.LocalisationHelper["ContentFoldersRes.FolderSync"],
                                                        CurrentItem.Name
                                                       )
      };
      var result = await DialogHost.Show(view, IdentifierDialogHost, ClosingSyncEventHandler);
    }
    private void ClosingSyncEventHandler(object sender, DialogClosingEventArgs eventArgs) {
      SampleProgressDialogViewModel dialog = ((eventArgs.Session.Content as SampleProgressDialog).DataContext as SampleProgressDialogViewModel);
      if ((bool)eventArgs.Parameter == false && !dialog.IsProgress) {
        return;
      } else if ((bool)eventArgs.Parameter == false && dialog.IsProgress) {
        if (!dialog.IsCompletedProgress) {
          if (MessageConfirmBox.Show(Settings.Instance.AppImageUrl,
                                     Settings.Instance.LocalisationHelper["ContentFoldersRes.InterruptSyncFoldersConfirm"],
                                     "",
                                     ConfirmBoxButtons.YesNo,
                                     ConfirmBoxImage.Question
                                    ) == MessageBoxResult.No) {
            eventArgs.Cancel();
            return;
          } else {
            ContentUtilities.Instance.WorkerSyncFolder.CancelAsync();
            eventArgs.Cancel();
            return;
          }
        } else {
          ContentUtilities.Instance.SynchronizationFolderEvent -= new Action<string>(dialog.ProgressTextBlock);
          ContentUtilities.Instance.SynchronizationFolderCompletedEvent -= new Action(dialog.TrackCompletedProgress);
          Load();
          return;
        }
      }
      eventArgs.Cancel();
      dialog.IsProgress = true;
      ContentUtilities.Instance.SynchronizationFolderEvent += new Action<string>(dialog.ProgressTextBlock);
      ContentUtilities.Instance.SynchronizationFolderCompletedEvent += new Action(dialog.TrackCompletedProgress);
      dialog.ProgressPanelVisibility = Visibility.Visible;
      try {
        ContentUtilities.Instance.SynchronizationFolder(CurrentItem.Name, _dmlCurrent, o => Insert(o), o => Update(o));
      } catch (Exception e) {
        ErrorProcessing.Show(e);
      }
    }

    protected override async void ExecuteRunDialog(string headerCaption, string name) {
      var view = new EntityNameSelectDialog {
        DataContext = new EntityNameSelectDialogViewModel(Settings.Same().AppImageUrl,
                                                          headerCaption,
                                                          name,
                                                          Settings.Same().ImageFolderContent,
                                                          Settings.Same().LocalisationHelper[GetColumnNameLocalizationIndex()],
                                                          800)
      };
      var result = await DialogHost.Show(view, IdentifierDialogHost, ClosingEventHandler);
    }
    protected override void ClosingEventHandler(object sender, DialogClosingEventArgs eventArgs) {
      EntityNameSelectDialogViewModel dialog = ((eventArgs.Session.Content as EntityNameSelectDialog).DataContext as EntityNameSelectDialogViewModel);
      if ((bool)eventArgs.Parameter == false && !dialog.IsProgress) {
        return;
      } else if ((bool)eventArgs.Parameter == false && dialog.IsProgress) {
        if (!dialog.IsCompletedProgress) {
          if (MessageConfirmBox.Show(Settings.Instance.AppImageUrl,
                                     Settings.Instance.LocalisationHelper["ContentFoldersRes.InterruptSyncFoldersConfirm"],
                                     "",
                                     ConfirmBoxButtons.YesNo,
                                     ConfirmBoxImage.Question
                                    ) == MessageBoxResult.No) {
            eventArgs.Cancel();
            return;
          } else {
            ContentUtilities.Instance.WorkerSyncFolder.CancelAsync();
            eventArgs.Cancel();
            return;
          }
        } else {
          ContentUtilities.Instance.SynchronizationFolderEvent -= new Action<string>(dialog.ProgressTextBlock);
          ContentUtilities.Instance.SynchronizationFolderCompletedEvent -= new Action(dialog.TrackCompletedProgress);
          Load();
          return;
        }
      }
      eventArgs.Cancel();
      dialog.IsProgress = true;
      string name = dialog.Name;
      dialog.ProgressTextBlockVisibility = Visibility.Visible;
      dialog.ProgressBarVisibility = Visibility.Visible;
      ContentUtilities.Instance.SynchronizationFolderEvent += new Action<string>(dialog.ProgressTextBlock);
      ContentUtilities.Instance.SynchronizationFolderCompletedEvent += new Action(dialog.TrackCompletedProgress);
      try {
        ContentUtilities.Instance.SynchronizationFolder(name, _dmlCurrent, o => Insert(o), o => Update(o));
      } catch (Exception e) {
        ErrorProcessing.Show(e);
      }
    }
    #endregion

    #region Helpers
    protected override void HeaderControlInit() {
      HeaderControl = new FoldersHeader() { DataContext = new FoldersHeaderViewModel(o => UpdCommandValidate(), o => DelCommandValidate()) };
      HeaderViewModel = HeaderControl.DataContext as FoldersHeaderViewModel;
      HeaderViewModel.AddCommandEvent += () => Add();
      HeaderViewModel.UpdCommandEvent += () => Upd();
      HeaderViewModel.DelCommandEvent += () => Del();
      HeaderViewModel.SyncCommandEvent += () => Sync();
      HeaderViewModel.ConditionsEvent += (o) => ImplementConditions(o);
    }
    protected override void SetColumNameCaption() {
      base.SetColumNameCaption();
      //DataGridViewDepProp.Instance.HeaderColumnNCountImageType = Settings.Instance.LocalisationHelper["ContentFoldersRes.CaptionCountImageType"];
      //DataGridViewDepProp.Instance.HeaderColumnNCountVideoType = Settings.Instance.LocalisationHelper["ContentFoldersRes.CaptionCountVideoType"];
    }
    protected override void Load() {
      DbRawSqlQuery<VRefPathway> query;
      if (string.IsNullOrWhiteSpace(ConditionName)) {
        query = _db.Database.SqlQuery<VRefPathway>("SELECT * FROM " + ViewName);
      } else {
        query = _db.Database.SqlQuery<VRefPathway>("SELECT * FROM " + ViewName + " WHERE Name LIKE \"" + "%" + ConditionName + "%\"");
      }
      ObservableCollection<VRefPathway> temp = new ObservableCollection<VRefPathway>();
      foreach (var item in query) {
        item.NCountImageType = item.NCountImageType == 0 ? null : item.NCountImageType;
        item.NCountVideoType = item.NCountVideoType == 0 ? null : item.NCountVideoType;
        temp.Add(item);
      }
      VDirectories = temp;
    }
    #endregion

  }
}
