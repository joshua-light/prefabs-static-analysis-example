using System;
using System.Diagnostics;

namespace Editor.Common
{
  public struct SimpleWatch : IDisposable
  {
    private readonly Action<long> _logElapsedMiliseconds;
    private readonly Stopwatch _watch;

    public SimpleWatch(Action<long> logElapsedMiliseconds)
    {
      _logElapsedMiliseconds = logElapsedMiliseconds;

      _watch = Stopwatch.StartNew();
    }

    public static SimpleWatch New(Action<long> logElapsedMiliseconds)
    {
      return new SimpleWatch(logElapsedMiliseconds);
    }

    public void Dispose()
    {
      _watch.Stop();
      _logElapsedMiliseconds(_watch.ElapsedMilliseconds);
    }
  }
}