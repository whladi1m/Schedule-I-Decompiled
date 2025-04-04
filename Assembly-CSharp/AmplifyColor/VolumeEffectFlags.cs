using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AmplifyColor
{
	// Token: 0x02000C28 RID: 3112
	[Serializable]
	public class VolumeEffectFlags
	{
		// Token: 0x060056EF RID: 22255 RVA: 0x0016CE60 File Offset: 0x0016B060
		public VolumeEffectFlags()
		{
			this.components = new List<VolumeEffectComponentFlags>();
		}

		// Token: 0x060056F0 RID: 22256 RVA: 0x0016CE74 File Offset: 0x0016B074
		public void AddComponent(Component c)
		{
			VolumeEffectComponentFlags volumeEffectComponentFlags;
			if ((volumeEffectComponentFlags = this.components.Find(delegate(VolumeEffectComponentFlags s)
			{
				string componentName = s.componentName;
				Type type = c.GetType();
				return componentName == (((type != null) ? type.ToString() : null) ?? "");
			})) != null)
			{
				volumeEffectComponentFlags.UpdateComponentFlags(c);
				return;
			}
			this.components.Add(new VolumeEffectComponentFlags(c));
		}

		// Token: 0x060056F1 RID: 22257 RVA: 0x0016CECC File Offset: 0x0016B0CC
		public void UpdateFlags(VolumeEffect effectVol)
		{
			using (List<VolumeEffectComponent>.Enumerator enumerator = effectVol.components.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					VolumeEffectComponent comp = enumerator.Current;
					VolumeEffectComponentFlags volumeEffectComponentFlags;
					if ((volumeEffectComponentFlags = this.components.Find((VolumeEffectComponentFlags s) => s.componentName == comp.componentName)) == null)
					{
						this.components.Add(new VolumeEffectComponentFlags(comp));
					}
					else
					{
						volumeEffectComponentFlags.UpdateComponentFlags(comp);
					}
				}
			}
		}

		// Token: 0x060056F2 RID: 22258 RVA: 0x0016CF64 File Offset: 0x0016B164
		public static void UpdateCamFlags(AmplifyColorEffect[] effects, AmplifyColorVolumeBase[] volumes)
		{
			foreach (AmplifyColorEffect amplifyColorEffect in effects)
			{
				amplifyColorEffect.EffectFlags = new VolumeEffectFlags();
				for (int j = 0; j < volumes.Length; j++)
				{
					VolumeEffect volumeEffect = volumes[j].EffectContainer.FindVolumeEffect(amplifyColorEffect);
					if (volumeEffect != null)
					{
						amplifyColorEffect.EffectFlags.UpdateFlags(volumeEffect);
					}
				}
			}
		}

		// Token: 0x060056F3 RID: 22259 RVA: 0x0016CFC8 File Offset: 0x0016B1C8
		public VolumeEffect GenerateEffectData(AmplifyColorEffect go)
		{
			VolumeEffect volumeEffect = new VolumeEffect(go);
			foreach (VolumeEffectComponentFlags volumeEffectComponentFlags in this.components)
			{
				if (volumeEffectComponentFlags.blendFlag)
				{
					Component component = go.GetComponent(volumeEffectComponentFlags.componentName);
					if (component != null)
					{
						volumeEffect.AddComponent(component, volumeEffectComponentFlags);
					}
				}
			}
			return volumeEffect;
		}

		// Token: 0x060056F4 RID: 22260 RVA: 0x0016D044 File Offset: 0x0016B244
		public VolumeEffectComponentFlags FindComponentFlags(string compName)
		{
			for (int i = 0; i < this.components.Count; i++)
			{
				if (this.components[i].componentName == compName)
				{
					return this.components[i];
				}
			}
			return null;
		}

		// Token: 0x060056F5 RID: 22261 RVA: 0x0016D090 File Offset: 0x0016B290
		public string[] GetComponentNames()
		{
			return (from r in this.components
			where r.blendFlag
			select r.componentName).ToArray<string>();
		}

		// Token: 0x04004095 RID: 16533
		public List<VolumeEffectComponentFlags> components;
	}
}
