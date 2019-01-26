

namespace BA_Studio.SingletonLocator
{
    // public static class StaticSingletonLazy<T> where T : class, new()
    // {
    //     private static readonly System.Lazy<T> instance = new System.Lazy<T>(() => new T());
            
    //     public static T Instance
    //     {
    //         get
    //         {
    //             return instance.Value;
    //         }
    //     }
    // }
    
    public static class SingletonLocator<T> where T : class
    {
        private static T instance;
        private static object m_Lock = new object();

        static readonly bool throwException;

        public static event System.Action InstanceSetToNull, InstanceSet;


        public static T Instance
        {
            get
            {
                lock (m_Lock) return instance;
            }

            set
            {
                lock (m_Lock)
                {
                    if (value == null)
                    {
                        instance = null;
                        InstanceSetToNull?.Invoke();
                    }
                    else
                    {
                        
                        if (instance == null)
                        {
                            instance = value;
                            InstanceSet?.Invoke();
                        }
                        else
                        {
                            if (throwException) throw new System.Exception(typeof(T).Name + "already registered a singleton instance.");
                        }
                    }
                }

            }
        }
    }
}