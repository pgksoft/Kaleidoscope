using Kaleidoscope.HelpersView;
using Kaleidoscope.HelpersViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Kaleidoscope.Helpers {
  public class MessageConfirmBox : IDisposable {
    private static MessageConfirmBox _instance;
    protected MessageConfirmBox(string ImageTitleUrl,string message) {
      _confirmBox = new MessageConfirm() {
        DataContext = new MessageConfirmViewModel(message,
                                                  string.Empty,
                                                  ConfirmBoxButtons.OK) { TitleImageUrl = ImageTitleUrl }
      };
    }
    protected MessageConfirmBox(string ImageTitleUrl,
                                string message,
                                string caption) {
      _confirmBox = new MessageConfirm() {
        DataContext = new MessageConfirmViewModel(message,
                                                  caption,
                                                  ConfirmBoxButtons.OK) { TitleImageUrl = ImageTitleUrl }
      };
    }
    protected MessageConfirmBox(string ImageTitleUrl,
                                string message,
                                string caption,
                                ConfirmBoxButtons messageBoxButtons) {
      _confirmBox = new MessageConfirm() {
        DataContext = new MessageConfirmViewModel(message,
                                                  caption,
                                                  messageBoxButtons) { TitleImageUrl = ImageTitleUrl }
      };
    }
    protected MessageConfirmBox(string ImageTitleUrl,
                                string message,
                                string caption,
                                ConfirmBoxButtons messageBoxButtons,
                                ConfirmBoxImage confirmBoxImage) {
      _confirmBox = new MessageConfirm() {
        DataContext = new MessageConfirmViewModel(message,
                                                  caption,
                                                  messageBoxButtons,
                                                  confirmBoxImage) { TitleImageUrl = ImageTitleUrl }
      };
    }
    protected MessageConfirmBox(string ImageTitleUrl,
                                string message,
                                string caption,
                                ConfirmBoxButtons messageBoxButtons,
                                ConfirmBoxImage confirmBoxImage,
                                MessageBoxResult confirmBoxResult) {
      _confirmBox = new MessageConfirm() {
        DataContext = new MessageConfirmViewModel(message,
                                                  caption,
                                                  messageBoxButtons,
                                                  confirmBoxImage,
                                                  confirmBoxResult) { TitleImageUrl = ImageTitleUrl }
      };
    }

    #region Fields
    private static MessageConfirm _confirmBox;
    private bool _disposed;
    #endregion

    #region Show
    public static MessageBoxResult Show(string ImageTitleUrl, string message) {
      _instance = new MessageConfirmBox(ImageTitleUrl, message);
      return ConfirmationResult();
    }

    public static MessageBoxResult Show(string ImageTitleUrl, 
                                        string message,
                                        string textHeader) {
      _instance = new MessageConfirmBox(ImageTitleUrl, message,
                                        textHeader);
      return ConfirmationResult();
    }
    public static MessageBoxResult Show(string ImageTitleUrl, 
                                        string message,
                                        string textHeader,
                                        ConfirmBoxButtons messageBoxButtons) {
      _instance = new MessageConfirmBox(ImageTitleUrl, message,
                                        textHeader,
                                        messageBoxButtons);
      return ConfirmationResult(messageBoxButtons);
    }
    public static MessageBoxResult Show(string ImageTitleUrl, 
                                        string message,
                                        string textHeader,
                                        ConfirmBoxButtons messageBoxButtons,
                                        ConfirmBoxImage confirmBoxIcon) {
      _instance = new MessageConfirmBox(ImageTitleUrl, message,
                                        textHeader,
                                        messageBoxButtons,
                                        confirmBoxIcon);
      return ConfirmationResult(messageBoxButtons);
    }
    #endregion

    #region Helpers
    private static MessageBoxResult ConfirmationResult(ConfirmBoxButtons messageBoxButtons = ConfirmBoxButtons.OK) {
      MessageBoxResult confirmationResult = MessageBoxResult.None;
      var resultConfirmBox = _confirmBox.ShowDialog();
      // OK
      if (messageBoxButtons == ConfirmBoxButtons.OK && resultConfirmBox == true) {
        confirmationResult = MessageBoxResult.OK;
      }
      // OKCancel
      else if (messageBoxButtons == ConfirmBoxButtons.OKCancel && resultConfirmBox == true) {
        confirmationResult = MessageBoxResult.OK;
      } else if (messageBoxButtons == ConfirmBoxButtons.OKCancel && resultConfirmBox == false) {
        confirmationResult = MessageBoxResult.Cancel;
      }
      // YesNo
      else if (messageBoxButtons == ConfirmBoxButtons.YesNo && resultConfirmBox == true) {
        confirmationResult = MessageBoxResult.Yes;
      } else if (messageBoxButtons == ConfirmBoxButtons.YesNo && resultConfirmBox == false) {
        confirmationResult = MessageBoxResult.No;
      };
      return confirmationResult;
    }
    #endregion

    #region IDisposable implementation
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
      if (_confirmBox != null) {
        _confirmBox.Close();
      }
      _confirmBox = null;
    }
    ~MessageConfirmBox() {
      Dispose(false);
    }
    #endregion

  }
}
