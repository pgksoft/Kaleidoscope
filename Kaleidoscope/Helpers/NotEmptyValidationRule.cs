using System.Globalization;
using System.Windows.Controls;

namespace Kaleidoscope.Helpers {
  public class NotEmptyValidationRule : ValidationRule {
    public override ValidationResult Validate(object value, CultureInfo cultureInfo) {
      return string.IsNullOrWhiteSpace((value ?? "").ToString())
          ? new ValidationResult(false, Settings.Same().LocalisationHelper["DMLRes.FieldIsRequired"])
          : ValidationResult.ValidResult;
    }
  }
}
