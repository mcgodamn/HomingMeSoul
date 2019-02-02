using System.Collections.Generic;
using UnityEngine;

public static class DDOLRegistry
{
    private static List<GameObject> DDOLs = new List<GameObject>();
    
    public static void DontDestroyOnLoad (this GameObject obj)
    {
        DDOLs.Add(obj);
        Object.DontDestroyOnLoad(obj);
    }
    
    public static void Destory (this GameObject obj)
    {
        DDOLs.Remove(obj);
        Destory(obj);
    }
    
    public static void DestoryAll (this GameObject obj)
    {
        foreach (GameObject o in DDOLs) Destory(o);
    }
    
    public static ICollection<GameObject> GetDDOLs ()
    { 
        return DDOLs.ToArray(); 
    }
}