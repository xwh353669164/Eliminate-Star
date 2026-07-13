using System.Collections.Generic;
using UnityEngine;

public class ResManager : MonoBehaviour
{
    private static ResManager instance;
    public static ResManager Instance
    {
        get
        {
            if (instance == null)
            {
                GameObject resManager = new GameObject("ResManager");
                instance = resManager.AddComponent<ResManager>();
                DontDestroyOnLoad(resManager);
            }
            return instance;
        }
    }

    public T Load<T>(string path, Transform parent = null, bool world = false) where T : Object
    {
        T prefab = Resources.Load<T>(path);

        if (prefab is GameObject)
            return Instantiate(prefab, parent, world);

        return prefab;
    }

    public T Load<T>(string path, Vector3 pos, Quaternion rot, Transform parent = null) where T : Object
    {
        T prefab = Resources.Load<T>(path);

        if (prefab is GameObject)
            return Instantiate(prefab, pos, rot, parent);

        return prefab;
    }
}
