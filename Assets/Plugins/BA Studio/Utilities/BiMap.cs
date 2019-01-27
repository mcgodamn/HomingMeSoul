using System;
using System.Runtime.Serialization;
using System.Collections;
using System.Collections.Generic;

namespace BA_Studio.DataStructure
{
    // For unique pair, a key is always only paired with a other key.
    public class BiMap<T1, T2> : ICollection<KeyValuePair<T1, T2>>,
                                 IEnumerable<KeyValuePair<T1, T2>>
    {
        private Dictionary<T1, T2> _forward;
        private Dictionary<T2, T1> _reverse;
        private Dictionary<T1, T2> Forward
        {
            get
            {
                if (_forward == null) _forward = new Dictionary<T1, T2>();
                return _forward;
            }
        }
        private Dictionary<T2, T1> Reverse
        {        
            get
            {
                if (_reverse == null) _reverse = new Dictionary<T2, T1>();
                return _reverse;
            }
        }

        public T2 this[T1 k1]
        {
            get
            {
                return Get(k1);
            }
            set
            {
                Forward[k1] = value;
                Reverse[value] = k1;
            }
        }

        public T1 this[T2 k2]
        {
            get
            {
                return Get(k2);
            }
            set
            {
                Forward[value] = k2;
                Reverse[k2] = value;
            }
        }

        public int Count
        {
            get
            {
                CountCheck();
                return _forward.Count;
            }
        }

        public bool IsReadOnly => throw new System.NotImplementedException();

        public ICollection<T1> Key1s => ((IDictionary<T1, T2>)Forward).Keys;

        public ICollection<T2> Key2s => ((IDictionary<T1, T2>)Forward).Values;

        public BiMap (int defaultCount = 5)
        {
            if (typeof(T1) == typeof(T2))
            {       
                throw new ArgumentException("Same key type", "You should use different type to work with BiMap.");
            }

            _forward = new Dictionary<T1, T2>(defaultCount);
            _reverse = new Dictionary<T2, T1>(defaultCount);
        }

        void InternalAdd(T1 t1, T2 t2)
        {
            try
            {
                if (t1 == null || t2 == null)
                    throw new ArgumentException("Null key value", "Null value is not allowed.");

                if (Forward.ContainsKey(t1) || Reverse.ContainsKey(t2))
                {
                    throw new System.ArgumentException("A pair with same item already exist!");
                }
                else
                {
                    Forward.Add(t1, t2);
                    Reverse.Add(t2, t1);
                }
            }
            catch (Exception e)
            {
                // throw new System.Exception("BiMap: Unknow exception in Add().");
                throw e;
            }
        }

        public T1 Get(T2 key) 
        {
            return Reverse[key];
        }

        public T2 Get (T1 key)
        {
            return Forward[key];
        }

        public IEnumerable<T1> GetKey1s () 
        {
            return Forward.Keys;
        }

        public IEnumerable<T2> GetKey2s () 
        {
            return Reverse.Keys;
        }

        public bool Contains (T2 key)
        {
            if (key == null)            
                throw new ArgumentException("Null key query", "Null is not allowed.");
            return Reverse.ContainsKey(key);
        }

        public bool Contains (T1 key)
        {
            if (key == null)            
                throw new ArgumentException("Null key query", "Null is not allowed.");
            return Forward.ContainsKey(key);
        }

        public void Clear ()
        {
            Reverse.Clear();
            Forward.Clear();
        }

        public void Add(KeyValuePair<T1, T2> item)
        {
            InternalAdd(item.Key, item.Value);
        }

        public bool Contains(KeyValuePair<T1, T2> item)
        {
            return (((ICollection<KeyValuePair<T1, T2>>) Forward).Contains(item) && Reverse.ContainsKey(item.Value) && Reverse[item.Value].Equals(item.Key));
        }

        public void CopyTo(KeyValuePair<T1, T2>[] array, int arrayIndex)
        {
            ((ICollection<KeyValuePair<T1, T2>>)Forward).CopyTo(array, arrayIndex);
        }

        public bool Remove(KeyValuePair<T1, T2> item)
        {
            if (Contains(item))
                return ((ICollection<KeyValuePair<T1, T2>>) Forward).Remove(item) && Reverse.Remove(item.Value);
            else return false;
        }

        public bool Remove(KeyValuePair<T2, T1> item)
        {
            if (Contains(item))
                return ((ICollection<KeyValuePair<T2, T1>>) Reverse).Remove(item) && Forward.Remove(item.Value);
            else return false;
        }

        public IEnumerator<KeyValuePair<T1, T2>> GetEnumerator()
        {
            return ((ICollection<KeyValuePair<T1, T2>>)Forward).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((ICollection<KeyValuePair<T1, T2>>)Forward).GetEnumerator();
        }

        public void Add(KeyValuePair<T2, T1> item)
        {
            InternalAdd(item.Value, item.Key);
        }

        public bool Contains(KeyValuePair<T2, T1> item)
        {
            return (((ICollection<KeyValuePair<T2, T1>>) Reverse).Contains(item) && Forward.ContainsKey(item.Value) && Forward[item.Value].Equals(item.Key));
        }

        public void CopyTo(KeyValuePair<T2, T1>[] array, int arrayIndex)
        {
            ((ICollection<KeyValuePair<T2, T1>>)Reverse).CopyTo(array, arrayIndex);
        }


        public bool ContainsKey(T1 key)
        {
            ContainingCheck(key);
            return Forward.ContainsKey(key);
        }

        public bool ContainsKey(T2 key)
        {
            ContainingCheck(key);
            return Reverse.ContainsKey(key);
        }

        public bool TryGetValue(T1 key, out T2 value)
        {
            return Forward.TryGetValue(key, out value);
        }

        public bool TryGetValue(T2 key, out T1 value)
        {
            return Reverse.TryGetValue(key, out value);
        }

        public void Add(T2 key2, T1 key1)
        {
            InternalAdd(key1, key2);
        }

        public void Add(T1 key1, T2 key2)
        {
            InternalAdd(key1, key2);
        }

        public bool Remove(T1 key)
        {
            ContainingCheck(key);
            MapCheck(key, Forward[key]);
            return Reverse.Remove(Forward[key]) && Forward.Remove(key);
        }

        public bool Remove(T2 key)
        {
            ContainingCheck(key);
            MapCheck(Reverse[key], key);
            return Forward.Remove(Reverse[key]) && Reverse.Remove(key);
        }

        void CountCheck ()
        {
            if (_forward.Count != _reverse.Count) throw new System.Exception("The amount of entries of the 2 internal dictionaries is not correct. Is the data corrupted?");
        }

        void MapCheck (T1 t1, T2 t2)
        {
            if (!_forward[t1].Equals(t2) || !_reverse[t2].Equals(t1)) 
                throw new System.Exception("The mapping between t1 and t2 is wrong in this BiMap.  Is the data corrupted?");
        }

        void ContainingCheck (T1 t1)
        {
            if (_forward.ContainsKey(t1) && !_reverse.ContainsValue(t1))            
                throw new KeyNotFoundException("t1 is contained only in forward dict of this map. Is the data corrupted?");
            if (!_forward.ContainsKey(t1) && _reverse.ContainsValue(t1))            
                throw new KeyNotFoundException("t1 is contained only in reverse dict of this map. Is the data corrupted?");
        }        
        void ContainingCheck (T2 t2)
        {
            if (_reverse.ContainsKey(t2) && !_forward.ContainsValue(t2))            
                throw new KeyNotFoundException("t2 is contained only in reverse dict of this map. Is the data corrupted?");
            if (!_reverse.ContainsKey(t2) && _forward.ContainsValue(t2))            
                throw new KeyNotFoundException("t2 is contained only in forward dict of this map. Is the data corrupted?");
        }
    }
}