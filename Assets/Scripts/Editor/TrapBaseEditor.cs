using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TrapBase), true)] // 'true' means "Apply to Child Classes too"
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
        // 1. Draw the Default Inspector (The child trap's specific settings)
        // This will draw all properties not explicitly handled below.
        DrawDefaultInspector();

        serializedObject.Update();

        // 2. Draw a Divider
        EditorGUILayout.Space(10);
        Rect rect = EditorGUILayout.GetControlRect(false, 1);
        EditorGUI.DrawRect(rect, new Color(0.5f, 0.5f, 0.5f, 1));
        EditorGUILayout.Space(10);

        // 3. Draw Mutation Logic
        EditorGUILayout.LabelField("🧬 Mutation System", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(changesDataProp);

        if (changesDataProp.boolValue)
        {
            PlayerData data = referenceDataProp.objectReferenceValue as PlayerData;
            
            if (data == null)
            {
                EditorGUILayout.HelpBox("Could not load 'PlayerData' from Resources! Check filename.", MessageType.Error);
                // Try reloading manually if button pressed
                if(GUILayout.Button("Retry Load"))
                {
                    (target as TrapBase).SendMessage("OnValidate");
                }
            }

            // Draw List
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
                        }
                    }
                }

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