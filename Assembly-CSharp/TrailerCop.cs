using System;
using EasyButtons;
using ScheduleOne.AvatarFramework.Equipping;
using ScheduleOne.NPCs;
using ScheduleOne.Police;
using UnityEngine;

// Token: 0x02000025 RID: 37
public class TrailerCop : MonoBehaviour
{
	// Token: 0x060000A9 RID: 169 RVA: 0x00005548 File Offset: 0x00003748
	[Button]
	public void Play()
	{
		this.Officer.Movement.Warp(this.StartPoint.position);
		this.Officer.SetEquippable_Networked(null, this.Equippable.AssetPath);
		if (this.RaiseWeapon)
		{
			this.Officer.SendEquippableMessage_Networked(null, "Raise");
		}
		this.Officer.Avatar.EmotionManager.AddEmotionOverride("Angry", "trailercop", 0f, 100);
		this.Officer.Movement.SpeedController.AddSpeedControl(new NPCSpeedController.SpeedControl("trailercop", 10, this.Speed));
		this.Officer.Movement.SetDestination(this.EndPoint.position);
	}

	// Token: 0x060000AA RID: 170 RVA: 0x00005608 File Offset: 0x00003808
	private void Update()
	{
		if (this.Officer.Movement.IsMoving)
		{
			this.Officer.Avatar.LookController.OverrideLookTarget(this.FaceTarget.position, 10, true);
		}
	}

	// Token: 0x060000AB RID: 171 RVA: 0x0000563F File Offset: 0x0000383F
	public void Shoot()
	{
		this.Officer.SendEquippableMessage_Networked_Vector(null, "Shoot", this.ShootTarget.position);
	}

	// Token: 0x04000097 RID: 151
	public PoliceOfficer Officer;

	// Token: 0x04000098 RID: 152
	public Transform StartPoint;

	// Token: 0x04000099 RID: 153
	public Transform EndPoint;

	// Token: 0x0400009A RID: 154
	public Transform FaceTarget;

	// Token: 0x0400009B RID: 155
	public AvatarEquippable Equippable;

	// Token: 0x0400009C RID: 156
	public float Speed = 0.3f;

	// Token: 0x0400009D RID: 157
	public bool RaiseWeapon;

	// Token: 0x0400009E RID: 158
	public Transform ShootTarget;
}
