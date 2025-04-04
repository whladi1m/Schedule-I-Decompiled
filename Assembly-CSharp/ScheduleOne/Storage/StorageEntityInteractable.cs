using System;
using ScheduleOne.Interaction;

namespace ScheduleOne.Storage
{
	// Token: 0x02000890 RID: 2192
	public class StorageEntityInteractable : InteractableObject
	{
		// Token: 0x06003BAC RID: 15276 RVA: 0x000FB8DC File Offset: 0x000F9ADC
		private void Awake()
		{
			this.StorageEntity = base.GetComponentInParent<StorageEntity>();
			this.MaxInteractionRange = this.StorageEntity.MaxAccessDistance;
		}

		// Token: 0x06003BAD RID: 15277 RVA: 0x000FB8FB File Offset: 0x000F9AFB
		public override void Hovered()
		{
			base.Hovered();
			base.SetInteractableState(this.StorageEntity.CanBeOpened() ? InteractableObject.EInteractableState.Default : InteractableObject.EInteractableState.Disabled);
		}

		// Token: 0x06003BAE RID: 15278 RVA: 0x000FB91A File Offset: 0x000F9B1A
		public override void StartInteract()
		{
			base.StartInteract();
			this.StorageEntity.Open();
		}

		// Token: 0x04002B16 RID: 11030
		private StorageEntity StorageEntity;
	}
}
