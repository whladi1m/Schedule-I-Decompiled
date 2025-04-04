using System;
using ScheduleOne.DevUtilities;
using ScheduleOne.PlayerScripts;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace ScheduleOne.UI
{
	// Token: 0x02000A0C RID: 2572
	public class PlayerEnergyUI : Singleton<PlayerEnergyUI>
	{
		// Token: 0x0600456A RID: 17770 RVA: 0x001230F5 File Offset: 0x001212F5
		protected override void Awake()
		{
			base.Awake();
			Player.onLocalPlayerSpawned = (Action)Delegate.Combine(Player.onLocalPlayerSpawned, new Action(delegate()
			{
				this.UpdateDisplayedEnergy();
				Player.Local.Energy.onEnergyChanged.AddListener(new UnityAction(this.UpdateDisplayedEnergy));
			}));
		}

		// Token: 0x0600456B RID: 17771 RVA: 0x0012311D File Offset: 0x0012131D
		private void UpdateDisplayedEnergy()
		{
			this.SetDisplayedEnergy(Player.Local.Energy.CurrentEnergy);
		}

		// Token: 0x0600456C RID: 17772 RVA: 0x00123134 File Offset: 0x00121334
		public void SetDisplayedEnergy(float energy)
		{
			this.displayedValue = energy;
			this.Slider.value = energy / 100f;
			this.FillImage.color = ((energy <= 20f) ? this.SliderColor_Red : this.SliderColor_Green);
		}

		// Token: 0x0600456D RID: 17773 RVA: 0x00123170 File Offset: 0x00121370
		protected virtual void Update()
		{
			if (this.displayedValue < 20f)
			{
				float num = Mathf.Clamp((20f - this.displayedValue) / 20f, 0.25f, 1f);
				float num2 = num * 3f;
				this.SliderRect.anchoredPosition = new Vector2(UnityEngine.Random.Range(-num2, num2), UnityEngine.Random.Range(-num2, num2));
				Color white = Color.white;
				Color b = Color.Lerp(Color.white, Color.red, num);
				white.a = this.Label.color.a;
				b.a = this.Label.color.a;
				this.Label.color = Color.Lerp(white, b, (Mathf.Sin(Time.timeSinceLevelLoad * num * 10f) + 1f) / 2f);
				return;
			}
			this.SliderRect.anchoredPosition = Vector2.zero;
			this.Label.color = new Color(1f, 1f, 1f, this.Label.color.a);
		}

		// Token: 0x04003348 RID: 13128
		public Slider Slider;

		// Token: 0x04003349 RID: 13129
		public RectTransform SliderRect;

		// Token: 0x0400334A RID: 13130
		public Image FillImage;

		// Token: 0x0400334B RID: 13131
		public TextMeshProUGUI Label;

		// Token: 0x0400334C RID: 13132
		[Header("Settings")]
		public Color SliderColor_Green;

		// Token: 0x0400334D RID: 13133
		public Color SliderColor_Red;

		// Token: 0x0400334E RID: 13134
		private float displayedValue = 1f;
	}
}
