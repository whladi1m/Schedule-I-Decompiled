using System;
using ScheduleOne.Audio;
using UnityEngine;

// Token: 0x02000047 RID: 71
[RequireComponent(typeof(AudioSource))]
public class FlockChildSound : MonoBehaviour
{
	// Token: 0x0600017A RID: 378 RVA: 0x00008CBC File Offset: 0x00006EBC
	public void Start()
	{
		this._flockChild = base.GetComponent<FlockChild>();
		this._audio = base.GetComponent<AudioSource>();
		base.InvokeRepeating("PlayRandomSound", UnityEngine.Random.value + 1f, 1f);
		if (this._scareSounds.Length != 0)
		{
			base.InvokeRepeating("ScareSound", 1f, 0.01f);
		}
	}

	// Token: 0x0600017B RID: 379 RVA: 0x00008D1C File Offset: 0x00006F1C
	public void PlayRandomSound()
	{
		if (base.gameObject.activeInHierarchy)
		{
			if (!this._audio.isPlaying && this._flightSounds.Length != 0 && this._flightSoundRandomChance > UnityEngine.Random.value && !this._flockChild._landing)
			{
				if (this.controller != null)
				{
					this.controller.Play();
					return;
				}
			}
			else if (!this._audio.isPlaying && this._idleSounds.Length != 0 && this._idleSoundRandomChance > UnityEngine.Random.value && this._flockChild._landing && this.controller != null)
			{
				this.controller.Play();
			}
		}
	}

	// Token: 0x0600017C RID: 380 RVA: 0x00008DCC File Offset: 0x00006FCC
	public void ScareSound()
	{
		if (base.gameObject.activeInHierarchy && this._hasLanded && !this._flockChild._landing && this._idleSoundRandomChance * 2f > UnityEngine.Random.value)
		{
			this._audio.clip = this._scareSounds[UnityEngine.Random.Range(0, this._scareSounds.Length)];
			this._audio.volume = UnityEngine.Random.Range(this._volumeMin, this._volumeMax);
			this._audio.PlayDelayed(UnityEngine.Random.value * 0.2f);
			this._hasLanded = false;
		}
	}

	// Token: 0x04000160 RID: 352
	public AudioSourceController controller;

	// Token: 0x04000161 RID: 353
	public AudioClip[] _idleSounds;

	// Token: 0x04000162 RID: 354
	public float _idleSoundRandomChance = 0.05f;

	// Token: 0x04000163 RID: 355
	public AudioClip[] _flightSounds;

	// Token: 0x04000164 RID: 356
	public float _flightSoundRandomChance = 0.05f;

	// Token: 0x04000165 RID: 357
	public AudioClip[] _scareSounds;

	// Token: 0x04000166 RID: 358
	public float _pitchMin = 0.85f;

	// Token: 0x04000167 RID: 359
	public float _pitchMax = 1f;

	// Token: 0x04000168 RID: 360
	public float _volumeMin = 0.6f;

	// Token: 0x04000169 RID: 361
	public float _volumeMax = 0.8f;

	// Token: 0x0400016A RID: 362
	private FlockChild _flockChild;

	// Token: 0x0400016B RID: 363
	private AudioSource _audio;

	// Token: 0x0400016C RID: 364
	private bool _hasLanded;
}
