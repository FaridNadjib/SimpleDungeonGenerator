
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TestEditorClass))]
public class TestEditor : Editor
{
    SerializedProperty prop;
    SerializedProperty prop2;

    private void OnEnable()
    {
        prop = serializedObject.FindProperty("angleLimits");
        prop2 = serializedObject.FindProperty("useAngleLimits");
    }

    public override void OnInspectorGUI()
    {
        //base.OnInspectorGUI();

        //var script = target as TestEditorClass;

        //GUILayout.BeginHorizontal();
        //script.useAngleLimits = GUILayout.Toggle(script.useAngleLimits, "Use Angle Limits:");

        //if (script.useAngleLimits)
        //{
        //    if (GUILayout.Button("Fafad")) Debug.Log("dasdassd");
        //}
        //GUILayout.EndHorizontal();
        //script.useAngleLimits = GUILayout.Toggle(script.useAngleLimits, "Use Angle Limits:");

        //script.useAngleLimits = GUILayout.Toggle(script.useAngleLimits, "Use Angle Limits:");
        serializedObject.Update();
        //prop2.boolValue = GUILayout.Toggle(prop2.boolValue, "Use Angle Limits:");
        //if (prop2.boolValue)
        //{
        //    if (GUILayout.Button("Fafad")) Debug.Log("dasdassd");
        //}

        EditorGUILayout.PropertyField(prop, GUIContent.none);

        //serializedObject.ApplyModifiedProperties();

    }
}
