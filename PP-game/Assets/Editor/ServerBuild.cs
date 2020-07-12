using UnityEditor;
using UnityEngine;
using System.IO;
using System.Diagnostics;
using System;

public class ServerBuild {
  private static Process serverProcess;
  [MenuItem("Server/Build And Run")]
  public static void BuildAndRun()
  {
    Stop();
    Build();
    Run();
  }
  // [MenuItem("Server/Run")]
  public static void Run()
  {
    Stop();

    serverProcess = new Process();
    serverProcess.StartInfo = new ProcessStartInfo("gnome-terminal", "-- ./run-server.sh unity-editor");
    serverProcess.Start();
  }
  // [MenuItem("Server/Stop")]
  public static void Stop()
  {
    if(serverProcess != null)
    {
      serverProcess.Close();

      serverProcess.WaitForExit(5);

      if(!serverProcess.HasExited)
      {
        UnityEngine.Debug.LogError("Unable to Close server. Killing");
        serverProcess.Kill();
      }

      serverProcess.Dispose();
      serverProcess = null;
    }
  }
  [MenuItem("Server/Build")]
  public static string Build() {
    UnityEngine.Debug.Log("Starting Build");
    // The defines for this build.
    string defines = "PP_SERVER";

    // Save the old defines before we set the new defines.
    // var oldDefines = PlayerSettings.GetScriptingDefineSymbolsForGroup(BuildTargetGroup.Standalone);
    // PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.Standalone, defines);

    UnityEngine.Debug.Log("Building with the following defines " + defines);

    // Build the project as a PP_SERVER.
    string filename;
    try {
      UnityEngine.Debug.Log("Building Server");
      filename = buildApplication();
    } catch (System.Exception e) {
      throw e;
    }

    // Set the defines back to how they where before we started poking around.
    // PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.Standalone, oldDefines);

    return filename;
  }

  private static string buildApplication() {
    var options = new BuildPlayerOptions();
    // The list of scenes to use in the server build.
    options.scenes = new[]
    {
        "Assets/Scenes/game-world.unity",
      };

    // For windows put the build in builds\server.exe
#if UNITY_STANDALONE_WIN
      options.locationPathName = "builds\\server.exe";
      options.target = BuildTarget.StandaloneWindows64

      // For linux put the build in builds/server
#elif UNITY_STANDALONE_LINUX
    options.locationPathName = "builds/server";
    options.target = BuildTarget.StandaloneLinux64;

    // For everything else... throw a hissyfit.
#else
      throw new NotImplementedException("This Platform is not Implemented");

#endif

    // Set the build target and extra options.
    options.targetGroup = BuildTargetGroup.Standalone;
    options.options = BuildOptions.EnableHeadlessMode | BuildOptions.Development;

    // Actually build the server.
    BuildPipeline.BuildPlayer(options);

    return options.locationPathName;
  }
}