using System;
using ScheduleOne.DevUtilities;
using UnityEngine;

namespace ScheduleOne.PlayerScripts
{
	// Token: 0x020005FD RID: 1533
	public class ViewmodelSway : PlayerSingleton<ViewmodelSway>
	{
		// Token: 0x1700060A RID: 1546
		// (get) Token: 0x06002832 RID: 10290 RVA: 0x000A53A1 File Offset: 0x000A35A1
		protected float calculatedJumpJoltHeight
		{
			get
			{
				return this.jumpJoltHeight;
			}
		}

		// Token: 0x06002833 RID: 10291 RVA: 0x000A53A9 File Offset: 0x000A35A9
		protected override void Start()
		{
			base.Start();
		}

		// Token: 0x06002834 RID: 10292 RVA: 0x000A53B1 File Offset: 0x000A35B1
		protected override void Awake()
		{
			base.Awake();
			this.initialPos = base.transform.localPosition;
		}

		// Token: 0x06002835 RID: 10293 RVA: 0x000A53CC File Offset: 0x000A35CC
		public override void OnStartClient(bool IsOwner)
		{
			base.OnStartClient(IsOwner);
			this.timeSinceLanded = this.landJoltTime;
			PlayerMovement instance = PlayerSingleton<PlayerMovement>.Instance;
			instance.onJump = (Action)Delegate.Combine(instance.onJump, new Action(this.StartJump));
			PlayerMovement instance2 = PlayerSingleton<PlayerMovement>.Instance;
			instance2.onLand = (Action)Delegate.Combine(instance2.onLand, new Action(this.Land));
			PlayerInventory instance3 = PlayerSingleton<PlayerInventory>.Instance;
			instance3.onInventoryStateChanged = (Action<bool>)Delegate.Combine(instance3.onInventoryStateChanged, new Action<bool>(this.InventoryStateChanged));
		}

		// Token: 0x06002836 RID: 10294 RVA: 0x000A5460 File Offset: 0x000A3660
		protected void Update()
		{
			if (Time.timeScale == 0f)
			{
				return;
			}
			if (this.breatheBobbingEnabled)
			{
				this.BreatheBob();
			}
			if (this.swayingEnabled)
			{
				this.Sway();
			}
			if (this.walkBobbingEnabled)
			{
				this.WalkBob();
			}
			if (this.jumpJoltEnabled)
			{
				this.UpdateJump();
			}
			Vector3 vector = this.landPos;
			if (PlayerSingleton<PlayerInventory>.Instance.currentEquipTime < this.equipBopTime)
			{
				this.equipBopPos = new Vector3(0f, this.equipBopVerticalOffset * (1f - Mathf.Sqrt(Mathf.Clamp(PlayerSingleton<PlayerInventory>.Instance.currentEquipTime / this.equipBopTime, 0f, 1f))), 0f);
			}
			else
			{
				this.equipBopPos = Vector3.zero;
			}
			if (!PlayerSingleton<PlayerInventory>.Instance.HotbarEnabled)
			{
				this.equipBopPos = new Vector3(0f, this.equipBopVerticalOffset, 0f);
			}
			if (!PlayerSingleton<PlayerInventory>.Instance.isAnythingEquipped)
			{
				this.equipBopPos = Vector3.zero;
			}
			this.RefreshViewmodel();
		}

		// Token: 0x06002837 RID: 10295 RVA: 0x000A5563 File Offset: 0x000A3763
		private void InventoryStateChanged(bool active)
		{
			if (active)
			{
				this.Update();
			}
		}

		// Token: 0x06002838 RID: 10296 RVA: 0x000A5570 File Offset: 0x000A3770
		public void RefreshViewmodel()
		{
			try
			{
				base.transform.localPosition = this.swayPos + this.breatheBobPos + this.walkBobPos + this.jumpPos + this.landPos + this.fallOffsetPos + this.equipBopPos;
			}
			catch
			{
				Console.LogWarning("Viewmodel pos set failed.", null);
			}
		}

