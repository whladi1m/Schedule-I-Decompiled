using System;
using ScheduleOne.AvatarFramework.Customization;
using UnityEngine;

namespace ScheduleOne.UI.CharacterCreator
{
	// Token: 0x02000B32 RID: 2866
	public class BaseCharacterCreatorField : MonoBehaviour
	{
		// Token: 0x06004C55 RID: 19541 RVA: 0x000045B1 File Offset: 0x000027B1
		protected virtual void Awake()
		{
		}

		// Token: 0x06004C56 RID: 19542 RVA: 0x000045B1 File Offset: 0x000027B1
		protected virtual void Start()
		{
		}

		// Token: 0x06004C57 RID: 19543 RVA: 0x000045B1 File Offset: 0x000027B1
		public virtual void ApplyValue()
		{
		}

		// Token: 0x06004C58 RID: 19544 RVA: 0x000045B1 File Offset: 0x000027B1
		public virtual void WriteValue(bool applyValue = true)
		{
		}

		// Token: 0x040039AF RID: 14767
		public string PropertyName;

		// Token: 0x040039B0 RID: 14768
		public CharacterCreator.ECategory Category;

		// Token: 0x040039B1 RID: 14769
		private CharacterCreator Creator;
	}
}
