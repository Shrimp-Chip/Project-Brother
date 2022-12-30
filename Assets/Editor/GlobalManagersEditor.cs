using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class GlobalManagersEditor : EditorWindow
{
    private static int _displayIndex;
    private static ScriptableObject[] _inspectorsToDraw = null;
    private Editor _currentEditor = null;

    [MenuItem("Window/Global Managers")]
    public static void ShowWindow()
    {
        _displayIndex = 0;
        EditorWindow.GetWindow<GlobalManagersEditor>("Global Managers");
    }
    public void OnGUI()
    {
        SetInspectors();
        EditorGUILayout.Space(StyleCollection.DoubleSpace);
        DrawInspectorSelect();

        EditorGUILayout.Space(StyleCollection.StandardSpace);

        if (_displayIndex >= _inspectorsToDraw.Length)
        {
            MissingInspectorError();
            return;
        }

        if (GUILayout.Button(_inspectorsToDraw[_displayIndex].name, "ObjectField"))
        { 
            EditorGUIUtility.PingObject(_inspectorsToDraw[_displayIndex]);
        }

        EditorGUILayout.Space(StyleCollection.DoubleSpace * 2);

        ShowSelectedEditor(_displayIndex);
    }

    private void SetInspectors()
    {
        _inspectorsToDraw = new ScriptableObject[]
        {
            GameModeManager.Instance,
            SceneLoader.Instance,
            AudioManager.Instance
        };
    }
    private void DrawInspectorSelect()
    {
        EditorGUILayout.BeginHorizontal();
        for (int i = 0; i < _inspectorsToDraw.Length; i++)
        {
            string title = StringUtility.AddSpacesToSentence(_inspectorsToDraw[i].GetType().Name);
            bool status = _displayIndex == i;
            if (GUILayout.Toggle(status, title, "Button") && !status)
            {
                SetSelectedEditor(i);
            }
        }
        EditorGUILayout.EndHorizontal();
    }
    private void MissingInspectorError()
    {
        EditorGUILayout.HelpBox("Inspector is not defined", MessageType.Error);
    }
    private void ShowSelectedEditor(int inspectorIndex)
    {
        _currentEditor?.OnInspectorGUI();
    }
    private void SetSelectedEditor(int inspectorIndex)
    {
        if (_displayIndex == inspectorIndex) return;

        _displayIndex = inspectorIndex;
        DestroyImmediate(_currentEditor);

        ScriptableObject currentObject = _inspectorsToDraw[inspectorIndex];
        Editor newEditor = Editor.CreateEditor(currentObject);
        _currentEditor = newEditor;
    }
}
