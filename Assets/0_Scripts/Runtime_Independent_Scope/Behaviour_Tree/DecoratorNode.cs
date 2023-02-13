using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DecoratorNode : Node
{
    [field: SerializeField] public Node Child { get; private set; }

    public override Node GetActiveNode()
    {
        return Child.GetActiveNode();
    }

    public void SetChild(Node newChild)
    {
        Child.Parent = null;
        Child = newChild;
        newChild.Parent = this;
    }
}
