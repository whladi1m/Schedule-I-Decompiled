using System;
using System.Collections.Generic;
using ScheduleOne.DevUtilities;
using ScheduleOne.GameTime;
using ScheduleOne.NPCs;
using ScheduleOne.PlayerScripts;
using ScheduleOne.Skating;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.AvatarFramework.Animation
{
	// Token: 0x02000974 RID: 2420
	public class AvatarAnimation : MonoBehaviour
	{
		// Token: 0x17000953 RID: 2387
		// (get) Token: 0x060041AF RID: 16815 RVA: 0x00113274 File Offset: 0x00111474
		// (set) Token: 0x060041B0 RID: 16816 RVA: 0x0011327C File Offset: 0x0011147C
		public bool IsCrouched { get; protected set; }

		// Token: 0x17000954 RID: 2388
		// (get) Token: 0x060041B1 RID: 16817 RVA: 0x00113285 File Offset: 0x00111485
		public bool IsSeated
		{
			get
			{
				return this.CurrentSeat != null;
			}
		}

		// Token: 0x17000955 RID: 2389
		// (get) Token: 0x060041B2 RID: 16818 RVA: 0x00113293 File Offset: 0x00111493
		// (set) Token: 0x060041B3 RID: 16819 RVA: 0x0011329B File Offset: 0x0011149B
		public float TimeSinceSitEnd { get; protected set; } = 1000f;

		// Token: 0x17000956 RID: 2390
		// (get) Token: 0x060041B4 RID: 16820 RVA: 0x001132A4 File Offset: 0x001114A4
		// (set) Token: 0x060041B5 RID: 16821 RVA: 0x001132AC File Offset: 0x001114AC
		public AvatarSeat CurrentSeat { get; protected set; }

		// Token: 0x17000957 RID: 2391
		// (get) Token: 0x060041B6 RID: 16822 RVA: 0x001132B5 File Offset: 0x001114B5
		// (set) Token: 0x060041B7 RID: 16823 RVA: 0x001132BD File Offset: 0x001114BD
		public bool StandUpAnimationPlaying { get; protected set; }

		// Token: 0x17000958 RID: 2392
		// (get) Token: 0x060041B8 RID: 16824 RVA: 0x001132C6 File Offset: 0x001114C6
		// (set) Token: 0x060041B9 RID: 16825 RVA: 0x001132CE File Offset: 0x001114CE
		public bool IsAvatarCulled { get; private set; }

		// Token: 0x060041BA RID: 16826 RVA: 0x001132D8 File Offset: 0x001114D8
		protected virtual void Awake()
		{
			this.initialCullingMode = this.animator.cullingMode;
			this.animator.cullingMode = AnimatorCullingMode.AlwaysAnimate;
			this.avatar = base.GetComponent<Avatar>();
			this.avatar.onRagdollChange.AddListener(new UnityAction<bool, bool, bool>(this.RagdollChange));
			this.standUpFromBackBoneTransforms = new BoneTransform[this.Bones.Length];
			this.standUpFromFrontBoneTransforms = new BoneTransform[this.Bones.Length];
			this.ragdollBoneTransforms = new BoneTransform[this.Bones.Length];
			this.standingBoneTransforms = new BoneTransform[this.Bones.Length];
			for (int i = 0; i < this.Bones.Length; i++)
			{
				this.standUpFromBackBoneTransforms[i] = new BoneTransform();
				this.standUpFromFrontBoneTransforms[i] = new BoneTransform();
				this.ragdollBoneTransforms[i] = new BoneTransform();
				this.standingBoneTransforms[i] = new BoneTransform();
			}
			this.PopulateBoneTransforms(this.standingBoneTransforms);
			base.InvokeRepeating("InfrequentUpdate", UnityEngine.Random.Range(0f, 0.5f), 0.1f);
		}

		// Token: 0x060041BB RID: 16827 RVA: 0x001133E8 File Offset: 0x001115E8
		protected virtual void Start()
		{
			this.PopulateAnimationStartBoneTransforms(this.StandUpFromFrontClipName, this.standUpFromFrontBoneTransforms);
			this.PopulateAnimationStartBoneTransforms(this.StandUpFromBackClipName, this.standUpFromBackBoneTransforms);
			if (NetworkSingleton<TimeManager>.InstanceExists)
			{
				TimeManager instance = NetworkSingleton<TimeManager>.Instance;
				instance.onMinutePass = (Action)Delegate.Remove(instance.onMinutePass, new Action(this.MinPass));
				TimeManager instance2 = NetworkSingleton<TimeManager>.Instance;
				instance2.onMinutePass = (Action)Delegate.Combine(instance2.onMinutePass, new Action(this.MinPass));
			}
			Player componentInParent = base.GetComponentInParent<Player>();
			if (componentInParent != null)
			{
				Player player = componentInParent;
				player.onSkateboardMounted = (Action<Skateboard>)Delegate.Combine(player.onSkateboardMounted, new Action<Skateboard>(this.SkateboardMounted));
				Player player2 = componentInParent;
				player2.onSkateboardDismounted = (Action)Delegate.Combine(player2.onSkateboardDismounted, new Action(this.SkateboardDismounted));
			}
			this.framesActive = 0;
		}

		// Token: 0x060041BC RID: 16828 RVA: 0x001134C7 File Offset: 0x001116C7
		private void OnDestroy()
		{
			if (NetworkSingleton<TimeManager>.InstanceExists)
			{
				TimeManager instance = NetworkSingleton<TimeManager>.Instance;
				instance.onMinutePass = (Action)Delegate.Remove(instance.onMinutePass, new Action(this.MinPass));
			}
		}

		// Token: 0x060041BD RID: 16829 RVA: 0x001134F6 File Offset: 0x001116F6
		private void OnEnable()
		{
			this.framesActive = 0;
		}

		// Token: 0x060041BE RID: 16830 RVA: 0x00113500 File Offset: 0x00111700
		private void Update()
		{
			if (this.IsSeated)
			{
				this.TimeSinceSitEnd = 0f;
			}
			else
			{
				this.TimeSinceSitEnd += Time.deltaTime;
			}
			if (this.seatRoutine == null && this.CurrentSeat != null)
			{
				base.transform.position = this.CurrentSeat.SittingPoint.position + AvatarAnimation.SITTING_OFFSET * base.transform.localScale.y;
				base.transform.rotation = this.CurrentSeat.SittingPoint.rotation;
			}
			if (base.gameObject.activeInHierarchy)
			{
				this.framesActive++;
			}
		}

		// Token: 0x060041BF RID: 16831 RVA: 0x001135BA File Offset: 0x001117BA
		private void InfrequentUpdate()
		{
			this.UpdateAnimationActive(false);
		}

		// Token: 0x060041C0 RID: 16832 RVA: 0x001135C4 File Offset: 0x001117C4
		private void MinPass()
		{
			if (this == null || this.animator == null)
			{
				return;
			}
			if (Time.timeSinceLevelLoad > 3f && this.animator.cullingMode != this.initialCullingMode)
			{
				this.animator.cullingMode = this.initialCullingMode;
			}
		}

		// Token: 0x060041C1 RID: 16833 RVA: 0x0011361C File Offset: 0x0011181C
		private void UpdateAnimationActive(bool forceWriteIdle = false)
		{
			if (!PlayerSingleton<PlayerCamera>.InstanceExists)
			{
				return;
			}
			float num = Vector3.SqrMagnitude(PlayerSingleton<PlayerCamera>.Instance.transform.position - base.transform.position);
			bool flag = num < 1600f * QualitySettings.lodBias;
			if (flag && num > 225f)
			{
				flag = (Vector3.Dot(PlayerSingleton<PlayerCamera>.Instance.transform.forward, base.transform.position - PlayerSingleton<PlayerCamera>.Instance.transform.position) > 0f);
			}
			if (Time.timeSinceLevelLoad < 3f)
			{
				flag = true;
			}
			if (!this.AllowCulling)
			{
				flag = true;
			}
			bool isAvatarCulled = this.IsAvatarCulled;
			this.IsAvatarCulled = false;
			if (this.avatar.UseImpostor && this.UseImpostor)
			{
				if (!flag)
				{
					this.IsAvatarCulled = true;
				}
				if (!flag && !this.avatar.Impostor.gameObject.activeSelf)
				{
					this.avatar.BodyContainer.gameObject.SetActive(false);
					this.avatar.Impostor.EnableImpostor();
					return;
				}
				if (flag && this.avatar.Impostor.gameObject.activeSelf)
				{
					this.avatar.BodyContainer.gameObject.SetActive(true);
					this.avatar.Impostor.DisableImpostor();
				}
			}
			this.animator.enabled = (this.animationEnabled && flag);
			if (!this.IsAvatarCulled)
			{
				this.animator.SetBool("Sitting", this.IsSeated);
				if (isAvatarCulled && this.avatar.CurrentEquippable != null)
				{
					this.avatar.CurrentEquippable.InitializeAnimation();
				}
			}
		}

		// Token: 0x060041C2 RID: 16834 RVA: 0x001137CE File Offset: 0x001119CE
		public void SetDirection(float dir)
		{
			this.animator.SetFloat("Direction", dir);
		}

		// Token: 0x060041C3 RID: 16835 RVA: 0x001137E1 File Offset: 0x001119E1
		public void SetStrafe(float strafe)
		{
			this.animator.SetFloat("Strafe", strafe);
		}

		// Token: 0x060041C4 RID: 16836 RVA: 0x001137F4 File Offset: 0x001119F4
		public void SetTimeAirborne(float airbone)
		{
			this.animator.SetFloat("TimeAirborne", airbone);
		}

		// Token: 0x060041C5 RID: 16837 RVA: 0x00113807 File Offset: 0x00111A07
		public void SetCrouched(bool crouched)
		{
			this.IsCrouched = crouched;
			this.animator.SetBool("isCrouched", crouched);
		}

		// Token: 0x060041C6 RID: 16838 RVA: 0x00113821 File Offset: 0x00111A21
		public void SetGrounded(bool grounded)
		{
			this.animator.SetBool("isGrounded", grounded);
		}

		// Token: 0x060041C7 RID: 16839 RVA: 0x00113834 File Offset: 0x00111A34
		public void Jump()
		{
			this.animator.SetTrigger("Jump");
		}

		// Token: 0x060041C8 RID: 16840 RVA: 0x00113846 File Offset: 0x00111A46
		public void SetAnimationEnabled(bool enabled)
		{
			this.animationEnabled = enabled;
			this.UpdateAnimationActive(false);
		}

		// Token: 0x060041C9 RID: 16841 RVA: 0x00113858 File Offset: 0x00111A58
		public void Flinch(Vector3 forceDirection, AvatarAnimation.EFlinchType flinchType)
		{
			Vector3 vector = base.transform.InverseTransformDirection(forceDirection);
			AvatarAnimation.EFlinchDirection eflinchDirection;
			if (Mathf.Abs(vector.z) > Mathf.Abs(vector.x))
			{
				if (vector.z > 0f)
				{
					eflinchDirection = AvatarAnimation.EFlinchDirection.Forward;
				}
				else
				{
					eflinchDirection = AvatarAnimation.EFlinchDirection.Backward;
				}
			}
			else if (vector.x > 0f)
			{
				eflinchDirection = AvatarAnimation.EFlinchDirection.Right;
			}
			else
			{
				eflinchDirection = AvatarAnimation.EFlinchDirection.Left;
			}
			if (flinchType != AvatarAnimation.EFlinchType.Light)
			{
				switch (eflinchDirection)
				{
				case AvatarAnimation.EFlinchDirection.Forward:
					this.animator.SetTrigger("Flinch_Heavy_Forward");
					break;
				case AvatarAnimation.EFlinchDirection.Backward:
					this.animator.SetTrigger("Flinch_Heavy_Backward");
					break;
				case AvatarAnimation.EFlinchDirection.Left:
					this.animator.SetTrigger("Flinch_Heavy_Left");
					break;
				case AvatarAnimation.EFlinchDirection.Right:
					this.animator.SetTrigger("Flinch_Heavy_Right");
					break;
				}
				if (this.onHeavyFlinch != null)
				{
					this.onHeavyFlinch.Invoke();
				}
				return;
			}
			switch (eflinchDirection)
			{
			case AvatarAnimation.EFlinchDirection.Forward:
				this.animator.SetTrigger("Flinch_Forward");
				return;
			case AvatarAnimation.EFlinchDirection.Backward:
				this.animator.SetTrigger("Flinch_Backward");
				return;
			case AvatarAnimation.EFlinchDirection.Left:
				this.animator.SetTrigger("Flinch_Left");
				return;
			case AvatarAnimation.EFlinchDirection.Right:
				this.animator.SetTrigger("Flinch_Right");
				return;
			default:
				return;
			}
		}

		// Token: 0x060041CA RID: 16842 RVA: 0x00113984 File Offset: 0x00111B84
		public void PlayStandUpAnimation()
		{
			AvatarAnimation.<>c__DisplayClass75_0 CS$<>8__locals1 = new AvatarAnimation.<>c__DisplayClass75_0();
			CS$<>8__locals1.<>4__this = this;
			this.StandUpAnimationPlaying = true;
			if (this.onStandupStart != null)
			{
				this.onStandupStart.Invoke();
			}
			this.PopulateBoneTransforms(this.ragdollBoneTransforms);
			CS$<>8__locals1.standUpFromBack = this.ShouldGetUpFromBack();
			this.PopulateAnimationStartBoneTransforms(this.StandUpFromFrontClipName, this.standUpFromFrontBoneTransforms);
			this.PopulateAnimationStartBoneTransforms(this.StandUpFromBackClipName, this.standUpFromBackBoneTransforms);
			CS$<>8__locals1.finalBoneTransforms = (CS$<>8__locals1.standUpFromBack ? this.standUpFromBackBoneTransforms : this.standUpFromFrontBoneTransforms);
			if (this.standUpRoutine != null)
			{
				base.StopCoroutine(this.standUpRoutine);
			}
			this.standUpRoutine = Singleton<CoroutineService>.Instance.StartCoroutine(CS$<>8__locals1.<PlayStandUpAnimation>g__StandUpRoutine|0());
		}

		// Token: 0x060041CB RID: 16843 RVA: 0x00113A3C File Offset: 0x00111C3C
		protected void RagdollChange(bool oldValue, bool ragdoll, bool playStandUpAnim)
		{
			bool flag = oldValue && !ragdoll && playStandUpAnim;
			if (ragdoll && this.IsSeated)
			{
				if (this.CurrentSeat != null)
				{
					this.CurrentSeat.SetOccupant(null);
					this.CurrentSeat = null;
				}
				this.animator.SetBool("Sitting", false);
				base.GetComponentInParent<NPCMovement>().SpeedController.RemoveSpeedControl("seated");
			}
			if (ragdoll && this.standUpRoutine != null)
			{
				base.StopCoroutine(this.standUpRoutine);
			}
			if (oldValue && !ragdoll)
			{
				this.AlignPositionToHips();
			}
			if (!flag)
			{
				this.SetAnimationEnabled(!ragdoll);
				return;
			}
			this.PlayStandUpAnimation();
		}

		// Token: 0x060041CC RID: 16844 RVA: 0x00113AE0 File Offset: 0x00111CE0
		private void AlignPositionToHips()
		{
			Vector3 position = this.HipBone.position;
			Quaternion rotation = this.HipBone.rotation;
			base.transform.position = this.HipBone.position;
			RaycastHit raycastHit;
			if (Physics.Raycast(base.transform.position, Vector3.down, out raycastHit, 10f, this.GroundingMask))
			{
				base.transform.position = new Vector3(base.transform.position.x, raycastHit.point.y, base.transform.position.z);
			}
			base.transform.rotation = Quaternion.LookRotation(Vector3.ProjectOnPlane(this.ShouldGetUpFromBack() ? (-this.HipBone.up) : this.HipBone.up, Vector3.up), Vector3.up);
			this.HipBone.position = position;
			this.HipBone.rotation = rotation;
		}

		// Token: 0x060041CD RID: 16845 RVA: 0x00113BDC File Offset: 0x00111DDC
		private bool ShouldGetUpFromBack()
		{
			return Vector3.Angle(this.HipBone.forward, Vector3.up) < 90f;
		}

		// Token: 0x060041CE RID: 16846 RVA: 0x00113BFC File Offset: 0x00111DFC
		private void PopulateBoneTransforms(BoneTransform[] boneTransforms)
		{
			for (int i = 0; i < this.Bones.Length; i++)
			{
				boneTransforms[i].Position = this.Bones[i].localPosition;
				boneTransforms[i].Rotation = this.Bones[i].localRotation;
			}
		}

		// Token: 0x060041CF RID: 16847 RVA: 0x00113C48 File Offset: 0x00111E48
		private List<Pose> GetBoneTransforms()
		{
			List<Pose> list = new List<Pose>();
			for (int i = 0; i < this.Bones.Length; i++)
			{
				list.Add(new Pose(this.Bones[i].localPosition, this.Bones[i].localRotation));
			}
			return list;
		}

		// Token: 0x060041D0 RID: 16848 RVA: 0x00113C94 File Offset: 0x00111E94
		private void PopulateAnimationStartBoneTransforms(string clipName, BoneTransform[] boneTransforms)
		{
			Vector3 position = this.animator.transform.position;
			Quaternion rotation = this.animator.transform.rotation;
			if (this.animator.runtimeAnimatorController == null)
			{
				return;
			}
			foreach (AnimationClip animationClip in this.animator.runtimeAnimatorController.animationClips)
			{
				if (animationClip.name == clipName)
				{
					animationClip.SampleAnimation(this.animator.gameObject, 0f);
					this.PopulateBoneTransforms(boneTransforms);
					break;
				}
			}
			this.animator.transform.position = position;
			this.animator.transform.rotation = rotation;
		}

		// Token: 0x060041D1 RID: 16849 RVA: 0x00113D4C File Offset: 0x00111F4C
		public void SetTrigger(string trigger)
		{
			if (string.IsNullOrEmpty(trigger))
			{
				return;
			}
			this.animator.SetTrigger(trigger);
			this.UpdateAnimationActive(true);
		}

		// Token: 0x060041D2 RID: 16850 RVA: 0x00113D6A File Offset: 0x00111F6A
		public void ResetTrigger(string trigger)
		{
			this.animator.ResetTrigger(trigger);
		}

		// Token: 0x060041D3 RID: 16851 RVA: 0x00113D78 File Offset: 0x00111F78
		public void SetBool(string id, bool value)
		{
			this.animator.SetBool(id, value);
			this.UpdateAnimationActive(true);
		}

		// Token: 0x060041D4 RID: 16852 RVA: 0x00113D90 File Offset: 0x00111F90
		public void SetSeat(AvatarSeat seat)
		{
			AvatarAnimation.<>c__DisplayClass85_0 CS$<>8__locals1 = new AvatarAnimation.<>c__DisplayClass85_0();
			CS$<>8__locals1.<>4__this = this;
			if (seat == this.CurrentSeat)
			{
				return;
			}
			if (this.CurrentSeat != null)
			{
				this.CurrentSeat.SetOccupant(null);
			}
			this.CurrentSeat = seat;
			if (this.CurrentSeat != null)
			{
				this.CurrentSeat.SetOccupant(base.GetComponentInParent<NPC>());
			}
			this.animator.SetBool("Sitting", this.IsSeated);
			CS$<>8__locals1.startPos = base.transform.position;
			CS$<>8__locals1.startRot = base.transform.rotation;
			if (this.seatRoutine != null)
			{
				Singleton<CoroutineService>.Instance.StopCoroutine(this.seatRoutine);
			}
			if (this.CurrentSeat != null)
			{
				CS$<>8__locals1.endPos = this.CurrentSeat.SittingPoint.position + AvatarAnimation.SITTING_OFFSET * base.transform.localScale.y;
				CS$<>8__locals1.endRot = this.CurrentSeat.SittingPoint.rotation;
				this.seatRoutine = Singleton<CoroutineService>.Instance.StartCoroutine(CS$<>8__locals1.<SetSeat>g__Lerp|0(false));
				base.GetComponentInParent<NPCMovement>().SpeedController.AddSpeedControl(new NPCSpeedController.SpeedControl("seated", 100, -1f));
				return;
			}
			CS$<>8__locals1.endPos = base.transform.parent.position;
			CS$<>8__locals1.endRot = base.transform.parent.rotation;
			this.seatRoutine = Singleton<CoroutineService>.Instance.StartCoroutine(CS$<>8__locals1.<SetSeat>g__Lerp|0(true));
		}

		// Token: 0x060041D5 RID: 16853 RVA: 0x00113F24 File Offset: 0x00112124
		public void SkateboardMounted(Skateboard board)
		{
			this.IKController.BodyIK.solvers.pelvis.target = board.Animation.PelvisAlignment.Transform;
			this.IKController.BodyIK.solvers.spine.target = board.Animation.SpineAlignment.Transform;
			this.IKController.BodyIK.solvers.leftFoot.target = board.Animation.LeftFootAlignment.Transform;
			this.IKController.BodyIK.solvers.rightFoot.target = board.Animation.RightFootAlignment.Transform;
			this.IKController.BodyIK.solvers.leftHand.target = board.Animation.LeftHandAlignment.Transform;
			this.IKController.BodyIK.solvers.rightHand.target = board.Animation.RightHandAlignment.Transform;
			this.IKController.BodyIK.solvers.rightFoot.SetBendPlaneToCurrent();
			this.IKController.BodyIK.solvers.leftFoot.SetBendPlaneToCurrent();
			this.IKController.OverrideLegBendTargets(board.Animation.LeftLegBendTarget.Transform, board.Animation.RightLegBendTarget.Transform);
			this.IKController.SetIKActive(true);
			this.avatar.SetEquippable(string.Empty);
			this.avatar.LookController.ForceLookTarget = board.Animation.AvatarFaceTarget;
			this.avatar.LookController.ForceLookRotateBody = true;
			this.SetBool("SkateIdle", true);
			this.activeSkateboard = board;
			this.activeSkateboard.OnPushStart.AddListener(new UnityAction(this.SkateboardPush));
		}

		// Token: 0x060041D6 RID: 16854 RVA: 0x00114104 File Offset: 0x00112304
		public void SkateboardDismounted()
		{
			this.IKController.ResetLegBendTargets();
			this.IKController.SetIKActive(false);
			this.avatar.LookController.ForceLookTarget = null;
			this.avatar.LookController.ForceLookRotateBody = false;
			this.SetBool("SkateIdle", false);
			this.activeSkateboard.OnPushStart.RemoveListener(new UnityAction(this.SkateboardPush));
			this.activeSkateboard = null;
		}

		// Token: 0x060041D7 RID: 16855 RVA: 0x00114179 File Offset: 0x00112379
		private void SkateboardPush()
		{
			this.SetTrigger("SkatePush");
		}

		// Token: 0x04002F9E RID: 12190
		public const float AnimationRangeSqr = 1600f;

		// Token: 0x04002F9F RID: 12191
		public const float FrustrumCullMinDist = 225f;

		// Token: 0x04002FA0 RID: 12192
		public const float RunningAnimationSpeed = 8f;

		// Token: 0x04002FA1 RID: 12193
		public const float MaxBoneOffset = 0.01f;

		// Token: 0x04002FA2 RID: 12194
		public const float MaxBoneOffsetSqr = 0.0001f;

		// Token: 0x04002FA3 RID: 12195
		public static Vector3 SITTING_OFFSET = new Vector3(0f, -0.825f, 0f);

		// Token: 0x04002FA4 RID: 12196
		public const float SEAT_TIME = 0.5f;

		// Token: 0x04002FAA RID: 12202
		public bool DEBUG_MODE;

		// Token: 0x04002FAB RID: 12203
		private int framesActive;

		// Token: 0x04002FAC RID: 12204
		[Header("References")]
		public Animator animator;

		// Token: 0x04002FAD RID: 12205
		public Transform HipBone;

		// Token: 0x04002FAE RID: 12206
		public Transform[] Bones;

		// Token: 0x04002FAF RID: 12207
		protected Avatar avatar;

		// Token: 0x04002FB0 RID: 12208
		public Transform LeftHandContainer;

		// Token: 0x04002FB1 RID: 12209
		public Transform RightHandContainer;

		// Token: 0x04002FB2 RID: 12210
		public Transform RightHandAlignmentPoint;

		// Token: 0x04002FB3 RID: 12211
		public Transform LeftHandAlignmentPoint;

		// Token: 0x04002FB4 RID: 12212
		public AvatarIKController IKController;

		// Token: 0x04002FB5 RID: 12213
		[Header("Settings")]
		public LayerMask GroundingMask;

		// Token: 0x04002FB6 RID: 12214
		public string StandUpFromBackClipName;

		// Token: 0x04002FB7 RID: 12215
		public string StandUpFromFrontClipName;

		// Token: 0x04002FB8 RID: 12216
		public bool UseImpostor = true;

		// Token: 0x04002FB9 RID: 12217
		public bool AllowCulling = true;

		// Token: 0x04002FBA RID: 12218
		public UnityEvent onStandupStart;

		// Token: 0x04002FBB RID: 12219
		public UnityEvent onStandupDone;

		// Token: 0x04002FBC RID: 12220
		public UnityEvent onHeavyFlinch;

		// Token: 0x04002FBD RID: 12221
		private BoneTransform[] standingBoneTransforms;

		// Token: 0x04002FBE RID: 12222
		private BoneTransform[] standUpFromBackBoneTransforms;

		// Token: 0x04002FBF RID: 12223
		private BoneTransform[] standUpFromFrontBoneTransforms;

		// Token: 0x04002FC0 RID: 12224
		private BoneTransform[] ragdollBoneTransforms;

		// Token: 0x04002FC1 RID: 12225
		private Coroutine standUpRoutine;

		// Token: 0x04002FC2 RID: 12226
		private Coroutine seatRoutine;

		// Token: 0x04002FC3 RID: 12227
		private Skateboard activeSkateboard;

		// Token: 0x04002FC4 RID: 12228
		private bool animationEnabled = true;

		// Token: 0x04002FC5 RID: 12229
		private AnimatorCullingMode initialCullingMode;

		// Token: 0x02000975 RID: 2421
		public enum EFlinchType
		{
			// Token: 0x04002FC7 RID: 12231
			Light,
			// Token: 0x04002FC8 RID: 12232
			Heavy
		}

		// Token: 0x02000976 RID: 2422
		public enum EFlinchDirection
		{
			// Token: 0x04002FCA RID: 12234
			Forward,
			// Token: 0x04002FCB RID: 12235
			Backward,
			// Token: 0x04002FCC RID: 12236
			Left,
			// Token: 0x04002FCD RID: 12237
			Right
		}
	}
}
