using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace AmplifyColor
{
	// Token: 0x02000C1C RID: 3100
	[Serializable]
	public class VolumeEffectComponent
	{
		// Token: 0x060056B9 RID: 22201 RVA: 0x0016BD64 File Offset: 0x00169F64
		public VolumeEffectComponent(string name)
		{
			this.componentName = name;
			this.fields = new List<VolumeEffectField>();
		}

		// Token: 0x060056BA RID: 22202 RVA: 0x0016BD7E File Offset: 0x00169F7E
		public VolumeEffectField AddField(FieldInfo pi, Component c)
		{
			return this.AddField(pi, c, -1);
		}

		// Token: 0x060056BB RID: 22203 RVA: 0x0016BD8C File Offset: 0x00169F8C
		public VolumeEffectField AddField(FieldInfo pi, Component c, int position)
		{
			VolumeEffectField volumeEffectField = VolumeEffectField.IsValidType(pi.FieldType.FullName) ? new VolumeEffectField(pi, c) : null;
			if (volumeEffectField != null)
			{
				if (position < 0 || position >= this.fields.Count)
				{
					this.fields.Add(volumeEffectField);
				}
				else
				{
					this.fields.Insert(position, volumeEffectField);
				}
			}
			return volumeEffectField;
		}

		// Token: 0x060056BC RID: 22204 RVA: 0x0016BDE7 File Offset: 0x00169FE7
		public void RemoveEffectField(VolumeEffectField field)
		{
			this.fields.Remove(field);
		}

		// Token: 0x060056BD RID: 22205 RVA: 0x0016BDF8 File Offset: 0x00169FF8
		public VolumeEffectComponent(Component c, VolumeEffectComponentFlags compFlags) : this(compFlags.componentName)
		{
			foreach (VolumeEffectFieldFlags volumeEffectFieldFlags in compFlags.componentFields)
			{
				if (volumeEffectFieldFlags.blendFlag)
				{
					FieldInfo field = c.GetType().GetField(volumeEffectFieldFlags.fieldName);
					VolumeEffectField volumeEffectField = VolumeEffectField.IsValidType(field.FieldType.FullName) ? new VolumeEffectField(field, c) : null;
					if (volumeEffectField != null)
					{
						this.fields.Add(volumeEffectField);
					}
				}
			}
		}

		// Token: 0x060056BE RID: 22206 RVA: 0x0016BE98 File Offset: 0x0016A098
		public void UpdateComponent(Component c, VolumeEffectComponentFlags compFlags)
		{
			using (List<VolumeEffectFieldFlags>.Enumerator enumerator = compFlags.componentFields.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					VolumeEffectFieldFlags fieldFlags = enumerator.Current;
					if (fieldFlags.blendFlag && !this.fields.Exists((VolumeEffectField s) => s.fieldName == fieldFlags.fieldName))
					{
						FieldInfo field = c.GetType().GetField(fieldFlags.fieldName);
						VolumeEffectField volumeEffectField = VolumeEffectField.IsValidType(field.FieldType.FullName) ? new VolumeEffectField(field, c) : null;
						if (volumeEffectField != null)
						{
							this.fields.Add(volumeEffectField);
						}
					}
				}
			}
		}

		// Token: 0x060056BF RID: 22207 RVA: 0x0016BF5C File Offset: 0x0016A15C
		public VolumeEffectField FindEffectField(string fieldName)
		{
			for (int i = 0; i < this.fields.Count; i++)
			{
				if (this.fields[i].fieldName == fieldName)
				{
					return this.fields[i];
				}
			}
			return null;
		}

		// Token: 0x060056C0 RID: 22208 RVA: 0x0016BFA8 File Offset: 0x0016A1A8
		public static FieldInfo[] ListAcceptableFields(Component c)
		{
			if (c == null)
			{
				return new FieldInfo[0];
			}
			return (from f in c.GetType().GetFields()
			where VolumeEffectField.IsValidType(f.FieldType.FullName)
			select f).ToArray<FieldInfo>();
		}

		// Token: 0x060056C1 RID: 22209 RVA: 0x0016BFF9 File Offset: 0x0016A1F9
		public string[] GetFieldNames()
		{
			return (from r in this.fields
			select r.fieldName).ToArray<string>();
		}

		// Token: 0x0400407C RID: 16508
		public string componentName;

		// Token: 0x0400407D RID: 16509
		public List<VolumeEffectField> fields;
	}
}
