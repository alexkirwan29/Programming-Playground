// This Editor extension adds a Open Terminal shortcut and menu item to the unity editor.
// Created by 45Ninjas.

// Works for unity 2019.3 running on Linux with Gnome 3.x.
// You should be smart enough to make it work for other versions or operating systems.

// https://gist.github.com/Those45Ninjas/8f4f6bb0c1aab30b7ea9fb873c1c2074

using System.Diagnostics;
using UnityEngine;
using UnityEditor;
using UnityEditor.ShortcutManagement;

public class OpenTerminalUtility
{
	[MenuItem("File/Open Terminal")]
	[Shortcut("OpenTerminal", null, KeyCode.T, ShortcutModifiers.Action, displayName = "Open Terminal")]
	public static void OpenTerminal()
	{
		// Create a new process of the gnome-terminal.
		using (Process newProcess = new Process())
		{

			// TODO: Support windows?
			// We are using the gnome-terminal executable.
			newProcess.StartInfo.FileName = "gnome-terminal";

			// With the --working-directory argument to set the WD to IO's CWD.
			newProcess.StartInfo.Arguments = $"--working-directory=\"{System.IO.Directory.GetCurrentDirectory()}\"";

			// Plop some output into the console, good or bad is better than none.
			if(newProcess.Start())
				UnityEngine.Debug.Log($"Opened gnome-terminal in {System.IO.Directory.GetCurrentDirectory()}");
			else
				UnityEngine.Debug.LogWarning("Unable to start gnome-terminal");
		}
	}
}
