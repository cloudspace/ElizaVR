using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
using System.Diagnostics;

public class BuildTargets : MonoBehaviour
{
	[MenuItem("Cloudspace/Build Cardboard")]
	public static void BuildCardboard() {
		// Get filename.
		string path = EditorUtility.SaveFolderPanel ("Choose Location of Built Game", "", "");
		string[] levels = new string[] { "Assets/Office.unity" };

		preExportCardboard ();

		// Build player.
		BuildPipeline.BuildPlayer(levels, path + "/cardboard.apk", BuildTarget.Android, BuildOptions.None);

		// Other things we might could do here.
		// Copy a file from the project folder to the build folder, alongside the built game.
		// FileUtil.CopyFileOrDirectory ("Assets/Templates/Readme.txt", path + "Readme.txt");

		// Run the game (Process class from System.Diagnostics).
		//Process proc = new Process ();
		//proc.StartInfo.FileName = path + "BuiltGame.exe";
		//proc.Start ();
	}

	[MenuItem("Cloudspace/Build Gear")]
	public static void BuildGear() {
		string path = EditorUtility.SaveFolderPanel ("Choose Location of Built Game", "", "");
		string[] levels = new string[] { "Assets/Office.unity" };

		preExportGear ();
		BuildPipeline.BuildPlayer(levels, path + "/gear.apk", BuildTarget.Android, BuildOptions.None);
	}

	public static void preExportCardboard() {
		PlayerSettings.virtualRealitySupported = false;
		FindAndSetActive ("Cardboard", true);
		FindAndSetActive ("Gear", false);
	}

	public static void preExportGear() {
		PlayerSettings.virtualRealitySupported = true;
		FindAndSetActive ("Cardboard", false);
		FindAndSetActive ("Gear", true);
	}

	public static void preExportOcculus() {
		// TODO: Get requirements
	}

	private static void FindAndSetActive( string name, bool active)
	{   // not the most efficent, but faster than pointing and clicking
		// And seems to be the only way to find inactive Game Objects by name
		foreach (GameObject go in Resources.FindObjectsOfTypeAll (typeof(GameObject)) as GameObject[]) {
			if (go.name == name) {
				go.SetActive (active);
				break;
			}
		}
	}
}