using UnityEngine;
using UnityEditor;

public class EnemyCreator : EditorWindow 
{
    Enemy enemy;
    int hp, mp, str, intl, dex, will, def, hpCounter, experiencePoints;
    Sprite initialSprite;
    Animation enemyAnim;

    string enemyName;

    [MenuItem("Tools/Enemy Creator")]
    private static void ShowWindow() 
    {
        var window = GetWindow<EnemyCreator>();
        window.titleContent = new GUIContent("EnemyCreator");
        window.Show();
    }

    private void OnGUI() 
    {
        EditorGUILayout.LabelField("Enemy Creator:", EditorStyles.boldLabel);
        initialSprite = (Sprite)EditorGUILayout.ObjectField("Initial Sprite:", initialSprite, typeof(Sprite), true);

        EditorGUILayout.Space();

        hp = EditorGUILayout.IntField("HP:", hp);
        mp = EditorGUILayout.IntField("MP:", mp);
        str = EditorGUILayout.IntField("Str:", str);
        intl = EditorGUILayout.IntField("Int:", intl);
        dex = EditorGUILayout.IntField("Dex:", dex);
        will = EditorGUILayout.IntField("Will:" ,will);
        def = EditorGUILayout.IntField("Def:", def);
        hpCounter = EditorGUILayout.IntField("# of heals:", hpCounter);
        experiencePoints = EditorGUILayout.IntField("EXP:", experiencePoints);

        EditorGUILayout.Space();
        EditorGUILayout.Space();

        enemyName = EditorGUILayout.TextField("Enemy name: ", enemyName);

        EditorGUILayout.Space();

        if(GUILayout.Button("Create Enemy"))
        {
            if(enemyName == null)
            {
                Debug.LogError("Please name the enemy.");
            }
            else
            {
                enemy = PrefabUtility.LoadPrefabContents("Assets/Prefabs/Enemies/Enemy.prefab").GetComponent<Enemy>();

                // Reset the animation
                enemy.anim.runtimeAnimatorController = null;

                if(initialSprite != null)
                {
                    enemy.GetComponent<SpriteRenderer>().sprite = initialSprite;
                }
                
                enemy.hp = hp;
                enemy.maxHP = hp;
                enemy.mp = mp;
                enemy.maxMP = mp;
                enemy.str = str;
                enemy.intl = intl;
                enemy.dex = dex;
                enemy.will = will;
                enemy.def = def;
                enemy.hpCounter = hpCounter;
                enemy.experiencePoints = experiencePoints;

                // Saving
                string localPath = "Assets/Prefabs/Enemies/" + enemyName + ".prefab";
                localPath = AssetDatabase.GenerateUniqueAssetPath(localPath);

                PrefabUtility.SaveAsPrefabAsset(enemy.gameObject, localPath);


            }

        }
    }
    
}