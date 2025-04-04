using System;
using ScheduleOne.DevUtilities;
using ScheduleOne.PlayerScripts;
using UnityEngine;
using UnityEngine.UI;

namespace ScheduleOne.Interaction
{
	// Token: 0x0200060E RID: 1550
	public class WorldSpaceLabel
	{
		// Token: 0x060028D8 RID: 10456 RVA: 0x000A86E0 File Offset: 0x000A68E0
		public WorldSpaceLabel(string _text, Vector3 _position)
		{
			this.text = _text;
			this.position = _position;
			this.rect = UnityEngine.Object.Instantiate<GameObject>(Singleton<InteractionManager>.Instance.WSLabelPrefab, Singleton<InteractionManager>.Instance.wsLabelContainer).GetComponent<RectTransform>();
			this.textComp = this.rect.GetComponent<Text>();
			Singleton<InteractionManager>.Instance.activeWSlabels.Add(this);
			this.RefreshDisplay();
		}

		// Token: 0x060028D9 RID: 10457 RVA: 0x000A8784 File Offset: 0x000A6984
		public void RefreshDisplay()
		{
			if (PlayerSingleton<PlayerCamera>.Instance.transform.InverseTransformPoint(this.position).z < -3f || !this.active)
			{
				this.rect.gameObject.SetActive(false);
				return;
			}
			this.textComp.text = this.text;
			this.textComp.color = this.color;
			this.rect.position = PlayerSingleton<PlayerCamera>.Instance.Camera.WorldToScreenPoint(this.position);
			float num = Mathf.Clamp(1f / Vector3.Distance(this.position, PlayerSingleton<PlayerCamera>.Instance.transform.position), 0f, 1f) * Singleton<InteractionManager>.Instance.displaySizeMultiplier * this.scale;
			this.rect.localScale = new Vector3(num, num, 1f);
			this.rect.gameObject.SetActive(true);
		}

		// Token: 0x060028DA RID: 10458 RVA: 0x000A887D File Offset: 0x000A6A7D
		public void Destroy()
		{
			Singleton<InteractionManager>.Instance.activeWSlabels.Remove(this);
			this.rect.gameObject.SetActive(false);
			UnityEngine.Object.Destroy(this.rect.gameObject);
		}

		// Token: 0x04001E03 RID: 7683
		public string text = string.Empty;

		// Token: 0x04001E04 RID: 7684
		public Color32 color = Color.white;

		// Token: 0x04001E05 RID: 7685
		public Vector3 position = Vector3.zero;

		// Token: 0x04001E06 RID: 7686
		public float scale = 1f;

		// Token: 0x04001E07 RID: 7687
		public RectTransform rect;

		// Token: 0x04001E08 RID: 7688
		public Text textComp;

		// Token: 0x04001E09 RID: 7689
		public bool active = true;
	}
}
