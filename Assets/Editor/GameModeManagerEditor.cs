using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

[CustomEditor(typeof(GameModeManager))]
public class GameModeManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        GameModeManager gmm = target as GameModeManager;
        DisplayModeWarnings(gmm);
    }

    private void DisplayModeWarnings(GameModeManager gmm)
    {
        foreach (int i in Enum.GetValues(typeof(GameMode)))
        {
            int occurenceCount = gmm._gameModeManagers.FindAll(x => (int)x.Key == i).Count;

            if (occurenceCount == 1) continue;
            EditorGUILayout.Space(StyleCollection.StandardSpace);

            if (occurenceCount == 0)
                EditorGUILayout.HelpBox($"Game Mode: '{(GameMode)i}' is not defined.", MessageType.Warning);
            else if (occurenceCount > 1)
                EditorGUILayout.HelpBox($"Game Mode: '{(GameMode)i}' defined {occurenceCount} times. Only the first definition will be used", MessageType.Warning);
        }
    }
}
