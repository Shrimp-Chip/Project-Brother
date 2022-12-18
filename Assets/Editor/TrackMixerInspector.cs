using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

public class TrackMixerInspector
{
    private TrackMixer _mixer = null;
    private Dictionary<Track, Texture2D> _waveformsByMixer = new Dictionary<Track, Texture2D>();
    private Rect _prevScreen;

    public TrackMixerInspector(ref TrackMixer mixer, Rect screen)
    {
        _mixer = mixer;
        for (int i = 0; i < mixer.Tracks.Count; i++)
        {
            if (_mixer.Tracks[i] == null) continue;
            _waveformsByMixer.Add(_mixer.Tracks[i], GenerateWaveForm(_mixer.Tracks[i], screen));
        }
        _prevScreen = screen;
    }

    public void Display(Rect screen)
    {
        if (_mixer.Tracks == null || _mixer.Tracks.Count == 0) return;
        for(int i = 0; i < _mixer.Tracks.Count; i++)
        {
            if (_mixer.Tracks[i] == null) continue;
            Track track = _mixer.Tracks[i];
            DrawTrack(ref track, screen);
            EditorGUILayout.Space(StyleCollection.StandardSpace);
        }
        _prevScreen = screen;
    }

    public void DrawTrack(ref Track track, Rect screen)
    {
        if (track.Clip == null) return;

        EditorGUILayout.LabelField(track.Clip.name, EditorStyles.boldLabel);

        EditorGUILayout.BeginHorizontal();
        Texture2D waveform = GetWaveform(track, screen);
        GUILayout.Label(waveform);

        EditorGUILayout.BeginVertical();
        EditorGUI.BeginChangeCheck();
        EditorGUILayout.LabelField("Volume");
        track.Volume = EditorGUILayout.Slider(track.Volume, 0f, 2f);
        if (EditorGUI.EndChangeCheck())
        {
            UpdateWaveform(track, screen);
        }
        EditorGUILayout.EndVertical();
        EditorGUILayout.EndHorizontal();
    }

    private Texture2D GetWaveform(Track track, Rect currentScreen)
    {
        Texture2D waveform = null;
        if (currentScreen.width == _prevScreen.width)
        {
            _waveformsByMixer.TryGetValue(track, out waveform);
            if (waveform != null) return waveform;
        }

        UpdateWaveform(track, currentScreen);
        _waveformsByMixer.TryGetValue(track, out waveform);
        return waveform;
    }
    public void UpdateWaveforms(Rect screen)
    {
        for (int i = 0; i < _mixer.Tracks.Count; i++)
        {
            UpdateWaveform(_mixer.Tracks[i], screen);
        }
    }
    private void UpdateWaveform(Track track, Rect screen)
    {
        // Debug.Log("Generating new waveform");
        Texture2D newWaveform = GenerateWaveForm(track, screen);
        if (_waveformsByMixer.ContainsKey(track)) _waveformsByMixer[track] = newWaveform;
        else _waveformsByMixer.Add(track, newWaveform);
    }
    private Texture2D GenerateWaveForm(Track track, Rect screen)
    {
        if (track == null || track.Clip == null) return Texture2D.blackTexture;
        return PaintWaveformSpectrum(track.Clip, Mathf.CeilToInt(screen.width * 0.5f), Mathf.CeilToInt(screen.width * 0.05f), Color.yellow, track.Volume);
    }
    private Texture2D PaintWaveformSpectrum(AudioClip audio, int width, int height, Color col, float amplitude = 1)
    {
        Texture2D tex = new Texture2D(width, height, TextureFormat.RGBA32, false);
        float[] samples = new float[audio.samples];
        float[] waveform = new float[width];
        audio.GetData(samples, 0);
        int packSize = (audio.samples / width) + 1;
        int s = 0;
        for (int i = 0; i < audio.samples; i += packSize)
        {
            waveform[s] = Mathf.Abs(samples[i]) * amplitude;
            s++;
        }

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                tex.SetPixel(x, y, new Color(0,0,0,0));
            }
        }

        for (int x = 0; x < waveform.Length; x++)
        {
            for (int y = 0; y <= waveform[x] * ((float)height * .75f); y++)
            {
                tex.SetPixel(x, (height / 2) + y, col);
                tex.SetPixel(x, (height / 2) - y, col);
            }
        }
        tex.Apply();

        return tex;
    }
}
