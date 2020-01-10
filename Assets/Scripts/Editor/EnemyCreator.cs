using UnityEngine;
using UnityEditor;
using UnityEditor.Animations;

public class EnemyCreator : EditorWindow 
{
    Enemy enemy;
    int hp, mp, str, intl, dex, will, def, experiencePoints, gold;
    Sprite initialSprite;
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
        experiencePoints = EditorGUILayout.IntField("EXP:", experiencePoints);
        gold = EditorGUILayout.IntField("Gold:", gold);

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

                // Stats
                enemy.hp = hp;
                enemy.maxHP = hp;
                enemy.mp = mp;
                enemy.maxMP = mp;
                enemy.str = str;
                enemy.intl = intl;
                enemy.dex = dex;
                enemy.will = will;
                enemy.def = def;
                enemy.experiencePoints = experiencePoints;
                enemy.gold = gold;

                // Creating animation folder + animator
                string enemyDestination = AssetDatabase.GUIDToAssetPath(AssetDatabase.CreateFolder("Assets/Enemies", enemyName));
                AssetDatabase.CreateFolder(enemyDestination, "Animations");
                AssetDatabase.CreateFolder(enemyDestination, "Art");

                var controller = AnimatorController.CreateAnimatorControllerAtPath(enemyDestination + "/Animations/" + enemyName + ".controller");

                string[] animationFilenames = new string[7];
                animationFilenames[0] = "/Idle.anim";
                animationFilenames[1] = "/Attack.anim";
                animationFilenames[2] = "/Hit.anim";
                animationFilenames[3] = "/Run.anim";
                animationFilenames[4] = "/Heal.anim";
                animationFilenames[5] = "/Cast.anim";
                animationFilenames[6] = "/Death.anim";

                AnimatorState[] animStates = new AnimatorState[7];

                for(int i = 0; i < animationFilenames.Length; i++)
                {
                    AnimationClip clip = new AnimationClip();
                    AssetDatabase.CreateAsset(clip, enemyDestination + "/Animations/" + animationFilenames[i]);

                    // Setting loop for Idle and Run animations.
                    if(i == 0 || i == 3)
                    {
                        AnimationClipSettings settings = AnimationUtility.GetAnimationClipSettings(clip);
                        settings.loopTime = true;
                        AnimationUtility.SetAnimationClipSettings(clip, settings);
                    }

                    animStates[i] = controller.AddMotion(clip);
                }

                enemy.anim.runtimeAnimatorController = controller;

                // Attaching animation states to each other
                string[] animationNames = new string[6];
                animationNames[0] = "Attack";
                animationNames[1] = "Hit";
                animationNames[2] = "Run";
                animationNames[3] = "UseItem";
                animationNames[4] = "Cast";
                animationNames[5] = "Death";
                
                controller.AddParameter(animationNames[0], AnimatorControllerParameterType.Trigger);
                controller.AddParameter(animationNames[1], AnimatorControllerParameterType.Trigger); 
                controller.AddParameter(animationNames[2], AnimatorControllerParameterType.Bool); 
                controller.AddParameter(animationNames[3], AnimatorControllerParameterType.Trigger); 
                controller.AddParameter(animationNames[4], AnimatorControllerParameterType.Trigger);
                controller.AddParameter(animationNames[5], AnimatorControllerParameterType.Trigger);

                for (int i = 1; i < animStates.Length; i++)
                {
                    var transition = animStates[0].AddTransition(animStates[i]);
                    transition.AddCondition(AnimatorConditionMode.If, 0, animationNames[i-1]);
                }

                // Only going to length - 1 as the Death state does not need to transition back into idle.
                for (int i = 1; i < animStates.Length - 1; i++)
                {
                    var transition = animStates[i].AddTransition(animStates[0]);
                    transition.hasExitTime = true;

                    if (i == 3)
                    {
                        transition.AddCondition(AnimatorConditionMode.IfNot, 0, "Run");
                    }
                }

                // These two transitions are manually done due to them not fitting into the transitions from/to idle (default state)
                // Run to Attack transition
                var attackTransition = animStates[3].AddTransition(animStates[1]);
                attackTransition.AddCondition(AnimatorConditionMode.If, 1, "Attack");
                attackTransition.hasExitTime = false;

                // Attack to run transition
                attackTransition = animStates[1].AddTransition(animStates[3]);
                attackTransition.hasExitTime = true;

                
                // Saving prefab
                string localPath = enemyDestination + "/" + enemyName + ".prefab";
                localPath = AssetDatabase.GenerateUniqueAssetPath(localPath);

                PrefabUtility.SaveAsPrefabAsset(enemy.gameObject, localPath);


            }

        }
    }
    
}