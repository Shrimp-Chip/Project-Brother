using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ActionNode : Node
{
    public override Node GetActiveNode()
    {
        return this;
    }
}
