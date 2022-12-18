using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public abstract class Sound : ScriptableObject
{
    [SerializeField] public List<AudioClip> Clips = new List<AudioClip>();
    public List<AudioSource> Sources { get; private set; }
    public List<AudioSource> CreateSources(Scene destinationScene)
    {
        GameObject soundParent = new GameObject($"{this.name} Sources");
        List<AudioSource> sources = new List<AudioSource>();

        for (int i = 0; i < Clips.Count; i ++)
        {
            if (Clips[i] == null) continue;

            GameObject clipObject = new GameObject($"{this.name} : {Clips[i].name}");
            clipObject.transform.parent = soundParent.transform;
            AudioSource clipSource = clipObject.AddComponent<AudioSource>();
            clipSource.clip = Clips[i];
            InitializeSource(clipSource);
            sources.Add(clipSource);
        }
        SceneManager.MoveGameObjectToScene(soundParent, destinationScene);
        return sources;
    }
    protected virtual void InitializeSource(AudioSource source)
    {
        // to implement
    }
    public void SetSources(List<AudioSource> sources) => this.Sources = sources;

    public abstract void Play(Vector3 position);
}
