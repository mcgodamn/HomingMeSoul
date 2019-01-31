using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

namespace BA_Studio.UnityLib.Utilities
{
	public class Reloader : MonoBehaviour
	{
		public string targetSceneName;

		public GameObjectFilter dontUnloadFilter;		

		void Awake ()
		{
			Reload();
		}

		public void Reload ()
		{
            List<GameObject> gos = new List<GameObject>();
			for (int i = 0; i < SceneManager.sceneCount; i++) gos.AddRange(SceneManager.GetSceneAt(i).GetRootGameObjects());


            foreach (var root in gos)
            {
				if (dontUnloadFilter.Match(root)) continue;
                else Destroy(root);
            }
			
			UnityEngine.SceneManagement.SceneManager.LoadScene(targetSceneName);
		}
	}
}
