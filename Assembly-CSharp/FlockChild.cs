using System;
using UnityEngine;

// Token: 0x02000046 RID: 70
public class FlockChild : MonoBehaviour
{
	// Token: 0x06000165 RID: 357 RVA: 0x00007FA4 File Offset: 0x000061A4
	public void Start()
	{
		this.FindRequiredComponents();
		this.Wander(0f);
		this.SetRandomScale();
		this._thisT.position = this.findWaypoint();
		this.RandomizeStartAnimationFrame();
		this.InitAvoidanceValues();
		this._speed = this._spawner._minSpeed;
		this._spawner._activeChildren += 1f;
		this._instantiated = true;
		if (this._spawner._updateDivisor > 1)
		{
			int num = this._spawner._updateDivisor - 1;
			FlockChild._updateNextSeed++;
			this._updateSeed = FlockChild._updateNextSeed;
			FlockChild._updateNextSeed %= num;
		}
	}

	// Token: 0x06000166 RID: 358 RVA: 0x00008053 File Offset: 0x00006253
	public void Update()
	{
		if (this._spawner._updateDivisor <= 1 || this._spawner._updateCounter == this._updateSeed)
		{
			this.SoarTimeLimit();
			this.CheckForDistanceToWaypoint();
			this.RotationBasedOnWaypointOrAvoidance();
			this.LimitRotationOfModel();
		}
	}

	// Token: 0x06000167 RID: 359 RVA: 0x0000808E File Offset: 0x0000628E
	public void OnDisable()
	{
		base.CancelInvoke();
		this._spawner._activeChildren -= 1f;
	}

	// Token: 0x06000168 RID: 360 RVA: 0x000080B0 File Offset: 0x000062B0
	public void OnEnable()
	{
		if (this._instantiated)
		{
			this._spawner._activeChildren += 1f;
			if (this._landing)
			{
				this._model.GetComponent<Animation>().Play(this._spawner._idleAnimation);
				return;
			}
			this._model.GetComponent<Animation>().Play(this._spawner._flapAnimation);
		}
	}

	// Token: 0x06000169 RID: 361 RVA: 0x00008120 File Offset: 0x00006320
	public void FindRequiredComponents()
	{
		if (this._thisT == null)
		{
			this._thisT = base.transform;
		}
		if (this._model == null)
		{
			this._model = this._thisT.Find("Model").gameObject;
		}
		if (this._modelT == null)
		{
			this._modelT = this._model.transform;
		}
	}

	// Token: 0x0600016A RID: 362 RVA: 0x00008190 File Offset: 0x00006390
	public void RandomizeStartAnimationFrame()
	{
		foreach (object obj in this._model.GetComponent<Animation>())
		{
			AnimationState animationState = (AnimationState)obj;
			animationState.time = UnityEngine.Random.value * animationState.length;
		}
	}

	// Token: 0x0600016B RID: 363 RVA: 0x000081FC File Offset: 0x000063FC
	public void InitAvoidanceValues()
	{
		this._avoidValue = UnityEngine.Random.Range(0.3f, 0.1f);
		if (this._spawner._birdAvoidDistanceMax != this._spawner._birdAvoidDistanceMin)
		{
			this._avoidDistance = UnityEngine.Random.Range(this._spawner._birdAvoidDistanceMax, this._spawner._birdAvoidDistanceMin);
			return;
		}
		this._avoidDistance = this._spawner._birdAvoidDistanceMin;
	}

	// Token: 0x0600016C RID: 364 RVA: 0x0000826C File Offset: 0x0000646C
	public void SetRandomScale()
	{
		float num = UnityEngine.Random.Range(this._spawner._minScale, this._spawner._maxScale);
		this._thisT.localScale = new Vector3(num, num, num);
	}

	// Token: 0x0600016D RID: 365 RVA: 0x000082A8 File Offset: 0x000064A8
	public void SoarTimeLimit()
	{
		if (this._soar && this._spawner._soarMaxTime > 0f)
		{
			if (this._soarTimer > this._spawner._soarMaxTime)
			{
				this.Flap();
				this._soarTimer = 0f;
				return;
			}
			this._soarTimer += this._spawner._newDelta;
		}
	}

	// Token: 0x0600016E RID: 366 RVA: 0x0000830C File Offset: 0x0000650C
	public void CheckForDistanceToWaypoint()
	{
		if (!this._landing && (this._thisT.position - this._wayPoint).magnitude < this._spawner._waypointDistance + this._stuckCounter)
		{
			this.Wander(0f);
			this._stuckCounter = 0f;
			return;
		}
		if (!this._landing)
		{
			this._stuckCounter += this._spawner._newDelta;
			return;
		}
		this._stuckCounter = 0f;
	}

