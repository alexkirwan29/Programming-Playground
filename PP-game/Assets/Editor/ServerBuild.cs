using UnityEditor;
using UnityEngine;
using System.IO;
using System.Diagnostics;
using System;

public class ServerBuild {
  private static Process serverProcess;

  [MenuItem("Networking/Client/Build And Run")]
  public static void BuildRunClient() {
    Stop(false);
    Build();
    Run(false);
  }
  [MenuItem("Networking/Server/Build And Run")]
  public static void BuildRunServer() {
    Stop(true);
    Build();
    Run(true);
  }
  [MenuItem("Networking/Client/Stop")]
  public static void StopClient() {
    Stop(false);
  }
  [MenuItem("Networking/Server/Stop")]
  public static void StopServer() {
    Stop(true);
  }
  [MenuItem("Networking/Client/Start")]
  public static void StartClient() {
    Run(false);
  }
  [MenuItem("Networking/Server/Restart")]
  public static void RestartServer() {
    Stop(true);
    Run(true);
  }
  [MenuItem("Networking/Server/Start")]
  public static void StartServer() {
    Run(true);
  }

  private static void Run(bool server) {
    if (server)
      Process.Start("./build-tools.sh", "server start");
    else
      Process.Start("./build-tools.sh", "client start");
  }
  private static void Stop(bool server) {
    if (server)
      Process.Start("./build-tools.sh", "server stop");
    else
      Process.Start("./build-tools.sh", "client stop");
  }

  [MenuItem("Networking/Build")]
  public static void Build() {
    UnityEngine.Debug.Log("Starting Build");
    var options = new BuildPlayerOptions();
    // The list of scenes to use in the server build.
    options.scenes = new[]
    {
        "Assets/Scenes/game-world.unity",
      };

#if UNITY_STANDALONE_WIN
      options.locationPathName = "builds\\pp-game.exe";
        options.target = BuildTarget.StandaloneWindows64;

#elif UNITY_STANDALONE_LINUX
    options.locationPathName = "builds/pp-game";
    options.target = BuildTarget.StandaloneLinux64;

    // For everything else... throw a hissyfit.
#else
      throw new NotImplementedException("This Platform is not Implemented");

#endif

    // Set the build target and extra options.
    options.targetGroup = BuildTargetGroup.Standalone;
    // options.options = BuildOptions.EnableHeadlessMode | BuildOptions.Development;
    options.options = BuildOptions.Development;

    // Actually build the server.
    BuildPipeline.BuildPlayer(options);
  }
}