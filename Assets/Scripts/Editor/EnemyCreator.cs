using UnityEngine;
using UnityEditor;

public class EnemyCreator : EditorWindow 
{
    Enemy enemy;
    int hp, mp, str, intl, dex, will, def, hpCounter, experiencePoints;
    AnimationClip attack, cast, hit, heal, run, idle;
    Sprite initialSprite;
    Animator animator;

    string enemyName;

    [MenuItem("Tools/EnemyCreator")]
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

        attack = (AnimationClip)EditorGUILayout.ObjectField("Attack:", attack, typeof(AnimationClip), true);
        cast = (AnimationClip)EditorGUILayout.ObjectField("Cast:", cast, typeof(AnimationClip), true);
        hit = (AnimationClip)EditorGUILayout.ObjectField("Hit:", hit, typeof(AnimationClip), true);
        heal = (AnimationClip)EditorGUILayout.ObjectField("Heal:", heal, typeof(AnimationClip), true);
        run = (AnimationClip)EditorGUILayout.ObjectField("Run:", run, typeof(AnimationClip), true);
        idle = (AnimationClip)EditorGUILayout.ObjectField("Idle:",idle, typeof(AnimationClip), true);

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

                Animation enemyAnim = enemy.GetComponent<Animation>();
                enemyAnim.AddClip(attack, "Attack");
                enemyAnim.AddClip(cast, "Cast");
                enemyAnim.AddClip(hit, "Hit");
                enemyAnim.AddClip(heal, "Heal");
                enemyAnim.AddClip(run, "Run");
                enemyAnim.AddClip(idle, "Idle");

                string localPath = "Assets/Prefabs/Enemies/" + enemyName + ".prefab";
                localPath = AssetDatabase.GenerateUniqueAssetPath(localPath);

                PrefabUtility.SaveAsPrefabAsset(enemy.gameObject, localPath);


            }

        }
        



    }
}