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
			DontDestroyOnLoad(this);
            foreach (var root in DDOLRegistry.GetDDOLs().Union(this.gameObject.scene.GetRootGameObjects()))
            {
				if ( ! dontUnloadFilter.Match(root as GameObject)) continue;
                else
				{					
					Destroy(root);
				}
            }
			
			UnityEngine.SceneManagement.SceneManager.LoadScene(targetSceneName);
		}
	}
}
