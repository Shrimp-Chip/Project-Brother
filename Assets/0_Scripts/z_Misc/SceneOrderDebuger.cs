using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneOrderDebuger : MonoBehaviour
{
    private void OnEnable()
    {
        Debug.Log("Scene Object On Enable");
    }
    private void Awake()
    {
        Debug.Log("Scene Object Awake");
    }
}
