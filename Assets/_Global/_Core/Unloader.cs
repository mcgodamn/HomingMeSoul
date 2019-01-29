using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace AngerStudio.WeedProject
{
	public class Unloader : MonoBehaviour
	{
		public string targetSceneName;
		void Start ()
		{			
			GameObject[] allGameObjects = GameObject.FindObjectsOfType<GameObject>();
			for (int i = 0; i < allGameObjects.Length; i++)
			{
				if (allGameObjects[i] != this.gameObject)
				{
					allGameObjects[i].SetActive(false);
					GameObject.Destroy(allGameObjects[i]);
				}
			}
			Destroy(this);
			UnityEngine.SceneManagement.SceneManager.LoadScene(targetSceneName);
		}
	}
}
