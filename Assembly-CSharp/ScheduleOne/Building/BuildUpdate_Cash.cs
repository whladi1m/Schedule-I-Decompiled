using System;
using System.Collections.Generic;
using ScheduleOne.DevUtilities;
using ScheduleOne.Interaction;
using ScheduleOne.ItemFramework;
using ScheduleOne.Money;
using ScheduleOne.ObjectScripts.Cash;
using ScheduleOne.PlayerScripts;
using ScheduleOne.Storage;
using UnityEngine;

namespace ScheduleOne.Building
{
	// Token: 0x02000777 RID: 1911
	public class BuildUpdate_Cash : BuildUpdate_StoredItem
	{
		// Token: 0x1700076F RID: 1903
		// (get) Token: 0x06003416 RID: 13334 RVA: 0x000D9AE5 File Offset: 0x000D7CE5
		private float placeAmount
		{
			get
			{
				return (float)Cash.amounts[this.amountIndex % Cash.amounts.Length];
			}
		}

		// Token: 0x06003417 RID: 13335 RVA: 0x000D9AFC File Offset: 0x000D7CFC
		private void Start()
		{
			Transform transform = this.ghostModel.transform.Find("Bills");
			for (int i = 0; i < transform.childCount; i++)
			{
				this.bills.Add(transform.GetChild(i));
			}
			this.RefreshGhostModelAppearance();
			this.amountLabel = new WorldSpaceLabel("Amount", Vector3.zero);
			this.amountLabel.scale = 1.25f;
		}

		// Token: 0x06003418 RID: 13336 RVA: 0x000D9B6D File Offset: 0x000D7D6D
		protected override void Update()
		{
			base.Update();
			if (GameInput.GetButtonDown(GameInput.ButtonCode.SecondaryClick))
			{
				this.amountIndex++;
				this.RefreshGhostModelAppearance();
			}
		}

		// Token: 0x06003419 RID: 13337 RVA: 0x000D9B94 File Offset: 0x000D7D94
		protected override void LateUpdate()
		{
			base.LateUpdate();
			if (this.GetRelevantCashBalane() < this.placeAmount)
			{
				if (this.GetRelevantCashBalane() < (float)Cash.amounts[0])
				{
					this.amountIndex = 0;
					this.RefreshGhostModelAppearance();
					this.validPosition = false;
					base.UpdateMaterials();
					this.amountLabel.text = "Insufficient cash";
					this.UpdateLabel();
					return;
				}
				while (this.GetRelevantCashBalane() < this.placeAmount)
				{
					this.amountIndex++;
					this.RefreshGhostModelAppearance();
				}
			}
			this.amountLabel.text = MoneyManager.FormatAmount(this.placeAmount, false, false);
			this.UpdateLabel();
		}

		// Token: 0x0600341A RID: 13338 RVA: 0x000D9C38 File Offset: 0x000D7E38
		private void UpdateLabel()
		{
			this.amountLabel.position = this.ghostModel.transform.position;
			Vector3 a = PlayerSingleton<PlayerCamera>.Instance.transform.position - this.ghostModel.transform.position;
			a.y = 0f;
			a.Normalize();
			this.amountLabel.position += a * 0.2f;
			if (this.validPosition)
			{
				this.amountLabel.color = Color.white;
				return;
			}
			this.amountLabel.color = new Color32(byte.MaxValue, 50, 50, byte.MaxValue);
		}

		// Token: 0x0600341B RID: 13339 RVA: 0x000D9CF8 File Offset: 0x000D7EF8
		private void RefreshGhostModelAppearance()
		{
			int billStacksToDisplay = Cash.GetBillStacksToDisplay(this.placeAmount);
			for (int i = 0; i < this.bills.Count; i++)
			{
				if (i < billStacksToDisplay)
				{
					this.bills[i].gameObject.SetActive(true);
				}
				else
				{
					this.bills[i].gameObject.SetActive(false);
				}
			}
		}

		// Token: 0x0600341C RID: 13340 RVA: 0x000D9D5C File Offset: 0x000D7F5C
		protected override void Place()
		{
			float rotation = Vector3.SignedAngle(this.bestIntersection.storageTile.ownerGrid.transform.forward, this.storedItemClass.buildPoint.forward, this.bestIntersection.storageTile.ownerGrid.transform.up);
			CashInstance cashInstance = new CashInstance(this.itemInstance.Definition, 1);
			cashInstance.SetBalance(this.placeAmount, false);
			Singleton<BuildManager>.Instance.CreateStoredItem(cashInstance, this.bestIntersection.storageTile.ownerGrid.GetComponentInParent<IStorageEntity>(), this.bestIntersection.storageTile.ownerGrid, base.GetOriginCoordinate(), rotation);
			this.mouseUpSincePlace = false;
			this.PostPlace();
		}

		// Token: 0x0600341D RID: 13341 RVA: 0x000D9E16 File Offset: 0x000D8016
		protected override void PostPlace()
		{
			NetworkSingleton<MoneyManager>.Instance.ChangeCashBalance(-this.placeAmount, true, false);
		}

		// Token: 0x0600341E RID: 13342 RVA: 0x000D9E2B File Offset: 0x000D802B
		public override void Stop()
		{
			base.Stop();
			this.amountLabel.Destroy();
		}

		// Token: 0x0600341F RID: 13343 RVA: 0x000D9E3E File Offset: 0x000D803E
		public float GetRelevantCashBalane()
		{
			return NetworkSingleton<MoneyManager>.Instance.cashBalance;
		}

		// Token: 0x04002551 RID: 9553
		public int amountIndex;

		// Token: 0x04002552 RID: 9554
		protected List<Transform> bills = new List<Transform>();

		// Token: 0x04002553 RID: 9555
		private WorldSpaceLabel amountLabel;
	}
}
