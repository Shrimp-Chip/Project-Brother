using UnityEngine;
using System;

public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    public static T Instance { get; private set; }
    public virtual void Awake()
    {
        if (Instance != null)
        {
            Destroy(this);
            return;
        }
        Instance = this as T;
    }
}


public abstract class GlobalManager<T> : ScriptableObject where T : ScriptableObject
{
    public static T Instance { get
        {
            if (instance == null)
            {
                T[] objects = Resources.LoadAll<T>("Managers");
                if (objects.Length == 0)
                {
                    throw new Exception($"Manager '{typeof(T)}' was not created in 'Resources/Managers'");
                }
                else if (objects.Length > 1)
                {
                    throw new Exception($"Manager '{typeof(T)}' defined more than once in 'Resources/Managers'");
                }
                else
                {
                    //Debug.Log($"Manager '{typeof(T)}' sucessfully loaded");
                    instance = objects[0];
                    return instance;
                }
            }
            else
            {
                return instance;
            }
        }
        private set
        {
            return;
        }
    }

    protected static T instance = null;
}
