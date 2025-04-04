using System;
using System.Collections.Generic;
using System.Linq;
using FishNet.Object;
using ScheduleOne.DevUtilities;
using ScheduleOne.FX;
using ScheduleOne.NPCs;
using ScheduleOne.Vehicles;
using ScheduleOne.Vision;
using UnityEngine;

namespace ScheduleOne.Stealth
{
	// Token: 0x020002C4 RID: 708
	public class PlayerVisibility : NetworkBehaviour
	{
		// Token: 0x17000328 RID: 808
		// (get) Token: 0x06000F15 RID: 3861 RVA: 0x00042813 File Offset: 0x00040A13
		// (set) Token: 0x06000F16 RID: 3862 RVA: 0x0004281B File Offset: 0x00040A1B
		public VisionEvent HighestVisionEvent { get; set; }

		// Token: 0x06000F17 RID: 3863 RVA: 0x00042824 File Offset: 0x00040A24
		public override void OnStartClient()
		{
			base.OnStartClient();
			if (base.IsOwner)
			{
				this.environmentalVisibility = new VisibilityAttribute("Environmental Brightess", 0f, 1f, -1);
			}
		}

		// Token: 0x06000F18 RID: 3864 RVA: 0x0004284F File Offset: 0x00040A4F
		private void FixedUpdate()
		{
			this.UpdateEnvironmentalVisibilityAttribute();
			this.CurrentVisibility = this.CalculateVisibility();
		}

		// Token: 0x06000F19 RID: 3865 RVA: 0x00042864 File Offset: 0x00040A64
		private float CalculateVisibility()
		{
			float num = 0f;
			Dictionary<string, float> maxPointsChangesByUniquenessCode = (from UniqueVisibilityAttribute uva in 
				from a in this.activeAttributes
				where a is UniqueVisibilityAttribute
				select a
			group uva by uva.uniquenessCode).ToDictionary((IGrouping<string, UniqueVisibilityAttribute> group) => group.Key, (IGrouping<string, UniqueVisibilityAttribute> group) => group.Max((UniqueVisibilityAttribute uva) => uva.pointsChange));
			this.filteredAttributes = this.activeAttributes.Where(delegate(VisibilityAttribute attr)
			{
				if (attr is UniqueVisibilityAttribute)
				{
					UniqueVisibilityAttribute uniqueVisibilityAttribute = attr as UniqueVisibilityAttribute;
					return uniqueVisibilityAttribute != null && uniqueVisibilityAttribute.pointsChange >= maxPointsChangesByUniquenessCode.GetValueOrDefault(uniqueVisibilityAttribute.uniquenessCode, 0f);
				}
				return true;
			}).ToList<VisibilityAttribute>();
			for (int i = 0; i < this.filteredAttributes.Count; i++)
			{
				num += this.filteredAttributes[i].pointsChange;
				if (this.filteredAttributes[i].multiplier != 1f)
				{
					num *= this.filteredAttributes[i].multiplier;
				}
			}
			return Mathf.Clamp(num, 0f, 100f);
		}

		// Token: 0x06000F1A RID: 3866 RVA: 0x000429A4 File Offset: 0x00040BA4
		public VisibilityAttribute GetAttribute(string name)
		{
			return this.activeAttributes.Find((VisibilityAttribute x) => x.name.ToLower() == name.ToLower());
		}

		// Token: 0x06000F1B RID: 3867 RVA: 0x000429D5 File Offset: 0x00040BD5
		private void UpdateEnvironmentalVisibilityAttribute()
		{
			if (this.environmentalVisibility == null)
			{
				return;
			}
			this.environmentalVisibility.multiplier = Singleton<EnvironmentFX>.Instance.normalizedEnvironmentalBrightness;
		}

