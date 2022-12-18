using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

[CreateAssetMenu(fileName = "G_Audio_Manager", menuName = "Managers/Global/Audio Manager")]
public class AudioManager : GlobalManager<AudioManager>
{
    [Header("Volume Settings")]
    [SerializeField] private float _masterVolume;
    [SerializeField] private float _musicVolume;
    [SerializeField] private float _VFXVolume;

    public float MasterVolume { get => _masterVolume;  }
    public float MusicVolume { get => _musicVolume * _masterVolume;  }
    public float VFXVolume { get => _VFXVolume * _masterVolume;  }

    [Header("Music Tracks")]
    [SerializeField] private List<Music> _musicTracks = new List<Music>();
    [SerializeField] private float _musicIntensity;
    public string MusicFolder;

    [Header("Sound Effects")]
    [SerializeField] private List<SFX> _soundEffects = new List<SFX>();
    public string SoundFolder;

    [Header("Sound Sources")]
    private List<AudioSource> _musicSources = new List<AudioSource>();
    private List<AudioSource> _soundSources = new List<AudioSource>();

    #region General
    public void CreateSources()
    {
        Scene audioScene = SceneManager.CreateScene("Audio Sources");

        foreach(Sound s in _musicTracks)
        {
            if (s == null) continue;

            List<AudioSource> sources = s.CreateSources(audioScene);
            s.SetSources(sources);
            _musicSources.AddRange(sources);
        }
        GameObject sfxParent = new GameObject("Sound Effects");
        foreach(Sound s in _soundEffects)
        {
            if (s == null) continue;

            List<AudioSource> sources = s.CreateSources(audioScene);
            s.SetSources(sources);
            _soundSources.AddRange(sources);
        }
    }

    public void SetSoundObjects<T>(T[] objects) where T : Sound
    {
        switch(objects)
        {
            case Music[] music:
                _musicTracks = music.ToList();
                break;
            case SFX[] sfx:
                _soundEffects = sfx.ToList();
                break;
            default:
                throw new System.Exception($"{typeof(T)} does not have a supported collection in the Audio Manager");
        }
    }
    #endregion
    #region Sound Effects
    public void PlaySoundEffect(string name, Vector3 worldPos = default(Vector3))
    {
        SFX soundEffect = _soundEffects.FirstOrDefault(x => x.name == name);
        if (soundEffect == null)
        {
            Debug.LogError($"Sound Effect '{name}' was not found.");
            return;
        }

        soundEffect.Play(worldPos);
    }
    #endregion
    #region Music
    public void SetMusic(string name)
    {

    }
    public void SetIntensity(int intensity)
    {
        _musicIntensity = Mathf.Clamp(intensity, 0f, 1f);
    }
    #endregion
}
