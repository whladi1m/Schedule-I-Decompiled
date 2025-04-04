using System;
using UnityEngine;

// Token: 0x02000049 RID: 73
public class FlockScare : MonoBehaviour
{
	// Token: 0x06000189 RID: 393 RVA: 0x00009528 File Offset: 0x00007728
	private void CheckProximityToLandingSpots()
	{
		this.IterateLandingSpots();
		if (this.currentController._activeLandingSpots > 0 && this.CheckDistanceToLandingSpot(this.landingSpotControllers[this.lsc]))
		{
			this.landingSpotControllers[this.lsc].ScareAll();
		}
		base.Invoke("CheckProximityToLandingSpots", this.scareInterval);
	}

	// Token: 0x0600018A RID: 394 RVA: 0x00009584 File Offset: 0x00007784
	private void IterateLandingSpots()
	{
		this.ls += this.checkEveryNthLandingSpot;
		this.currentController = this.landingSpotControllers[this.lsc];
		int childCount = this.currentController.transform.childCount;
		if (this.ls > childCount - 1)
		{
			this.ls -= childCount;
			if (this.lsc < this.landingSpotControllers.Length - 1)
			{
				this.lsc++;
				return;
			}
			this.lsc = 0;
		}
	}

	// Token: 0x0600018B RID: 395 RVA: 0x0000960C File Offset: 0x0000780C
	private bool CheckDistanceToLandingSpot(LandingSpotController lc)
	{
		Transform child = lc.transform.GetChild(this.ls);
		return child.GetComponent<LandingSpot>().landingChild != null && (child.position - base.transform.position).sqrMagnitude < this.distanceToScare * this.distanceToScare;
	}

	// Token: 0x0600018C RID: 396 RVA: 0x00009670 File Offset: 0x00007870
	private void Invoker()
	{
		for (int i = 0; i < this.InvokeAmounts; i++)
		{
			float num = this.scareInterval / (float)this.InvokeAmounts * (float)i;
			base.Invoke("CheckProximityToLandingSpots", this.scareInterval + num);
		}
	}

	// Token: 0x0600018D RID: 397 RVA: 0x000096B3 File Offset: 0x000078B3
	private void OnEnable()
	{
		base.CancelInvoke("CheckProximityToLandingSpots");
		if (this.landingSpotControllers.Length != 0)
		{
			this.Invoker();
		}
	}

	// Token: 0x0600018E RID: 398 RVA: 0x000096CF File Offset: 0x000078CF
	private void OnDisable()
	{
		base.CancelInvoke("CheckProximityToLandingSpots");
	}

	// Token: 0x040001A0 RID: 416
	public LandingSpotController[] landingSpotControllers;

	// Token: 0x040001A1 RID: 417
	public float scareInterval = 0.1f;

	// Token: 0x040001A2 RID: 418
	public float distanceToScare = 2f;

	// Token: 0x040001A3 RID: 419
	public int checkEveryNthLandingSpot = 1;

	// Token: 0x040001A4 RID: 420
	public int InvokeAmounts = 1;

	// Token: 0x040001A5 RID: 421
	private int lsc;

	// Token: 0x040001A6 RID: 422
	private int ls;

	// Token: 0x040001A7 RID: 423
	private LandingSpotController currentController;
}
