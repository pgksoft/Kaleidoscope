using Kaleidoscope.Helpers;
using Kaleidoscope.HelpersViewModel;
using Kaleidoscope.Model;
using Kaleidoscope.View;
using Kaleidoscope.ViewDialog;
using Kaleidoscope.ViewModelDialog;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Kaleidoscope.ViewModel {
  public class CatalogViewModel : ViewModelBaseDialogHost {
    public CatalogViewModel(DialogSession sessionMainDialogHost, ContentDB db) {
      _sessionMainDialogHost = sessionMainDialogHost;
      HeaderControl = new CatalogHeader() { DataContext = new CatalogHeaderViewModel(o => UpdCommandValidate(), o => DelCommandValidate()) };
      HeaderControlHorizontalAlignment = HorizontalAlignment.Left;
      _headerViewModel = HeaderControl.DataContext as CatalogHeaderViewModel;
      // Subscriptions on events
      _headerViewModel.AddRootCommandEvent += () => AddRoot();
      _headerViewModel.AddCommandEvent += () => Add();
      _headerViewModel.UpdCommandEvent += () => Upd();
      _headerViewModel.DelCommandEvent += () => Del();
      _headerViewModel.ReplaceCommandEvent += () => Replace();
      _headerViewModel.ConditionsEvent += (o) => ImplementConditions(o);
      CatalogSelectedItemChangedCommand = new DelegateCommand(CatalogSelectedItemChanged);
      // Init data
      _db = db;
      SelectedIndex = -1;
      Load();
    }

    #region Fields
    private CatalogHeaderViewModel _headerViewModel;
    private ContentDB _db;
    private string _tableName = "VCatalogs";
    private string _tableCatalog = "Catalogs";
    private string _tableNodes = "RefNodes";
    private ObservableCollection<VCatalogExt> _catalogTree;
    private VCatalog _selectedItem;
    private int _selectedIndex;
    private string _conditionName = string.Empty;
    private DML _dmlCurrent = DML.Empty;
    #endregion

    #region Properties
    public string TableName => _tableName;
    public string TableCatalog => _tableCatalog;
    public string TableNodes => _tableNodes;
    public ObservableCollection<VCatalogExt> CatalogTree {
      get { return _catalogTree; }
      set { this.MutateVerbose(ref _catalogTree, value, RaisePropertyChanged()); }
    }
    public VCatalog SelectedItem {
      get { return _selectedItem; }
      set {
        this.MutateVerbose(ref _selectedItem, value, RaisePropertyChanged());
        SelectedIndex = SelectedItem == null ? -1 : SelectedItem.Id;
      }
    }
    public int SelectedIndex {
      get { return _selectedIndex; }
      set { this.MutateVerbose(ref _selectedIndex, value, RaisePropertyChanged()); }
    }
    public string ConditionName {
      get { return _conditionName; }
      set { this.MutateVerbose(ref _conditionName, value, RaisePropertyChanged()); }
    }
    #endregion

    #region Implementation of subscriptions on events
    private void AddRoot() {
      _dmlCurrent = DML.InsertRoot;
      ExecuteChoiceDialog(Settings.Same().LocalisationHelper["DMLRes.InsertRoot"], string.Empty, DataGridSelectionMode.Extended);
    }
    private void Add() {
      _dmlCurrent = DML.Insert;
      ExecuteChoiceDialog(Settings.Same().LocalisationHelper["DMLRes.Insert"], string.Empty, DataGridSelectionMode.Extended);
    }
    private void Upd() {
      _dmlCurrent = DML.Update;
      ExecuteUpdDialog(Settings.Same().LocalisationHelper["DMLRes.Update"], SelectedItem.SNode);
    }
    private void Replace() {
      _dmlCurrent = DML.Replace;
      ExecuteChoiceDialog(Settings.Same().LocalisationHelper["DMLRes.Replace"], SelectedItem.SNode, DataGridSelectionMode.Single);
    }
    private void Del() {
      _dmlCurrent = DML.Delete;
      Delete();
    }
    private void ImplementConditions(string conditionName) {
      ConditionName = conditionName;
      Load();
    }
    #endregion

    #region Commands 
    public DelegateCommand CatalogSelectedItemChangedCommand { get; }
    public DelegateCommand AddRootCommand => _headerViewModel.AddRootCommand;
    public DelegateCommand AddCommand => _headerViewModel.AddCommand;
    public DelegateCommand UpdCommand => _headerViewModel.UpdCommand;
    public DelegateCommand ReplaceCommand => _headerViewModel.ReplaceCommand;
    public DelegateCommand DelCommand => _headerViewModel.DelCommand;
    public DelegateCommand ClearConditionCommand => _headerViewModel.ClearConditionCommand;
    #endregion

    #region Commands iplementation
    private void CatalogSelectedItemChanged(object param) {
      if (param is VCatalogExt item && item != null) {
        SelectedItem = item;
      } else {
        SelectedItem = null;
      }
    }
    private async void ExecuteChoiceDialog(string headerCaption, string name, DataGridSelectionMode selectionMode) {
      ChoiceDialog view = new ChoiceDialog {
        DataContext = new ChoiceDialogDictionaryViewModel<RefNode>(Settings.Same().AppImageUrl, headerCaption, name) {
          DictionaryContent = new Directories {
            DataContext = new DirectoriesViewModel<RefNode>(null, "ChoiceDialog", _db, 400, selectionMode),
          }
        }
      };
      var result = await DialogHost.Show(view, "MainDialog", ClosingEventHandlerChoiceDialog);
    }
    private void ClosingEventHandlerChoiceDialog(object sender, DialogClosingEventArgs eventArgs) {
      if ((bool)eventArgs.Parameter == false) return;
      // Получаем список выборанных значений
      IList selectedItems = (((eventArgs.Session.Content as ChoiceDialog)
                               .DataContext as ChoiceDialogDictionaryViewModel<RefNode>)
                               .DictionaryContent.DataContext as DirectoriesViewModel<RefNode>)
                               .SelectedItems;
      if (selectedItems != null) {
        if (_dmlCurrent == DML.InsertRoot) {
          InsertRoot(selectedItems);
        } else if (_dmlCurrent == DML.Insert) {
          Insert(selectedItems);
        } else if (_dmlCurrent == DML.Replace) {
          Replace(selectedItems);
        }
      }
      _dmlCurrent = DML.Empty;
    }

    private async void ExecuteUpdDialog(string headerCaption, string name) {
      var view = new TableReferenceAddUpdDialog {
        DataContext = new TableReferenceAddUpdDialogViewModel(Settings.Same().AppImageUrl, headerCaption, name, Settings.Same().LocalisationHelper["ContentCatalogItemsRes.CaptionName"])
      };
      var result = await DialogHost.Show(view, "MainDialog", ClosingEventHandlerUpdDialog);
    }
    private void ClosingEventHandlerUpdDialog(object sender, DialogClosingEventArgs eventArgs) {
      if ((bool)eventArgs.Parameter == false) return;
      // Получаем значение поля ввода
      string name = ((eventArgs.Session.Content as TableReferenceAddUpdDialog).DataContext as TableReferenceAddUpdDialogViewModel).Name;
      Update(name);
      _dmlCurrent = DML.Empty;
    }
    #endregion

    #region Helpers
    private bool UpdCommandValidate() {
      return SelectedIndex > -1;
    }
    private bool DelCommandValidate() {
      return SelectedIndex > -1;
    }
    private void Load() {
      DbRawSqlQuery<VCatalog> query;
      if (string.IsNullOrWhiteSpace(ConditionName)) {
        query = _db.Database.SqlQuery<VCatalog>("SELECT * FROM " + TableName + " WHERE ParentID IS NULL ORDER BY SNode");
      } else {
        query = _db.Database.SqlQuery<VCatalog>("SELECT * FROM " + TableName + " WHERE ParentID IS NULL  ORDER BY SNode");
      }

      ObservableCollection<VCatalogExt> temp = new ObservableCollection<VCatalogExt>();
      foreach (var item in query) {
        temp.Add(new VCatalogExt().Init(item));
      }
      CatalogTree = temp;
    }
    private void Delete() {
      if (MessageConfirmBox.Show(Settings.Same().AppImageUrl,
                                 $"{SelectedItem.SNode}",
                                 Settings.Same().LocalisationHelper["DMLRes.ConfirmDeleteCaption"],
                                 ConfirmBoxButtons.OKCancel,
                                 ConfirmBoxImage.Question) == MessageBoxResult.OK) {
        try {
          int deleted = _db.Database.ExecuteSqlCommand("DELETE FROM " + TableCatalog + " WHERE Id = " + SelectedItem.Id);
        } catch (Exception e) {
          ErrorProcessing.Show(e);
        }
        Load();
      }
    }
    private void Update(string name) {
      if (!string.IsNullOrWhiteSpace(name)) {
        try {
          int updated = _db.Database.ExecuteSqlCommand("UPDATE " + TableNodes + " SET Name = '" + name + "' WHERE Id = " + SelectedItem.Node);
        } catch (Exception e) {
          ErrorProcessing.Show(e, name);
        }
        Load();
      }
    }
    private void Replace(IList selectedItems) {
      if (selectedItems.Count == 1) {
        foreach (var item in selectedItems) {
          if (item is RefNode recDic && recDic != null) {
            try {
              int updated = _db.Database.ExecuteSqlCommand("UPDATE " + TableCatalog + " SET Node = " + recDic.Id + " WHERE Id = " + SelectedItem.Id);
            } catch (Exception e) {
              ErrorProcessing.Show(e);
            }
          }
        }
        Load();
      } else {
        throw new Exception(Settings.Same().LocalisationHelper["DMLRes.MsgExeptionCatalogReplaceSelectedItemsMany"]);
      }
    }
    private void Insert(IList selectedItems) {
      foreach (var item in selectedItems) {
        if (item is RefNode recDic && recDic != null) {
          try {
            int inserted = _db.Database.ExecuteSqlCommand("INSERT INTO " + TableCatalog + " (Node,ParentID) VALUES (" + recDic.Id + "," + SelectedItem.Id + ")");
          } catch (Exception e) {
            ErrorProcessing.Show(e);
          }
        }
      }
      Load();
    }
    private void InsertRoot(IList selectedItems) {
      foreach (var item in selectedItems) {
        if (item is RefNode recDic && recDic != null) {
          try {
            int inserted = _db.Database.ExecuteSqlCommand("INSERT INTO " + TableCatalog + " (Node,ParentID) VALUES (" + recDic.Id + "," + SelectedItem.ParentID + ")");
          } catch (Exception e) {
            ErrorProcessing.Show(e);
          }
        }
      }
      Load();
    }
    #endregion

  }
}
