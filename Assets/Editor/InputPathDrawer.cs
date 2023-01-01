using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(InputPathAttribute))]
public class InputPathDrawer : PropertyDrawer
{
    private float _standardPropertyHeight = 16f;
    private float _standardSpace = 6f;

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        float buttonWidth = _standardPropertyHeight * 2f;
        float buttonX = position.width - buttonWidth/2f;
        Rect buttonPosition = new Rect(buttonX, position.y, buttonWidth, _standardPropertyHeight);

        Rect propertyPosition = new Rect(position.x, position.y, position.width - buttonWidth - _standardSpace, _standardPropertyHeight);

        EditorGUI.PropertyField(propertyPosition, property, new GUIContent(property.name));

        if (GUI.Button(buttonPosition, new GUIContent(EditorGUIUtility.IconContent("Search On Icon")))) DrawInputSelect();
    }

    private void DrawInputSelect()
    {
        Debug.Log("Opening Input Select");
    }
}
