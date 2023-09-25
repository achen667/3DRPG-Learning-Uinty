using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T: Singleton<T>
{
    private static T instance;
    public static  T Instance
    {
        get { return instance; }
    }

    protected virtual void Awake()
    {
        //只保留一个实例
        if (instance != null)
            Destroy(gameObject);
        else
            instance = (T)this;

        //DontDestroyOnLoad(this);
    }
    
    //判断是否被实例化
    public static bool IsInitialized
    {
        get { return instance != null; }
    }

    protected virtual void OnDestroy()
    {
        if (instance == this)
            instance = null;
    }

}
