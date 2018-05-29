using Kaleidoscope.HelpersViewModel;
using System;
using System.Threading.Tasks;

namespace Kaleidoscope.Helpers {
  public sealed class ErrorProcessing {
    public static void Show(Exception e, bool isExceptionBase = true) {
      Exception eBase;
      if (isExceptionBase) {
        eBase = e.GetBaseException();
      } else {
        eBase = e;
      }
      MessageConfirmBox.Show(Properties.Settings.Default.AppImage,
                             $"\nError: {eBase.Message} " +
                             $"\nType: {eBase.TargetSite.ReflectedType.Name}" +
                             $"\nTargetSite: {eBase.TargetSite.Name}",
                             $"{eBase.TargetSite.Module.Name}",
                             ConfirmBoxButtons.OK,
                             ConfirmBoxImage.Error);
    }
    public static void Show(Exception e, string titleComment, bool isExceptionBase = true) {
      Exception eBase;
      if (isExceptionBase) {
        eBase = e.GetBaseException();
      } else {
        eBase = e;
      }
      MessageConfirmBox.Show(Properties.Settings.Default.AppImage,
                             $"\n{titleComment}" +
                             $"\nError: {eBase.Message} " +
                             $"\nType: {eBase.TargetSite.ReflectedType.Name}" +
                             $"\nTargetSite: {eBase.TargetSite.Name}",
                             $"{eBase.TargetSite.Module.Name}",
                             ConfirmBoxButtons.OK,
                             ConfirmBoxImage.Error
                            );
    }
    public static string GetExeptionContent(Exception e) {
      return $"\nError: {e.Message} " +
             $"\nType: {e.TargetSite.ReflectedType.Name}" +
             $"\nTargetSite: {e.TargetSite.Name}" +
             $"\n{e.TargetSite.Module.Name}" +
             $"\n--- Base Exception ---";
    }
  }
}
