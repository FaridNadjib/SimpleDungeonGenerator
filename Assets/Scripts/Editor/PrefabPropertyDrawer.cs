using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(Prefab))]
public class PrefabPropertyDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        // Get the properties.
        SerializedProperty prefab = property.FindPropertyRelative("prefab");
        SerializedProperty spawnChance = property.FindPropertyRelative("spawnChance");

        // Set label size.
        EditorGUIUtility.labelWidth = 15f;
        int indent = EditorGUI.indentLevel;
        EditorGUI.indentLevel = 0;
        
        // Calc the rects.
        Rect pos1 = new Rect(position.x, position.y, position.width * 0.5f - 2, position.height);
        Rect pos2 = new Rect(position.x + position.width * 0.5f, position.y, position.width * 0.5f, position.height);

        // Show the properties.
        EditorGUI.PropertyField(pos1, prefab, GUIContent.none);
        EditorGUI.Slider(pos2, spawnChance, 0f, 1f, new GUIContent("%:", "The chance of the object to be picked."));

        EditorGUI.indentLevel = indent;
        EditorGUI.EndProperty();
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return base.GetPropertyHeight(property, label);
    }
}
