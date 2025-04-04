using System;
using System.Collections.Generic;
using System.Linq;

namespace AmplifyColor
{
	// Token: 0x02000C21 RID: 3105
	[Serializable]
	public class VolumeEffectContainer
	{
		// Token: 0x060056D6 RID: 22230 RVA: 0x0016C9DD File Offset: 0x0016ABDD
		public VolumeEffectContainer()
		{
			this.volumes = new List<VolumeEffect>();
		}

		// Token: 0x060056D7 RID: 22231 RVA: 0x0016C9F0 File Offset: 0x0016ABF0
		public void AddColorEffect(AmplifyColorEffect colorEffect)
		{
			VolumeEffect volumeEffect;
			if ((volumeEffect = this.FindVolumeEffect(colorEffect)) != null)
			{
				volumeEffect.UpdateVolume();
				return;
			}
			volumeEffect = new VolumeEffect(colorEffect);
			this.volumes.Add(volumeEffect);
			volumeEffect.UpdateVolume();
		}

		// Token: 0x060056D8 RID: 22232 RVA: 0x0016CA28 File Offset: 0x0016AC28
		public VolumeEffect AddJustColorEffect(AmplifyColorEffect colorEffect)
		{
			VolumeEffect volumeEffect = new VolumeEffect(colorEffect);
			this.volumes.Add(volumeEffect);
			return volumeEffect;
		}

		// Token: 0x060056D9 RID: 22233 RVA: 0x0016CA4C File Offset: 0x0016AC4C
		public VolumeEffect FindVolumeEffect(AmplifyColorEffect colorEffect)
		{
			for (int i = 0; i < this.volumes.Count; i++)
			{
				if (this.volumes[i].gameObject == colorEffect)
				{
					return this.volumes[i];
				}
			}
			for (int j = 0; j < this.volumes.Count; j++)
			{
				if (this.volumes[j].gameObject != null && this.volumes[j].gameObject.SharedInstanceID == colorEffect.SharedInstanceID)
				{
					return this.volumes[j];
				}
			}
			return null;
		}

		// Token: 0x060056DA RID: 22234 RVA: 0x0016CAF5 File Offset: 0x0016ACF5
		public void RemoveVolumeEffect(VolumeEffect volume)
		{
			this.volumes.Remove(volume);
		}

		// Token: 0x060056DB RID: 22235 RVA: 0x0016CB04 File Offset: 0x0016AD04
		public AmplifyColorEffect[] GetStoredEffects()
		{
			return (from r in this.volumes
			select r.gameObject).ToArray<AmplifyColorEffect>();
		}

		// Token: 0x04004087 RID: 16519
		public List<VolumeEffect> volumes;
	}
}
