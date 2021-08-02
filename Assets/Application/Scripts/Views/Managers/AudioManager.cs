using System.Linq;
using Application.Scripts.Model;
using UnityEngine;

namespace Application.Scripts.Views.Managers
{
	public class AudioManager : MonoBehaviour
	{

		public static AudioManager instance;

		[SerializeField]
		private Sound[] sounds;

		private Sound currentTheme;

		void Awake ()
		{
			if (instance != null) {
				if (instance != this) {
					Destroy (this.gameObject);
				}
			} else {
				instance = this;	
				DontDestroyOnLoad (this);
			}

			for (int i = 0; i < sounds.Length; i++) {
				GameObject gameObject = new GameObject ("Sound_" + sounds [i].name);
				gameObject.transform.SetParent (this.transform);
				sounds [i].SetSource (gameObject.AddComponent<AudioSource> ());
			}

		}
    
		public void PlaySound(string _name, bool isTheme,  bool _canReplay = true, bool _playOneShoot = false)
		{
			Sound sound = sounds.FirstOrDefault (x => x.name == _name);
			if (sound != null) {
				if (isTheme)
				{
					currentTheme = sound;
				}
            
				sound.Play (isTheme, _canReplay, _playOneShoot);
            
			} else {
				Debug.LogWarning ("No se encontro el sonido: " + _name);
			}
		}

		public void AdjustThemeVolume(float value)
		{
			if (currentTheme == null)
				return;

			value = Mathf.Clamp(value, 0f, 1f);

			currentTheme.SetVolume(value);
		}

		public void ResetVolume()
		{
			if (currentTheme == null)
				return;

			currentTheme.ResetVolume();
		}

		public void StopSound (string _name)
		{
			Sound sound = sounds.FirstOrDefault (x => x.name == _name);
			if (sound != null) {
				sound.Stop ();
			} else {
				Debug.LogWarning ("No se encontro el sonido: " + _name);
			}
		}
    
		public void StopAllSounds()
		{
			for (int i = 0; i < sounds.Length; i++)
			{
				sounds[i].Stop();
			}
		}

		[System.Serializable]
		public class Sound
		{
			public string name;
			public AudioClip audioClip;
			public bool loop;

			[Range (0f, 1f)]
			public float volume = 1f;
			[Range (0.5f, 1.5f)]
			public float pitch = 1f;

			[Range (0f, 0.5f)]
			public float randomVolume = 0.1f;
			[Range (0f, 0.5f)]
			public float randomPitch = 0.1f;

			private AudioSource audioSource;

			public void SetSource (AudioSource _audioSource)
			{
				audioSource = _audioSource;
				audioSource.loop = loop;
				audioSource.clip = audioClip;
			}

		
			public void Play(bool isTheme, bool canReplay, bool playOneShoot)
			{
				if (!canReplay && audioSource.isPlaying)
				{
					return;
				}

				audioSource.volume = volume * (1 + Random.Range(-randomVolume / 2f, randomVolume / 2f));
				audioSource.pitch = pitch * (1 + Random.Range(-randomVolume / 2f, randomVolume / 2f));

				if (isTheme)
				{
					if (GameSettings.GetMusic() == 0)
					{
						audioSource.volume = 0f;
					}
				}
				else
				{
					if (GameSettings.GetSound() == 0)
					{
						audioSource.volume = 0f;
					}
				}
            


				if (playOneShoot)
				{
					audioSource.PlayOneShot(audioSource.clip);
				}
				else
				{
					audioSource.Play();
				}
            
			}

			public void Stop ()
			{
				audioSource.Stop ();				
			}

			public void SetVolume(float value)
			{
				audioSource.volume = value;
			}

			public void ResetVolume()
			{
				audioSource.volume = volume;
			}
		}

	}
}
