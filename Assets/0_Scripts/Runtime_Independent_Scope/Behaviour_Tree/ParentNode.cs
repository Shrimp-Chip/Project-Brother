using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ParentNode : Node
{
    [field: SerializeField] public List<Node> Children { get; private set; } = new List<Node>();

    public void AddChildren(params Node[] behaviours)
    {
        for (int i = 0; i < behaviours.Length; i++)
        {
            behaviours[i].Parent = this;
            Children.Add(behaviours[i]);
        }
    }
}
