using System;
using System.Linq;
using EasyButtons;
using UnityEngine;

namespace ScheduleOne.Properties.MixMaps
{
	// Token: 0x0200032E RID: 814
	public class MixerMapGenerator : MonoBehaviour
	{
		// Token: 0x060011D8 RID: 4568 RVA: 0x0004DA18 File Offset: 0x0004BC18
		private void OnValidate()
		{
			this.BasePlateMesh.localScale = Vector3.one * this.MapRadius * 2f * 0.01f;
			base.gameObject.name = this.MapName;
		}

		// Token: 0x060011D9 RID: 4569 RVA: 0x0004DA68 File Offset: 0x0004BC68
		[Button]
		public void CreateEffectPrefabs()
		{
			foreach (Property property in Resources.LoadAll<Property>("Properties"))
			{
				if (this.GetEffect(property) == null)
				{
					Effect effect = UnityEngine.Object.Instantiate<Effect>(this.EffectPrefab, base.transform);
					effect.Property = property;
					effect.Radius = 0.5f;
					effect.transform.position = new Vector3(UnityEngine.Random.Range(-this.MapRadius, this.MapRadius), 0.1f, UnityEngine.Random.Range(-this.MapRadius, this.MapRadius));
				}
			}
		}

		// Token: 0x060011DA RID: 4570 RVA: 0x0004DAFC File Offset: 0x0004BCFC
		[Button]
		public Effect GetEffect(Property property)
		{
			return base.GetComponentsInChildren<Effect>().FirstOrDefault((Effect effect) => effect.Property == property);
		}

		// Token: 0x04001163 RID: 4451
		public float MapRadius = 5f;

		// Token: 0x04001164 RID: 4452
		public string MapName = "New Map";

		// Token: 0x04001165 RID: 4453
		public Transform BasePlateMesh;

		// Token: 0x04001166 RID: 4454
		public Effect EffectPrefab;
	}
}
