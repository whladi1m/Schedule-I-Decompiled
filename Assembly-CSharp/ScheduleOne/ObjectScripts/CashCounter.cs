using System;
using System.Collections;
using System.Collections.Generic;
using ScheduleOne.Audio;
using UnityEngine;

namespace ScheduleOne.ObjectScripts
{
	// Token: 0x02000B66 RID: 2918
	public class CashCounter : MonoBehaviour
	{
		// Token: 0x06004DC6 RID: 19910 RVA: 0x001481B0 File Offset: 0x001463B0
		public virtual void LateUpdate()
		{
			this.UpperNotes.gameObject.SetActive(this.IsOn);
			this.LowerNotes.gameObject.SetActive(this.IsOn);
			if (this.IsOn)
			{
				if (!this.lerping)
				{
					this.lerping = true;
					for (int i = 0; i < this.MovingNotes.Count; i++)
					{
						base.StartCoroutine(this.LerpNote(this.MovingNotes[i]));
					}
				}
				if (!this.Audio.AudioSource.isPlaying)
				{
					this.Audio.Play();
					return;
				}
			}
			else
			{
				this.lerping = false;
				if (this.Audio.AudioSource.isPlaying)
				{
					this.Audio.Stop();
				}
			}
		}

		// Token: 0x06004DC7 RID: 19911 RVA: 0x00148271 File Offset: 0x00146471
		private IEnumerator LerpNote(Transform note)
		{
			yield return new WaitForSeconds((float)this.MovingNotes.IndexOf(note) / (float)(this.MovingNotes.Count + 1) * 0.18f);
			note.gameObject.SetActive(true);
			while (this.IsOn)
			{
				note.position = this.NoteStartPoint.position;
				note.rotation = this.NoteStartPoint.rotation;
				for (float i = 0f; i < 0.18f; i += Time.deltaTime)
				{
					note.position = Vector3.Lerp(this.NoteStartPoint.position, this.NoteEndPoint.position, i / 0.18f);
					note.rotation = Quaternion.Lerp(this.NoteStartPoint.rotation, this.NoteEndPoint.rotation, i / 0.18f);
					yield return new WaitForEndOfFrame();
				}
			}
			note.gameObject.SetActive(false);
			yield break;
		}

		// Token: 0x04003AD8 RID: 15064
		public const float NoteLerpTime = 0.18f;

		// Token: 0x04003AD9 RID: 15065
		public bool IsOn;

		// Token: 0x04003ADA RID: 15066
		[Header("References")]
		public GameObject UpperNotes;

		// Token: 0x04003ADB RID: 15067
		public GameObject LowerNotes;

		// Token: 0x04003ADC RID: 15068
		public Transform NoteStartPoint;

		// Token: 0x04003ADD RID: 15069
		public Transform NoteEndPoint;

		// Token: 0x04003ADE RID: 15070
		public List<Transform> MovingNotes = new List<Transform>();

		// Token: 0x04003ADF RID: 15071
		public AudioSourceController Audio;

		// Token: 0x04003AE0 RID: 15072
		private bool lerping;
	}
}
