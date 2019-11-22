using UnityEngine;
using UnityEditor;
using UnityEditor.Animations;

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

                // Creating animation folder + animator
                AssetDatabase.CreateFolder("Assets/Animations", enemyName);
                var controller = UnityEditor.Animations.AnimatorController.CreateAnimatorControllerAtPath("Assets/Animations/" + enemyName + "/" + enemyName + ".controller");

                string[] animationFilenames = new string[6];
                animationFilenames[0] = "/Idle.anim";
                animationFilenames[1] = "/Run.anim";
                animationFilenames[2] = "/Attack.anim";
                animationFilenames[3] = "/Hit.anim";
                animationFilenames[4] = "/Heal.anim";
                animationFilenames[5] = "/Cast.anim";

                AnimatorState[] animStates = new AnimatorState[6];

                for(int i = 0; i < animationFilenames.Length; i++)
                {
                    AnimationClip clip = new AnimationClip();
                    AssetDatabase.CreateAsset(clip, "Assets/Animations/" + enemyName + animationFilenames[i]);
                    animStates[i] = controller.AddMotion(clip);
                }

                enemy.anim.runtimeAnimatorController = controller;

                // Attaching animation states to each other
                string[] animationNames = new string[6];
                animationFilenames[0] = "Attack";
                animationFilenames[1] = "Hit";
                animationFilenames[2] = "Run";
                animationFilenames[3] = "BackwardsRun";
                animationFilenames[4] = "UseItem";
                animationFilenames[5] = "Cast";
                
                controller.AddParameter(animationNames[0], AnimatorControllerParameterType.Trigger);
                controller.AddParameter(animationNames[1], AnimatorControllerParameterType.Trigger); 
                controller.AddParameter(animationNames[2], AnimatorControllerParameterType.Bool); 
                controller.AddParameter(animationNames[3], AnimatorControllerParameterType.Bool); 
                controller.AddParameter(animationNames[4], AnimatorControllerParameterType.Trigger); 
                controller.AddParameter(animationNames[5], AnimatorControllerParameterType.Trigger);

                for(int i = 0; i < animStates.Length; i++)
                {
                    var transition = animStates[0].AddTransition(animStates[i]);
                    transition.AddCondition(AnimatorConditionMode.If, 0, "Idle");

                }






                
                // Saving prefab
                string localPath = "Assets/Prefabs/Enemies/" + enemyName + ".prefab";
                localPath = AssetDatabase.GenerateUniqueAssetPath(localPath);

                PrefabUtility.SaveAsPrefabAsset(enemy.gameObject, localPath);


            }

        }
    }
    
}