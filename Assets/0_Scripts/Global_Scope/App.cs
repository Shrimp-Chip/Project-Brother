using UnityEngine;
using UnityEngine.SceneManagement;

public class App
{

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    public static void Bootstrap() 
    {
        GameModeManager.Instance.DressScene(SceneManager.GetActiveScene().name);
        AudioManager.Instance.CreateSources();
    }
}
