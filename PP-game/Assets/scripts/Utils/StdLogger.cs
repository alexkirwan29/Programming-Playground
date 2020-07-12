using UnityEngine;
using System;

namespace PP.Utils
{
  public class StdLogger : ILogHandler
  {
    private static StdLogger instance;

    public void LogException(Exception exception, UnityEngine.Object context)
    {
      var colour = Console.ForegroundColor;
      Console.ForegroundColor = ConsoleColor.Red;
      if(context != null)
        Console.WriteLine($"[{context.ToString()}] Exception: {exception.ToString()}");
      else
        Console.WriteLine($"[ Undefined ] Exception: {exception.ToString()}");
      Console.ForegroundColor = colour;
    }

    public void LogFormat(LogType logType, UnityEngine.Object context, string format, params object[] args)
    {
      var colour = Console.ForegroundColor;
      
      switch (logType)
      {
          case LogType.Assert:
          case LogType.Error:
          case LogType.Exception:
            Console.ForegroundColor = ConsoleColor.Red;
          break;
          case LogType.Warning:
            Console.ForegroundColor = ConsoleColor.Yellow;
          break;
      }

      if(context != null)
        Console.Write($"[{context.ToString()}] ");
      else
        Console.Write("[ Undefined ]");
        
      Console.WriteLine(format, args);

      Console.ForegroundColor = colour;
    }

    public static void MakeDefaultLogger(bool clear)
    {
      if(StdLogger.instance == null)
        StdLogger.instance = new StdLogger();

      Debug.unityLogger.logHandler = StdLogger.instance;

      if(clear)
        Console.Clear();
    }
  }
}