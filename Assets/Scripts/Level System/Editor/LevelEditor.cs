using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Level))]
public class LevelEditor : Editor
{
    private SerializedProperty fishSpawnSettings;
    private SerializedProperty smallFishWeight;
    private SerializedProperty bigFishWeight;
    private SerializedProperty scoreNeeded;
    private SerializedProperty timeGiven;
    private SerializedProperty environment;

    private void OnEnable()
    {
        // Cache serialized properties
        fishSpawnSettings = serializedObject.FindProperty("fishSpawnSettings");
        scoreNeeded = serializedObject.FindProperty("scoreNeeded");
        timeGiven = serializedObject.FindProperty("timeGiven");
        environment = serializedObject.FindProperty("environment");

        if (fishSpawnSettings != null)
        {
            smallFishWeight = fishSpawnSettings.FindPropertyRelative("smallFishWeight");
            bigFishWeight = fishSpawnSettings.FindPropertyRelative("bigFishWeight");
        }
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        // Draw ScoreNeeded and TimeGiven properties
        EditorGUILayout.PropertyField(scoreNeeded);
        EditorGUILayout.PropertyField(timeGiven);
        EditorGUILayout.PropertyField(environment);

        // Display FishSpawnSettings foldout
        if (fishSpawnSettings != null)
        {
            EditorGUILayout.PropertyField(fishSpawnSettings, new GUIContent("Fish Spawn Settings"), false);
            if (fishSpawnSettings.isExpanded)
            {
                EditorGUI.indentLevel++;

                // Display smallFishWeight slider with auto-adjustment for bigFishWeight
                EditorGUI.BeginChangeCheck();
                EditorGUILayout.Slider(smallFishWeight, 0f, 1f, new GUIContent("Small Fish Weight"));
                if (EditorGUI.EndChangeCheck())
                {
                    bigFishWeight.floatValue = 1f - smallFishWeight.floatValue;
                }

                // Display bigFishWeight slider with auto-adjustment for smallFishWeight
                EditorGUI.BeginChangeCheck();
                EditorGUILayout.Slider(bigFishWeight, 0f, 1f, new GUIContent("Big Fish Weight (Auto)"));
                if (EditorGUI.EndChangeCheck())
                {
                    smallFishWeight.floatValue = 1f - bigFishWeight.floatValue;
                }

                // Draw other FishSpawnSettings properties
                EditorGUILayout.PropertyField(fishSpawnSettings.FindPropertyRelative("bonusFishProbability"));
                EditorGUILayout.PropertyField(fishSpawnSettings.FindPropertyRelative("speedMultiplier"));
                EditorGUILayout.PropertyField(fishSpawnSettings.FindPropertyRelative("fishSpawnInterval"));
                EditorGUILayout.PropertyField(fishSpawnSettings.FindPropertyRelative("fishSpawnIntervalRandomRange"));
                EditorGUILayout.PropertyField(fishSpawnSettings.FindPropertyRelative("smallFishPool"), true);
                EditorGUILayout.PropertyField(fishSpawnSettings.FindPropertyRelative("bigFishPool"), true);

                EditorGUI.indentLevel--;
            }
        }

        serializedObject.ApplyModifiedProperties();
    }
}
