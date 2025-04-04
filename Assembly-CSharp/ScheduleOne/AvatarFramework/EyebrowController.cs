using System;
using UnityEngine;

namespace ScheduleOne.AvatarFramework
{
	// Token: 0x0200094F RID: 2383
	public class EyebrowController : MonoBehaviour
	{
		// Token: 0x060040F0 RID: 16624 RVA: 0x00110F0C File Offset: 0x0010F10C
		public void ApplySettings(AvatarSettings settings)
		{
			this.SetLeftBrowRestingHeight(settings.EyebrowRestingHeight);
			this.SetRightBrowRestingHeight(settings.EyebrowRestingHeight);
			this.leftBrow.SetScale(settings.EyebrowScale);
			this.rightBrow.SetScale(settings.EyebrowScale);
			this.leftBrow.SetThickness(settings.EyebrowThickness);
			this.rightBrow.SetThickness(settings.EyebrowThickness);
			this.leftBrow.SetRestingAngle(settings.EyebrowRestingAngle);
			this.rightBrow.SetRestingAngle(settings.EyebrowRestingAngle);
			this.leftBrow.SetColor(settings.HairColor);
			this.rightBrow.SetColor(settings.HairColor);
		}

		// Token: 0x060040F1 RID: 16625 RVA: 0x00110FB9 File Offset: 0x0010F1B9
		public void SetLeftBrowRestingHeight(float normalizedHeight)
		{
			this.leftBrow.SetRestingHeight(normalizedHeight);
		}

		// Token: 0x060040F2 RID: 16626 RVA: 0x00110FC7 File Offset: 0x0010F1C7
		public void SetRightBrowRestingHeight(float normalizedHeight)
		{
			this.rightBrow.SetRestingHeight(normalizedHeight);
		}

		// Token: 0x04002ED4 RID: 11988
		[Header("References")]
		public Eyebrow leftBrow;

		// Token: 0x04002ED5 RID: 11989
		public Eyebrow rightBrow;
	}
}
