using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputTester : MonoBehaviour
{
    [InputPath] public string Input;

    void Update()
    {
        Debug.Log(InputManager.Instance.GetContextByPath(Input));
    }
}
