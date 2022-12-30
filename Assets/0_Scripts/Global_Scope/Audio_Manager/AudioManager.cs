using System;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;
using System.Linq;
using System.Threading.Tasks;


[CreateAssetMenu(fileName = "G_Audio_Manager", menuName = "Managers/Global/Audio Manager")]
public class AudioManager : GlobalManager<AudioManager>
{
    [Header("Audio References")]
    [SerializeField] private List<Pair<AudioType, AudioGroup>> _audioGroupsByType;
    [SerializeField] private List<EventInstance> _eventInstances = new List<EventInstance>();
    [Header("Music")]
    private EventInstance _musicInstance;
    [SerializeField] private float _defaultTransitionTime = 1f;

    #region Generic Audio Playing
    public EventInstance PlayAudio(string audioName, AudioType type, AudioParameters parameters, IAudioPlayer player, Vector3 worldPos = default(Vector3))
    {
        EventInstance instance = CreateInstance(audioName, type, worldPos);
        parameters.SetInstanceParameters(instance);
        player.PlayAudio(instance);
        return instance;
    }
    public EventInstance PlayAudio(string audioName, AudioType type, IAudioPlayer player, Vector3 worldPos = default(Vector3))
    {
        EventInstance instance = CreateInstance(audioName, type, worldPos);
        player.PlayAudio(instance);
        return instance;
    }
    #endregion
    #region Music
    public void SetMusic(string songName)
    {
        SetMusic(songName, new InstanceVolumeAnimator(0f, _defaultTransitionTime), new InstanceVolumeAnimator(1f, _defaultTransitionTime, 0f));
    }
    public async Task SetMusic(string songName, IValueAnimator<EventInstance> fadeInAnimation, IValueAnimator<EventInstance> fadeOutAnimation)
    {
        if (fadeInAnimation != null) await fadeInAnimation.AnimateValue(_musicInstance);
        _musicInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        _musicInstance.release();
        // create an instance without utilities to avoid being added to cleanup
        EventReference musicRef = GetReference(songName, AudioType.Music);
        _musicInstance = RuntimeManager.CreateInstance(musicRef);
        _musicInstance.start();
        if (fadeOutAnimation != null) await fadeOutAnimation.AnimateValue(_musicInstance);
    }
    #endregion
    #region Bus Control
    public void SetBusVolume(string busName, float volume)
    {
        Bus bus = GetBus(busName);
        bus.setVolume(volume);
    }
    #endregion
    #region Utilities
    private EventInstance CreateInstance(string audioName, AudioType type, Vector3 worldPos = default(Vector3))
    {
        EventReference reference = GetReference(audioName, type);
        EventInstance instance = CreateInstance(reference, worldPos);
        return instance;
    }
    private EventInstance CreateInstance(string audioName, AudioType type, GameObject gameObject)
    {
        EventReference reference = GetReference(audioName, type);
        EventInstance instance = CreateInstance(reference, gameObject);
        return instance;
    }
    private EventInstance CreateInstance(EventReference reference, Vector3 worldPos)
    {
        EventInstance instance = RuntimeManager.CreateInstance(reference);
        instance.set3DAttributes(RuntimeUtils.To3DAttributes(worldPos));
        _eventInstances.Add(instance);
        return instance;
    }
    private EventInstance CreateInstance(EventReference reference, GameObject gameObject)
    {
        EventInstance instance = RuntimeManager.CreateInstance(reference);
        instance.set3DAttributes(RuntimeUtils.To3DAttributes(gameObject));
        _eventInstances.Add(instance);
        return instance;
    }
    private EventReference GetReference(string audioName, AudioType type)
    {
        // Find the correct audiogroup to search
        Pair<AudioType, AudioGroup> groupPair = _audioGroupsByType.FirstOrDefault(x => x.Key == type);
        if (groupPair == null) throw new Exception($"Audio Group of type: '{type}' not found.");

        return groupPair.Value.FindReference(audioName);
    }

    private Bus GetBus(string busName) => RuntimeManager.GetBus($"bus:/{busName}");
    public void Cleanup()
    {
        foreach(EventInstance instance in _eventInstances)
        {
            instance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            instance.release();
        }
    }
    #endregion
}
