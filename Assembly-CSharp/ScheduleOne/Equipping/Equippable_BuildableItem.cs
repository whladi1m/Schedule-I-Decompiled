using System;
using ScheduleOne.Building;
using ScheduleOne.DevUtilities;

namespace ScheduleOne.Equipping
{
	// Token: 0x02000905 RID: 2309
	public class Equippable_BuildableItem : Equippable_StorableItem
	{
		// Token: 0x06003E88 RID: 16008 RVA: 0x00107E00 File Offset: 0x00106000
		protected override void Update()
		{
			base.CheckLookingAtStorageObject();
			if (this.lookingAtStorageObject && this.isBuilding)
			{
				this.isBuilding = false;
				if (Singleton<BuildManager>.Instance.currentBuildHandler.GetComponent<BuildUpdate_Grid>() != null)
				{
					this.rotation = Singleton<BuildManager>.Instance.currentBuildHandler.GetComponent<BuildUpdate_Grid>().CurrentRotation;
				}
			}
			base.Update();
			if (!this.lookingAtStorageObject && !this.isBuilding)
			{
				this.isBuilding = true;
				Singleton<BuildManager>.Instance.StartBuilding(this.itemInstance);
				if (Singleton<BuildManager>.Instance.currentBuildHandler.GetComponent<BuildUpdate_Grid>() != null)
				{
					Singleton<BuildManager>.Instance.currentBuildHandler.GetComponent<BuildUpdate_Grid>().CurrentRotation = this.rotation;
				}
			}
		}

		// Token: 0x06003E89 RID: 16009 RVA: 0x00107EB9 File Offset: 0x001060B9
		public override void Unequip()
		{
			if (this.isBuilding)
			{
				Singleton<BuildManager>.Instance.StopBuilding();
			}
			base.Unequip();
		}

		// Token: 0x04002D04 RID: 11524
		protected bool isBuilding;
	}
}
