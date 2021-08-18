using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Build.Reporting;

namespace BuildScript
{
	// ahhh
	class MyEditorScript
	{
		private static string[] Scenes = FindEnabledEditorScenes();

		private static string TargetDir = "build/";

		[MenuItem("Custom/CI/Build WebGL")]
		static void PerformWebGLBuild()
		{
			const string AppName = "CrawlingDungeons";
			GenericBuild(Scenes, TargetDir + AppName, BuildTarget.WebGL, BuildOptions.None);
		}

		[MenuItem("Custom/CI/Build Android")]
		static void PerformAndroidBuild()
		{
			const string query = "AndroidKeystorePassword";
			string keystorePassword = Environment.GetEnvironmentVariable(query);

			PlayerSettings.keyaliasPass = keystorePassword;
			PlayerSettings.keystorePass = keystorePassword;
			PlayerSettings.Android.bundleVersionCode++;
			PlayerSettings.bundleVersion = PlayerSettings.Android.bundleVersionCode.ToString();
			EditorUserBuildSettings.buildAppBundle = true;

			const string AppName = "CrawlingDungeons.aab";
			GenericBuild(Scenes, TargetDir + AppName, BuildTarget.Android, BuildOptions.None);
		}

		private static string[] FindEnabledEditorScenes()
		{
			List<string> EditorScenes = new List<string>();
			foreach (EditorBuildSettingsScene scene in EditorBuildSettings.scenes)
			{
				if (!scene.enabled) continue;
				EditorScenes.Add(scene.path);
			}

			return EditorScenes.ToArray();
		}

		static void GenericBuild(string[] scenes, string target_dir, BuildTarget build_target, BuildOptions build_options)
		{
			EditorUserBuildSettings.SwitchActiveBuildTarget(build_target);
			BuildReport res = BuildPipeline.BuildPlayer(scenes, target_dir, build_target, build_options);
		}
	}
}