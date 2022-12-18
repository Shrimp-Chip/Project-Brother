using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;
using System.Linq;

[CreateAssetMenu(fileName = "G_Game_Mode_Manager", menuName = "Managers/Global/Game Mode Manager")]
public class GameModeManager : GlobalManager<GameModeManager>
{
    [Tooltip("Scenes that aren't gameplay must be defined here")]
    [SerializeField] private List<Pair<SceneAsset, GameMode>> _nonGameplayScenes = new List<Pair<SceneAsset, GameMode>>();
    [Tooltip("Define managers needed for each Game Mode")]
    [SerializeField] public List<Pair<GameMode, List<SceneScopeManager>>> _gameModeManagers = new List<Pair<GameMode, List<SceneScopeManager>>>();

    public GameMode GetSceneGameMode(Scene scene) => GetSceneGameMode(scene.name);
    public GameMode GetSceneGameMode(string scene)
    {
        Pair<SceneAsset, GameMode> pair = _nonGameplayScenes.FirstOrDefault(x => scene == (x.Key as SceneAsset).name);
        if (pair == null) return GameMode.Gameplay;
        return pair.Value;
    }

    public void DressScene(string sceneName) => DressScene(sceneName, GetSceneGameMode(sceneName));

    public void DressScene(string sceneName, GameMode gameMode)
    {
        Pair<GameMode, List<SceneScopeManager>> modeManagersPair = _gameModeManagers.FirstOrDefault(x => x.Key == gameMode);
        if (modeManagersPair == null) throw new Exception($"Mode '{gameMode}' managers not defined.");
        string managerSceneName = GetManagerSceneName(sceneName);

        Debug.Log($"Creating {managerSceneName}");
        List<SceneScopeManager> managers = modeManagersPair.Value;
        List<GameObject> managerObjects = new List<GameObject>();
        foreach (SceneScopeManager sm in managers)
        {
            if (sm == null) continue;
            managerObjects.Add(Instantiate(sm.gameObject, Vector3.zero, Quaternion.identity));
        }

        Scene managerScene = SceneManager.CreateScene(managerSceneName);
        foreach (GameObject m in managerObjects)
        {
            SceneManager.MoveGameObjectToScene(m, managerScene);
        }
    }
    public Scene GetManagerScene(string sceneName)
    {
        return SceneManager.GetSceneByName(GetManagerSceneName(sceneName));
    }
    private string GetManagerSceneName(string sceneName)
    {
        return $"{sceneName} : {GetSceneGameMode(sceneName)}";
    }
    public List<SceneScopeManager> GetManagers(GameMode mode)
    {
        return _gameModeManagers.FirstOrDefault(x => x.Key == mode).Value;
    }
}



