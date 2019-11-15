using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class SceneCreator : EditorWindow 
{
    Object background;
    public string levelName;
    Enemy enemy;
    Enemy enemy2;
    Enemy enemy3;
    Enemy enemy4;

    [SerializeField] List<Object> enemyList = new List<Object>();
    int numberOfEnemies;
    ReorderableList list;


    private void Start() {
        
    }
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
        GUILayout.Label("A background size of 1280x720 is recommended.");
        background = EditorGUILayout.ObjectField("Image", background, typeof(Sprite), true);
        
        numberOfEnemies = EditorGUILayout.DelayedIntField("Number of enemies:", numberOfEnemies);

        enemy = (Enemy)EditorGUILayout.ObjectField("Enemy 1", enemy, typeof(Enemy), true);
        enemy2 = (Enemy)EditorGUILayout.ObjectField("Enemy 2", enemy2, typeof(Enemy), true);
        enemy3 = (Enemy)EditorGUILayout.ObjectField("Enemy 3", enemy3, typeof(Enemy), true);
        enemy4 = (Enemy)EditorGUILayout.ObjectField("Enemy 4", enemy4, typeof(Enemy), true);

        levelName = EditorGUILayout.TextField("Level Name: ", levelName);

        if(GUILayout.Button("Make Level!"))
        {
            if(levelName == null)
            {
                Debug.LogError("Please name the level.");
            }
            else
            {
                AssetDatabase.CopyAsset("Assets/Scenes/Level1.unity", "Assets/Scenes/" + levelName + ".unity");
            }
            
        }
        
    }
}