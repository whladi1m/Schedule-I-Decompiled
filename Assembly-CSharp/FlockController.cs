using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000048 RID: 72
public class FlockController : MonoBehaviour
{
	// Token: 0x0600017E RID: 382 RVA: 0x00008EC4 File Offset: 0x000070C4
	public void Start()
	{
		this._thisT = base.transform;
		if (this._positionSphereDepth == -1f)
		{
			this._positionSphereDepth = this._positionSphere;
		}
		if (this._spawnSphereDepth == -1f)
		{
			this._spawnSphereDepth = this._spawnSphere;
		}
		this._posBuffer = this._thisT.position + this._startPosOffset;
		if (!this._slowSpawn)
		{
			this.AddChild(this._childAmount);
		}
		if (this._randomPositionTimer > 0f)
		{
			base.InvokeRepeating("SetFlockRandomPosition", this._randomPositionTimer, this._randomPositionTimer);
		}
	}

	// Token: 0x0600017F RID: 383 RVA: 0x00008F64 File Offset: 0x00007164
	public void AddChild(int amount)
	{
		if (this._groupChildToNewTransform)
		{
			this.InstantiateGroup();
		}
		for (int i = 0; i < amount; i++)
		{
			FlockChild flockChild = UnityEngine.Object.Instantiate<FlockChild>(this._childPrefab);
			flockChild._spawner = this;
			this._roamers.Add(flockChild);
			this.AddChildToParent(flockChild.transform);
		}
	}

	// Token: 0x06000180 RID: 384 RVA: 0x00008FB6 File Offset: 0x000071B6
	public void AddChildToParent(Transform obj)
	{
		if (this._groupChildToFlock)
		{
			obj.parent = base.transform;
			return;
		}
		if (this._groupChildToNewTransform)
		{
			obj.parent = this._groupTransform;
			return;
		}
	}

	// Token: 0x06000181 RID: 385 RVA: 0x00008FE4 File Offset: 0x000071E4
	public void RemoveChild(int amount)
	{
		for (int i = 0; i < amount; i++)
		{
			Component component = this._roamers[this._roamers.Count - 1];
			this._roamers.RemoveAt(this._roamers.Count - 1);
			UnityEngine.Object.Destroy(component.gameObject);
		}
	}

	// Token: 0x06000182 RID: 386 RVA: 0x00009038 File Offset: 0x00007238
	public void Update()
	{
		if (this._activeChildren > 0f)
		{
			if (this._updateDivisor > 1)
			{
				this._updateCounter++;
				this._updateCounter %= this._updateDivisor;
				this._newDelta = Time.deltaTime * (float)this._updateDivisor;
			}
			else
			{
				this._newDelta = Time.deltaTime;
			}
		}
		this.UpdateChildAmount();
	}

	// Token: 0x06000183 RID: 387 RVA: 0x000090A4 File Offset: 0x000072A4
	public void InstantiateGroup()
	{
		if (this._groupTransform != null)
		{
			return;
		}
		GameObject gameObject = new GameObject();
		this._groupTransform = gameObject.transform;
		this._groupTransform.position = this._thisT.position;
		if (this._groupName != "")
		{
			gameObject.name = this._groupName;
			return;
		}
		gameObject.name = this._thisT.name + " Fish Container";
	}

	// Token: 0x06000184 RID: 388 RVA: 0x00009122 File Offset: 0x00007322
	public void UpdateChildAmount()
	{
		if (this._childAmount >= 0 && this._childAmount < this._roamers.Count)
		{
			this.RemoveChild(1);
			return;
		}
		if (this._childAmount > this._roamers.Count)
		{
			this.AddChild(1);
		}
	}

	// Token: 0x06000185 RID: 389 RVA: 0x00009164 File Offset: 0x00007364
	public void OnDrawGizmos()
	{
		if (this._thisT == null)
		{
			this._thisT = base.transform;
		}
		if (!Application.isPlaying && this._posBuffer != this._thisT.position + this._startPosOffset)
		{
			this._posBuffer = this._thisT.position + this._startPosOffset;
		}
		if (this._positionSphereDepth == -1f)
		{
			this._positionSphereDepth = this._positionSphere;
		}
		if (this._spawnSphereDepth == -1f)
		{
			this._spawnSphereDepth = this._spawnSphere;
		}
		Gizmos.color = Color.blue;
		Gizmos.DrawWireCube(this._posBuffer, new Vector3(this._spawnSphere * 2f, this._spawnSphereHeight * 2f, this._spawnSphereDepth * 2f));
		Gizmos.color = Color.cyan;
		Gizmos.DrawWireCube(this._thisT.position, new Vector3(this._positionSphere * 2f + this._spawnSphere * 2f, this._positionSphereHeight * 2f + this._spawnSphereHeight * 2f, this._positionSphereDepth * 2f + this._spawnSphereDepth * 2f));
	}

