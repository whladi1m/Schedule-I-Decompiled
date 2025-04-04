using System;
using ScheduleOne.Construction.Features;
using ScheduleOne.DevUtilities;
using ScheduleOne.PlayerScripts;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ScheduleOne.UI.Construction
{
	// Token: 0x02000B56 RID: 2902
	public class FeatureIcon : MonoBehaviour
	{
		// Token: 0x17000AA0 RID: 2720
		// (get) Token: 0x06004D19 RID: 19737 RVA: 0x0014585C File Offset: 0x00143A5C
		// (set) Token: 0x06004D1A RID: 19738 RVA: 0x00145864 File Offset: 0x00143A64
		public Feature feature { get; protected set; }

		// Token: 0x17000AA1 RID: 2721
		// (get) Token: 0x06004D1B RID: 19739 RVA: 0x0014586D File Offset: 0x00143A6D
		// (set) Token: 0x06004D1C RID: 19740 RVA: 0x00145875 File Offset: 0x00143A75
		public bool isSelected { get; protected set; }

		// Token: 0x06004D1D RID: 19741 RVA: 0x00145880 File Offset: 0x00143A80
		public void AssignFeature(Feature _feature)
		{
			this.feature = _feature;
			this.icon.sprite = this.feature.featureIcon;
			this.text.text = this.feature.featureName;
			this.text.gameObject.SetActive(false);
		}

		// Token: 0x06004D1E RID: 19742 RVA: 0x001458D4 File Offset: 0x00143AD4
		public void UpdateTransform()
		{
			Vector3 position = this.feature.featureIconLocation.position;
			if (PlayerSingleton<PlayerCamera>.Instance.transform.InverseTransformPoint(position).z < 0f)
			{
				base.gameObject.SetActive(false);
				return;
			}
			this.rectTransform.position = PlayerSingleton<PlayerCamera>.Instance.Camera.WorldToScreenPoint(position);
			float num = 0.3f;
			float num2 = 1f;
			float num3 = 3f;
			float num4 = 30f;
			float num5 = Vector3.Distance(position, PlayerSingleton<PlayerCamera>.Instance.transform.position);
			float num6 = 1f - Mathf.Clamp((num5 - num3) / (num4 - num3), 0f, 1f);
			float num7 = num + (num2 - num) * num6;
			this.rectTransform.localScale = new Vector3(num7, num7, num7);
			base.gameObject.SetActive(true);
		}

		// Token: 0x06004D1F RID: 19743 RVA: 0x001459B2 File Offset: 0x00143BB2
		public void Clicked()
		{
			this.SetIsSelected(!this.isSelected);
			if (this.isSelected)
			{
				Singleton<FeaturesManager>.Instance.OpenFeatureMenu(this.feature);
				return;
			}
			Singleton<FeaturesManager>.Instance.CloseFeatureMenu();
		}

		// Token: 0x06004D20 RID: 19744 RVA: 0x001459E8 File Offset: 0x00143BE8
		public void SetIsSelected(bool s)
		{
			this.isSelected = s;
			if (this.isSelected)
			{
				if (FeatureIcon.selectedFeatureIcon != null && FeatureIcon.selectedFeatureIcon != this)
				{
					FeatureIcon.selectedFeatureIcon.SetIsSelected(false);
				}
				FeatureIcon.selectedFeatureIcon = this;
			}
			else if (FeatureIcon.selectedFeatureIcon == this)
			{
				FeatureIcon.selectedFeatureIcon = null;
			}
			if (!this.hovered)
			{
				this.text.gameObject.SetActive(false);
			}
			this.UpdateColors();
		}

		// Token: 0x06004D21 RID: 19745 RVA: 0x00145A64 File Offset: 0x00143C64
		private void UpdateColors()
		{
			if (this.isSelected)
			{
				this.background.color = new Color32(byte.MaxValue, 156, 37, byte.MaxValue);
				this.icon.color = Color.white;
				return;
			}
			this.background.color = Color.white;
			this.icon.color = new Color32(byte.MaxValue, 156, 37, byte.MaxValue);
		}

		// Token: 0x06004D22 RID: 19746 RVA: 0x00145AE6 File Offset: 0x00143CE6
		public void PointerEnter()
		{
			this.hovered = true;
			this.text.gameObject.SetActive(true);
		}

		// Token: 0x06004D23 RID: 19747 RVA: 0x00145B00 File Offset: 0x00143D00
		public void PointerExit()
		{
			this.hovered = false;
			if (!this.isSelected)
			{
				this.text.gameObject.SetActive(false);
			}
		}

		// Token: 0x04003A62 RID: 14946
		public static FeatureIcon selectedFeatureIcon;

		// Token: 0x04003A63 RID: 14947
		[Header("References")]
		public RectTransform rectTransform;

		// Token: 0x04003A64 RID: 14948
		public Image icon;

		// Token: 0x04003A65 RID: 14949
		public TextMeshProUGUI text;

		// Token: 0x04003A66 RID: 14950
		public Image background;

		// Token: 0x04003A69 RID: 14953
		private bool hovered;
	}
}
