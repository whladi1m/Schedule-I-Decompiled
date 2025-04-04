using System;
using FishNet.Object;
using ScheduleOne.DevUtilities;
using ScheduleOne.Employees;
using ScheduleOne.GameTime;
using ScheduleOne.Interaction;
using ScheduleOne.PlayerScripts;
using ScheduleOne.Tools;
using ScheduleOne.UI;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.ObjectScripts
{
	// Token: 0x02000B72 RID: 2930
	public class Bed : NetworkBehaviour
	{
		// Token: 0x17000AD4 RID: 2772
		// (get) Token: 0x06004E83 RID: 20099 RVA: 0x0014B050 File Offset: 0x00149250
		// (set) Token: 0x06004E84 RID: 20100 RVA: 0x0014B058 File Offset: 0x00149258
		public Employee AssignedEmployee { get; protected set; }

		// Token: 0x06004E85 RID: 20101 RVA: 0x0014B061 File Offset: 0x00149261
		public virtual void Awake()
		{
			this.NetworkInitialize___Early();
			this.Awake_UserLogic_ScheduleOne.ObjectScripts.Bed_Assembly-CSharp.dll();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06004E86 RID: 20102 RVA: 0x0014B078 File Offset: 0x00149278
		public void Hovered()
		{
			if (Singleton<ManagementClipboard>.Instance.IsEquipped)
			{
				this.intObj.SetInteractableState(InteractableObject.EInteractableState.Disabled);
				return;
			}
			if (this.AssignedEmployee != null)
			{
				this.intObj.SetInteractableState(InteractableObject.EInteractableState.Invalid);
				this.intObj.SetMessage("Assigned to " + this.AssignedEmployee.fullName);
				return;
			}
			if (this.CanSleep())
			{
				this.intObj.SetInteractableState(InteractableObject.EInteractableState.Default);
				this.intObj.SetMessage("Sleep");
				return;
			}
			this.intObj.SetInteractableState(InteractableObject.EInteractableState.Invalid);
			this.intObj.SetMessage("Can't sleep before " + ScheduleOne.GameTime.TimeManager.Get12HourTime(1800f, true));
		}

		// Token: 0x06004E87 RID: 20103 RVA: 0x0014B12A File Offset: 0x0014932A
		public void Interacted()
		{
			Player.Local.CurrentBed = base.NetworkObject;
			Singleton<SleepCanvas>.Instance.SetIsOpen(true);
		}

		// Token: 0x06004E88 RID: 20104 RVA: 0x0014B147 File Offset: 0x00149347
		private bool CanSleep()
		{
			return GameManager.IS_TUTORIAL || NetworkSingleton<ScheduleOne.GameTime.TimeManager>.Instance.IsCurrentTimeWithinRange(1800, 400);
		}

		// Token: 0x06004E89 RID: 20105 RVA: 0x0014B168 File Offset: 0x00149368
		public void SetAssignedEmployee(Employee employee)
		{
			this.AssignedEmployee = employee;
			if (this.AssignedEmployee != null)
			{
				this.MugshotSprite.sprite = this.AssignedEmployee.MugshotSprite;
				this.NameLabel.text = this.AssignedEmployee.FirstName + "\n" + this.AssignedEmployee.LastName;
				this.Clipboard.gameObject.SetActive(true);
			}
			else
			{
				this.Clipboard.gameObject.SetActive(false);
			}
			this.UpdateMaterial();
			if (this.onAssignedEmployeeChanged != null)
			{
				this.onAssignedEmployeeChanged.Invoke();
			}
		}

		// Token: 0x06004E8A RID: 20106 RVA: 0x0014B208 File Offset: 0x00149408
		private void UpdateMaterial()
		{
			if (this.BlanketMesh == null)
			{
				return;
			}
			Material material = this.DefaultBlanket;
			if (this.AssignedEmployee != null)
			{
				switch (this.AssignedEmployee.EmployeeType)
				{
				case EEmployeeType.Botanist:
					material = this.BotanistBlanket;
					break;
				case EEmployeeType.Handler:
					material = this.PackagerBlanket;
					break;
				case EEmployeeType.Chemist:
					material = this.ChemistBlanket;
					break;
				case EEmployeeType.Cleaner:
					material = this.CleanerBlanket;
					break;
				}
			}
			this.BlanketMesh.material = material;
		}

		// Token: 0x06004E8C RID: 20108 RVA: 0x0014B28B File Offset: 0x0014948B
		public virtual void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.ObjectScripts.BedAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.ObjectScripts.BedAssembly-CSharp.dll_Excuted = true;
		}

		// Token: 0x06004E8D RID: 20109 RVA: 0x0014B29E File Offset: 0x0014949E
		public virtual void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.ObjectScripts.BedAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.ObjectScripts.BedAssembly-CSharp.dll_Excuted = true;
		}

		// Token: 0x06004E8E RID: 20110 RVA: 0x0014B2B1 File Offset: 0x001494B1
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06004E8F RID: 20111 RVA: 0x0014B2BF File Offset: 0x001494BF
		private void dll()
		{
			if (this.Clipboard != null)
			{
				this.Clipboard.gameObject.SetActive(false);
			}
			this.UpdateMaterial();
		}

		// Token: 0x04003B5C RID: 15196
		public const int MIN_SLEEP_TIME = 1800;

		// Token: 0x04003B5D RID: 15197
		public const float SLEEP_TIME_SCALE = 1f;

		// Token: 0x04003B5F RID: 15199
		[Header("References")]
		[SerializeField]
		protected InteractableObject intObj;

		// Token: 0x04003B60 RID: 15200
		public GameObject Clipboard;

		// Token: 0x04003B61 RID: 15201
		public SpriteRenderer MugshotSprite;

		// Token: 0x04003B62 RID: 15202
		public TextMeshPro NameLabel;

		// Token: 0x04003B63 RID: 15203
		public MeshRenderer BlanketMesh;

		// Token: 0x04003B64 RID: 15204
		[Header("Materials")]
		public Material DefaultBlanket;

		// Token: 0x04003B65 RID: 15205
		public Material BotanistBlanket;

		// Token: 0x04003B66 RID: 15206
		public Material ChemistBlanket;

		// Token: 0x04003B67 RID: 15207
		public Material PackagerBlanket;

		// Token: 0x04003B68 RID: 15208
		public Material CleanerBlanket;

		// Token: 0x04003B69 RID: 15209
		public UnityEvent onAssignedEmployeeChanged;

		// Token: 0x04003B6A RID: 15210
		private bool dll_Excuted;

		// Token: 0x04003B6B RID: 15211
		private bool dll_Excuted;
	}
}
