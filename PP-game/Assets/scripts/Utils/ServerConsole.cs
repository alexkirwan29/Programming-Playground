using UnityEngine;
using System;
using System.IO;


namespace PP.Utils {
  public class ServerConsole : MonoBehaviour, ILogHandler {
    public static ServerConsole instance;

    private ILogHandler defaultlogHandler;

    public void LogException(Exception exception, UnityEngine.Object context) {
      var colour = Console.ForegroundColor;

      Console.ForegroundColor = ConsoleColor.Red;

      if (context != null)
        Console.Write($"[{context}] ");

      Console.WriteLine("Exception: " + exception.ToString());

      Console.ForegroundColor = colour;
    }

    public void LogFormat(LogType logType, UnityEngine.Object context, string format, params object[] args) {
      var colour = Console.ForegroundColor;

      switch (logType) {
        case LogType.Assert:
        case LogType.Error:
        case LogType.Exception:
          Console.ForegroundColor = ConsoleColor.Red;
          break;
        case LogType.Warning:
          Console.ForegroundColor = ConsoleColor.Yellow;
          break;
      }

      if (context != null)
        Console.Write($"[{context}] ");

      Console.WriteLine(format, args);

      Console.ForegroundColor = colour;
    }

    private void OnEnable() {
      instance = this;

      defaultlogHandler = Debug.unityLogger.logHandler;
      Debug.unityLogger.logHandler = this;

      Console.CancelKeyPress += CancelPress;

      Debug.Log("Console Started", this);
    }

    private void CancelPress(object sender, ConsoleCancelEventArgs e) {
      Application.Quit(0);
    }

    private void OnDisable() {
      instance = null;

      Debug.unityLogger.logHandler = defaultlogHandler;

      Debug.Log("Console Stopped", this);
    }
  }
}