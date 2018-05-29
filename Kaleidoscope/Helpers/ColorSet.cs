using Kaleidoscope.ViewModel;
using RandomFill;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kaleidoscope.Helpers {
  public class ColorSet : ViewModelBase {
    const int defaultCapacity = 21;
    public ColorSet() : this(defaultCapacity) {
      _instance = this;
    }
    protected ColorSet(int capacity) {
      if (capacity > 0 && capacity<100) {
        Capacity = capacity;
      } else {
        Capacity = defaultCapacity;
      }
      OccasionalColor = _randomFill.GetColor().ToString();
      RandomColors = GetColors();
    }

    public static ColorSet Create() {
      if (_instance == null) {
        _instance = new ColorSet(defaultCapacity);
      }
      return _instance;
    }
    public static ColorSet Create(int capacity) {
      if (_instance==null) {
        _instance = new ColorSet(capacity);
      }
      return _instance;
    }

    #region Fields
    private static ColorSet _instance;
    private RandomFills _randomFill = RandomFills.Instance();
    private string[] _randomColors;
    private string _occasionalColor;
    private Random _rand = new Random();
    #endregion

    #region Properties
    public int Capacity { get; }
    public string[] RandomColors {
      get { return _randomColors; }
      set { this.MutateVerbose(ref _randomColors, value, RaisePropertyChanged()); }
    }
    public string OccasionalColor {
      get { return RandomColors[_rand.Next(0, Capacity)]; }
      set { this.MutateVerbose(ref _occasionalColor, value, RaisePropertyChanged()); }
    }
    #endregion

    #region Event implementation
    public void RedefineColors() {
      RandomColors = GetColors();
      OccasionalColor = OccasionalColor;
    }
    #endregion

    #region Helepers
    private string[] GetColors() {
      string[] temp = new string[Capacity];
      for (int i = 0; i < temp.Length; i++) {
        temp[i] = _randomFill.GetColor().ToString();
      }
      return temp;
    }
    #endregion

  }
}
