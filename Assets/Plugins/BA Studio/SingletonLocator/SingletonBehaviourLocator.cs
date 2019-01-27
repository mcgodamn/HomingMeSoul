using UnityEngine;

namespace BA_Studio.UnityLib.SingletonLocator
{
    ///You need to initialize this in Awake to take effect.
    public static class SingletonBehaviourLocator<T> where T : MonoBehaviour
    {
        static T instance;
        private static object m_Lock = new object();

        public static T Instance
        {
            get { lock (m_Lock) return instance; }        
        }

        static SingletonBehaviourLocator()
        {
            instance = null;
        }
        
        public static void Set (T newT)
        {
            lock (m_Lock)
            {
                if (newT == null)
                {
                    if (instance != null) MonoBehaviour.Destroy(instance);
                    instance = null;
                    return;
                }
                
                if (!Application.isEditor)
                {
                    Debug.Log($"Trying to set Singleton of type {newT.GetType().Name}...instance: " + newT);
                    if (SingletonBehaviourLocator<T>.instance == null) SingletonBehaviourLocator<T>.instance = newT;
                    if (SingletonBehaviourLocator<T>.instance != newT) MonoBehaviour.DestroyImmediate(newT);
                    
                }
                else
                {
                    Debug.Log($"EditorMode: Force setting Singleton of type {newT.GetType().Name}...instance: " + newT);
                    SingletonBehaviourLocator<T>.instance = newT;                
                }
                MonoBehaviour.DontDestroyOnLoad(newT);  
            }
        }
    }
}