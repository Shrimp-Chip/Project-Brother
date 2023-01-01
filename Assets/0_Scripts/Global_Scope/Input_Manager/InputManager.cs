using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Linq;

[CreateAssetMenu(fileName= "G_Input_Manager", menuName = "Managers/Global/Input Manager")]
public class InputManager : GlobalManager<InputManager>
{
    [SerializeField] private InputActionAsset _inputActions;

    private Dictionary<string, List<ActionStatus>> _statusesByMap = new Dictionary<string, List<ActionStatus>>();

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    public void Initialize()
    {
        _inputActions.Enable();

        _statusesByMap = new Dictionary<string, List<ActionStatus>>();
        CreateStatusDictionary(_inputActions);
        Debug.Log(_statusesByMap.Count);
    }

    private void CreateStatusDictionary(InputActionAsset actionAsset)
    {
        foreach(InputActionMap map in actionAsset.actionMaps)
        {
            foreach (InputAction action in map.actions)
            {
                ActionStatus status = new ActionStatus(action);
                _statusesByMap.TryGetValue(map.name, out List<ActionStatus> statuses);

                if (statuses == null)
                {
                    statuses = new List<ActionStatus>();
                    statuses.Add(status);
                    _statusesByMap.Add(map.name, statuses);
                    continue;
                }

                statuses.Add(status);
            }
        }
    }

    public InputAction.CallbackContext GetContext(string actionName, string mapName = "Player")
    {
        _statusesByMap.TryGetValue(mapName, out List<ActionStatus> statuses);
        if (statuses == null) throw new System.Exception($"Action Map: '{mapName}' does not exist in the Input Action Asset.");

        ActionStatus currentStatus = statuses.FirstOrDefault(status => status.Action.name == actionName);
        if (currentStatus == null) throw new System.Exception($"Action : '{actionName}' in Action Map : '{mapName}' does not exist in the Input Action Asset.");
        return currentStatus.Context;
    }
    /// <summary>
    /// Paths are formated by [mapName]/[actionName]
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public InputAction.CallbackContext GetContextByPath(string path)
    {
        int pos = path.LastIndexOf("/");
        string mapName = path.Substring(0, pos);
        string actionName = path.Substring(pos + 1, path.Length - pos - 1);
        return GetContext(actionName, mapName);
    }
}



public class ActionStatus
{
    public InputAction Action { get; private set; }
    public InputAction.CallbackContext Context { get; private set; }

    public ActionStatus(InputAction action)
    {
        Action = action;

        action.started += UpdateContext;
        action.performed += UpdateContext;
        action.canceled += UpdateContext;
    }

    private void UpdateContext(InputAction.CallbackContext newContext)
    {
        Context = newContext;
    }
}
