using System;
using UnityEngine;
using VLB;

namespace VLB_Samples
{
	// Token: 0x0200015F RID: 351
	public class LightGenerator : MonoBehaviour
	{
		// Token: 0x060006C6 RID: 1734 RVA: 0x0001E6A4 File Offset: 0x0001C8A4
		public void Generate()
		{
			for (int i = 0; i < this.CountX; i++)
			{
				for (int j = 0; j < this.CountY; j++)
				{
					GameObject gameObject;
					if (this.AddLight)
					{
						gameObject = new GameObject("Light_" + i.ToString() + "_" + j.ToString(), new Type[]
						{
							typeof(Light),
							typeof(VolumetricLightBeamSD),
							typeof(Rotater)
						});
					}
					else
					{
						gameObject = new GameObject("Light_" + i.ToString() + "_" + j.ToString(), new Type[]
						{
							typeof(VolumetricLightBeamSD),
							typeof(Rotater)
						});
					}
					gameObject.transform.SetPositionAndRotation(new Vector3((float)i * this.OffsetUnits, this.PositionY, (float)j * this.OffsetUnits), Quaternion.Euler((float)UnityEngine.Random.Range(-45, 45) + 90f, (float)UnityEngine.Random.Range(0, 360), 0f));
					VolumetricLightBeamSD component = gameObject.GetComponent<VolumetricLightBeamSD>();
					if (this.AddLight)
					{
						Light component2 = gameObject.GetComponent<Light>();
						component2.type = LightType.Spot;
						component2.color = new Color(UnityEngine.Random.value, UnityEngine.Random.value, UnityEngine.Random.value, 1f);
						component2.range = UnityEngine.Random.Range(3f, 8f);
						component2.intensity = UnityEngine.Random.Range(0.2f, 5f);
						component2.spotAngle = UnityEngine.Random.Range(10f, 90f);
						if (Config.Instance.geometryOverrideLayer)
						{
							component2.cullingMask = ~(1 << Config.Instance.geometryLayerID);
						}
					}
					else
					{
						component.color = new Color(UnityEngine.Random.value, UnityEngine.Random.value, UnityEngine.Random.value, 1f);
						component.fallOffEnd = UnityEngine.Random.Range(3f, 8f);
						component.spotAngle = UnityEngine.Random.Range(10f, 90f);
					}
					component.coneRadiusStart = UnityEngine.Random.Range(0f, 0.1f);
					component.geomCustomSides = UnityEngine.Random.Range(12, 36);
					component.fresnelPow = UnityEngine.Random.Range(1f, 7.5f);
					component.noiseMode = (this.NoiseEnabled ? NoiseMode.WorldSpace : NoiseMode.Disabled);
					gameObject.GetComponent<Rotater>().EulerSpeed = new Vector3(0f, (float)UnityEngine.Random.Range(-500, 500), 0f);
				}
			}
		}

		// Token: 0x04000781 RID: 1921
		[Range(1f, 100f)]
		[SerializeField]
		private int CountX = 10;

		// Token: 0x04000782 RID: 1922
		[Range(1f, 100f)]
		[SerializeField]
		private int CountY = 10;

		// Token: 0x04000783 RID: 1923
		[SerializeField]
		private float OffsetUnits = 1f;

		// Token: 0x04000784 RID: 1924
		[SerializeField]
		private float PositionY = 1f;

		// Token: 0x04000785 RID: 1925
		[SerializeField]
		private bool NoiseEnabled;

		// Token: 0x04000786 RID: 1926
		[SerializeField]
		private bool AddLight = true;
	}
}
