using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200004E RID: 78
public class LandingSpotController : MonoBehaviour
{
	// Token: 0x060001A4 RID: 420 RVA: 0x0000A75C File Offset: 0x0000895C
	public void Start()
	{
		if (this._thisT == null)
		{
			this._thisT = base.transform;
		}
		if (this._flock == null)
		{
			this._flock = (FlockController)UnityEngine.Object.FindObjectOfType(typeof(FlockController));
			Debug.Log(((this != null) ? this.ToString() : null) + " has no assigned FlockController, a random FlockController has been assigned");
		}
		if (this._landOnStart)
		{
			base.StartCoroutine(this.InstantLandOnStart(0.1f));
		}
	}

	// Token: 0x060001A5 RID: 421 RVA: 0x0000A7E1 File Offset: 0x000089E1
	public void ScareAll()
	{
		this.ScareAll(0f, 1f);
	}

	// Token: 0x060001A6 RID: 422 RVA: 0x0000A7F4 File Offset: 0x000089F4
	public void ScareAll(float minDelay, float maxDelay)
	{
		for (int i = 0; i < this._thisT.childCount; i++)
		{
			if (this._thisT.GetChild(i).GetComponent<LandingSpot>() != null)
			{
				this._thisT.GetChild(i).GetComponent<LandingSpot>().Invoke("ReleaseFlockChild", UnityEngine.Random.Range(minDelay, maxDelay));
			}
		}
	}

	// Token: 0x060001A7 RID: 423 RVA: 0x0000A854 File Offset: 0x00008A54
	public void LandAll()
	{
		for (int i = 0; i < this._thisT.childCount; i++)
		{
			if (this._thisT.GetChild(i).GetComponent<LandingSpot>() != null)
			{
				LandingSpot component = this._thisT.GetChild(i).GetComponent<LandingSpot>();
				base.StartCoroutine(component.GetFlockChild(0f, 2f));
			}
		}
	}

	// Token: 0x060001A8 RID: 424 RVA: 0x0000A8B9 File Offset: 0x00008AB9
	public IEnumerator InstantLandOnStart(float delay)
	{
		yield return new WaitForSeconds(delay);
		for (int i = 0; i < this._thisT.childCount; i++)
		{
			if (this._thisT.GetChild(i).GetComponent<LandingSpot>() != null)
			{
				this._thisT.GetChild(i).GetComponent<LandingSpot>().InstantLand();
			}
		}
		yield break;
	}

	// Token: 0x060001A9 RID: 425 RVA: 0x0000A8CF File Offset: 0x00008ACF
	public IEnumerator InstantLand(float delay)
	{
		yield return new WaitForSeconds(delay);
		for (int i = 0; i < this._thisT.childCount; i++)
		{
			if (this._thisT.GetChild(i).GetComponent<LandingSpot>() != null)
			{
				this._thisT.GetChild(i).GetComponent<LandingSpot>().InstantLand();
			}
		}
		yield break;
	}

	// Token: 0x040001B9 RID: 441
	public bool _randomRotate = true;

	// Token: 0x040001BA RID: 442
	public Vector2 _autoCatchDelay = new Vector2(10f, 20f);

	// Token: 0x040001BB RID: 443
	public Vector2 _autoDismountDelay = new Vector2(10f, 20f);

	// Token: 0x040001BC RID: 444
	public float _maxBirdDistance = 20f;

	// Token: 0x040001BD RID: 445
	public float _minBirdDistance = 5f;

	// Token: 0x040001BE RID: 446
	public bool _takeClosest;

	// Token: 0x040001BF RID: 447
	public FlockController _flock;

	// Token: 0x040001C0 RID: 448
	public bool _landOnStart;

	// Token: 0x040001C1 RID: 449
	public bool _soarLand = true;

	// Token: 0x040001C2 RID: 450
	public bool _onlyBirdsAbove;

	// Token: 0x040001C3 RID: 451
	public float _landingSpeedModifier = 0.5f;

	// Token: 0x040001C4 RID: 452
	public float _landingTurnSpeedModifier = 5f;

	// Token: 0x040001C5 RID: 453
	public Transform _featherPS;

	// Token: 0x040001C6 RID: 454
	public Transform _thisT;

	// Token: 0x040001C7 RID: 455
	public int _activeLandingSpots;

	// Token: 0x040001C8 RID: 456
	public float _snapLandDistance = 0.1f;

	// Token: 0x040001C9 RID: 457
	public float _landedRotateSpeed = 0.01f;

	// Token: 0x040001CA RID: 458
	public float _gizmoSize = 0.2f;
}
