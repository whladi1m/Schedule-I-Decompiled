using System;
using ScheduleOne.DevUtilities;
using ScheduleOne.PlayerScripts;
using UnityEngine;

namespace ScheduleOne.PlayerTasks
{
	// Token: 0x0200033C RID: 828
	public class Moveable : Clickable
	{
		// Token: 0x06001288 RID: 4744 RVA: 0x00050E54 File Offset: 0x0004F054
		public override void StartClick(RaycastHit hit)
		{
			base.StartClick(hit);
			this.clickDist = Vector3.Distance(base.transform.position, PlayerSingleton<PlayerCamera>.Instance.transform.position);
			this.clickOffset = base.transform.position - PlayerSingleton<PlayerCamera>.Instance.Camera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, this.clickDist));
		}

		// Token: 0x06001289 RID: 4745 RVA: 0x00050ED4 File Offset: 0x0004F0D4
		protected virtual void Update()
		{
			if (base.IsHeld)
			{
				base.transform.position = Vector3.Lerp(base.transform.position, PlayerSingleton<PlayerCamera>.Instance.Camera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, this.clickDist)) + this.clickOffset, Time.deltaTime * 10f);
				base.transform.localPosition = new Vector3(base.transform.localPosition.x, Mathf.Clamp(base.transform.localPosition.y, this.yMin, this.yMax), base.transform.localPosition.z);
			}
		}

		// Token: 0x0600128A RID: 4746 RVA: 0x00050F9C File Offset: 0x0004F19C
		public override void EndClick()
		{
			base.EndClick();
		}

		// Token: 0x040011E1 RID: 4577
		protected Vector3 clickOffset = Vector3.zero;

		// Token: 0x040011E2 RID: 4578
		protected float clickDist;

		// Token: 0x040011E3 RID: 4579
		[Header("Bounds")]
		[SerializeField]
		protected float yMax = 10f;

		// Token: 0x040011E4 RID: 4580
		[SerializeField]
		protected float yMin = -10f;
	}
}
