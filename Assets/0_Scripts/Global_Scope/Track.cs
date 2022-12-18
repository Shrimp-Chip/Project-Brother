using UnityEngine;

[System.Serializable]
public class Track
{
    [SerializeField] public AudioClip Clip;

    [SerializeField] public float Volume = 1f;

    public Track(AudioClip source) 
    { 
        Clip = source;
    }
}
