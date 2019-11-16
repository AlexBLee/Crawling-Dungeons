using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine.SceneManagement;
using UnityEditor.SceneManagement;
using System.Collections.Generic;

public class EnemyCreator : EditorWindow 
{

    [MenuItem("Tools/EnemyCreator")]
    private static void ShowWindow() 
    {

        var window = GetWindow<EnemyCreator>();
        window.titleContent = new GUIContent("EnemyCreator");
        window.Show();
    }

    private void OnGUI() 
    {
        

    }
}