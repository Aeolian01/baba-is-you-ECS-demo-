
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : Singleton<T>,new()
{
    private static T _instance;
    public static T Instance { get => _instance; set => _instance = value; }

    static Singleton() {
        if (Instance == null)
        {
            Instance = new T();
        }
        else {
            Debug.LogError("singleton instance already exists! TypeName: "+typeof(T));
        }
    }
}

