using System;
using System.Collections;
using ScheduleOne.DevUtilities;
using ScheduleOne.UI.Tooltips;
using UnityEngine;

namespace ScheduleOne.UI.Phone
{
	// Token: 0x02000A8B RID: 2699
	public class AppsCanvas : PlayerSingleton<AppsCanvas>
	{
		// Token: 0x17000A2C RID: 2604
		// (get) Token: 0x060048A5 RID: 18597 RVA: 0x0013038D File Offset: 0x0012E58D
		// (set) Token: 0x060048A6 RID: 18598 RVA: 0x00130395 File Offset: 0x0012E595
		public bool isOpen { get; private set; }

		// Token: 0x060048A7 RID: 18599 RVA: 0x0013039E File Offset: 0x0012E59E
		protected override void Awake()
		{
			base.Awake();
			this.SetIsOpen(false);
		}

		// Token: 0x060048A8 RID: 18600 RVA: 0x001303B0 File Offset: 0x0012E5B0
		public override void OnStartClient(bool IsOwner)
		{
			base.OnStartClient(IsOwner);
			if (!IsOwner)
			{
				return;
			}
			Phone instance = PlayerSingleton<Phone>.Instance;
			instance.onPhoneOpened = (Action)Delegate.Combine(instance.onPhoneOpened, new Action(this.PhoneOpened));
			Phone instance2 = PlayerSingleton<Phone>.Instance;
			instance2.onPhoneClosed = (Action)Delegate.Combine(instance2.onPhoneClosed, new Action(this.PhoneClosed));
		}

		// Token: 0x060048A9 RID: 18601 RVA: 0x00130414 File Offset: 0x0012E614
		protected void PhoneOpened()
		{
			if (this.isOpen)
			{
				this.SetCanvasActive(true);
			}
		}

		// Token: 0x060048AA RID: 18602 RVA: 0x00130425 File Offset: 0x0012E625
		protected void PhoneClosed()
		{
			this.delayedSetOpenRoutine = base.StartCoroutine(this.DelayedSetCanvasActive(false, 0.25f));
		}

		// Token: 0x060048AB RID: 18603 RVA: 0x0013043F File Offset: 0x0012E63F
		private IEnumerator DelayedSetCanvasActive(bool active, float delay)
		{
			yield return new WaitForSeconds(delay);
			this.delayedSetOpenRoutine = null;
			this.SetCanvasActive(active);
			yield break;
		}

		// Token: 0x060048AC RID: 18604 RVA: 0x0013045C File Offset: 0x0012E65C
		public void SetIsOpen(bool o)
		{
			this.isOpen = o;
			this.SetCanvasActive(o);
		}

		// Token: 0x060048AD RID: 18605 RVA: 0x0013046C File Offset: 0x0012E66C
		private void SetCanvasActive(bool a)
		{
			if (this.delayedSetOpenRoutine != null)
			{
				base.StopCoroutine(this.delayedSetOpenRoutine);
			}
			this.canvas.enabled = a;
		}

		// Token: 0x060048AE RID: 18606 RVA: 0x0013048E File Offset: 0x0012E68E
		protected override void Start()
		{
			base.Start();
			Singleton<TooltipManager>.Instance.AddCanvas(this.canvas);
		}

		// Token: 0x0400360C RID: 13836
		[Header("References")]
		public Canvas canvas;

		// Token: 0x0400360D RID: 13837
		private Coroutine delayedSetOpenRoutine;
	}
}
