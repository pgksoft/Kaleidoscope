using Kaleidoscope.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kaleidoscope.ViewModel {
  public enum NormalizeSize { S1024x768, S800x600, S640x480, S448x336, S314x235, S160x120 }
  public class OptionsMediaViewModel : ViewModelPrimary {

    #region Constructor
    public OptionsMediaViewModel(ContentWorkMode workMode) {
      _workMode = workMode;
      foreach (NormalizeSize item in Enum.GetValues(typeof(NormalizeSize))) {
        _listSizeImage.Add(item);
      }
      for (int i = Settings.Instance.SizePageImageLoadMin; i <= Settings.Instance.SizePageImageLoadMax; i = i + Settings.Instance.SizePageImageLoadStep) {
        _listImageSizePage.Add(i);
      }
      for (int i = Settings.Instance.SizePageVideoLoadMin; i <= Settings.Instance.SizePageVideoLoadMax; i = i + Settings.Instance.SizePageVideoLoadStep) {
        _listVideoSizePage.Add(i);
      }
      SizeImageSelectedIndex = Settings.Instance.GetNormalizedSizeImageIndex(WorkMode);
      SizePageImageSelectedIndex = Settings.Instance.GetSizePageImageLoadIndex(WorkMode);
      SizePageVideoSelectedIndex = Settings.Instance.GetSizePageVideoLoadIndex(WorkMode);
      _isCreated = true;
    }
    #endregion

    #region Fields
    private bool _isCreated = false;
    private ContentWorkMode _workMode = ContentWorkMode.None;
    private List<NormalizeSize> _listSizeImage = new List<NormalizeSize>();
    private List<int> _listImageSizePage = new List<int>();
    private List<int> _listVideoSizePage = new List<int>();
    private int _sizeImageSelectedIndex;
    private int _sizePageImageSelectedIndex;
    private int _sizePageVideoSelectedIndex;
    #endregion

    #region Properties
    public ContentWorkMode WorkMode => _workMode;
    public List<NormalizeSize> ListSizeImage => _listSizeImage;
    public List<int> ListImageSizePage => _listImageSizePage;
    public List<int> ListVideoSizePage => _listVideoSizePage;
    public int SizeImageSelectedIndex {
      get { return _sizeImageSelectedIndex; }
      set {
        this.MutateVerbose(ref _sizeImageSelectedIndex, value, RaisePropertyChanged());
        SaveOptions();
      }
    }
    public int SizePageImageSelectedIndex {
      get { return _sizePageImageSelectedIndex; }
      set {
        this.MutateVerbose(ref _sizePageImageSelectedIndex, value, RaisePropertyChanged());
        SaveOptions();
      }
    }
    public int SizePageVideoSelectedIndex {
      get { return _sizePageVideoSelectedIndex; }
      set {
        this.MutateVerbose(ref _sizePageVideoSelectedIndex, value, RaisePropertyChanged());
        SaveOptions();
      }
    }
    #endregion

    #region Helpers
    private void SaveOptions() {
      if (_isCreated) {
        if (WorkMode == ContentWorkMode.Main) {
          Properties.Settings.Default.NormalizedSizeImageMainIndex = SizeImageSelectedIndex;
          Properties.Settings.Default.SizePageImageLoadMainIndex = SizePageImageSelectedIndex;
          Properties.Settings.Default.SizePageVideoLoadMainIndex = SizePageVideoSelectedIndex;
        } else if (WorkMode == ContentWorkMode.Folders) {
          Properties.Settings.Default.NormalizedSizeImageFoldersIndex = SizeImageSelectedIndex;
          Properties.Settings.Default.SizePageImageLoadFoldersIndex = SizePageImageSelectedIndex;
          Properties.Settings.Default.SizePageVideoLoadFoldersIndex = SizePageVideoSelectedIndex;
        } else if (WorkMode == ContentWorkMode.Catalogs) {
          Properties.Settings.Default.NormalizedSizeImageCatalogsIndex = SizeImageSelectedIndex;
          Properties.Settings.Default.SizePageImageLoadCatalogsIndex = SizePageImageSelectedIndex;
          Properties.Settings.Default.SizePageVideoLoadCatalogsIndex = SizePageVideoSelectedIndex;
        }
        Properties.Settings.Default.Save();
      }
    }
    #endregion

  }
}
