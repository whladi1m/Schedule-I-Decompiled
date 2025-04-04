using System;
using System.Collections.Generic;
using ScheduleOne.DevUtilities;
using ScheduleOne.PlayerScripts;
using UnityEngine;

namespace ScheduleOne.UI.WorldspacePopup
{
	// Token: 0x02000A3C RID: 2620
	public class WorldspacePopupCanvas : MonoBehaviour
	{
		// Token: 0x06004698 RID: 18072 RVA: 0x0012735C File Offset: 0x0012555C
		private void Update()
		{
			if (!PlayerSingleton<PlayerCamera>.InstanceExists)
			{
				return;
			}
			List<WorldspacePopup> list = new List<WorldspacePopup>();
			List<WorldspacePopup> list2 = new List<WorldspacePopup>();
			for (int i = 0; i < WorldspacePopup.ActivePopups.Count; i++)
			{
				if (!this.popupsWithUI.Contains(WorldspacePopup.ActivePopups[i]) && this.ShouldCreateUI(WorldspacePopup.ActivePopups[i]))
				{
					list.Add(WorldspacePopup.ActivePopups[i]);
				}
			}
			for (int j = 0; j < this.popupsWithUI.Count; j++)
			{
				if (!WorldspacePopup.ActivePopups.Contains(this.popupsWithUI[j]) || !this.ShouldCreateUI(this.popupsWithUI[j]))
				{
					list2.Add(this.popupsWithUI[j]);
				}
			}
			foreach (WorldspacePopup worldspacePopup in list)
			{
				this.CreateWorldspaceIcon(worldspacePopup);
				if (worldspacePopup.DisplayOnHUD)
				{
					this.CreateHUDIcon(worldspacePopup);
				}
			}
			foreach (WorldspacePopup worldspacePopup2 in list2)
			{
				this.DestroyWorldspaceIcon(worldspacePopup2);
				if (worldspacePopup2.DisplayOnHUD)
				{
					this.DestroyHUDIcon(worldspacePopup2);
				}
			}
		}

		// Token: 0x06004699 RID: 18073 RVA: 0x001274CC File Offset: 0x001256CC
		private void LateUpdate()
		{
			if (!PlayerSingleton<PlayerCamera>.InstanceExists)
			{
				return;
			}
			for (int i = 0; i < this.popupsWithUI.Count; i++)
			{
				if (PlayerSingleton<PlayerCamera>.Instance.transform.InverseTransformPoint(this.popupsWithUI[i].transform.position).z > 0f)
				{
					Vector3 vector = this.popupsWithUI[i].transform.position + this.popupsWithUI[i].WorldspaceOffset;
					Vector2 v = PlayerSingleton<PlayerCamera>.Instance.Camera.WorldToScreenPoint(vector);
					float num = 1f;
					if (this.popupsWithUI[i].ScaleWithDistance)
					{
						float f = Vector3.Distance(vector, PlayerSingleton<PlayerCamera>.Instance.transform.position);
						num = 1f / Mathf.Sqrt(f);
					}
					num *= this.popupsWithUI[i].SizeMultiplier;
					num *= 0.4f;
					this.popupsWithUI[i].WorldspaceUI.Rect.position = v;
					this.popupsWithUI[i].WorldspaceUI.Rect.localScale = new Vector3(num, num, 1f);
					this.popupsWithUI[i].WorldspaceUI.gameObject.SetActive(true);
				}
				else
				{
					this.popupsWithUI[i].WorldspaceUI.gameObject.SetActive(false);
				}
			}
			for (int j = 0; j < this.popupsWithUI.Count; j++)
			{
				if (this.popupsWithUI[j].HUDUI != null)
				{
					float num2 = Vector3.SignedAngle(Vector3.ProjectOnPlane(PlayerSingleton<PlayerCamera>.Instance.transform.forward, Vector3.up), (this.popupsWithUI[j].transform.position - PlayerSingleton<PlayerCamera>.Instance.transform.position).normalized, Vector3.up);
					this.popupsWithUI[j].HUDUI.localRotation = Quaternion.Euler(0f, 0f, 0f - num2);
					this.popupsWithUI[j].HUDUIIcon.transform.up = Vector3.up;
					float num3 = 1f;
					float num4 = Mathf.Abs(num2);
					float num5 = 15f;
					if (num4 < 45f)
					{
						num3 = (num4 - num5) / (45f - num5);
						num3 = Mathf.Clamp01(num3);
					}
					this.popupsWithUI[j].HUDUICanvasGroup.alpha = Mathf.MoveTowards(this.popupsWithUI[j].HUDUICanvasGroup.alpha, num3, Time.deltaTime * 3f);
				}
			}
		}

