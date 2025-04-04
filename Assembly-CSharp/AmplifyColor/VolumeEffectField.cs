using System;
using System.Reflection;
using UnityEngine;

namespace AmplifyColor
{
	// Token: 0x02000C1B RID: 3099
	[Serializable]
	public class VolumeEffectField
	{
		// Token: 0x060056B5 RID: 22197 RVA: 0x0016BC09 File Offset: 0x00169E09
		public VolumeEffectField(string fieldName, string fieldType)
		{
			this.fieldName = fieldName;
			this.fieldType = fieldType;
		}

		// Token: 0x060056B6 RID: 22198 RVA: 0x0016BC20 File Offset: 0x00169E20
		public VolumeEffectField(FieldInfo pi, Component c) : this(pi.Name, pi.FieldType.FullName)
		{
			object value = pi.GetValue(c);
			this.UpdateValue(value);
		}

		// Token: 0x060056B7 RID: 22199 RVA: 0x0016BC54 File Offset: 0x00169E54
		public static bool IsValidType(string type)
		{
			return type == "System.Single" || type == "System.Boolean" || type == "UnityEngine.Color" || type == "UnityEngine.Vector2" || type == "UnityEngine.Vector3" || type == "UnityEngine.Vector4";
		}

		// Token: 0x060056B8 RID: 22200 RVA: 0x0016BCB4 File Offset: 0x00169EB4
		public void UpdateValue(object val)
		{
			string a = this.fieldType;
			if (a == "System.Single")
			{
				this.valueSingle = (float)val;
				return;
			}
			if (a == "System.Boolean")
			{
				this.valueBoolean = (bool)val;
				return;
			}
			if (a == "UnityEngine.Color")
			{
				this.valueColor = (Color)val;
				return;
			}
			if (a == "UnityEngine.Vector2")
			{
				this.valueVector2 = (Vector2)val;
				return;
			}
			if (a == "UnityEngine.Vector3")
			{
				this.valueVector3 = (Vector3)val;
				return;
			}
			if (!(a == "UnityEngine.Vector4"))
			{
				return;
			}
			this.valueVector4 = (Vector4)val;
		}

		// Token: 0x04004074 RID: 16500
		public string fieldName;

		// Token: 0x04004075 RID: 16501
		public string fieldType;

		// Token: 0x04004076 RID: 16502
		public float valueSingle;

		// Token: 0x04004077 RID: 16503
		public Color valueColor;

		// Token: 0x04004078 RID: 16504
		public bool valueBoolean;

		// Token: 0x04004079 RID: 16505
		public Vector2 valueVector2;

		// Token: 0x0400407A RID: 16506
		public Vector3 valueVector3;

		// Token: 0x0400407B RID: 16507
		public Vector4 valueVector4;
	}
}
