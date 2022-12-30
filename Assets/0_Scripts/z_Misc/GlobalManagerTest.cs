using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;

public class GlobalManagerTest : MonoBehaviour
{
    public string StartingSong = "Level_01";

    public void Start()
    {
        AudioManager.Instance.SetMusic(StartingSong);
    }
    public void Transition(SceneAsset scene)
    {
        SceneLoader.Instance.LoadScene(scene);
    }

    public void PlaySoundEffect(string sound)
    {
        AudioManager.Instance.PlayAudio(sound, AudioType.SFX, new OneShotPlayer(), Vector3.one);
    }

    public void SetMusic(string song)
    {
        AudioManager.Instance.SetMusic(song);
    }
}
