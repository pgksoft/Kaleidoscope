using Kaleidoscope.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kaleidoscope.ViewModel {
  public class CatalogHeaderViewModel : TableReferenceHeaderControlViewModel {
    public CatalogHeaderViewModel(Predicate<object> canUpdExecute, Predicate<object> canDelExecute) : base(canUpdExecute, canDelExecute) {
      Init();
    }

    #region Eventns
    public event Action AddRootCommandEvent;
    public event Action ReplaceCommandEvent;
    #endregion

    #region Commands
    public DelegateCommand AddRootCommand { get; private set; }
    public DelegateCommand ReplaceCommand { get; private set; }
    #endregion

    #region Commands implemenation
    protected virtual void AddRoot() {
      AddRootCommandEvent?.Invoke();
    }
    protected virtual void Replace() {
      ReplaceCommandEvent?.Invoke();
    }
    #endregion

    #region Helpers
    protected override void Init() {
      base.Init();
      AddCommand = new DelegateCommand(o => Add(), _canUpdExecute);
      AddRootCommand = new DelegateCommand(o => AddRoot(), _canUpdExecute);
      ReplaceCommand = new DelegateCommand(o => Replace(), _canUpdExecute);
    }
    #endregion

  }
}
