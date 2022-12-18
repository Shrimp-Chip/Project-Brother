using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestApp : MonoBehaviour
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void Fuck()
    {
        Debug.Log("Fuck");
    }
}
