using UnityEditor;
using UnityEngine;
using System.IO;
using System.Diagnostics;

public class ServerBuild
{

  static Process runningProcess;

  [MenuItem("Build/Build And Run Server")]
  public static void BuildAndRun()
  {
    // Stop the old process.
    if(runningProcess != null)
      runningProcess.Close();
    
    // Build the server.
    var buildExecutable = Build();
    
    // Start a new process.
    runningProcess.StartInfo.FileName = buildExecutable;
    runningProcess.Start();
  }

  [MenuItem("Build/Build Server")]
  public static string Build()
  {
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
    options.options = BuildOptions.EnableHeadlessMode | BuildOptions.AllowDebugging;

    // Actually build the server.
    BuildPipeline.BuildPlayer(options);

    return options.locationPathName;
  }
}