		// Token: 0x06002839 RID: 10297 RVA: 0x000A55F0 File Offset: 0x000A37F0
		protected void BreatheBob()
		{
			this.lastHeight = this.breatheBobPos.y + (Mathf.Sin(Time.timeSinceLevelLoad * this.breathingSpeedMultiplier) - this.lastHeight) * this.breathingHeightMultiplier;
			this.breatheBobPos = new Vector3(0f, this.lastHeight, 0f);
		}

		// Token: 0x0600283A RID: 10298 RVA: 0x000A564C File Offset: 0x000A384C
		protected void Sway()
		{
			float x = this.swayPos.x;
			float y = this.swayPos.y;
			float num = 0f;
			float num2 = 0f;
			if (PlayerSingleton<PlayerCamera>.Instance.canLook)
			{
				num = x - GameInput.MouseDelta.x * this.horizontalSwayMultiplier;
				num2 = y - GameInput.MouseDelta.y * this.verticalSwayMultiplier;
			}
			num = Mathf.Clamp(num, -this.maxHorizontal, this.maxHorizontal);
			num2 = Mathf.Clamp(num2, -this.maxVertical, this.maxVertical);
			Vector3 a = Vector3.Lerp(new Vector3(num, num2, 0f), Vector3.zero, Time.deltaTime * this.returnMultiplier / (1f + Mathf.Sqrt(Mathf.Abs(GameInput.MouseDelta.x) + Mathf.Abs(GameInput.MouseDelta.y))));
			this.swayPos = Vector3.Lerp(this.swayPos, a + this.initialPos, Time.deltaTime * this.swaySmooth);
		}

		// Token: 0x0600283B RID: 10299 RVA: 0x000A5754 File Offset: 0x000A3954
		protected void WalkBob()
		{
			bool flag = false;
			float d = Mathf.Abs(PlayerSingleton<PlayerMovement>.Instance.Movement.x) + Mathf.Abs(PlayerSingleton<PlayerMovement>.Instance.Movement.z);
			if (Mathf.Abs(PlayerSingleton<PlayerMovement>.Instance.Movement.x) > 0f || Mathf.Abs(PlayerSingleton<PlayerMovement>.Instance.Movement.z) > 0f)
			{
				flag = true;
			}
			if (!flag)
			{
				this.timeSinceWalkStart_vert = 0f;
				this.timeSinceWalkStart_horiz = 0f;
			}
			float num = 1f;
			if (PlayerSingleton<PlayerMovement>.Instance.isSprinting)
			{
				num = 1.4f;
			}
			this.walkBobPos = Vector3.Lerp(this.walkBobPos, new Vector3(this.horizontalMovement.Evaluate(this.timeSinceWalkStart_horiz % 1f) * this.horizontalBobWidth * num, this.verticalMovement.Evaluate(this.timeSinceWalkStart_vert % 1f) * this.verticalBobHeight * num, 0f) * d, Time.deltaTime * this.walkBobSmooth);
			if (flag)
			{
				float num2 = 1f;
				if (PlayerSingleton<PlayerMovement>.Instance.isSprinting)
				{
					num2 = 1.6f;
				}
				this.timeSinceWalkStart_vert += Time.deltaTime * this.verticalBobSpeed * num2;
				this.timeSinceWalkStart_horiz += Time.deltaTime * this.horizontalBobSpeed * num2;
			}
		}

		// Token: 0x0600283C RID: 10300 RVA: 0x000A58B4 File Offset: 0x000A3AB4
		protected void StartJump()
		{
			this.timeSinceJumpStart = 0f;
		}

