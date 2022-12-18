using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TrackMixer
{
    [SerializeField] public List<Track> Tracks = new List<Track>();
    public TrackMixer(List<AudioClip> sources)
    {
        foreach(AudioClip s in sources)
        {
            Tracks.Add(new Track(s));
        }
    }

    public void UpdateTracks(List<AudioClip> newClips)
    {
        Debug.Log("Updating Track Clips");
        List<Track> newTracks = new List<Track>();
        int maxIndex = Tracks.Count - 1;
        for(int i = 0; i < newClips.Count; i++)
        {
            if (i > maxIndex)
            {
                newTracks.Add(new Track(newClips[i]));
                continue;
            }

            Debug.Log($"NEW : {Tracks[i].Clip} : {Tracks[i].Volume}");
            Tracks[i].Clip = newClips[i];
            newTracks.Add(Tracks[i]);
        }

        Tracks = newTracks;
    }

    public void DebugVolumes()
    {
        for (int i = 0; i < Tracks.Count; i++)
        {
            if (Tracks[i] == null) continue;
            Debug.Log($"{Tracks[i].Clip} : {Tracks[i].Volume}");
        }
    }
}
