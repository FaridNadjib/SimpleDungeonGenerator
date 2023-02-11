using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(MinMaxValue<>))]
public class MinMaxValuePropertyDrawer : PropertyDrawer
{
    float min = 0f;
    float max = 0f;

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);
        
        // Get the properties.
        SerializedProperty minValue = property.FindPropertyRelative("minValue");
        SerializedProperty minLimit = property.FindPropertyRelative("minLimit");
        SerializedProperty maxValue = property.FindPropertyRelative("maxValue");
        SerializedProperty maxLimit = property.FindPropertyRelative("maxLimit");

        // Set label size.
        //EditorGUIUtility.labelWidth = 80f;
        
        position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);
        int indent = EditorGUI.indentLevel;
        EditorGUI.indentLevel = 0;

        // Calc the rects.
        Rect pos1 = new Rect(position.x, position.y, position.width * 0.15f, position.height);
        Rect pos2 = new Rect(position.x + position.width * 0.15f, position.y, position.width * 0.15f - 5, position.height);
        Rect pos3 = new Rect(position.x + position.width * 0.3f, position.y, position.width * 0.4f - 5, position.height);
        Rect pos4 = new Rect(position.x + position.width * 0.7f, position.y, position.width * 0.15f, position.height);
        Rect pos5 = new Rect(position.x + position.width * 0.85f, position.y, position.width * 0.15f, position.height);

        if (minLimit.type == "float")
        {
            // Show the properties.
            EditorGUI.PropertyField(pos1, minLimit, GUIContent.none);
            EditorGUI.PropertyField(pos2, minValue, GUIContent.none);
            EditorGUI.PropertyField(pos4, maxValue, GUIContent.none);
            EditorGUI.PropertyField(pos5, maxLimit, GUIContent.none);

            min = minValue.floatValue;
            max = maxValue.floatValue;

            EditorGUI.MinMaxSlider(pos3, ref min, ref max, minLimit.floatValue, maxLimit.floatValue);

            // Check for right values.
            if (min < minLimit.floatValue)
            {
                min = minLimit.floatValue;
            }
            else if (min > maxValue.floatValue)
            {
                min = maxValue.floatValue;
            }
            else if (max > maxLimit.floatValue)
            {
                max = maxLimit.floatValue;
            }
            else if (max < minValue.floatValue)
            {
                max = minValue.floatValue;
            }

            minValue.floatValue = min;
            maxValue.floatValue = max;
        }
        else if (minLimit.type == "int")
        {
            // Show the properties.
            EditorGUI.PropertyField(pos1, minLimit, GUIContent.none);
            EditorGUI.PropertyField(pos2, minValue, GUIContent.none);
            EditorGUI.PropertyField(pos4, maxValue, GUIContent.none);
            EditorGUI.PropertyField(pos5, maxLimit, GUIContent.none);

            min = minValue.intValue;
            max = maxValue.intValue;

            EditorGUI.MinMaxSlider(pos3, ref min, ref max, minLimit.intValue, maxLimit.intValue);

            // Check for right values.
            if (min < minLimit.intValue)
            {
                min = minLimit.intValue;
            }
            else if (min > maxValue.intValue)
            {
                min = maxValue.intValue;
            }
            else if (max > maxLimit.intValue)
            {
                max = maxLimit.intValue;
            }
            else if (max < minValue.intValue)
            {
                max = minValue.intValue;
            }

            minValue.intValue = (int)min;
            maxValue.intValue = (int)max;
        }

        EditorGUI.indentLevel = indent;
        EditorGUI.EndProperty();
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return base.GetPropertyHeight(property, label);
    }
}
