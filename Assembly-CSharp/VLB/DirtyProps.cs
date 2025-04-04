using System;

namespace VLB
{
	// Token: 0x0200010B RID: 267
	[Flags]
	public enum DirtyProps
	{
		// Token: 0x040005C6 RID: 1478
		None = 0,
		// Token: 0x040005C7 RID: 1479
		Intensity = 2,
		// Token: 0x040005C8 RID: 1480
		HDRPExposureWeight = 4,
		// Token: 0x040005C9 RID: 1481
		ColorMode = 8,
		// Token: 0x040005CA RID: 1482
		Color = 16,
		// Token: 0x040005CB RID: 1483
		BlendingMode = 32,
		// Token: 0x040005CC RID: 1484
		Cone = 64,
		// Token: 0x040005CD RID: 1485
		SideSoftness = 128,
		// Token: 0x040005CE RID: 1486
		Attenuation = 256,
		// Token: 0x040005CF RID: 1487
		Dimensions = 512,
		// Token: 0x040005D0 RID: 1488
		RaymarchingQuality = 1024,
		// Token: 0x040005D1 RID: 1489
		Jittering = 2048,
		// Token: 0x040005D2 RID: 1490
		NoiseMode = 4096,
		// Token: 0x040005D3 RID: 1491
		NoiseIntensity = 8192,
		// Token: 0x040005D4 RID: 1492
		NoiseVelocityAndScale = 16384,
		// Token: 0x040005D5 RID: 1493
		CookieProps = 32768,
		// Token: 0x040005D6 RID: 1494
		ShadowProps = 65536,
		// Token: 0x040005D7 RID: 1495
		AllWithoutMaterialChange = 125142,
		// Token: 0x040005D8 RID: 1496
		OnlyMaterialChangeOnly = 5928,
		// Token: 0x040005D9 RID: 1497
		All = 131070
	}
}
