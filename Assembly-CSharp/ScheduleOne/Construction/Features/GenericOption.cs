using System;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.Construction.Features
{
	// Token: 0x02000715 RID: 1813
	public class GenericOption : MonoBehaviour
	{
		// Token: 0x0600311D RID: 12573 RVA: 0x000CBBA6 File Offset: 0x000C9DA6
		public virtual void Install()
		{
			if (this.onInstalled != null)
			{
				this.onInstalled.Invoke();
			}
			this.SetVisible();
		}

		// Token: 0x0600311E RID: 12574 RVA: 0x000CBBC1 File Offset: 0x000C9DC1
		public virtual void Uninstall()
		{
			if (this.onUninstalled != null)
			{
				this.onUninstalled.Invoke();
			}
			this.SetInvisible();
		}

		// Token: 0x0600311F RID: 12575 RVA: 0x000CBBDC File Offset: 0x000C9DDC
		public virtual void SetVisible()
		{
			if (this.onSetVisible != null)
			{
				this.onSetVisible.Invoke();
			}
		}

		// Token: 0x06003120 RID: 12576 RVA: 0x000CBBF1 File Offset: 0x000C9DF1
		public virtual void SetInvisible()
		{
			if (this.onSetInvisible != null)
			{
				this.onSetInvisible.Invoke();
			}
		}

		// Token: 0x0400231F RID: 8991
		[Header("Interface settings")]
		public string optionName;

		// Token: 0x04002320 RID: 8992
		public Color optionButtonColor;

		// Token: 0x04002321 RID: 8993
		public float optionPrice;

		// Token: 0x04002322 RID: 8994
		[Header("Events")]
		public UnityEvent onInstalled;

		// Token: 0x04002323 RID: 8995
		public UnityEvent onUninstalled;

		// Token: 0x04002324 RID: 8996
		public UnityEvent onSetVisible;

		// Token: 0x04002325 RID: 8997
		public UnityEvent onSetInvisible;
	}
}