	// Token: 0x06000186 RID: 390 RVA: 0x000092AC File Offset: 0x000074AC
	public void SetFlockRandomPosition()
	{
		Vector3 zero = Vector3.zero;
		zero.x = UnityEngine.Random.Range(-this._positionSphere, this._positionSphere) + this._thisT.position.x;
		zero.z = UnityEngine.Random.Range(-this._positionSphereDepth, this._positionSphereDepth) + this._thisT.position.z;
		zero.y = UnityEngine.Random.Range(-this._positionSphereHeight, this._positionSphereHeight) + this._thisT.position.y;
		this._posBuffer = zero;
		if (this._forceChildWaypoints)
		{
			for (int i = 0; i < this._roamers.Count; i++)
			{
				this._roamers[i].Wander(UnityEngine.Random.value * this._forcedRandomDelay);
			}
		}
	}

	// Token: 0x06000187 RID: 391 RVA: 0x00009380 File Offset: 0x00007580
	public void destroyBirds()
	{
		for (int i = 0; i < this._roamers.Count; i++)
		{
			UnityEngine.Object.Destroy(this._roamers[i].gameObject);
		}
		this._childAmount = 0;
		this._roamers.Clear();
	}

	// Token: 0x0400016D RID: 365
	public FlockChild _childPrefab;

	// Token: 0x0400016E RID: 366
	public int _childAmount = 250;

	// Token: 0x0400016F RID: 367
	public bool _slowSpawn;

	// Token: 0x04000170 RID: 368
	public float _spawnSphere = 3f;

	// Token: 0x04000171 RID: 369
	public float _spawnSphereHeight = 3f;

	// Token: 0x04000172 RID: 370
	public float _spawnSphereDepth = -1f;

	// Token: 0x04000173 RID: 371
	public float _minSpeed = 6f;

	// Token: 0x04000174 RID: 372
	public float _maxSpeed = 10f;

	// Token: 0x04000175 RID: 373
	public float _minScale = 0.7f;

	// Token: 0x04000176 RID: 374
	public float _maxScale = 1f;

	// Token: 0x04000177 RID: 375
	public float _soarFrequency;

	// Token: 0x04000178 RID: 376
	public string _soarAnimation = "Soar";

	// Token: 0x04000179 RID: 377
	public string _flapAnimation = "Flap";

	// Token: 0x0400017A RID: 378
	public string _idleAnimation = "Idle";

	// Token: 0x0400017B RID: 379
	public float _diveValue = 7f;

	// Token: 0x0400017C RID: 380
	public float _diveFrequency = 0.5f;

	// Token: 0x0400017D RID: 381
	public float _minDamping = 1f;

	// Token: 0x0400017E RID: 382
	public float _maxDamping = 2f;

	// Token: 0x0400017F RID: 383
	public float _waypointDistance = 1f;

	// Token: 0x04000180 RID: 384
	public float _minAnimationSpeed = 2f;

	// Token: 0x04000181 RID: 385
	public float _maxAnimationSpeed = 4f;

	// Token: 0x04000182 RID: 386
	public float _randomPositionTimer = 10f;

	// Token: 0x04000183 RID: 387
	public float _positionSphere = 25f;

	// Token: 0x04000184 RID: 388
	public float _positionSphereHeight = 25f;

	// Token: 0x04000185 RID: 389
	public float _positionSphereDepth = -1f;

	// Token: 0x04000186 RID: 390
	public bool _childTriggerPos;

	// Token: 0x04000187 RID: 391
	public bool _forceChildWaypoints;

	// Token: 0x04000188 RID: 392
	public float _forcedRandomDelay = 1.5f;

	// Token: 0x04000189 RID: 393
	public bool _flatFly;

	// Token: 0x0400018A RID: 394
	public bool _flatSoar;

	// Token: 0x0400018B RID: 395
	public bool _birdAvoid;

	// Token: 0x0400018C RID: 396
	public int _birdAvoidHorizontalForce = 1000;

	// Token: 0x0400018D RID: 397
	public bool _birdAvoidDown;

	// Token: 0x0400018E RID: 398
	public bool _birdAvoidUp;

	// Token: 0x0400018F RID: 399
	public int _birdAvoidVerticalForce = 300;

	// Token: 0x04000190 RID: 400
	public float _birdAvoidDistanceMax = 4.5f;

	// Token: 0x04000191 RID: 401
	public float _birdAvoidDistanceMin = 5f;

	// Token: 0x04000192 RID: 402
	public float _soarMaxTime;

	// Token: 0x04000193 RID: 403
	public LayerMask _avoidanceMask = -1;

	// Token: 0x04000194 RID: 404
	public List<FlockChild> _roamers;

	// Token: 0x04000195 RID: 405
	public Vector3 _posBuffer;

	// Token: 0x04000196 RID: 406
	public int _updateDivisor = 1;

	// Token: 0x04000197 RID: 407
	public float _newDelta;

	// Token: 0x04000198 RID: 408
	public int _updateCounter;

	// Token: 0x04000199 RID: 409
	public float _activeChildren;

	// Token: 0x0400019A RID: 410
	public bool _groupChildToNewTransform;

	// Token: 0x0400019B RID: 411
	public Transform _groupTransform;

	// Token: 0x0400019C RID: 412
	public string _groupName = "";

	// Token: 0x0400019D RID: 413
	public bool _groupChildToFlock;

	// Token: 0x0400019E RID: 414
	public Vector3 _startPosOffset;

	// Token: 0x0400019F RID: 415
	public Transform _thisT;
}
