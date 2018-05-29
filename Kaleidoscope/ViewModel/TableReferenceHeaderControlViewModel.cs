using Kaleidoscope.Helpers;
using System;

namespace Kaleidoscope.ViewModel {
  public class TableReferenceHeaderControlViewModel : ViewModelPrimary {
    public TableReferenceHeaderControlViewModel(Predicate<object> canUpdExecute, Predicate<object> canDelExecute) {
      _canUpdExecute = canUpdExecute;
      _canDelExecute = canDelExecute;
      Init();
    }

    #region Fields
    protected readonly Predicate<object> _canUpdExecute;
    protected readonly Predicate<object> _canDelExecute;
    private string _conditionName = string.Empty;
    #endregion

    #region Properties
    public string ConditionName {
      get { return _conditionName; }
      set {
        this.MutateVerbose(ref _conditionName, value, RaisePropertyChanged());
        ConditionsEvent?.Invoke(ConditionName);
      }
    }
    #endregion

    #region Events
    public event Action AddCommandEvent;
    public event Action UpdCommandEvent;
    public event Action DelCommandEvent;
    public event Action ClearConditionCommandEvent;
    public event Action<string> ConditionsEvent;
    #endregion

    #region Commands
    public DelegateCommand AddCommand { get; protected set; }
    public DelegateCommand UpdCommand { get; protected set; }
    public DelegateCommand DelCommand { get; protected set; }
    public DelegateCommand ClearConditionCommand { get; protected set; }
    #endregion

    #region Commands implemenation
    protected virtual void Add() {
      AddCommandEvent?.Invoke();
    }
    protected virtual void Upd() {
      UpdCommandEvent?.Invoke();
    }
    protected virtual void Del() {
      DelCommandEvent?.Invoke();
    }
    protected virtual void ClearCondition() {
      ConditionName = string.Empty;
      ClearConditionCommandEvent?.Invoke();
    }
    protected virtual bool ClearConditionValidate() {
      return !String.IsNullOrWhiteSpace(ConditionName);
    }
    #endregion

    #region Helpers
    protected virtual void Init() {
      AddCommand = new DelegateCommand(o => Add());
      UpdCommand = new DelegateCommand(o => Upd(), _canUpdExecute);
      DelCommand = new DelegateCommand(o => Del(), _canDelExecute);
      ClearConditionCommand = new DelegateCommand(o => ClearCondition(), o => ClearConditionValidate());
    }
    #endregion

  }
}

