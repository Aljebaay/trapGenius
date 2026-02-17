using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TrapBase), true)]
public class TrapBaseEditor : Editor
{
    private SerializedProperty mutationsProp;
    private SerializedProperty changesDataProp;
    private SerializedProperty referenceDataProp;

    private void OnEnable()
    {
        mutationsProp = serializedObject.FindProperty("mutations");
        changesDataProp = serializedObject.FindProperty("changesPlayerData");
        referenceDataProp = serializedObject.FindProperty("referenceData");
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        serializedObject.Update();

        EditorGUILayout.Space(10);
        Rect rect = EditorGUILayout.GetControlRect(false, 1);
        EditorGUI.DrawRect(rect, new Color(0.5f, 0.5f, 0.5f, 1));
        EditorGUILayout.Space(10);

        EditorGUILayout.LabelField("🧬 Mutation System", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(changesDataProp);

        if (changesDataProp.boolValue)
        {
            PlayerData data = referenceDataProp.objectReferenceValue as PlayerData;
            
            if (data == null)
            {
                EditorGUILayout.HelpBox("Could not load 'PlayerData' from Resources! Check filename.", MessageType.Error);
                if(GUILayout.Button("Retry Load")) (target as TrapBase).SendMessage("OnValidate");
            }

            if (GUILayout.Button("Add New Mutation"))
            {
                mutationsProp.InsertArrayElementAtIndex(mutationsProp.arraySize);
            }

            for (int i = 0; i < mutationsProp.arraySize; i++)
            {
                SerializedProperty item = mutationsProp.GetArrayElementAtIndex(i);
                SerializedProperty statType = item.FindPropertyRelative("statToChange");
                SerializedProperty numVal = item.FindPropertyRelative("numberValue");
                SerializedProperty boolVal = item.FindPropertyRelative("booleanValue");
                
                // NEW FIELDS
                SerializedProperty isTemp = item.FindPropertyRelative("isTemporaryChange");
                SerializedProperty duration = item.FindPropertyRelative("duration");

                EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                
                EditorGUILayout.PropertyField(statType, new GUIContent("Stat Type"));

                PlayerMutation.StatType type = (PlayerMutation.StatType)statType.enumValueIndex;
                string oldValText = "---";

                if (type == PlayerMutation.StatType.InvertControls)
                {
                    EditorGUILayout.PropertyField(boolVal, new GUIContent("New Value"));
                    if (data != null) oldValText = data.invertControls.ToString();
                }
                else
                {
                    EditorGUILayout.PropertyField(numVal, new GUIContent("New Value"));
                    if (data != null)
                    {
                        switch (type)
                        {
                            case PlayerMutation.StatType.MoveSpeed: oldValText = data.moveSpeed.ToString(); break;
                            case PlayerMutation.StatType.JumpHeight: oldValText = data.jumpHeight.ToString(); break;
                            case PlayerMutation.StatType.GravityMultiplier: oldValText = data.fallGravityMultiplier.ToString(); break;
                            case PlayerMutation.StatType.PlayerScale: oldValText = "1.0 (Multiplier)"; break; 
                        }
                    }
                }

                // --- NEW UI DRAWING ---
                EditorGUILayout.Space(5);
                EditorGUILayout.PropertyField(isTemp, new GUIContent("Is Temporary?"));
                if (isTemp.boolValue)
                {
                    EditorGUILayout.PropertyField(duration, new GUIContent("Duration (Seconds)"));
                }
                // ---------------------

                if (data != null)
                {
                    EditorGUILayout.LabelField($"Default Value: {oldValText}", EditorStyles.miniLabel);
                }

                if (GUILayout.Button("Remove"))
                {
                    mutationsProp.DeleteArrayElementAtIndex(i);
                }
                
                EditorGUILayout.EndVertical();
                EditorGUILayout.Space(2);
            }
        }

        serializedObject.ApplyModifiedProperties();
    }
    
    
}