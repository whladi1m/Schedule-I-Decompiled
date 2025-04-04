using System;
using System.Collections.Generic;
using ScheduleOne.ItemFramework;
using ScheduleOne.Properties;
using UnityEngine;

namespace ScheduleOne.Product
{
	// Token: 0x020008E8 RID: 2280
	[CreateAssetMenu(fileName = "PropertyItemDefinition", menuName = "ScriptableObjects/PropertyItemDefinition", order = 1)]
	[Serializable]
	public class PropertyItemDefinition : StorableItemDefinition
	{
		// Token: 0x06003E0B RID: 15883 RVA: 0x00105FFF File Offset: 0x001041FF
		public virtual void Initialize(List<Property> properties)
		{
			this.Properties.AddRange(properties);
		}

		// Token: 0x06003E0C RID: 15884 RVA: 0x0010600D File Offset: 0x0010420D
		public bool HasProperty(Property property)
		{
			return this.Properties.Contains(property);
		}

		// Token: 0x04002C96 RID: 11414
		[Header("Properties")]
		public List<Property> Properties = new List<Property>();
	}
}
