using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Kaleidoscope.ViewModel {
  public abstract class ViewModelPrimary : INotifyPropertyChanged, IDisposable {
    protected ViewModelPrimary() {
    }
    #region Fields
    protected bool _disposed = false;
    #endregion

    public event PropertyChangedEventHandler PropertyChanged;
    public Action<PropertyChangedEventArgs> RaisePropertyChanged() {
      PropertyChangedEventHandler handler = this.PropertyChanged;
      return args => handler?.Invoke(this, args);
    }
    public virtual void OnPropertyChanged([CallerMemberName] string propertyName = null) {
      PropertyChangedEventHandler handler = this.PropertyChanged;
      handler?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    #region Dispose
    public void Dispose() {
      Dispose(true);
      this.OnDispose();
      GC.SuppressFinalize(this);
    }
    protected virtual void Dispose(bool disposing) {
      if (!this._disposed) {
        if (disposing) {
          OnDispose();
        }
        _disposed = true;
      }
    }
    protected virtual void OnDispose() {
    }
    ~ViewModelPrimary() {
      Dispose(false);
    }
    #endregion
  }
}
