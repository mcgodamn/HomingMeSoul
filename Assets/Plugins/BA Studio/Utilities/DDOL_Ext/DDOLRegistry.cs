using System.Collections.Generic;
using UnityEngine;

public static class DDOLRegistry
{
    private static List<Object> DDOLs = new List<Object>();
    
    public static void DontDestroyOnLoad (this Object obj)
    {
        DDOLs.Add(obj);
        Object.DontDestroyOnLoad(obj);
    }
    
    public static void Destory (this Object obj)
    {
        DDOLs.Remove(obj);
        Destory(obj);
    }
    
    public static void DestoryAll (this Object obj)
    {
        foreach (Object o in DDOLs) Destory(o);
    }
    
    public static ICollection<Object> GetDDOLs ()
    { 
        return DDOLs.ToArray(); 
    }
}