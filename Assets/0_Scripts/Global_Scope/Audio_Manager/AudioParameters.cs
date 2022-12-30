using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMOD.Studio;

[System.Serializable]
public struct AudioParameters
{
    private List<Pair<string, float>> _parameters;
    public AudioParameters(params(string name, float value)[] parameters)
    {
        _parameters = new List<Pair<string, float>>();
        foreach(var (name, value) in parameters)
        {
            _parameters.Add(new Pair<string, float>(name, value));
        }
    }

    public void SetInstanceParameters(EventInstance instance)
    {
        if (_parameters == null) return;

        foreach(Pair<string, float> pair in _parameters)
        {
            if (pair == null) continue;
            instance.setParameterByName(pair.Key, pair.Value);
        }
    }
}
