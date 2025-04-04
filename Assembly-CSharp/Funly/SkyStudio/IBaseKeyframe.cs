using System;

namespace Funly.SkyStudio
{
	// Token: 0x020001AD RID: 429
	public interface IBaseKeyframe
	{
		// Token: 0x170001C4 RID: 452
		// (get) Token: 0x060008B0 RID: 2224
		string id { get; }

		// Token: 0x170001C5 RID: 453
		// (get) Token: 0x060008B1 RID: 2225
		// (set) Token: 0x060008B2 RID: 2226
		float time { get; set; }

		// Token: 0x170001C6 RID: 454
		// (get) Token: 0x060008B3 RID: 2227
		// (set) Token: 0x060008B4 RID: 2228
		InterpolationCurve interpolationCurve { get; set; }

		// Token: 0x170001C7 RID: 455
		// (get) Token: 0x060008B5 RID: 2229
		// (set) Token: 0x060008B6 RID: 2230
		InterpolationDirection interpolationDirection { get; set; }
	}
}
