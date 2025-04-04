using System;
using ScheduleOne.AvatarFramework.Customization;
using ScheduleOne.Clothing;
using ScheduleOne.DevUtilities;

namespace ScheduleOne.UI.CharacterCreator
{
	// Token: 0x02000B35 RID: 2869
	public class CharacterCreatorField<T> : BaseCharacterCreatorField
	{
		// Token: 0x17000A91 RID: 2705
		// (get) Token: 0x06004C61 RID: 19553 RVA: 0x00141B60 File Offset: 0x0013FD60
		// (set) Token: 0x06004C62 RID: 19554 RVA: 0x00141B68 File Offset: 0x0013FD68
		public T value { get; protected set; }

		// Token: 0x06004C63 RID: 19555 RVA: 0x00141B71 File Offset: 0x0013FD71
		public virtual T ReadValue()
		{
			return Singleton<CharacterCreator>.Instance.ActiveSettings.GetValue<T>(this.PropertyName);
		}

		// Token: 0x06004C64 RID: 19556 RVA: 0x00141B88 File Offset: 0x0013FD88
		public override void WriteValue(bool applyValue = true)
		{
			base.WriteValue(applyValue);
			Singleton<CharacterCreator>.Instance.SetValue<T>(this.PropertyName, this.value, this.selectedClothingDefinition);
			Singleton<CharacterCreator>.Instance.RefreshCategory(this.Category);
			if (applyValue)
			{
				this.ApplyValue();
			}
		}

		// Token: 0x06004C65 RID: 19557 RVA: 0x00141BC7 File Offset: 0x0013FDC7
		public override void ApplyValue()
		{
			base.ApplyValue();
			this.value = this.ReadValue();
		}

		// Token: 0x040039BC RID: 14780
		protected ClothingDefinition selectedClothingDefinition;
	}
}
