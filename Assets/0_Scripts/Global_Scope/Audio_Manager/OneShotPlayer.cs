using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;
using System.Threading.Tasks;

public class OneShotPlayer : IAudioPlayer
{
    public void PlayAudio(EventInstance instance)
    {
        instance.start();
        instance.release();
    }
}
