using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BA_Studio.UnityLib.GameObjectPool
{
	public class GameObjectPool<T> where T: MonoBehaviour {

		GameObject PoolGO;

		List<T> poolList;

		GameObject[] pooledPrefab;

		public GameObjectPool (GameObject prefab, int defaultPop = 5, Transform parent = null, bool dontDestroyOnLoad = true) {
			if (parent != null) PoolGO = parent.gameObject;
			TryInit();
			if (dontDestroyOnLoad) MonoBehaviour.DontDestroyOnLoad(PoolGO);
			pooledPrefab = new GameObject[1] { prefab };
			poolList.AddRange(NewPooledObjects(defaultPop));
		}

		public GameObjectPool (GameObject[] prefabs, int defaultPop = 5, Transform parent = null, bool dontDestroyOnLoad = true) {
			if (parent != null) PoolGO = parent.gameObject;
			TryInit();
			if (dontDestroyOnLoad) MonoBehaviour.DontDestroyOnLoad(PoolGO);
			pooledPrefab = prefabs;
			poolList.AddRange(NewPooledObjects(defaultPop));
		}

		public GameObjectPool (T prefab, int defaultPop = 5, Transform parent = null, bool dontDestroyOnLoad = true) {
			if (parent != null) PoolGO = parent.gameObject;
			TryInit();
			if (dontDestroyOnLoad) MonoBehaviour.DontDestroyOnLoad(PoolGO);
			pooledPrefab = new GameObject[1] { prefab.gameObject };
			poolList.AddRange(NewPooledObjects(defaultPop));
		}

		public GameObjectPool (T[] prefabs, int defaultPop = 5, Transform parent = null, bool dontDestroyOnLoad = true) {
			if (parent != null) PoolGO = parent.gameObject;
			TryInit();
			if (dontDestroyOnLoad) MonoBehaviour.DontDestroyOnLoad(PoolGO);
			pooledPrefab = new GameObject[prefabs.Length];
			for (int i = 0; i < prefabs.Length; ++i) pooledPrefab[i] = prefabs[i].gameObject;
			poolList.AddRange(NewPooledObjects(defaultPop));
		}

		void TryInit () {
			if (PoolGO == null) {
				PoolGO = new GameObject();
				PoolGO.name = "GameObjectPool (" + typeof(T).Name + ")";
				PoolGO.SetActive(false);
			}
			if (poolList == null) poolList = new List<T>();
		}

		public T GetObjectFromPool (Transform targetParent) {
			if (poolList.Count < 1) {
				poolList.AddRange(NewPooledObjects(3));
			}
			T returningO = poolList[0];
			poolList.RemoveAt(0);
			returningO.transform.SetParent(targetParent, false);
			return returningO;
		}

		public List<T> GetObjectsFromPool (int count, Transform targetParent) {
			if (poolList.Count < count) {
				poolList.AddRange(NewPooledObjects(count + 5));
			}
			List<T> returningOs = poolList.GetRange(0, count);
			poolList.RemoveRange(0, count);
			foreach (T obj in returningOs) {
				obj.transform.SetParent(targetParent, false);
			}
			return returningOs;
		}

		public void ReturnToPool (params T[] returning) {
			foreach (T obj in returning) obj.transform.SetParent(PoolGO.transform, false);
			poolList.AddRange(returning);	
		}

		public List<T> NewPooledObjects(int count) {
			List<T> newOs = new List<T>();
			for (int i = 0; i < count; i++){
				if (pooledPrefab.Length <= 1) newOs.Add(GameObject.Instantiate(pooledPrefab[0], PoolGO.transform, false).GetComponent<T>());
				else newOs.Add(GameObject.Instantiate(pooledPrefab[Random.Range(0, pooledPrefab.Length)], PoolGO.transform, false).GetComponent<T>());
			}
			return newOs;
		}

		public void HelpOrganizeList (List<T> list, int newCount, Transform targetParent, System.Action<T> beforeReturn)
		{
			if (list.Count < newCount)
			{
				list.AddRange(GetObjectsFromPool(newCount - list.Count, targetParent));
			}
			else if (list.Count > newCount)
			{
				T[] c = list.GetRange(newCount, list.Count - newCount).ToArray();
				list.RemoveRange(newCount, list.Count - newCount);
				foreach (T t in c) beforeReturn(t);
				ReturnToPool(c);
			}
		}
	}
}