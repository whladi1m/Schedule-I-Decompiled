using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ScheduleOne.Audio
{
	// Token: 0x0200078D RID: 1933
	[RequireComponent(typeof(Button))]
	[RequireComponent(typeof(EventTrigger))]
	[RequireComponent(typeof(AudioSourceController))]
	public class ButtonSound : MonoBehaviour
	{
		// Token: 0x060034A2 RID: 13474 RVA: 0x000DD5C8 File Offset: 0x000DB7C8
		public void Awake()
		{
			this.AddEventTrigger(this.EventTrigger, EventTriggerType.PointerEnter, new Action(this.Hovered));
			this.AddEventTrigger(this.EventTrigger, EventTriggerType.PointerClick, new Action(this.Clicked));
			this.AudioSource.AudioSource.playOnAwake = false;
			this.Button = base.GetComponent<Button>();
		}

		// Token: 0x060034A3 RID: 13475 RVA: 0x000DD626 File Offset: 0x000DB826
		private void OnValidate()
		{
			if (this.AudioSource == null)
			{
				this.AudioSource = base.GetComponent<AudioSourceController>();
			}
			if (this.EventTrigger == null)
			{
				this.EventTrigger = base.GetComponent<EventTrigger>();
			}
		}

		// Token: 0x060034A4 RID: 13476 RVA: 0x000DD65C File Offset: 0x000DB85C
		public void AddEventTrigger(EventTrigger eventTrigger, EventTriggerType eventTriggerType, Action action)
		{
			EventTrigger.Entry entry = new EventTrigger.Entry();
			entry.eventID = eventTriggerType;
			entry.callback.AddListener(delegate(BaseEventData data)
			{
				action();
			});
			eventTrigger.triggers.Add(entry);
		}

		// Token: 0x060034A5 RID: 13477 RVA: 0x000DD6A8 File Offset: 0x000DB8A8
		protected virtual void Hovered()
		{
			if (!this.Button.interactable)
			{
				return;
			}
			this.AudioSource.VolumeMultiplier = this.HoverSoundVolume;
			this.AudioSource.AudioSource.clip = this.HoverClip;
			this.AudioSource.PitchMultiplier = 0.9f;
			this.AudioSource.Play();
		}

		// Token: 0x060034A6 RID: 13478 RVA: 0x000DD708 File Offset: 0x000DB908
		protected virtual void Clicked()
		{
			if (!this.Button.interactable)
			{
				return;
			}
			this.AudioSource.VolumeMultiplier = this.ClickSoundVolume;
			this.AudioSource.AudioSource.clip = this.ClickClip;
			this.AudioSource.Play();
		}

		// Token: 0x040025E8 RID: 9704
		public AudioSourceController AudioSource;

		// Token: 0x040025E9 RID: 9705
		public EventTrigger EventTrigger;

		// Token: 0x040025EA RID: 9706
		[Header("Clips")]
		public AudioClip HoverClip;

		// Token: 0x040025EB RID: 9707
		public float HoverSoundVolume = 1f;

		// Token: 0x040025EC RID: 9708
		public AudioClip ClickClip;

		// Token: 0x040025ED RID: 9709
		public float ClickSoundVolume = 1f;

		// Token: 0x040025EE RID: 9710
		private Button Button;
	}
}