	// Token: 0x0600016F RID: 367 RVA: 0x00008398 File Offset: 0x00006598
	public void RotationBasedOnWaypointOrAvoidance()
	{
		Vector3 vector = this._wayPoint - this._thisT.position;
		if (this._targetSpeed > -1f && vector != Vector3.zero)
		{
			Quaternion b = Quaternion.LookRotation(vector);
			this._thisT.rotation = Quaternion.Slerp(this._thisT.rotation, b, this._spawner._newDelta * this._damping);
		}
		if (this._spawner._childTriggerPos && (this._thisT.position - this._spawner._posBuffer).magnitude < 1f)
		{
			this._spawner.SetFlockRandomPosition();
		}
		this._speed = Mathf.Lerp(this._speed, this._targetSpeed, this._spawner._newDelta * 2.5f);
		if (this._move)
		{
			this._thisT.position += this._thisT.forward * this._speed * this._spawner._newDelta;
			if (this._avoid && this._spawner._birdAvoid)
			{
				this.Avoidance();
			}
		}
	}

	// Token: 0x06000170 RID: 368 RVA: 0x000084D8 File Offset: 0x000066D8
	public bool Avoidance()
	{
		RaycastHit raycastHit = default(RaycastHit);
		Vector3 forward = this._modelT.forward;
		bool result = false;
		Quaternion rotation = Quaternion.identity;
		Vector3 eulerAngles = Vector3.zero;
		Vector3 position = Vector3.zero;
		position = this._thisT.position;
		rotation = this._thisT.rotation;
		eulerAngles = this._thisT.rotation.eulerAngles;
		if (Physics.Raycast(this._thisT.position, forward + this._modelT.right * this._avoidValue, out raycastHit, this._avoidDistance, this._spawner._avoidanceMask))
		{
			eulerAngles.y -= (float)this._spawner._birdAvoidHorizontalForce * this._spawner._newDelta * this._damping;
			rotation.eulerAngles = eulerAngles;
			this._thisT.rotation = rotation;
			result = true;
		}
		else if (Physics.Raycast(this._thisT.position, forward + this._modelT.right * -this._avoidValue, out raycastHit, this._avoidDistance, this._spawner._avoidanceMask))
		{
			eulerAngles.y += (float)this._spawner._birdAvoidHorizontalForce * this._spawner._newDelta * this._damping;
			rotation.eulerAngles = eulerAngles;
			this._thisT.rotation = rotation;
			result = true;
		}
		if (this._spawner._birdAvoidDown && !this._landing && Physics.Raycast(this._thisT.position, -Vector3.up, out raycastHit, this._avoidDistance, this._spawner._avoidanceMask))
		{
			eulerAngles.x -= (float)this._spawner._birdAvoidVerticalForce * this._spawner._newDelta * this._damping;
			rotation.eulerAngles = eulerAngles;
			this._thisT.rotation = rotation;
			position.y += (float)this._spawner._birdAvoidVerticalForce * this._spawner._newDelta * 0.01f;
			this._thisT.position = position;
			result = true;
		}
		else if (this._spawner._birdAvoidUp && !this._landing && Physics.Raycast(this._thisT.position, Vector3.up, out raycastHit, this._avoidDistance, this._spawner._avoidanceMask))
		{
			eulerAngles.x += (float)this._spawner._birdAvoidVerticalForce * this._spawner._newDelta * this._damping;
			rotation.eulerAngles = eulerAngles;
			this._thisT.rotation = rotation;
			position.y -= (float)this._spawner._birdAvoidVerticalForce * this._spawner._newDelta * 0.01f;
			this._thisT.position = position;
			result = true;
		}
		return result;
	}

	// Token: 0x06000171 RID: 369 RVA: 0x000087E0 File Offset: 0x000069E0
	public void LimitRotationOfModel()
	{
		Quaternion localRotation = Quaternion.identity;
		Vector3 eulerAngles = Vector3.zero;
		localRotation = this._modelT.localRotation;
		eulerAngles = localRotation.eulerAngles;
		if ((((this._soar && this._spawner._flatSoar) || (this._spawner._flatFly && !this._soar)) && this._wayPoint.y > this._thisT.position.y) || this._landing)
		{
			eulerAngles.x = Mathf.LerpAngle(this._modelT.localEulerAngles.x, -this._thisT.localEulerAngles.x, this._spawner._newDelta * 1.75f);
			localRotation.eulerAngles = eulerAngles;
			this._modelT.localRotation = localRotation;
			return;
		}
		eulerAngles.x = Mathf.LerpAngle(this._modelT.localEulerAngles.x, 0f, this._spawner._newDelta * 1.75f);
		localRotation.eulerAngles = eulerAngles;
		this._modelT.localRotation = localRotation;
	}

	// Token: 0x06000172 RID: 370 RVA: 0x000088F8 File Offset: 0x00006AF8
	public void Wander(float delay)
	{
		if (!this._landing)
		{
			this._damping = UnityEngine.Random.Range(this._spawner._minDamping, this._spawner._maxDamping);
			this._targetSpeed = UnityEngine.Random.Range(this._spawner._minSpeed, this._spawner._maxSpeed);
			base.Invoke("SetRandomMode", delay);
		}
	}

