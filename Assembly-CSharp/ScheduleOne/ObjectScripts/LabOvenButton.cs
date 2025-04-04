using System;
using System.Collections;
using ScheduleOne.DevUtilities;
using ScheduleOne.Misc;
using ScheduleOne.PlayerTasks;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.ObjectScripts
{
	// Token: 0x02000BAB RID: 2987
	public class LabOvenButton : MonoBehaviour
	{
		// Token: 0x17000B96 RID: 2966
		// (get) Token: 0x0600527F RID: 21119 RVA: 0x0015C14A File Offset: 0x0015A34A
		// (set) Token: 0x06005280 RID: 21120 RVA: 0x0015C152 File Offset: 0x0015A352
		public bool Pressed { get; private set; }

		// Token: 0x06005281 RID: 21121 RVA: 0x0015C15B File Offset: 0x0015A35B
		private void Start()
		{
			this.SetInteractable(false);
			this.Clickable.onClickStart.AddListener(new UnityAction<RaycastHit>(this.Press));
		}

		// Token: 0x06005282 RID: 21122 RVA: 0x0015C180 File Offset: 0x0015A380
		public void SetInteractable(bool interactable)
		{
			this.Clickable.ClickableEnabled = interactable;
		}

		// Token: 0x06005283 RID: 21123 RVA: 0x0015C18E File Offset: 0x0015A38E
		public void Press(RaycastHit hit)
		{
			this.SetPressed(true);
		}

		// Token: 0x06005284 RID: 21124 RVA: 0x0015C198 File Offset: 0x0015A398
		public void SetPressed(bool pressed)
		{
			if (this.Pressed == pressed)
			{
				return;
			}
			this.Pressed = pressed;
			this.Light.isOn = pressed;
			if (this.Pressed)
			{
				if (this.pressCoroutine != null)
				{
					base.StopCoroutine(this.pressCoroutine);
				}
				this.pressCoroutine = Singleton<CoroutineService>.Instance.StartCoroutine(this.MoveButton(this.PressedTransform));
				return;
			}
			if (this.pressCoroutine != null)
			{
				base.StopCoroutine(this.pressCoroutine);
			}
			this.pressCoroutine = Singleton<CoroutineService>.Instance.StartCoroutine(this.MoveButton(this.DepressedTransform));
		}

		// Token: 0x06005285 RID: 21125 RVA: 0x0015C22B File Offset: 0x0015A42B
		private IEnumerator MoveButton(Transform destination)
		{
			Vector3 startPos = this.Button.localPosition;
			Vector3 endPos = destination.localPosition;
			float lerpTime = 0.2f;
			for (float t = 0f; t < lerpTime; t += Time.deltaTime)
			{
				this.Button.localPosition = Vector3.Lerp(startPos, endPos, t / lerpTime);
				yield return null;
			}
			this.Button.localPosition = endPos;
			this.pressCoroutine = null;
			yield break;
		}

		// Token: 0x04003D99 RID: 15769
		public Transform Button;

		// Token: 0x04003D9A RID: 15770
		public Transform PressedTransform;

		// Token: 0x04003D9B RID: 15771
		public Transform DepressedTransform;

		// Token: 0x04003D9C RID: 15772
		public ToggleableLight Light;

		// Token: 0x04003D9D RID: 15773
		public Clickable Clickable;

		// Token: 0x04003D9E RID: 15774
		private Coroutine pressCoroutine;
	}
}
