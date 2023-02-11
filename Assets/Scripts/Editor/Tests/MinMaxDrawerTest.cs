//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEditor;

//[CustomPropertyDrawer(typeof(MinMaxValue<float>))]
//public class MinMaxDrawerTest : PropertyDrawer
//{
//    // default line height is 16 pixel.
//    private const float FOLDOUTHEIGHT = 16f;

//    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
//    {
//        //base.OnGUI(position, property, label);
//        EditorGUI.BeginProperty(position, null, property);

//        // Get the properties.
//        SerializedProperty minValue = property.FindPropertyRelative("minValue");
//        SerializedProperty maxValue = property.FindPropertyRelative("maxValue");

//        // Draw label and calculate new position.
//        label.tooltip = "tooltip";
//        label.text = "values:";
//        position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

//        int indent = EditorGUI.indentLevel;
//        EditorGUI.indentLevel = 0;

//        // Rect calculation.
//        Rect minRect = new Rect(position.x, position.y, position.width * 0.5f - 5, position.height);
//        Rect maxRect = new Rect(position.x + position.width * 0.5f, position.y, position.width * 0.5f - 5, position.height);

        
//        //EditorGUI.PropertyField(minRect, minValue, GUIContent.none);
//        EditorGUI.PropertyField(maxRect, maxValue, GUIContent.none);
//        float f1 = 1f;
//        float f2 = 10f;
//        float f3 = 2f;
//        float f4 = 8f;
//        EditorGUI.MinMaxSlider(minRect, ref f3, ref f4, f1, f2);
//        maxValue.floatValue = f4;


//        // Check for right useage.
//        if (minValue.floatValue > maxValue.floatValue)
//        {
//            minValue.floatValue = maxValue.floatValue;
//        }
//        else if (maxValue.floatValue < minValue.floatValue)
//            maxValue.floatValue = minValue.floatValue;

//        EditorGUI.indentLevel = indent;
//        EditorGUI.EndProperty();
//    }

//    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
//    {
//        // Eventually: the height * the lines my property needs.
//        return base.GetPropertyHeight(property, label);

//        //height = constante oben.
//        // enumType.enumNames.Length
//        // arrayType.arraySize
//        // height += EditorGUI.GetPropertyHeight(arrayType.GetArrayElementAtIndex(i))
//        // return height;
//        if (property.isExpanded)
//        {

//        }
//    }

//    //property.isExpanded = EditorGUI.Foldout(foldoutRect, property.isExpanded, label)
//    //GUI.backgroundcolor = Color;
//    // boool mybool = proptery.FindPropertyRelative("boolName").boolValue;



//}
