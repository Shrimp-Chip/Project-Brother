using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AI/Behaviour Tree/Tree")]
public class BehaviourTree : ScriptableObject
{
    [field: SerializeField] public Node RootNode { get; private set; }
    [field: SerializeField] public Node ActiveNode { get; private set; }
    [field: SerializeField] public Node.BehaviourState TreeState { get; private set;}

    public void Update()
    {
        TreeState = RootNode.Update();
        ActiveNode = RootNode.GetActiveNode();
    }
}
