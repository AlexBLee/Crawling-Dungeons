using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Build.Reporting;

namespace BuildScript
{
	// ahhh
	class MyEditorScript
	{
		static string[] SCENES = FindEnabledEditorScenes();

		static string APP_NAME = "CrawlingDungeons";
		static string TARGET_DIR = "target";

		[MenuItem("Custom/CI/Build WebGL")]
		static void PerformWebGLBuild()
		{
			string target_dir = APP_NAME;
			GenericBuild(SCENES, TARGET_DIR + "/" + target_dir, BuildTarget.WebGL, BuildOptions.None);
		}

		[MenuItem("Custom/CI/Build Android")]
		static void PerformAndroidBuild()
		{
			string query = "%AndroidKeystorePassword%";
			string keystorePassword = Environment.ExpandEnvironmentVariables(query);

			string target_dir = "build";
			string target = APP_NAME + ".apk";

			GenericBuild(SCENES, target_dir + "/" + target, BuildTarget.Android, BuildOptions.None);
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