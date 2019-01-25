using UnityEngine;

namespace BA_Studio.UnityLib.GlobalAudio
{
    [System.Serializable]
	public class NamedAudioClip
	{
		public string ID;

		public AudioClip clip;

		public bool volumeOverwrite = true;

		public float volume = 1f;

		public bool pitchOverwrite = true;

		public float pitch = 1f;
	}
}
