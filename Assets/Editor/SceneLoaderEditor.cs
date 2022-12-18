using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using System.Linq;

[CustomEditor(typeof(SceneLoader))]
public class SceneLoaderEditor : Editor
{
    public override void OnInspectorGUI()
    {
        //base.OnInspectorGUI();
        SceneLoader sl = target as SceneLoader;

        DrawBuildFileSelect(sl);

        EditorGUILayout.Space(StyleCollection.DoubleSpace);

        base.OnInspectorGUI();

        DrawTransitionWarnings(sl);
    }

    private void DrawBuildFileSelect(SceneLoader sl)
    {
        EditorGUILayout.LabelField("Build Scene Path", EditorStyles.boldLabel);
        GUILayout.BeginHorizontal();
        EditorGUILayout.TextField(sl.ScenePath);
        if (GUILayout.Button("Select"))
        {
            string absolute = EditorUtility.SaveFolderPanel("Select Build Scene Directory", Application.dataPath, "");
            sl.ScenePath = FileUtil.GetProjectRelativePath(absolute);
        }
        GUILayout.EndHorizontal();
        GUILayout.Space(StyleCollection.StandardSpace);
        if (GUILayout.Button("Set Build Scenes"))
        {
            int newScenesCount = AssetUtility.GetAssetsAtPath<SceneAsset>(sl.ScenePath).Length;
            if (EditorUtility.DisplayDialog("Set Build Scenes?", 
                $"By selecting 'Yes,' all scenes in the build will be replaced with the {newScenesCount} " +
                $"{((newScenesCount != 1) ? "scenes" : "scene")} in '{sl.ScenePath}.' \n You may need to reorder scenes after committing this change.",
                "Yes", "No"))
            {
                SceneAdder.SetBuildScenes(sl.ScenePath);
            }
        }
    }

    private void DrawTransitionWarnings(SceneLoader sl)
    {
        foreach(int i in Enum.GetValues(typeof(SceneTransition)))
        {
            int occurenceCount = sl.SceneTransitions.FindAll(x => (int)x.Key == i).Count;

            if (occurenceCount == 1) continue;
            EditorGUILayout.Space(StyleCollection.StandardSpace);

            if (occurenceCount == 0)
                EditorGUILayout.HelpBox($"Transition type: '{(SceneTransition)i}' is not defined.", MessageType.Warning);
            else if (occurenceCount > 1)
                EditorGUILayout.HelpBox($"Transition type: '{(SceneTransition)i}' defined {occurenceCount} times. Only the first definition will be used", MessageType.Warning);
        }
    }
}
