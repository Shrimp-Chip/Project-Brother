using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using FMODUnity;
using FMOD.Studio;
using System.Linq;

[CustomEditor(typeof(AudioGroup))]
public class AudioGroupEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        AudioGroup ag = target as AudioGroup;
        
        if (ag.SourceBank != "" && GUILayout.Button("Create References"))
        {
            CreateReferencePairs(ag);
        }
    }

    private void CreateReferencePairs(AudioGroup ag)
    {
        EditorUtils.LoadPreviewBanks();
        Bank bank = LoadBank(ag.SourceBank);
        EventReference[] references = GetReferences(bank);
        Pair<string, EventReference>[] pairedReferences = GenerateReferencePairs(references);
        ag.SetReferences(pairedReferences.ToList());
        EditorUtils.UnloadPreviewBanks();
    }

    private Bank LoadBank(string path)
    {
        FMOD.RESULT res = EditorUtils.System.getBank($"bank:/{path}", out Bank bank);

        switch (res)
        {
            case FMOD.RESULT.OK :
                return bank;
            default :
                throw new System.Exception($"Encountered Error while loading bank from path : '{path}' \n FMOD Result : {res}");
        }
    }

    private EventReference[] GetReferences(Bank bank)
    {
        bank.getEventList(out EventDescription[] descriptions);
        EventReference[] references = new EventReference[descriptions.Length];

        for (int i = 0; i < references.Length; i++)
        {
            descriptions[i].getPath(out string path);
            descriptions[i].getID(out FMOD.GUID id);

            EventReference newReference = new EventReference() { Path = path, Guid = id };

            references[i] = newReference;
        }

        return references;
    }

    private Pair<string, EventReference>[] GenerateReferencePairs(EventReference[] references)
    {
        Pair<string, EventReference>[] pairs = new Pair<string, EventReference>[references.Length];
        for(int i = 0; i < pairs.Length; i++)
        {
            Pair<string, EventReference> pair = new Pair<string, EventReference>(GetReferenceName(references[i]), references[i]);
            pairs[i] = pair;
        }

        return pairs;
    }

    private string GetReferenceName(EventReference reference)
    {
        string path = reference.Path;
        int pos = path.LastIndexOf("/") + 1;
        return path.Substring(pos, path.Length - pos).Replace(" ", "_");
    }
}
