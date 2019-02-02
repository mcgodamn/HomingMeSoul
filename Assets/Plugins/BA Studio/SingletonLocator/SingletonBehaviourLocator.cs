using UnityEngine;

namespace BA_Studio.UnityLib.SingletonLocator
{
    ///You need to initialize this in Awake to take effect.
    public static class SingletonBehaviourLocator<T> where T : MonoBehaviour
    {
        static T instance = null;
        private static object m_Lock = new object();

        public static T Instance
        {
            get { lock (m_Lock) return instance; }        
        }

        static SingletonBehaviourLocator()
        {
            instance = null;
        }
        
        public static void Set (T newT, bool overwrite = false, System.Action<T> ifOverwriteDoThisOnOriginal = null, System.Action<T> ifNotOverwriteDoThisOnNewT = null)
        {
            lock (m_Lock)
            {
                if (newT == null)
                {
                    if (instance != null) MonoBehaviour.Destroy(instance);
                    instance = null;
                    return;
                }
                
                Debug.Log($"Trying to set Singleton of type {newT.GetType().Name}...instance: " + newT);
                if (SingletonBehaviourLocator<T>.instance == null) SingletonBehaviourLocator<T>.instance = newT;
                if (SingletonBehaviourLocator<T>.instance != newT)
                {
                    if (!overwrite)
                    {
                        ifNotOverwriteDoThisOnNewT?.Invoke(newT);
                        MonoBehaviour.DestroyImmediate(newT);
                    }
                    else 
                    {
                        ifOverwriteDoThisOnOriginal?.Invoke(Instance);
                        MonoBehaviour.DestroyImmediate(Instance);
                        SingletonBehaviourLocator<T>.instance = newT;
                    }
                }
                
                try
                {
                    #if DDOL_EXT
                    DDOLRegistry.DontDestroyOnLoad(newT.gameObject);
                    #else
                    MonoBehaviour.DontDestroyOnLoad(newT);
                    #endif
                }
                catch (System.Exception e)
                {
                    Debug.Log("SBL:: Setting singleton of " + newT.GetType().Name + ", Exception: " + e);
                }
            }
        }
    }
}