		// Token: 0x0600283D RID: 10301 RVA: 0x000A58C4 File Offset: 0x000A3AC4
		protected void UpdateJump()
		{
			if (!PlayerSingleton<PlayerInventory>.Instance.isAnythingEquipped || !PlayerSingleton<PlayerInventory>.Instance.HotbarEnabled)
			{
				return;
			}
			if (PlayerSingleton<PlayerMovement>.Instance.airTime > 0f)
			{
				this.timeSinceJumpStart += Time.deltaTime;
				Vector3 b = new Vector3(0f, this.jumpCurve.Evaluate(Mathf.Clamp(this.timeSinceJumpStart / this.jumpJoltTime, 0f, 1f)) * this.calculatedJumpJoltHeight, 0f);
				this.jumpPos = Vector3.Lerp(this.jumpPos, b, Time.deltaTime * this.jumpJoltSmooth);
			}
			else if (PlayerSingleton<PlayerMovement>.Instance.IsGrounded)
			{
				this.timeSinceJumpStart = 0f;
				Vector3 b2 = new Vector3(0f, this.landCurve.Evaluate(Mathf.Clamp(this.timeSinceLanded / this.landJoltTime, 0f, 1f)) * this.landJoltMultiplier, 0f);
				if (this.landJoltMultiplier > 0f)
				{
					this.landPos = Vector3.Lerp(this.landPos, b2, Mathf.Abs(Time.deltaTime * this.landJoltSmooth / this.landJoltMultiplier));
				}
				else
				{
					this.landPos = Vector3.zero;
				}
				this.timeSinceLanded += Time.deltaTime;
				Vector3 zero = Vector3.zero;
				this.jumpPos = Vector3.Lerp(this.jumpPos, zero, Time.deltaTime * this.jumpJoltSmooth);
			}
			if (!PlayerSingleton<PlayerMovement>.Instance.IsGrounded && (this.timeSinceJumpStart > this.jumpJoltTime || PlayerSingleton<PlayerMovement>.Instance.airTime == 0f))
			{
				this.fallOffsetPos.y = this.fallOffsetPos.y + this.fallOffsetRate * Time.deltaTime;
				this.fallOffsetPos.y = Mathf.Clamp(this.fallOffsetPos.y, 0f, this.maxFallOffsetAmount);
				return;
			}
			this.fallOffsetPos.y = 0f;
		}

		// Token: 0x0600283E RID: 10302 RVA: 0x000A5AC0 File Offset: 0x000A3CC0
		protected void Land()
		{
			this.landJoltMultiplier = this.jumpPos.y + this.fallOffsetPos.y + this.landPos.y;
			this.landPos.y = this.landCurve.Evaluate(Mathf.Clamp(0f / this.landJoltTime, 0f, 1f)) * this.landJoltMultiplier;
			this.timeSinceLanded = 0f;
			this.jumpPos.y = 0f;
			this.fallOffsetPos.y = 0f;
		}

		// Token: 0x04001D4E RID: 7502
		[Header("Settings - Breathing")]
		public bool breatheBobbingEnabled = true;

		// Token: 0x04001D4F RID: 7503
		[Range(0f, 0.0004f)]
		[SerializeField]
		protected float breathingHeightMultiplier = 5E-05f;

		// Token: 0x04001D50 RID: 7504
		[Range(0f, 10f)]
		[SerializeField]
		protected float breathingSpeedMultiplier = 1f;

		// Token: 0x04001D51 RID: 7505
		private float lastHeight;

		// Token: 0x04001D52 RID: 7506
		private Vector3 breatheBobPos;

		// Token: 0x04001D53 RID: 7507
		[Header("Settings - Sway - Movement")]
		public bool swayingEnabled = true;

		// Token: 0x04001D54 RID: 7508
		[Range(0f, 0.1f)]
		[SerializeField]
		protected float horizontalSwayMultiplier = 1f;

		// Token: 0x04001D55 RID: 7509
		[Range(0f, 0.1f)]
		[SerializeField]
		protected float verticalSwayMultiplier = 1f;

		// Token: 0x04001D56 RID: 7510
		[Range(0f, 0.5f)]
		[SerializeField]
		protected float maxHorizontal = 0.1f;

		// Token: 0x04001D57 RID: 7511
		[Range(0f, 0.5f)]
		[SerializeField]
		protected float maxVertical = 0.1f;

		// Token: 0x04001D58 RID: 7512
		[SerializeField]
		protected float swaySmooth = 3f;

		// Token: 0x04001D59 RID: 7513
		[SerializeField]
		protected float returnMultiplier = 0.1f;

		// Token: 0x04001D5A RID: 7514
		private Vector3 initialPos = Vector3.zero;