	// Token: 0x06000173 RID: 371 RVA: 0x0000895C File Offset: 0x00006B5C
	public void SetRandomMode()
	{
		base.CancelInvoke("SetRandomMode");
		if (!this._dived && UnityEngine.Random.value < this._spawner._soarFrequency)
		{
			this.Soar();
			return;
		}
		if (!this._dived && UnityEngine.Random.value < this._spawner._diveFrequency)
		{
			this.Dive();
			return;
		}
		this.Flap();
	}

	// Token: 0x06000174 RID: 372 RVA: 0x000089BC File Offset: 0x00006BBC
	public void Flap()
	{
		if (this._move)
		{
			if (this._model != null)
			{
				this._model.GetComponent<Animation>().CrossFade(this._spawner._flapAnimation, 0.5f);
			}
			this._soar = false;
			this.animationSpeed();
			this._wayPoint = this.findWaypoint();
			this._dived = false;
		}
	}

	// Token: 0x06000175 RID: 373 RVA: 0x00008A20 File Offset: 0x00006C20
	public Vector3 findWaypoint()
	{
		Vector3 zero = Vector3.zero;
		zero.x = UnityEngine.Random.Range(-this._spawner._spawnSphere, this._spawner._spawnSphere) + this._spawner._posBuffer.x;
		zero.z = UnityEngine.Random.Range(-this._spawner._spawnSphereDepth, this._spawner._spawnSphereDepth) + this._spawner._posBuffer.z;
		zero.y = UnityEngine.Random.Range(-this._spawner._spawnSphereHeight, this._spawner._spawnSphereHeight) + this._spawner._posBuffer.y;
		return zero;
	}

	// Token: 0x06000176 RID: 374 RVA: 0x00008AD0 File Offset: 0x00006CD0
	public void Soar()
	{
		if (this._move)
		{
			this._model.GetComponent<Animation>().CrossFade(this._spawner._soarAnimation, 1.5f);
			this._wayPoint = this.findWaypoint();
			this._soar = true;
		}
	}

	// Token: 0x06000177 RID: 375 RVA: 0x00008B10 File Offset: 0x00006D10
	public void Dive()
	{
		if (this._spawner._soarAnimation != null)
		{
			this._model.GetComponent<Animation>().CrossFade(this._spawner._soarAnimation, 1.5f);
		}
		else
		{
			foreach (object obj in this._model.GetComponent<Animation>())
			{
				AnimationState animationState = (AnimationState)obj;
				if (this._thisT.position.y < this._wayPoint.y + 25f)
				{
					animationState.speed = 0.1f;
				}
			}
		}
		this._wayPoint = this.findWaypoint();
		this._wayPoint.y = this._wayPoint.y - this._spawner._diveValue;
		this._dived = true;
	}

	// Token: 0x06000178 RID: 376 RVA: 0x00008BF4 File Offset: 0x00006DF4
	public void animationSpeed()
	{
		foreach (object obj in this._model.GetComponent<Animation>())
		{
			AnimationState animationState = (AnimationState)obj;
			if (!this._dived && !this._landing)
			{
				animationState.speed = UnityEngine.Random.Range(this._spawner._minAnimationSpeed, this._spawner._maxAnimationSpeed);
			}
			else
			{
				animationState.speed = this._spawner._maxAnimationSpeed;
			}
		}
	}

	// Token: 0x0400014B RID: 331
	[HideInInspector]
	public FlockController _spawner;

	// Token: 0x0400014C RID: 332
	[HideInInspector]
	public Vector3 _wayPoint;

	// Token: 0x0400014D RID: 333
	public float _speed;

	// Token: 0x0400014E RID: 334
	[HideInInspector]
	public bool _dived = true;

	// Token: 0x0400014F RID: 335
	[HideInInspector]
	public float _stuckCounter;

	// Token: 0x04000150 RID: 336
	[HideInInspector]
	public float _damping;

	// Token: 0x04000151 RID: 337
	[HideInInspector]
	public bool _soar = true;

	// Token: 0x04000152 RID: 338
	[HideInInspector]
	public bool _landing;

	// Token: 0x04000153 RID: 339
	[HideInInspector]
	public float _targetSpeed;

	// Token: 0x04000154 RID: 340
	[HideInInspector]
	public bool _move = true;

	// Token: 0x04000155 RID: 341
	public GameObject _model;

	// Token: 0x04000156 RID: 342
	public Transform _modelT;

	// Token: 0x04000157 RID: 343
	[HideInInspector]
	public float _avoidValue;

	// Token: 0x04000158 RID: 344
	[HideInInspector]
	public float _avoidDistance;

	// Token: 0x04000159 RID: 345
	private float _soarTimer;

	// Token: 0x0400015A RID: 346
	private bool _instantiated;

	// Token: 0x0400015B RID: 347
	private static int _updateNextSeed;

	// Token: 0x0400015C RID: 348
	private int _updateSeed = -1;

	// Token: 0x0400015D RID: 349
	[HideInInspector]
	public bool _avoid = true;

	// Token: 0x0400015E RID: 350
	public Transform _thisT;

	// Token: 0x0400015F RID: 351
	public Vector3 _landingPosOffset;
}
