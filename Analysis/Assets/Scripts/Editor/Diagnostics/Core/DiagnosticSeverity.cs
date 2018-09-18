namespace Editor.Diagnostics.Core
{
  public static class DiagnosticSeverityExtensions
  {
    public static bool IsHighestLevel(this DiagnosticSeverity self) =>
      self == DiagnosticSeverity.Error;

    public static bool IsGreaterThen(this DiagnosticSeverity self, DiagnosticSeverity other) =>
      self > other;
  }
  
  public enum DiagnosticSeverity
  {
    None = 0,
    Hint = 1,
    Warning = 2,
    Error = 3
  }
}