using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;
using UnityEngine.UIElements;

[CustomEditor(typeof(Music))]
public class MusicEditor : Editor
{
    private int _trackIndex = 0;
    private Dictionary<TrackMixer, TrackMixerInspector> _trackInspectors = new Dictionary<TrackMixer, TrackMixerInspector>();
    public override VisualElement CreateInspectorGUI()
    {
        _trackIndex = 0;
        _trackInspectors = new Dictionary<TrackMixer, TrackMixerInspector>();
        return base.CreateInspectorGUI();
    }
    public override void OnInspectorGUI()
    {
        //base.OnInspectorGUI();
        Music source = target as Music;

        bool forceUpdate = false;
        EditorGUI.BeginChangeCheck();
        SerializedProperty clips = serializedObject.FindProperty("Clips");
        EditorGUILayout.PropertyField(clips);
        serializedObject.ApplyModifiedProperties();
        if (EditorGUI.EndChangeCheck())
        {
            forceUpdate = true;
        }
        serializedObject.ApplyModifiedProperties();

        DebugVolumes(source);
        if (_trackIndex > source.MixersByIntensity.Count - 1) _trackIndex = source.MixersByIntensity.Count - 1;

        DisplayTrackIntensityEditor(source, forceUpdate);
        UpdateTrackOrder(source);
        UpdateTrackClips(source);
    }

    private void DisplayTrackIntensityEditor(Music source, bool forceUpdate = false)
    {
        EditorGUILayout.LabelField("Track Editor", EditorStyles.boldLabel);
        if (source.MixersByIntensity == null || source.MixersByIntensity.Count == 0)
        {
            DisplayTrackControls(source, false);
            return;
        }

        DisplayTrackSelect(source);

        GUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Intensity");
        source.MixersByIntensity[_trackIndex].Key = EditorGUILayout.Slider(source.MixersByIntensity[_trackIndex].Key, 0f, 1f);
        GUILayout.EndHorizontal();

        EditorGUILayout.Space(StyleCollection.DoubleSpace);
        DisplayTrackEditor(source, forceUpdate);
        DisplayTrackControls(source, source.MixersByIntensity.Count > 1);
    }
    
    private void DisplayTrackSelect(Music source)
    {
        if (source.MixersByIntensity == null || source.MixersByIntensity.Count == 0) return;

        GUILayout.BeginHorizontal();
        for (int i = 0; i < source.MixersByIntensity.Count; i++)
        {
            bool status = i == _trackIndex;
            if (GUILayout.Toggle(status, source.MixersByIntensity[i].Key.ToString(), "Button") && !status)
            {
                _trackIndex = i;
            }
        }
        GUILayout.EndHorizontal();
    }

    private void DisplayTrackEditor(Music source, bool forceUpdate = false)
    {
        if (source.MixersByIntensity == null || source.MixersByIntensity.Count == 0) return;

        TrackMixer mixer = source.MixersByIntensity[_trackIndex].Value;
        if (mixer == null) return;

        Rect screen = Screen.safeArea;

        _trackInspectors.TryGetValue(mixer, out TrackMixerInspector inspector);
        if (inspector == null)
        {
            inspector = new TrackMixerInspector(ref mixer, screen);
            _trackInspectors.Add(mixer, inspector);
        }

        if (forceUpdate) inspector.UpdateWaveforms(screen);
        inspector.Display(screen);
    }

    private void DisplayTrackControls(Music source, bool showRemove)
    {
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Track Controls");
        DisplayAddButton(source);
        if (showRemove) DisplayRemoveButton(source);
        EditorGUILayout.EndHorizontal();
    }
    private void DisplayAddButton(Music source)
    {
        if (GUILayout.Button("Add Track", EditorStyles.miniButton))
        {
            float value = 0;
            if (source.MixersByIntensity == null || source.MixersByIntensity.Count == 0) value = 1;
            else if (_trackIndex == source.MixersByIntensity.Count - 1) value = 1;
            else value = (source.MixersByIntensity[_trackIndex].Key + source.MixersByIntensity[_trackIndex + 1].Key) / 2f;

            Pair<float, TrackMixer> mixerPair = new Pair<float, TrackMixer>(value, new TrackMixer(source.Clips));
            if (source.MixersByIntensity.Count == 0)
            {
                source.MixersByIntensity.Add(mixerPair);
                _trackIndex = 0;
            }
            else
            { 
                source.MixersByIntensity.Insert(_trackIndex + 1, mixerPair);
                _trackIndex += 1;
            }
        }
    }

    private void DisplayRemoveButton(Music source)
    {
        int oldCount = source.MixersByIntensity.Count;
        if (GUILayout.Button("Remove Track", EditorStyles.miniButton))
        {
            source.MixersByIntensity.RemoveAt(_trackIndex);
            if (_trackIndex == oldCount - 1) _trackIndex = oldCount - 2;
        }
    }

    private void UpdateTrackOrder(Music source)
    {
        if (source.MixersByIntensity == null || source.MixersByIntensity.Count == 0) return;
        TrackMixer currentMixer = source.MixersByIntensity[_trackIndex].Value;
        source.MixersByIntensity = source.MixersByIntensity.OrderBy(x => x.Key).ToList();
        _trackIndex = source.MixersByIntensity.FindIndex(0, source.MixersByIntensity.Count, x => x.Value == currentMixer);
    }

    private void UpdateTrackClips(Music source)
    {
        if (source.MixersByIntensity == null || source.MixersByIntensity.Count == 0) return;
        for(int i = 0; i < source.MixersByIntensity.Count; i++)
        {
            source.MixersByIntensity[i].Value.UpdateTracks(source.Clips);
        }
    }

    private void DebugVolumes(Music source)
    {
        if (source.MixersByIntensity == null || source.MixersByIntensity.Count == 0) return;
        for (int i = 0; i < source.MixersByIntensity.Count; i++)
        {
            source.MixersByIntensity[i].Value.DebugVolumes();
        }
    }
}
