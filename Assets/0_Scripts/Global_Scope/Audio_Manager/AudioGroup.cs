using UnityEngine;
using System.Collections.Generic;
using FMODUnity;
using System.Linq;
using System;
using FMOD.Studio;

[System.Serializable]
[CreateAssetMenu(fileName = "Audio_Group", menuName = "Audio/Audio Group")]
public class AudioGroup : ScriptableObject
{
    [field: SerializeField] private List<Pair<string, EventReference>> _references = new List<Pair<string, EventReference>>();
    [field : BankRef] [field: SerializeField] public string SourceBank { get; private set; }
    
    public EventReference FindReference(string name)
    {
        Pair<string, EventReference> namedReference = _references.FirstOrDefault(x => x.Key == name);
        if (namedReference == null) throw new Exception($"Reference by name : '{name}' not found in Audio Group : '{this.name}'");
        return namedReference.Value;
    }
    public void SetReferences(List<Pair<string, EventReference>> references)
    {
        _references = references;
    }
}
