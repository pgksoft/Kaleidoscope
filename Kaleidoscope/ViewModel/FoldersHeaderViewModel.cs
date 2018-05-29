using Kaleidoscope.Helpers;
using Kaleidoscope.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kaleidoscope.ViewModel {
  public class FoldersHeaderViewModel : TableReferenceHeaderControlViewModel {
    public FoldersHeaderViewModel(Predicate<object> canUpdExecute, Predicate<object> canDelExecute) : base(canUpdExecute, canDelExecute) {
        
    }

    #region Fields
    private ContentHeader _contentControlHeader;
    #endregion

    #region Properties
    public ContentHeader ContentControlHeader {
      get { return _contentControlHeader; }
      set { this.MutateVerbose(ref _contentControlHeader, value, RaisePropertyChanged()); }
    }
    #endregion

    #region Events
    public event Action SyncCommandEvent;
    #endregion

    #region Commands
    public DelegateCommand SyncCommand { get; protected set; }
    #endregion

    #region Commands implemenation
    protected virtual void Sync() {
      SyncCommandEvent?.Invoke();
    }
    #endregion

    #region Helpers
    protected override void Init() {
      base.Init();
      SyncCommand = new DelegateCommand(o => Sync(), _canUpdExecute);
    }
    #endregion
  }
}
