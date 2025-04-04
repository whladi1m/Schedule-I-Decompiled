using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace AmplifyColor
{
	// Token: 0x02000C24 RID: 3108
	[Serializable]
	public class VolumeEffectComponentFlags
	{
		// Token: 0x060056E1 RID: 22241 RVA: 0x0016CB95 File Offset: 0x0016AD95
		public VolumeEffectComponentFlags(string name)
		{
			this.componentName = name;
			this.componentFields = new List<VolumeEffectFieldFlags>();
		}

		// Token: 0x060056E2 RID: 22242 RVA: 0x0016CBB0 File Offset: 0x0016ADB0
		public VolumeEffectComponentFlags(VolumeEffectComponent comp) : this(comp.componentName)
		{
			this.blendFlag = true;
			foreach (VolumeEffectField volumeEffectField in comp.fields)
			{
				if (VolumeEffectField.IsValidType(volumeEffectField.fieldType))
				{
					this.componentFields.Add(new VolumeEffectFieldFlags(volumeEffectField));
				}
			}
		}

		// Token: 0x060056E3 RID: 22243 RVA: 0x0016CC30 File Offset: 0x0016AE30
		public VolumeEffectComponentFlags(Component c)
		{
			Type type = c.GetType();
			this..ctor(((type != null) ? type.ToString() : null) ?? "");
			foreach (FieldInfo fieldInfo in c.GetType().GetFields())
			{
				if (VolumeEffectField.IsValidType(fieldInfo.FieldType.FullName))
				{
					this.componentFields.Add(new VolumeEffectFieldFlags(fieldInfo));
				}
			}
		}

		// Token: 0x060056E4 RID: 22244 RVA: 0x0016CCA0 File Offset: 0x0016AEA0
		public void UpdateComponentFlags(VolumeEffectComponent comp)
		{
			using (List<VolumeEffectField>.Enumerator enumerator = comp.fields.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					VolumeEffectField field = enumerator.Current;
					if (this.componentFields.Find((VolumeEffectFieldFlags s) => s.fieldName == field.fieldName) == null && VolumeEffectField.IsValidType(field.fieldType))
					{
						this.componentFields.Add(new VolumeEffectFieldFlags(field));
					}
				}
			}
		}

		// Token: 0x060056E5 RID: 22245 RVA: 0x0016CD38 File Offset: 0x0016AF38
		public void UpdateComponentFlags(Component c)
		{
			FieldInfo[] fields = c.GetType().GetFields();
			for (int i = 0; i < fields.Length; i++)
			{
				FieldInfo pi = fields[i];
				if (!this.componentFields.Exists((VolumeEffectFieldFlags s) => s.fieldName == pi.Name) && VolumeEffectField.IsValidType(pi.FieldType.FullName))
				{
					this.componentFields.Add(new VolumeEffectFieldFlags(pi));
				}
			}
		}

		// Token: 0x060056E6 RID: 22246 RVA: 0x0016CDB4 File Offset: 0x0016AFB4
		public string[] GetFieldNames()
		{
			return (from r in this.componentFields
			where r.blendFlag
			select r.fieldName).ToArray<string>();
		}

		// Token: 0x0400408D RID: 16525
		public string componentName;

		// Token: 0x0400408E RID: 16526
		public List<VolumeEffectFieldFlags> componentFields;

		// Token: 0x0400408F RID: 16527
		public bool blendFlag;
	}
}
