using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public abstract class Node : ScriptableObject
{
    [field : SerializeField] public int Priority { get; private set; } = 0;
    [field : SerializeField] public Node Parent = null;
    [field : SerializeField] public List<Node> Children { get; private set; } = new List<Node>();
    [field : SerializeField] public BehaviourState State { get; private set; } = BehaviourState.SUCCESS;

    #region Abstract Methods
    public abstract bool Evaluate();
    protected abstract void OnEnter();
    protected abstract void Update();
    protected abstract bool IsDone();
    #endregion

    #region Generic Methods
    public void AddChildren(params Node[] behaviours)
    {
        for(int i = 0; i < behaviours.Length; i++)
        {
            behaviours[i].Parent = this;
            Children.Add(behaviours[i]);
        }
    }
    #endregion

    public enum BehaviourState
    {
        FAILURE,
        RUNNING,
        SUCCESS
    }
}
