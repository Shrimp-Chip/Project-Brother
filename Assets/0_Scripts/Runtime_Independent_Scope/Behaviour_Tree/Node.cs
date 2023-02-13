using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public abstract class Node : ScriptableObject
{
    [field : SerializeField] public int Priority { get; private set; } = 0;
    [field : SerializeField] public Node Parent = null;
    [field : SerializeField] public BehaviourState State { get; private set; } = BehaviourState.SUCCESS;

    [SerializeField] private bool _started = false;

    #region Abstract Methods
    protected abstract void OnEnter();
    protected abstract BehaviourState OnUpdate();
    protected abstract void OnStop();
    public abstract Node GetActiveNode();
    #endregion

    #region Generic Methods
    public BehaviourState Update()
    {
        if (_started == false)
        {
            OnEnter();
            _started = true;
        }

        State = OnUpdate();

        if (State == BehaviourState.FAILURE || State == BehaviourState.SUCCESS)
        {
            OnStop();
            _started = false;
        }

        return State;
    }
    #endregion

    public enum BehaviourState
    {
        FAILURE,
        RUNNING,
        SUCCESS
    }
}
