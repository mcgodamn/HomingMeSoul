using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using BA_Studio.UnityLib.SingletonLocator;

namespace BA_Studio.UnityLib.GlobalAudio
{
    public class GlobalAudio : MonoBehaviour
	{
		public static GlobalAudio Instance { get => SingletonBehaviourLocator<GlobalAudio>.Instance; }

		public static Dictionary<string, AudioSource> audioSources, audioSourcesBackUp;

		public List<NamedAudioClipReference> preLoadedClips;

		void Awake ()
		{
			SingletonBehaviourLocator<GlobalAudio>.Set(this, false, null, (ga) => { SingletonBehaviourLocator<GlobalAudio>.Instance.preLoadedClips.AddRange(ga.preLoadedClips); } );
			audioSources = new Dictionary<string, AudioSource>();
			audioSourcesBackUp = new Dictionary<string, AudioSource>();
			audioSources.Add("Default", this.gameObject.AddComponent<AudioSource>());
			audioSources["Default"].playOnAwake = false;
			audioSources["Default"].bypassReverbZones = true;
			audioSourcesBackUp.Add("Default", this.gameObject.AddComponent<AudioSource>());
			audioSourcesBackUp["Default"].playOnAwake = false;
			audioSourcesBackUp["Default"].bypassReverbZones = true;
		}

		public static void PlayPreLoadedClipByID (string ID, string channel = "Default")
		{
			NamedAudioClip nac = Instance.preLoadedClips.Find(r => r.Value.ID == ID)?.Value;
			if (nac == null) return;
			
			if (!audioSources.ContainsKey(channel)) 
			{
				audioSources.Add(channel, Instance.gameObject.AddComponent<AudioSource>());
				audioSources[channel].playOnAwake = false;
				audioSources[channel].bypassReverbZones = true;
			}
			if (!audioSourcesBackUp.ContainsKey(channel)) 
			{
				audioSourcesBackUp.Add(channel, Instance.gameObject.AddComponent<AudioSource>());
				audioSourcesBackUp[channel].playOnAwake = false;
				audioSourcesBackUp[channel].bypassReverbZones = true;
			}
		    if (nac.volumeOverwrite)
			{
				audioSources[channel].volume = nac.volume;
				audioSourcesBackUp[channel].volume = nac.volume;
			}
			if (nac.pitchOverwrite)
			{
				audioSources[channel].pitch = nac.pitch;
				audioSourcesBackUp[channel].pitch = nac.pitch;
			}
			if (audioSources[channel].clip != nac.clip)
			{
				audioSources[channel].clip = nac.clip;
				audioSourcesBackUp[channel].clip = nac.clip;
			}
			if (audioSources[channel].isPlaying) 
			{
				audioSourcesBackUp[channel].enabled = true;
				audioSourcesBackUp[channel].Play();
			}
			else
			{
				audioSourcesBackUp[channel].enabled = false;
				audioSources[channel].Play();
			}
		}

		public void PlayPreLoadedClipByID_Proxy (string ID)
		{
			PlayPreLoadedClipByID(ID, "Default");
		}
	}
}
