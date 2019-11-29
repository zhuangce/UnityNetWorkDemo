using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>
{
    
    private static T instance;
    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<T>();
                //if (instance == null)
                //{
                //    instance = new GameObject("go").AddComponent<T>();
                //}
                // instance.gameObject.name = instance.name;
            }
            return instance;
        }
       // set
       // {
       //     value = instance;
       // }
    }

    protected virtual void Awake()
    {
       
        if (instance == null)
        {
            instance = this as T;
        }
        else
        {
            if (instance == this) return;
           // DebugTool.Log("Destroy" + transform.name);
            Destroy(gameObject);
        }
        
         

    }



    public void Test()
    {
        Debug.Log("SIngleton");
    }

    protected virtual void OnDestroy()
    {
        // Debug.Log("destroy");
        if (instance == this) {
            instance = null;
         //   DebugTool.Log(transform.name+"Instance Destroy");
        }
           
    }
}
