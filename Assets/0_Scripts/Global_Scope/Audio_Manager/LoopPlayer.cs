using FMOD.Studio;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoopPlayer : IAudioPlayer
{
    public void PlayAudio(EventInstance instance)
    {
        instance.start();
    }
}
