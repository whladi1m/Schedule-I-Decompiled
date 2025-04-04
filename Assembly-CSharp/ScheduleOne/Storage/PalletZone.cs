using System;
using System.Collections.Generic;
using UnityEngine;

namespace ScheduleOne.Storage
{
	// Token: 0x0200088B RID: 2187
	public class PalletZone : MonoBehaviour
	{
		// Token: 0x17000855 RID: 2133
		// (get) Token: 0x06003B57 RID: 15191 RVA: 0x000F9F6E File Offset: 0x000F816E
		public bool isClear
		{
			get
			{
				return (this.pallets.Count == 0 || this.AreAllPalletsClear()) && !this.orderReceivedThisFrame;
			}
		}

		// Token: 0x06003B58 RID: 15192 RVA: 0x000F9F90 File Offset: 0x000F8190
		protected void OnTriggerStay(Collider other)
		{
			Pallet componentInParent = other.GetComponentInParent<Pallet>();
			if (componentInParent != null && !this.pallets.Contains(componentInParent))
			{
				this.pallets.Add(componentInParent);
			}
		}

		// Token: 0x06003B59 RID: 15193 RVA: 0x000F9FC7 File Offset: 0x000F81C7
		protected void FixedUpdate()
		{
			this.pallets.Clear();
		}

		// Token: 0x06003B5A RID: 15194 RVA: 0x000F9FD4 File Offset: 0x000F81D4
		protected void LateUpdate()
		{
			this.orderReceivedThisFrame = false;
		}

		// Token: 0x06003B5B RID: 15195 RVA: 0x000F9FDD File Offset: 0x000F81DD
		public Pallet GeneratePallet()
		{
			Pallet component = UnityEngine.Object.Instantiate<GameObject>(this.palletPrefab).GetComponent<Pallet>();
			component.transform.position = base.transform.position;
			component.transform.rotation = base.transform.rotation;
			return component;
		}

		// Token: 0x06003B5C RID: 15196 RVA: 0x000FA01C File Offset: 0x000F821C
		private bool AreAllPalletsClear()
		{
			for (int i = 0; i < this.pallets.Count; i++)
			{
				if (!this.pallets[i].isEmpty)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x04002AFE RID: 11006
		private List<Pallet> pallets = new List<Pallet>();

		// Token: 0x04002AFF RID: 11007
		[Header("Prefabs")]
		[SerializeField]
		protected GameObject palletPrefab;

		// Token: 0x04002B00 RID: 11008
		private bool orderReceivedThisFrame;
	}
}
