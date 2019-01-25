using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace BA_Studio.UnityLib.GlobalAudio
{
	public class GlobalAudio_PrefabAgent : MonoBehaviour
	{

		public void PlayPreLoadedClipByID_Proxy (string ID)
		{
			GlobalAudio.Instance.PlayPreLoadedClipByID_Proxy(ID);
		}
	}
}
