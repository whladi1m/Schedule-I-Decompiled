using System;
using ScheduleOne.Building;
using ScheduleOne.DevUtilities;

namespace ScheduleOne.Equipping
{
	// Token: 0x0200090F RID: 2319
	public class Equippable_StorableItem : Equippable
	{
		// Token: 0x06003EDD RID: 16093 RVA: 0x0010977E File Offset: 0x0010797E
		protected virtual void Update()
		{
			this.CheckLookingAtStorageObject();
			if (this.lookingAtStorageObject)
			{
				if (!this.isBuildingStoredItem)
				{
					this.StartBuildingStoredItem();
					return;
				}
			}
			else if (this.isBuildingStoredItem)
			{
				this.StopBuildingStoredItem();
			}
		}

		// Token: 0x06003EDE RID: 16094 RVA: 0x001097AC File Offset: 0x001079AC
		protected void CheckLookingAtStorageObject()
		{
			this.lookingAtStorageObject = false;
		}

		// Token: 0x06003EDF RID: 16095 RVA: 0x001097C0 File Offset: 0x001079C0
		public override void Unequip()
		{
			if (this.lookingAtStorageObject)
			{
				Singleton<BuildManager>.Instance.StopBuilding();
			}
			base.Unequip();
		}

		// Token: 0x06003EE0 RID: 16096 RVA: 0x001097DA File Offset: 0x001079DA
		protected virtual void StartBuildingStoredItem()
		{
			this.isBuildingStoredItem = true;
			Singleton<BuildManager>.Instance.StartBuildingStoredItem(this.itemInstance);
			Singleton<BuildManager>.Instance.currentBuildHandler.GetComponent<BuildUpdate_StoredItem>().currentRotation = this.rotation;
		}

		// Token: 0x06003EE1 RID: 16097 RVA: 0x0010980D File Offset: 0x00107A0D
		protected virtual void StopBuildingStoredItem()
		{
			this.isBuildingStoredItem = false;
			this.rotation = Singleton<BuildManager>.Instance.currentBuildHandler.GetComponent<BuildUpdate_StoredItem>().currentRotation;
			Singleton<BuildManager>.Instance.StopBuilding();
		}

		// Token: 0x04002D5F RID: 11615
		protected bool isBuildingStoredItem;

		// Token: 0x04002D60 RID: 11616
		protected bool lookingAtStorageObject;

		// Token: 0x04002D61 RID: 11617
		protected float rotation;
	}
}
