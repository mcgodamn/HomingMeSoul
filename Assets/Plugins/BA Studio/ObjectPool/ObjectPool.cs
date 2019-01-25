using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BA_Studio.ObjectPool
{
	public class ObjectPool<T> where T : new()
	{
		List<T> pooled;

		public ObjectPool (int defaultPop = 5) {
			TryInit();
			pooled.AddRange(NewPooledObjects(defaultPop));
		}

		void TryInit () {
			if (pooled == null) pooled = new List<T>();
		}

		public T GetObjectFromPool () {
			if (pooled.Count < 1) {
				pooled.AddRange(NewPooledObjects(3));
			}
			T returningO = pooled[pooled.Count - 1];
			pooled.RemoveAt(pooled.Count - 1);
			return returningO;
		}

		public T[] GetObjectsFromPool (int count) {
			if (pooled.Count < count) {
				pooled.AddRange(NewPooledObjects(count + 5));
			}
			List<T> returningOs = pooled.GetRange(pooled.Count - count, count);
			pooled.RemoveRange(pooled.Count - count, count);
			return returningOs.ToArray();
		}

		public void ReturnToPool (params T[] returning) {
			pooled.AddRange(returning);	
		}

		public T[] NewPooledObjects(int count) {
			T[] newOs = new T[count];
			for (int i = 0; i < newOs.Length; i++) newOs[i] = new T();
			return newOs;
		}
	}
}