		// Token: 0x0600469A RID: 18074 RVA: 0x001277B0 File Offset: 0x001259B0
		private bool ShouldCreateUI(WorldspacePopup popup)
		{
			return Vector3.Distance(popup.transform.position, PlayerSingleton<PlayerCamera>.Instance.transform.position) <= popup.Range;
		}

		// Token: 0x0600469B RID: 18075 RVA: 0x001277DC File Offset: 0x001259DC
		private WorldspacePopupUI CreateWorldspaceIcon(WorldspacePopup popup)
		{
			WorldspacePopupUI worldspacePopupUI = popup.CreateUI(this.WorldspaceContainer);
			this.activeWorldspaceUIs.Add(worldspacePopupUI);
			this.popupsWithUI.Add(popup);
			popup.WorldspaceUI = worldspacePopupUI;
			return worldspacePopupUI;
		}

		// Token: 0x0600469C RID: 18076 RVA: 0x00127818 File Offset: 0x00125A18
		private RectTransform CreateHUDIcon(WorldspacePopup popup)
		{
			RectTransform component = UnityEngine.Object.Instantiate<GameObject>(this.HudIconContainerPrefab, this.HudContainer).GetComponent<RectTransform>();
			WorldspacePopupUI huduiicon = popup.CreateUI(component.Find("Container").GetComponent<RectTransform>());
			popup.HUDUI = component;
			popup.HUDUIIcon = huduiicon;
			popup.HUDUICanvasGroup = component.GetComponent<CanvasGroup>();
			popup.HUDUICanvasGroup.alpha = 0f;
			this.activeHUDUIs.Add(component);
			return component;
		}

		// Token: 0x0600469D RID: 18077 RVA: 0x0012788C File Offset: 0x00125A8C
		private void DestroyWorldspaceIcon(WorldspacePopup popup)
		{
			for (int i = 0; i < this.activeWorldspaceUIs.Count; i++)
			{
				if (this.activeWorldspaceUIs[i].Popup == popup)
				{
					this.activeWorldspaceUIs[i].Destroy();
					this.activeWorldspaceUIs.RemoveAt(i);
					this.popupsWithUI.Remove(popup);
					return;
				}
			}
		}

		// Token: 0x0600469E RID: 18078 RVA: 0x001278F4 File Offset: 0x00125AF4
		private void DestroyHUDIcon(WorldspacePopup popup)
		{
			for (int i = 0; i < this.activeHUDUIs.Count; i++)
			{
				if (this.activeHUDUIs[i].GetComponentInChildren<WorldspacePopupUI>().Popup == popup)
				{
					this.activeHUDUIs[i].GetComponentInChildren<WorldspacePopupUI>().Destroy();
					UnityEngine.Object.Destroy(this.activeHUDUIs[i].gameObject);
					this.activeHUDUIs.RemoveAt(i);
					return;
				}
			}
		}

		// Token: 0x04003455 RID: 13397
		public const float WORLDSPACE_ICON_SCALE_MULTIPLIER = 0.4f;

		// Token: 0x04003456 RID: 13398
		[Header("References")]
		public RectTransform WorldspaceContainer;

		// Token: 0x04003457 RID: 13399
		public RectTransform HudContainer;

		// Token: 0x04003458 RID: 13400
		[Header("Prefabs")]
		public GameObject HudIconContainerPrefab;

		// Token: 0x04003459 RID: 13401
		private List<WorldspacePopupUI> activeWorldspaceUIs = new List<WorldspacePopupUI>();

		// Token: 0x0400345A RID: 13402
		private List<RectTransform> activeHUDUIs = new List<RectTransform>();

		// Token: 0x0400345B RID: 13403
		private List<WorldspacePopup> popupsWithUI = new List<WorldspacePopup>();
	}
}
