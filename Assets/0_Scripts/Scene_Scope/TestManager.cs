using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestManager : SceneScopeManager
{
    public float TestValue;
    private void OnEnable()
    {
        Debug.Log("On Enable");
    }

    void Awake()
    {
        Debug.Log("On Awake");
    }

    void Start()
    {
        Debug.Log("On Start");
    }
}
