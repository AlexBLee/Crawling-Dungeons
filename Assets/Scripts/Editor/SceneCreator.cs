using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class SceneCreator : EditorWindow 
{
    Object background;
    SerializedObject enemy;
    [SerializeField] List<Object> enemyList = new List<Object>();
    int numberOfEnemies;

    [MenuItem("2D-RPG-Mobile-Game/SceneCreator")]
    private static void ShowWindow() 
    {
        var window = GetWindow<SceneCreator>();
        window.titleContent = new GUIContent("SceneCreator");
        window.Show();
    }

    private void OnGUI() 
    {
        GUILayout.Label("Background", EditorStyles.boldLabel);
        background = EditorGUILayout.ObjectField("Image", background, typeof(Sprite), true);

        Rect rect = (Rect)EditorGUILayout.BeginVertical("Enemy List");
        numberOfEnemies = EditorGUILayout.DelayedIntField("Number of enemies:", numberOfEnemies);
        var list = enemy.FindProperty("enemyList");
        EditorGUILayout.PropertyField(list, new GUIContent("Test"), true);

        EditorGUILayout.EndVertical();
        
    }
}