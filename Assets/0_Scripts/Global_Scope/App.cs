using UnityEngine;
using UnityEngine.SceneManagement;

public class App
{

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    public static void Bootstrap() 
    {
        GameModeManager.Instance.DressScene(SceneManager.GetActiveScene().name);

        // Kinda icky but the best way to do this
        SceneLoader.Instance.OnSceneLoad += AudioManager.Instance.Cleanup;
    }
}
