using Kaleidoscope.Helpers;
using LocalizatorHelper;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kaleidoscope.ViewModel {
  class KaleidoscopeHeaderViewModel : ViewModelBase {
    #region Constants
    const double runingStateProcess = 20;
    const double stopedStateProcess = 100;
    #endregion
    public KaleidoscopeHeaderViewModel() {
      ResourceManagerService.LocaleChanged += (o, e) => OnListOfFillTypesLocalization(this, e);
      _listOfGridSize = new List<int>(Enumerable.Range(1, 100));
      _listTimerInterval = CreateListTimerInterval();
      SetListOfFillTypes();
      _listPropertiesGrid = new List<string>() { PropNameColumns, PropNameRows, PropNameFillTypes, PropNameIndexFillTypes };
      SelectedValueColumns = Properties.Settings.Default.SelectedValueColumns;
      SelectedValueRows = Properties.Settings.Default.SelectedValueRows;
      SelectedIndexFillTypes = SelectedIndexFillTypeValidate();
      SelectedValueFillTypes = Properties.Settings.Default.SelectedValueFillTypes;
      SelectedTimerInterval = Properties.Settings.Default.SelectedTimerInterval;
      FillProcessingCommand = new DelegateCommand(o => FillProcessing());
      IsRotation = true;
      StateProcess = runingStateProcess;
      _isCreated = true;
    }

    #region Fields
    private LocalisationHelper _localisationHelper = new LocalisationHelper();

    private bool _isAllowReFill = true;
    private IList<int> _listOfGridSize;
    private ObservableCollection<string> _listOfFillTypes;
    private IList<int> _listTimerInterval;
    private IList<string> _listPropertiesGrid;

    private int _selectedValueColumns;
    private int _selectedValueRows;
    private string _selectedValueFillTypes;
    private int _selectedIndexFillTypes;
    private int _selectedTimerInterval;

    private bool _isCreated;
    private bool _isProcessing;
    private bool _isRotation;
    private double _stateProcess;
    #endregion

    #region Properties
    public bool IsAllowReFill {
      get { return _isAllowReFill; }
      private set { _isAllowReFill = value; }
    }
    public IList<int> ListOfGridSize { get { return _listOfGridSize; } }
    public ObservableCollection<string> ListOfFillTypes { get { return _listOfFillTypes; } }
    public IList<int> ListTimerInterval { get { return _listTimerInterval; } }
    public IList<string> ListPropertiesGrid { get { return _listPropertiesGrid; } }
    public string PropNameColumns { get { return "SelectedValueColumns"; } }
    public int SelectedValueColumns {
      get { return _selectedValueColumns; }
      set {
        this.MutateVerbose(ref _selectedValueColumns, value, RaisePropertyChanged());
        UserSettingsSave();
      }
    }
    public string PropNameRows { get { return "SelectedValueRows"; } }
    public int SelectedValueRows {
      get { return _selectedValueRows; }
      set {
        this.MutateVerbose(ref _selectedValueRows, value, RaisePropertyChanged());
        UserSettingsSave();
      }
    }
    public string PropNameFillTypes { get { return "SelectedValueFillTypes"; } }
    public string SelectedValueFillTypes {
      get { return _selectedValueFillTypes; }
      set {
        this.MutateVerbose(ref _selectedValueFillTypes, value, RaisePropertyChanged());
        UserSettingsSave();
      }
    }
    public string PropNameIndexFillTypes { get { return "SelectedIndexFillTypes"; } }
    public int SelectedIndexFillTypes {
      get { return _selectedIndexFillTypes; }
      set { this.MutateVerbose(ref _selectedIndexFillTypes, value, RaisePropertyChanged()); }
    }
    public string PropNameTimerInterval { get { return "SelectedTimerInterval"; } }
    public int SelectedTimerInterval {
      get { return _selectedTimerInterval; }
      set {
        this.MutateVerbose(ref _selectedTimerInterval, value, RaisePropertyChanged());
        UserSettingsSave();
      }
    }
    public string PropNameIsProcessing { get { return "IsProcessing"; } }
    public bool IsProcessing {
      get { return _isProcessing; }
      set {
        this.MutateVerbose(ref _isProcessing, value, RaisePropertyChanged());
      }
    }
    public bool IsRotation {
      get { return _isRotation; }
      set {
        this.MutateVerbose(ref _isRotation, value, RaisePropertyChanged());
      }
    }
    public double StateProcess {
      get { return _stateProcess; }
      set {
        this.MutateVerbose(ref _stateProcess, value, RaisePropertyChanged());
      }
    }
    #endregion

    #region Commands
    public DelegateCommand FillProcessingCommand { get; }
    #endregion

    #region Commands implementation
    private void FillProcessing() {
      if (IsProcessing == true) {
        IsProcessing = false;
        IsRotation = false;
        StateProcess = stopedStateProcess;
        return;
      }
      IsRotation = true;
      IsProcessing = true;
      StateProcess = runingStateProcess;
    }
    #endregion

    #region Events implamentation
    private void OnListOfFillTypesLocalization(object sender, LocaleChangedEventArgs e) {
      IsAllowReFill = false;
      int indexFillType = SelectedIndexFillTypes;
      SetListOfFillTypes();
      SelectedIndexFillTypes = indexFillType;
      IsAllowReFill = true;
    }
    #endregion

    #region Helpers
    private void SetListOfFillTypes() {
      if (ListOfFillTypes == null) {
        _listOfFillTypes = new ObservableCollection<string> {
          _localisationHelper["ColorsRes.FillTypeItemAll"],
          _localisationHelper["ColorsRes.FillTypeItemLinearGradient"],
          _localisationHelper["ColorsRes.FillTypeItemRadialGradient"]
        };
      } else {
        _listOfFillTypes[0] = _localisationHelper["ColorsRes.FillTypeItemAll"];
        _listOfFillTypes[1] = _localisationHelper["ColorsRes.FillTypeItemLinearGradient"];
        _listOfFillTypes[2] = _localisationHelper["ColorsRes.FillTypeItemRadialGradient"];
      }
    }
    private void UserSettingsSave() {
      if (_isCreated) {
        Properties.Settings.Default.SelectedValueColumns = SelectedValueColumns;
        Properties.Settings.Default.SelectedValueRows = SelectedValueRows;
        Properties.Settings.Default.SelectedValueFillTypes = SelectedValueFillTypes;
        Properties.Settings.Default.SelectedTimerInterval = SelectedTimerInterval;
        Properties.Settings.Default.SelectedIndexFillTypes = SelectedIndexFillTypes;
        Properties.Settings.Default.Save();
      }
    }
    private IList<int> CreateListTimerInterval() {
      IList<int> temp = new List<int>(Enumerable.Range(1, 10));
      for (int i = 20; i < 1001; i = i + 10) {
        temp.Add(i);
      }
      for (int i = 1100; i < 5001; i = i + 100) {
        temp.Add(i);
      }
      return temp;
    }
    private int SelectedIndexFillTypeValidate() {
      if (Settings.Same().SelectedIndexFillType >= 0 && Settings.Same().SelectedIndexFillType < 3) {
        return Settings.Same().SelectedIndexFillType;
      } else {
        return 0;
      }
    }
    #endregion
  }
}
