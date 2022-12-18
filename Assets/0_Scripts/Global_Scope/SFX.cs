using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SFX" ,menuName = "Sounds/SFX")]
public class SFX : Sound
{
    [Header("Sound Effect Settings")]
    [SerializeField] private CenteralizedRange _volume;
    [SerializeField] private CenteralizedRange _pitch;
    [SerializeField] private bool loop;

    public override void Play(Vector3 position)
    {
        if (Clips == null || Clips.Count == 0)
        {
            Debug.LogError($"No audio clips assigned to sound : {name}");
            return;
        }

        // Choose a random source and assign random properties
        AudioSource source = GetRandomSource();
        source.gameObject.transform.position = position;
        source.volume = _volume.Evaluate() * AudioManager.Instance.VFXVolume;
        source.pitch = _pitch.Evaluate();
        source.loop = loop;

        source.Play();
    }

    private AudioSource GetRandomSource()
    {
        List<AudioSource> availableSources = Sources.FindAll(x => x.isPlaying == false);
        if (availableSources.Count > 0)
        {
            // Available source
            return availableSources[Random.Range(0, availableSources.Count)];
        }

        // There is no available source -- we must create one
        // Find a clip for the source
        AudioClip clip = Clips[Random.Range(0, Clips.Count)];
        GameObject tempSourceObject = new GameObject($"TEMP - {name} : {clip.name}");
        AudioSource tempSource = tempSourceObject.AddComponent<AudioSource>();
        tempSource.clip = clip;
        // Because this is a temp source, we want to delete the source after some time
        Destroy(tempSourceObject, clip.length + Time.deltaTime);
        return tempSource;
    }
}
