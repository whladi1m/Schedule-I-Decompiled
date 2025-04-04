using System;
using UnityEngine;

namespace Funly.SkyStudio
{
	// Token: 0x020001E2 RID: 482
	[RequireComponent(typeof(MeshRenderer))]
	public class WeatherEnclosure : MonoBehaviour
	{
		// Token: 0x04000B9F RID: 2975
		public Vector2 nearTextureTiling = new Vector3(1f, 1f);

		// Token: 0x04000BA0 RID: 2976
		public Vector2 farTextureTiling = new Vector2(1f, 1f);
	}
}
