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
			List<GameObject> t = new List<GameObject>(this.gameObject.scene.GetRootGameObjects());
			#if DDOL_EXT
			t.Union(DDOLRegistry.GetDDOLs());
			#endif
            foreach (var root in t)
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
