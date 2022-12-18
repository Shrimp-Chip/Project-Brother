using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;

[CreateAssetMenu(fileName = "G_Scene_Loader",menuName = "Managers/Global/Scene Loader")]
[System.Serializable]
public class SceneLoader : GlobalManager<SceneLoader>
{
    public string ScenePath;

    public List<Pair<SceneTransition, SceneTransitionPlayer>> SceneTransitions = new List<Pair<SceneTransition, SceneTransitionPlayer>>();
    public async Task LoadScene(SceneAsset sa, SceneTransition transition = SceneTransition.Standard) => await LoadSceneAsync(sa.name, transition);
    public async Task LoadScene(Scene scene, SceneTransition transition = SceneTransition.Standard) => await LoadSceneAsync(scene.name, transition);
    private async Task LoadSceneAsync(string sceneName, SceneTransition transition = SceneTransition.Standard)
    {
        Debug.Log($"Loading scene {sceneName}");
        string oldActiveScene = SceneManager.GetActiveScene().name;
        string oldManagerScene = GameModeManager.instance.GetManagerScene(oldActiveScene).name;

        AsyncOperation loadingOperation = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        loadingOperation.allowSceneActivation = false;

        // ENTER LOAD
        List<Task> loadingTasks = new List<Task>();
        // Add actual loading task
        loadingTasks.Add(TaskUtility.WaitUntil(() =>
        {
            //Debug.Log($"Loading... {loadingOperation.progress * 100}%");
            return loadingOperation.progress >= 0.9f; // Progress is haulted at 0.9 because we set allowSceneActivation to false
        }));
        // Add transition task
        SceneTransitionPlayer transitionPlayer = GetTransitionPlayer(transition);
        if (transitionPlayer != null) loadingTasks.Add(transitionPlayer.EnterTransition());
        // Wait until both the loading and the transition tasks are complete
        await Task.WhenAll(loadingTasks);

        // PREPARE SCENE
        // Create manager scene
        GameModeManager.Instance.DressScene(sceneName);
        // Unload old scenes
        SceneManager.UnloadSceneAsync(oldActiveScene);
        SceneManager.UnloadSceneAsync(oldManagerScene);
        // Wait until next frame to allow for dependencies to be built
        await Task.Yield();

        // OPEN NEW SCENE
        // Allow transition
        loadingOperation.allowSceneActivation = true;
        // Wait for transition
        await TaskUtility.WaitUntil(() =>
        {
            //Debug.Log($"Loading... {loadingOperation.progress * 100}%");
            return loadingOperation.isDone;
        });
        await Task.Yield();

        // REVEAL SCENE
        if (transitionPlayer != null) await transitionPlayer.ExitTransition();
    }

    private SceneTransitionPlayer GetTransitionPlayer(SceneTransition transition)
    {
        SceneTransitionPlayer player = SceneTransitions.FirstOrDefault(x => x.Key == transition).Value;
        SceneTransitionPlayer playerObject = Instantiate(player, Vector3.zero, Quaternion.identity);
        DontDestroyOnLoad(playerObject);
        return playerObject;
    }
}