		// Token: 0x04001D5B RID: 7515
		private Vector3 swayPos;

		// Token: 0x04001D5C RID: 7516
		[Header("Settings - Walk Bob")]
		public bool walkBobbingEnabled = true;

		// Token: 0x04001D5D RID: 7517
		[SerializeField]
		protected AnimationCurve verticalMovement;

		// Token: 0x04001D5E RID: 7518
		[SerializeField]
		protected AnimationCurve horizontalMovement;

		// Token: 0x04001D5F RID: 7519
		[Range(0f, 0.1f)]
		[SerializeField]
		protected float verticalBobHeight = 0.1f;

		// Token: 0x04001D60 RID: 7520
		[Range(0f, 5f)]
		[SerializeField]
		protected float verticalBobSpeed = 2f;

		// Token: 0x04001D61 RID: 7521
		[Range(0f, 0.1f)]
		[SerializeField]
		protected float horizontalBobWidth = 0.1f;

		// Token: 0x04001D62 RID: 7522
		[Range(0f, 5f)]
		[SerializeField]
		protected float horizontalBobSpeed = 2f;

		// Token: 0x04001D63 RID: 7523
		[SerializeField]
		protected float walkBobSmooth = 3f;

		// Token: 0x04001D64 RID: 7524
		[SerializeField]
		protected float sprintSpeedMultiplier = 1.25f;

		// Token: 0x04001D65 RID: 7525
		[HideInInspector]
		public float walkBobMultiplier = 1f;

		// Token: 0x04001D66 RID: 7526
		private Vector3 walkBobPos;

		// Token: 0x04001D67 RID: 7527
		private float timeSinceWalkStart_vert;

		// Token: 0x04001D68 RID: 7528
		private float timeSinceWalkStart_horiz;

		// Token: 0x04001D69 RID: 7529
		[Header("Settings - Jump Jolt")]
		public bool jumpJoltEnabled = true;

		// Token: 0x04001D6A RID: 7530
		[SerializeField]
		protected AnimationCurve jumpCurve;

		// Token: 0x04001D6B RID: 7531
		[SerializeField]
		protected float jumpJoltTime = 0.6f;

		// Token: 0x04001D6C RID: 7532
		[SerializeField]
		protected float jumpJoltHeight = 0.2f;

		// Token: 0x04001D6D RID: 7533
		[SerializeField]
		protected float jumpJoltSmooth = 5f;

		// Token: 0x04001D6E RID: 7534
		[Header("Settings - Equip Bop")]
		[SerializeField]
		protected float equipBopVerticalOffset = -0.5f;

		// Token: 0x04001D6F RID: 7535
		[SerializeField]
		protected float equipBopTime = 0.2f;

		// Token: 0x04001D70 RID: 7536
		private Vector3 equipBopPos;

		// Token: 0x04001D71 RID: 7537
		private float timeSinceJumpStart;

		// Token: 0x04001D72 RID: 7538
		private Vector3 jumpPos = Vector3.zero;

		// Token: 0x04001D73 RID: 7539
		[Header("Settings - Falling")]
		[Range(0f, 1f)]
		[SerializeField]
		protected float fallOffsetRate = 0.1f;

		// Token: 0x04001D74 RID: 7540
		[Range(0f, 2f)]
		[SerializeField]
		protected float maxFallOffsetAmount = 0.2f;

		// Token: 0x04001D75 RID: 7541
		private Vector3 fallOffsetPos = Vector3.zero;

		// Token: 0x04001D76 RID: 7542
		[Header("Settings - Land Jolt")]
		[SerializeField]
		protected AnimationCurve landCurve;

		// Token: 0x04001D77 RID: 7543
		[SerializeField]
		protected float landJoltTime = 0.6f;

		// Token: 0x04001D78 RID: 7544
		[SerializeField]
		protected float landJoltSmooth = 5f;

		// Token: 0x04001D79 RID: 7545
		private Vector3 landPos = Vector3.zero;

		// Token: 0x04001D7A RID: 7546
		private float timeSinceLanded;

		// Token: 0x04001D7B RID: 7547
		private float landJoltMultiplier = 1f;
	}
}
