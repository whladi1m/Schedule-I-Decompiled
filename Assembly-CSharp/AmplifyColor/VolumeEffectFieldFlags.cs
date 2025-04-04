using System;
using System.Reflection;

namespace AmplifyColor
{
	// Token: 0x02000C23 RID: 3107
	[Serializable]
	public class VolumeEffectFieldFlags
	{
		// Token: 0x060056DF RID: 22239 RVA: 0x0016CB49 File Offset: 0x0016AD49
		public VolumeEffectFieldFlags(FieldInfo pi)
		{
			this.fieldName = pi.Name;
			this.fieldType = pi.FieldType.FullName;
		}

		// Token: 0x060056E0 RID: 22240 RVA: 0x0016CB6E File Offset: 0x0016AD6E
		public VolumeEffectFieldFlags(VolumeEffectField field)
		{
			this.fieldName = field.fieldName;
			this.fieldType = field.fieldType;
			this.blendFlag = true;
		}

		// Token: 0x0400408A RID: 16522
		public string fieldName;

		// Token: 0x0400408B RID: 16523
		public string fieldType;

		// Token: 0x0400408C RID: 16524
		public bool blendFlag;
	}
}
