using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    public void Transition(SceneAsset scene)
    {
        SceneLoader.Instance.LoadScene(scene);
    }
}
