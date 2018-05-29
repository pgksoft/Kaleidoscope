using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kaleidoscope.Helpers {
  public class ManagerMediaPages {
    public ManagerMediaPages(int countItems, int sizePage) {
      _countItems = countItems;
      _sizePage = sizePage;
      _countPages = countItems <= 0 || sizePage <= 0 ? 0 :
                    countItems <= sizePage ? 1 :
                    countItems % sizePage == 0 ? countItems / sizePage :
                    countItems / sizePage + 1;
      if (CountPages > 0) {
        MoveToFirstPage();
      }
    }

    #region Fields
    private int _countItems;
    private int _sizePage;
    private int _currentPage;
    private int _countPages;
    private int _indexFirstItemPage = 0;
    private int _indexLastItemPage = 0;
    #endregion

    #region Properties
    public int CountItems => _countItems;
    public int SizePage => _sizePage;
    public int CountPages => _countPages;
    public int CurrentPage {
      get { return _currentPage; }
      private set { _currentPage = value; }
    }
    public int IndexFirstItemPage {
      get { return _indexFirstItemPage; }
      private set { _indexFirstItemPage = value; }
    }
    public int IndexLastItemPage {
      get { return _indexLastItemPage; }
      private set { _indexLastItemPage = value; }
    }
    #endregion

    #region Methods
    public void FirstPage() {
      if (CanFirstPage()) {
        MoveToFirstPage();
      }
    }
    public void StepBackwardPage() {
      if (CanStepBackwardPage()) {
        MoveToBackwardPage();
      }
    }
    public void StepForwardPage() {
      if (CanStepForwardPage()) {
        MoveToForwardPage();
      }
    }
    public void LastPage() {
      if (CanLastPage()) {
        MoveToLastPage();
      }
    }

    public bool CanFirstPage() {
      return CountPages > 1 && CurrentPage != 1;
    }
    public bool CanStepBackwardPage() {
      return CountPages > 1 && CurrentPage != 1;
    }
    public bool CanStepForwardPage() {
      return CountPages > 1 && CurrentPage < CountPages;
    }
    public bool CanLastPage() {
      return CountPages > 1 && CurrentPage < CountPages;
    }

    public string CurrentStateCaption() {
      return CurrentPage == 0 || CountPages == 1 ? string.Empty : $"{CurrentPage} / {CountPages} ({IndexFirstItemPage}-{IndexLastItemPage})";
    }
    #endregion

    #region Helpers
    private void MoveToFirstPage() {
      CurrentPage = 1;
      SetIndexes();
    }
    private void MoveToBackwardPage() {
      CurrentPage--;
      SetIndexes();
    }
    private void MoveToForwardPage() {
      CurrentPage++;
      SetIndexes();
    }
    private void MoveToLastPage() {
      CurrentPage = CountPages;
      SetIndexes();
    }
    private void SetIndexes() {
      IndexFirstItemPage = (CurrentPage - 1) * SizePage + 1;
      IndexLastItemPage = CurrentPage < CountPages ? CurrentPage * SizePage : CountItems;
    }
    #endregion
  }
}
