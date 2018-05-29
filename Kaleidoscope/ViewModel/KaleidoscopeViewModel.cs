using Kaleidoscope.Helpers;
using Kaleidoscope.HelpersViewModel;
using MaterialDesignThemes.Wpf;
using RandomFill;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Kaleidoscope.ViewModel {
  public class KaleidoscopeViewModel : ViewModelBase {
    public KaleidoscopeViewModel(UserControl headerControl, HorizontalAlignment headerControlHorizontalAlignment) {
      _headerControl = headerControl;
      _headerControlHorizontalAlignment = headerControlHorizontalAlignment;
      _headerViewModel = _headerControl.DataContext as KaleidoscopeHeaderViewModel;
      _headerViewModel.PropertyChanged += ChangedProperty;
      _currentTypeFill = _typesFill[_headerViewModel.SelectedIndexFillTypes];
      _isStarted = true;
      GridKaleidoscope = CreateUniformGrid();
      _dispatcherTimer = new DispatcherTimer(TimeSpan.FromMilliseconds(_headerViewModel.SelectedTimerInterval), 
                                             DispatcherPriority.Normal, 
                                             new EventHandler(ReFill), 
                                             Dispatcher.CurrentDispatcher);
      _dispatcherTimer.Stop();
    }

    #region Fields
    private DispatcherTimer _dispatcherTimer;
    private RandomFills _randomFill = RandomFills.Instance();
    private bool _isStarted;
    private KaleidoscopeHeaderViewModel _headerViewModel;
    private UniformGrid _gridKaleidoscope;
    private TypeFill[] _typesFill = (TypeFill[])Enum.GetValues(typeof(TypeFill));
    private TypeFill _currentTypeFill;
    #endregion

    #region Properties
    public UniformGrid GridKaleidoscope {
      get { return _gridKaleidoscope; }
      set { this.MutateVerbose(ref _gridKaleidoscope, value, RaisePropertyChanged()); }
    }
    #endregion

    #region Events implementation
    private void ChangedProperty(object sender, PropertyChangedEventArgs e) {
      if (!_isStarted) {
        return;
      }
      if (e.PropertyName == _headerViewModel.PropNameIsProcessing) {
        if (_headerViewModel.IsProcessing) {
          _dispatcherTimer.Start();
        } else {
          _dispatcherTimer.Stop();
        }
      };
      if (_headerViewModel.ListPropertiesGrid.Contains(e.PropertyName)) {
        _currentTypeFill = _typesFill[_headerViewModel.SelectedIndexFillTypes];
        if (_headerViewModel.IsAllowReFill) {
          GridKaleidoscope = CreateUniformGrid();
        }
      }
      if (e.PropertyName == _headerViewModel.PropNameTimerInterval) {
        _dispatcherTimer.Interval = TimeSpan.FromMilliseconds(_headerViewModel.SelectedTimerInterval);
      }
    }
    #endregion

    #region Kaleidoscope
    public UniformGrid CreateUniformGrid() {
      UniformGrid grid = new UniformGrid() { Rows = _headerViewModel.SelectedValueRows, Columns = _headerViewModel.SelectedValueColumns };
      for (int i = 0; i < grid.Rows * grid.Columns; i++) {
        int margin = 3;
        margin = (grid.Rows > 40 || grid.Columns > 40) ? 1 : (grid.Rows > 20 || grid.Columns > 20 ? 2 : margin);
        grid.Children.Add(GetRectangle(margin));
      }
      return grid;
    }
    private void RectangleMouse(object sender, MouseEventArgs e) {
      (sender as Rectangle).Fill = GetGradientBrush();
    }
    public void ReFill() {
      foreach (var item in GridKaleidoscope.Children) {
        (item as Rectangle).Fill = GetGradientBrush();
      }
    }
    private void ReFill(object sender, EventArgs e) {
      ReFill();
    }
    private GradientBrush GetGradientBrush() {
      return _currentTypeFill == TypeFill.LineGradient ? _randomFill.GetLinearGradientBrush()
                                 : _currentTypeFill == TypeFill.RadialGradient ? _randomFill.GetRadialGradientBrush()
                                 : _randomFill.GetGradientBrush();
    }
    private Rectangle GetRectangle(int margin) {
      Rectangle rectanagle = new Rectangle();
      rectanagle.Margin = new Thickness(margin);
      rectanagle.MouseEnter += RectangleMouse;
      rectanagle.MouseLeave += RectangleMouse;
      rectanagle.Fill = GetGradientBrush();
      return rectanagle;
    }
    #endregion
  }
}
