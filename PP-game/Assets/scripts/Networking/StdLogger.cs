using UnityEngine;
using System;

public class StdLogger : ILogHandler
{
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
}