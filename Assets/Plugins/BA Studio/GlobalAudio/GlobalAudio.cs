using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using BA_Studio.UnityLib.SingletonLocator;

namespace BA_Studio.UnityLib.GlobalAudio
{
    public class GlobalAudio : MonoBehaviour
	{
		public static GlobalAudio Instance { get => SingletonBehaviourLocator<GlobalAudio>.Instance; }

		public static Dictionary<string, AudioSource> audioSources;

		public List<NamedAudioClipReference> preLoadedClips;

		void Awake ()
		{
			SingletonBehaviourLocator<GlobalAudio>.Set(this);
			audioSources = new Dictionary<string, AudioSource>();
			audioSources.Add("Default", this.gameObject.AddComponent<AudioSource>());
		}

		public static void PlayPreLoadedClipByID (string ID, string channel = "Default")
		{
			NamedAudioClip nac = Instance.preLoadedClips.Find(r => r.Value.ID == ID)?.Value;
			if (nac == null) return;
			if (!audioSources.ContainsKey(channel)) 
				audioSources.Add(channel, Instance.gameObject.AddComponent<AudioSource>());
		    if (nac.volumeOverwrite)
			{
				audioSources[channel].volume = nac.volume;
			}
			if (nac.pitchOverwrite)
			{
				audioSources[channel].pitch = nac.pitch;
			}
			audioSources[channel].clip = nac.clip;
			audioSources[channel].Play();
		}

		public void PlayPreLoadedClipByID_Proxy (string ID)
		{
			PlayPreLoadedClipByID(ID, "Default");
		}
	}
}
