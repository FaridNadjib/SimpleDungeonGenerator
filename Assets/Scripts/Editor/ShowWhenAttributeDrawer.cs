using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


[CustomPropertyDrawer(typeof(ShowWhenAttribute))]
public class ShowWhenAttributeDrawer : PropertyDrawer
{
    private bool showField = true;

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        
            EditorGUI.BeginProperty(position, label, property);
        ShowWhenAttribute attribute = (ShowWhenAttribute)this.attribute;
        SerializedProperty conditionField = property.serializedObject.FindProperty(attribute.conditionFieldName);

        showField = conditionField.boolValue;

        if (showField)
        {
            EditorGUI.PropertyField(position, property, true);
        }
            EditorGUI.EndProperty();
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return base.GetPropertyHeight(property, label);
    }
}
