using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

namespace AngerStudio.WeedProject
{
	public class Unloader : MonoBehaviour
	{
		public string targetSceneName;
		void Start ()
		{
            var go = new GameObject("Sacrificial Lamb");
            DontDestroyOnLoad(go);
			Scene dontDestroyOnLoad = go.scene;

            GameObject[] allGameObjects = dontDestroyOnLoad.GetRootGameObjects();
            foreach (var root in allGameObjects)
            {
				if (root.name == "[DOTween]" || root.name == "SimpleInput")
					continue;
                Destroy(root);
            }

			// GameObject[] allGameObjects = GameObject.FindObjectsOfType<GameObject>();
			// for (int i = 0; i < allGameObjects.Length; i++)
			// {
			// 	if (allGameObjects[i] != this.gameObject)
			// 	{
					

			// 		allGameObjects[i].SetActive(false);
			// 		GameObject.Destroy(allGameObjects[i]);
			// 	}
			// }
			// Destroy(this);
			UnityEngine.SceneManagement.SceneManager.LoadScene(targetSceneName);
		}
	}
}
