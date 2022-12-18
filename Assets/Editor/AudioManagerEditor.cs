using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(AudioManager))]
public class AudioManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        AudioManager am = target as AudioManager;
        DrawSoundFileSelect(am);
        EditorGUILayout.Space(StyleCollection.DoubleSpace);
        base.OnInspectorGUI();
    }

    private void DrawSoundFileSelect(AudioManager am)
    {
        DrawFileSelect<Music>(am, "Music Objects", ref am.MusicFolder);
        GUILayout.Space(StyleCollection.StandardSpace);
        DrawFileSelect<SFX>(am, "Sound Effect Objects", ref am.SoundFolder);
    }

    private void DrawFileSelect<T>(AudioManager am, string title, ref string path) where T : Sound
    {
        EditorGUILayout.LabelField(title, EditorStyles.boldLabel);
        GUILayout.BeginHorizontal();
        EditorGUILayout.TextField(path);
        if (GUILayout.Button("Select"))
        {
            string absolute = EditorUtility.SaveFolderPanel("Select Build Scene Directory", Application.dataPath, "");
            path = FileUtil.GetProjectRelativePath(absolute);
        }
        GUILayout.EndHorizontal();

        GUILayout.Space(StyleCollection.StandardSpace);

        if (GUILayout.Button($"Set {title}"))
        {
            T[] objects = AssetUtility.GetAssetsAtPath<T>(path);
            int newObjectCount = objects.Length;
            if (EditorUtility.DisplayDialog("Set Build Scenes?",
                $"By selecting 'Yes,' all {title} will be replaced with the {newObjectCount} " +
                $"{((newObjectCount != 1) ? "objects" : "object")} in '{path}.'",
                "Yes", "No"))
            {
                am.SetSoundObjects<T>(objects);
            }
        }
    }
}
