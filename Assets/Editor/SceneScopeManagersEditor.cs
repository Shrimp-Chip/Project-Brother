using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using System.Linq;

public class SceneScopeManagersEditor : EditorWindow
{
    private static GameMode _currentMode = GameMode.Gameplay;
    private static int _currentManager = -1;

    private Editor _currentEditor;

    private const int _MAX_TABS_PER_ROW = 3;

    [MenuItem("Window/Scene Scope Managers")]
    public static void ShowWindow()
    {
        _currentMode = GameMode.Gameplay;
        _currentManager = -1;
        GetWindow<SceneScopeManagersEditor>("Scene Scope Managers");
    }

    public void OnGUI()
    {
        EditorGUILayout.Space(StyleCollection.DoubleSpace);

        EditorGUILayout.LabelField("Mode", EditorStyles.boldLabel);
        DrawModeSelect();

        EditorGUILayout.Space(StyleCollection.StandardSpace);

        EditorGUILayout.LabelField("Manager", EditorStyles.boldLabel);
        DrawManagerSelect();

        EditorGUILayout.Space(StyleCollection.DoubleSpace * 2);

        ShowSelectedEditor();
    }

    public void DrawModeSelect()
    {
        EditorGUILayout.BeginHorizontal();
        foreach(int i in Enum.GetValues(typeof(GameMode)))
        {
            if (i % _MAX_TABS_PER_ROW == 0)
            {
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.BeginHorizontal();
            }

            bool status = i == (int)_currentMode;
            if (GUILayout.Toggle(status, ((GameMode)i).ToString(), "Button") && !status) // Only when the value isn't already true
            {
                Debug.Log($"Selected new mode {(GameMode)i}");
                SetSelectedEditor(i);
            }
        }
        EditorGUILayout.EndHorizontal();
    }

    public void DrawManagerSelect()
    {
        List<SceneScopeManager> managers = GameModeManager.Instance.GetManagers(_currentMode);
        if (managers == null || managers.Count == 0 || !managers.Any(x => x != null))
        {
            EditorGUILayout.HelpBox($"No managers defined for {_currentMode}. Set them in the Game Mode Manager", MessageType.Warning);
            return;
        }

        EditorGUILayout.BeginHorizontal();
        int decrement = 0;
        for (int i = 0; i < managers.Count; i++)
        {
            if (managers[i] == null)
            {
                decrement++;
                continue;
            }

            if ((i - decrement) % _MAX_TABS_PER_ROW == 0)
            {
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.BeginHorizontal();
            }
            bool status = _currentManager == i;
            if (GUILayout.Toggle(status, StringUtility.AddSpacesToSentence(managers[i].name), "Button") && !status)
            {
                SetSelectedEditor((int)_currentMode, i);
            }
        }
        EditorGUILayout.EndHorizontal();
    }

    private void SetSelectedEditor(int gameMode, int manager = -1)
    {
        bool changedMode = (gameMode != (int)_currentMode);
        bool changedManager = (manager != _currentManager);

        if (changedMode && manager == -1) UpdateEditor(gameMode, manager);
        else if (changedManager) UpdateEditor(gameMode, manager);
    }

    private void UpdateEditor(int gameMode, int manager)
    {
        _currentMode = (GameMode)gameMode;
        _currentManager = manager;

        DestroyImmediate(_currentEditor);
        if (manager == -1)
        {
            _currentEditor = null;
            return;
        }

        SceneScopeManager inspected = GameModeManager.Instance.GetManagers((GameMode)gameMode)[manager];

        _currentEditor = Editor.CreateEditor(inspected);
    }

    private void ShowSelectedEditor()
    {
        if (_currentEditor == null)
        {
            EditorGUILayout.HelpBox("Please select a manager.", MessageType.Info);
            return;
        }

        _currentEditor.OnInspectorGUI();
    }
}
