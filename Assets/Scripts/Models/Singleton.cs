using System;
using UnityEngine;

public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static readonly Lazy<T> LazyInstance = new Lazy<T>(CreateSingleton);
    protected static bool isQuitting = false;

    public static T Instance => isQuitting ? null : LazyInstance.Value;

    private static T CreateSingleton()
    {
        var existingObject = FindObjectOfType(typeof(T)) as T;
        if (existingObject != null)
        {
            DontDestroyOnLoad(existingObject);
            return existingObject;
        }
        var ownerObject = new GameObject($"{typeof(T).Name} (lazy single)");
        var instance = ownerObject.AddComponent<T>();
        DontDestroyOnLoad(ownerObject);
        return instance;
    }

    protected virtual void OnApplicationQuit()
    {
        isQuitting = true;
    }
}
