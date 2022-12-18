using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Music", menuName = "Sounds/Music")]
public class Music : Sound
{
    public List<Pair<float, TrackMixer>> MixersByIntensity;

    public override void Play(Vector3 position)
    {
        
    }
}