		// Token: 0x06000F1C RID: 3868 RVA: 0x000429F8 File Offset: 0x00040BF8
		public float CalculateExposureToPoint(Vector3 point, float checkRange = 50f, NPC checkingNPC = null)
		{
			float num = 0f;
			if (Vector3.Distance(point, base.transform.position) > checkRange + 1f)
			{
				return 0f;
			}
			List<VisionObscurer> list = new List<VisionObscurer>();
			foreach (Transform transform in this.visibilityPoints)
			{
				float num2 = Vector3.Distance(point, transform.position);
				if (num2 <= checkRange)
				{
					this.hits = Physics.RaycastAll(point, (transform.position - point).normalized, Mathf.Min(checkRange, num2), this.visibilityCheckMask, QueryTriggerInteraction.Collide).ToList<RaycastHit>();
					for (int i = 0; i < this.hits.Count; i++)
					{
						LandVehicle componentInParent = this.hits[i].collider.GetComponentInParent<LandVehicle>();
						if (checkingNPC != null && componentInParent != null)
						{
							if (checkingNPC.CurrentVehicle == componentInParent)
							{
								this.hits.RemoveAt(i);
								i--;
							}
						}
						else
						{
							VisionObscurer componentInParent2 = this.hits[i].collider.GetComponentInParent<VisionObscurer>();
							if (componentInParent2 != null)
							{
								if (transform == this.visibilityPoints[1] && !list.Contains(componentInParent2))
								{
									list.Add(componentInParent2);
								}
								this.hits.RemoveAt(i);
								i--;
							}
							else if (this.hits[i].collider.isTrigger)
							{
								this.hits.RemoveAt(i);
								i--;
							}
						}
					}
					if (this.hits.Count > 0)
					{
						Debug.DrawRay(point, this.hits[0].point - point, Color.red, 0.1f);
					}
					else
					{
						Debug.DrawRay(point, (transform.position - point).normalized * num2, Color.green, 0.1f);
						num += 1f / (float)this.visibilityPoints.Count;
					}
				}
			}
			float num3 = 1f;
			for (int j = 0; j < list.Count; j++)
			{
				num3 *= 1f - list[j].ObscuranceAmount;
			}
			return num * num3;
		}

		// Token: 0x06000F1E RID: 3870 RVA: 0x00042CC5 File Offset: 0x00040EC5
		public virtual void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.Stealth.PlayerVisibilityAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.Stealth.PlayerVisibilityAssembly-CSharp.dll_Excuted = true;
		}

		// Token: 0x06000F1F RID: 3871 RVA: 0x00042CD8 File Offset: 0x00040ED8
		public virtual void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.Stealth.PlayerVisibilityAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.Stealth.PlayerVisibilityAssembly-CSharp.dll_Excuted = true;
		}

		// Token: 0x06000F20 RID: 3872 RVA: 0x00042CEB File Offset: 0x00040EEB
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06000F21 RID: 3873 RVA: 0x00042CEB File Offset: 0x00040EEB
		public virtual void Awake()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x04000F82 RID: 3970
		public const float MAX_VISIBLITY = 100f;

		// Token: 0x04000F83 RID: 3971
		public float CurrentVisibility;

		// Token: 0x04000F84 RID: 3972
		public List<VisibilityAttribute> activeAttributes = new List<VisibilityAttribute>();

		// Token: 0x04000F85 RID: 3973
		public List<VisibilityAttribute> filteredAttributes = new List<VisibilityAttribute>();

		// Token: 0x04000F86 RID: 3974
		[Header("Settings")]
		public LayerMask visibilityCheckMask;

		// Token: 0x04000F87 RID: 3975
		[Header("References")]
		public List<Transform> visibilityPoints = new List<Transform>();

		// Token: 0x04000F88 RID: 3976
		private VisibilityAttribute environmentalVisibility;

		// Token: 0x04000F8A RID: 3978
		private List<RaycastHit> hits;

		// Token: 0x04000F8B RID: 3979
		private bool dll_Excuted;

		// Token: 0x04000F8C RID: 3980
		private bool dll_Excuted;
	}
}
