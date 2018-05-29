using SQLite.Net.Attributes;
using System;
using System.ComponentModel;

namespace Kaleidoscope.Model {
  public enum DML { Empty, Insert, InsertRoot, Update, Replace, Delete, Synchronization }
  public abstract class ModelBase : INotifyPropertyChanged {
    protected ModelBase() {
    }

    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    public event PropertyChangedEventHandler PropertyChanged;

    public Action<PropertyChangedEventArgs> RaisePropertyChanged() {
      PropertyChangedEventHandler handler = this.PropertyChanged;
      return args => handler?.Invoke(this, args);
    }

    public virtual void OnPropertyChanged(string propertyName) {
      PropertyChangedEventHandler handler = this.PropertyChanged;
      handler?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
  }
}
