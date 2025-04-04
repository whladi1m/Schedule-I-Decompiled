using System;
using System.Collections.Generic;
using System.Linq;
using RootMotion.FinalIK;
using ScheduleOne.NPCs;
using ScheduleOne.PlayerScripts;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.AvatarFramework.Animation
{
	// Token: 0x0200097D RID: 2429
	public class AvatarLookController : MonoBehaviour
	{
		// Token: 0x060041F4 RID: 16884 RVA: 0x001148E8 File Offset: 0x00112AE8
		private void Awake()
		{
			this.avatar = base.GetComponent<Avatar>();
			this.avatar.onRagdollChange.AddListener(new UnityAction<bool, bool, bool>(this.RagdollChange));
			this.defaultIKWeight = this.Aim.solver.GetIKPositionWeight();
			this.lookAtTarget = new GameObject("LookAtTarget (" + base.gameObject.name + ")").transform;
			Transform transform = this.lookAtTarget;
			GameObject gameObject = GameObject.Find("_Temp");
			transform.SetParent((gameObject != null) ? gameObject.transform : null);
			this.LookForward();
			this.lookAtTarget.transform.position = this.lookAtPos;
			this.lastFrameOffset = this.LookOrigin.InverseTransformPoint(this.lookAtTarget.position);
			this.NPC = base.GetComponentInParent<NPC>();
			base.InvokeRepeating("UpdateNearestPlayer", 0f, 0.5f);
		}

		// Token: 0x060041F5 RID: 16885 RVA: 0x001149D8 File Offset: 0x00112BD8
		private void UpdateShit()
		{
			if (this.ForceLookTarget != null && this.CanLookAt(this.ForceLookTarget.position))
			{
				this.OverrideLookTarget(this.ForceLookTarget.position, 100, this.ForceLookRotateBody);
				return;
			}
			if (this.AutoLookAtPlayer && Player.Local != null && (Player.Local.Paranoid || Player.Local.Schizophrenic))
			{
				this.OverrideLookTarget(Player.Local.MimicCamera.position, 200, false);
				this.Aim.enabled = (this.nearestPlayerDist < 20f * QualitySettings.lodBias);
				this.Aim.solver.clampWeight = Mathf.MoveTowards(this.Aim.solver.clampWeight, this.AimIKWeight, Time.deltaTime * 2f);
				return;
			}
			if (this.DEBUG)
			{
				Console.Log("Nearest player: " + ((this.nearestPlayer != null) ? this.nearestPlayer.name : "null") + " dist: " + this.nearestPlayerDist.ToString(), null);
				Console.Log("Visibility: " + this.NPC.awareness.VisionCone.GetPlayerVisibility(this.nearestPlayer).ToString(), null);
				Console.Log("AutoLookAtPlayer: " + this.AutoLookAtPlayer.ToString(), null);
				Console.Log("CanLookAt: " + this.CanLookAt(this.nearestPlayer.EyePosition).ToString(), null);
			}
			if (this.nearestPlayer != null && this.AutoLookAtPlayer && this.CanLookAt(this.nearestPlayer.EyePosition) && (this.NPC == null || this.NPC.awareness.VisionCone.GetPlayerVisibility(this.nearestPlayer) > this.NPC.awareness.VisionCone.MinVisionDelta))
			{
				Vector3 a = this.nearestPlayer.EyePosition;
				if (this.nearestPlayer.IsOwner)
				{
					a = this.nearestPlayer.MimicCamera.position;
				}
				if (this.nearestPlayerDist < 4f)
				{
					this.lookAtPos = a;
					if (this.DEBUG)
					{
						Console.Log("Looking at player: " + this.nearestPlayer.name, null);
					}
				}
				else if (this.nearestPlayerDist < 10f && Vector3.Angle(a - this.HeadBone.position, this.HeadBone.forward) < 45f)
				{
					Transform mimicCamera = this.nearestPlayer.MimicCamera;
					if (Vector3.Angle(mimicCamera.forward, (this.HeadBone.position - mimicCamera.position).normalized) < 15f)
					{
						this.lookAtPos = a;
						if (this.DEBUG)
						{
							Console.Log("Looking at player: " + this.nearestPlayer.name, null);
						}
					}
					else
					{
						this.LookForward();
					}
				}
				else
				{
					this.LookForward();
				}
			}
			else
			{
				this.LookForward();
			}
			if (this.Aim != null)
			{
				if (this.avatar.Ragdolled || this.avatar.Anim.StandUpAnimationPlaying)
				{
					this.Aim.solver.clampWeight = 0f;
					this.Aim.enabled = false;
					return;
				}
				this.Aim.enabled = (this.nearestPlayerDist < 20f * QualitySettings.lodBias);
				this.Aim.solver.clampWeight = Mathf.MoveTowards(this.Aim.solver.clampWeight, this.AimIKWeight, Time.deltaTime * 2f);
			}
		}

		// Token: 0x060041F6 RID: 16886 RVA: 0x00114DC0 File Offset: 0x00112FC0
		private void UpdateNearestPlayer()
		{
			if (Player.Local == null)
			{
				return;
			}
			this.localPlayerDist = Vector3.Distance(Player.Local.Avatar.CenterPoint, base.transform.position);
			this.cullRange = 30f * QualitySettings.lodBias;
			if (this.localPlayerDist > this.cullRange)
			{
				return;
			}
			List<Player> list = new List<Player>();
			foreach (Player player in Player.PlayerList)
			{
				if (player.Avatar.LookController == this)
				{
					list.Add(player);
				}
			}
			this.nearestPlayer = Player.GetClosestPlayer(base.transform.position, out this.nearestPlayerDist, list);
		}

		// Token: 0x060041F7 RID: 16887 RVA: 0x00114E9C File Offset: 0x0011309C
		private void LateUpdate()
		{
			if (this.localPlayerDist > this.cullRange)
			{
				if (this.Aim != null && this.Aim.enabled)
				{
					this.Aim.enabled = false;
				}
				this.lastFrameLookOriginPos = this.LookOrigin.position;
				this.lastFrameLookOriginForward = this.LookOrigin.forward;
				return;
			}
			this.UpdateShit();
			if (this.overrideLookAt)
			{
				this.lookAtPos = this.overriddenLookTarget;
			}
			if (!this.avatar.Ragdolled)
			{
				if (this.overrideLookAt && this.overrideRotateBody)
				{
					Vector3 to = this.lookAtPos - base.transform.position;
					to.y = 0f;
					to.Normalize();
					float y = Vector3.SignedAngle(base.transform.parent.forward, to, Vector3.up);
					if (this.DEBUG)
					{
						Console.Log("Body rotation: " + y.ToString(), null);
					}
					this.avatar.transform.localRotation = Quaternion.Lerp(this.avatar.transform.localRotation, Quaternion.Euler(0f, y, 0f), Time.deltaTime * this.BodyRotationSpeed);
				}
				else if (this.avatar.transform.parent != null)
				{
					this.avatar.transform.localRotation = Quaternion.Lerp(this.avatar.transform.localRotation, Quaternion.identity, Time.deltaTime * this.BodyRotationSpeed);
				}
			}
			this.LerpTargetTransform();
			this.Eyes.LookAt(this.lookAtPos, false);
			this.overrideLookAt = false;
			this.overriddenLookTarget = Vector3.zero;
			this.overrideLookPriority = 0;
			this.overrideRotateBody = false;
			this.lastFrameLookOriginPos = this.LookOrigin.position;
			this.lastFrameLookOriginForward = this.LookOrigin.forward;
		}

		// Token: 0x060041F8 RID: 16888 RVA: 0x00115094 File Offset: 0x00113294
		public void OverrideLookTarget(Vector3 targetPosition, int priority, bool rotateBody = false)
		{
			if (this.overrideLookAt && priority < this.overrideLookPriority)
			{
				return;
			}
			if (this.DEBUG)
			{
				Debug.DrawLine(base.transform.position, targetPosition, Color.red, 0.1f);
				string str = "Overriding look target to: ";
				Vector3 vector = targetPosition;
				Console.Log(str + vector.ToString() + " priority: " + priority.ToString(), null);
			}
			this.overrideLookAt = true;
			this.overriddenLookTarget = targetPosition;
			this.overrideLookPriority = priority;
			this.overrideRotateBody = rotateBody;
		}

		// Token: 0x060041F9 RID: 16889 RVA: 0x00115120 File Offset: 0x00113320
		private void LookForward()
		{
			if (this.DEBUG)
			{
				Console.Log("Looking forward", null);
			}
			this.LookForwardTarget.position = this.HeadBone.position + base.transform.forward * 1f;
			this.lookAtPos = this.LookForwardTarget.position;
		}

		// Token: 0x060041FA RID: 16890 RVA: 0x00115184 File Offset: 0x00113384
		private void LerpTargetTransform()
		{
			this.lookAtTarget.position = this.LookOrigin.TransformPoint(this.lastFrameOffset);
			Vector3 normalized = (this.lookAtTarget.position - this.LookOrigin.position).normalized;
			Vector3 normalized2 = (this.lookAtPos - this.LookOrigin.position).normalized;
			Vector3 b = Vector3.Lerp(normalized, normalized2, Time.deltaTime * this.LookLerpSpeed);
			this.lookAtTarget.position = this.LookOrigin.position + b;
			if (this.Aim != null)
			{
				this.Aim.solver.target = this.lookAtTarget;
			}
			this.lastFrameOffset = this.LookOrigin.InverseTransformPoint(this.lookAtTarget.position);
		}

		// Token: 0x060041FB RID: 16891 RVA: 0x00115260 File Offset: 0x00113460
		private Player GetNearestPlayer()
		{
			List<Player> playerList = Player.PlayerList;
			if (playerList.Count <= 0)
			{
				return null;
			}
			return (from p in playerList
			orderby Vector3.Distance(p.transform.position, base.transform.position)
			select p).First<Player>();
		}

		// Token: 0x060041FC RID: 16892 RVA: 0x00115298 File Offset: 0x00113498
		private bool CanLookAt(Vector3 position)
		{
			Vector3 forward = this.avatar.transform.forward;
			Vector3 normalized = (position - this.avatar.transform.position).normalized;
			return Vector3.SignedAngle(forward, normalized, Vector3.up) < 90f;
		}

		// Token: 0x060041FD RID: 16893 RVA: 0x000045B1 File Offset: 0x000027B1
		protected void RagdollChange(bool oldValue, bool ragdoll, bool playStandUpAnim)
		{
		}

		// Token: 0x060041FE RID: 16894 RVA: 0x001152E6 File Offset: 0x001134E6
		public void OverrideIKWeight(float weight)
		{
			this.Aim.solver.SetIKPositionWeight(weight);
		}

		// Token: 0x060041FF RID: 16895 RVA: 0x001152F9 File Offset: 0x001134F9
		public void ResetIKWeight()
		{
			this.Aim.solver.SetIKPositionWeight(this.defaultIKWeight);
		}

		// Token: 0x04002FEE RID: 12270
		public const float LookAtPlayerRange = 4f;

		// Token: 0x04002FEF RID: 12271
		public const float EyeContractRange = 10f;

		// Token: 0x04002FF0 RID: 12272
		public const float AimIKRange = 20f;

		// Token: 0x04002FF1 RID: 12273
		public bool DEBUG;

		// Token: 0x04002FF2 RID: 12274
		[Header("References")]
		public AimIK Aim;

		// Token: 0x04002FF3 RID: 12275
		public Transform HeadBone;

		// Token: 0x04002FF4 RID: 12276
		public Transform LookForwardTarget;

		// Token: 0x04002FF5 RID: 12277
		public Transform LookOrigin;

		// Token: 0x04002FF6 RID: 12278
		public EyeController Eyes;

		// Token: 0x04002FF7 RID: 12279
		[Header("Optional NPC reference")]
		public NPC NPC;

		// Token: 0x04002FF8 RID: 12280
		[Header("Settings")]
		public bool AutoLookAtPlayer = true;

		// Token: 0x04002FF9 RID: 12281
		public float LookLerpSpeed = 1f;

		// Token: 0x04002FFA RID: 12282
		public float AimIKWeight = 0.6f;

		// Token: 0x04002FFB RID: 12283
		public float BodyRotationSpeed = 1f;

		// Token: 0x04002FFC RID: 12284
		private Avatar avatar;

		// Token: 0x04002FFD RID: 12285
		private Vector3 lookAtPos = Vector3.zero;

		// Token: 0x04002FFE RID: 12286
		private Transform lookAtTarget;

		// Token: 0x04002FFF RID: 12287
		private Vector3 lastFrameOffset = Vector3.zero;

		// Token: 0x04003000 RID: 12288
		private bool overrideLookAt;

		// Token: 0x04003001 RID: 12289
		private Vector3 overriddenLookTarget = Vector3.zero;

		// Token: 0x04003002 RID: 12290
		private int overrideLookPriority;

		// Token: 0x04003003 RID: 12291
		private bool overrideRotateBody;

		// Token: 0x04003004 RID: 12292
		private Vector3 lastFrameLookOriginPos;

		// Token: 0x04003005 RID: 12293
		private Vector3 lastFrameLookOriginForward;

		// Token: 0x04003006 RID: 12294
		public Transform ForceLookTarget;

		// Token: 0x04003007 RID: 12295
		public bool ForceLookRotateBody;

		// Token: 0x04003008 RID: 12296
		private float defaultIKWeight = 0.6f;

		// Token: 0x04003009 RID: 12297
		private Player nearestPlayer;

		// Token: 0x0400300A RID: 12298
		private float nearestPlayerDist;

		// Token: 0x0400300B RID: 12299
		private float localPlayerDist;

		// Token: 0x0400300C RID: 12300
		private float cullRange = 100f;
	}
}
