using System;
using System.Collections;
using System.Runtime.CompilerServices;
using ScheduleOne.DevUtilities;
using ScheduleOne.UI.Phone.Map;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ScheduleOne.Map
{
	// Token: 0x02000C00 RID: 3072
	public class POI : MonoBehaviour
	{
		// Token: 0x17000C10 RID: 3088
		// (get) Token: 0x060055E6 RID: 21990 RVA: 0x00168DD9 File Offset: 0x00166FD9
		// (set) Token: 0x060055E7 RID: 21991 RVA: 0x00168DE1 File Offset: 0x00166FE1
		public bool UISetup { get; protected set; }

		// Token: 0x17000C11 RID: 3089
		// (get) Token: 0x060055E8 RID: 21992 RVA: 0x00168DEA File Offset: 0x00166FEA
		// (set) Token: 0x060055E9 RID: 21993 RVA: 0x00168DF2 File Offset: 0x00166FF2
		public string MainText { get; protected set; } = string.Empty;

		// Token: 0x17000C12 RID: 3090
		// (get) Token: 0x060055EA RID: 21994 RVA: 0x00168DFB File Offset: 0x00166FFB
		// (set) Token: 0x060055EB RID: 21995 RVA: 0x00168E03 File Offset: 0x00167003
		public RectTransform UI { get; protected set; }

		// Token: 0x17000C13 RID: 3091
		// (get) Token: 0x060055EC RID: 21996 RVA: 0x00168E0C File Offset: 0x0016700C
		// (set) Token: 0x060055ED RID: 21997 RVA: 0x00168E14 File Offset: 0x00167014
		public RectTransform IconContainer { get; protected set; }

		// Token: 0x060055EE RID: 21998 RVA: 0x00168E20 File Offset: 0x00167020
		private void OnEnable()
		{
			if (this.UI == null)
			{
				if (PlayerSingleton<MapApp>.Instance == null)
				{
					base.StartCoroutine(this.<OnEnable>g__Wait|27_0());
					return;
				}
				if (this.UI == null)
				{
					this.UI = UnityEngine.Object.Instantiate<GameObject>(this.UIPrefab, PlayerSingleton<MapApp>.Instance.PoIContainer).GetComponent<RectTransform>();
					this.InitializeUI();
				}
			}
		}

		// Token: 0x060055EF RID: 21999 RVA: 0x00168E8A File Offset: 0x0016708A
		private void OnDisable()
		{
			if (this.UI != null)
			{
				UnityEngine.Object.Destroy(this.UI.gameObject);
				this.UI = null;
			}
		}

		// Token: 0x060055F0 RID: 22000 RVA: 0x00168EB1 File Offset: 0x001670B1
		private void Update()
		{
			if (this.AutoUpdatePosition && PlayerSingleton<MapApp>.InstanceExists && PlayerSingleton<MapApp>.Instance.isOpen)
			{
				this.UpdatePosition();
			}
		}

		// Token: 0x060055F1 RID: 22001 RVA: 0x00168ED4 File Offset: 0x001670D4
		public void SetMainText(string text)
		{
			this.mainTextSet = true;
			this.MainText = text;
			if (this.mainLabel != null)
			{
				this.mainLabel.text = text;
			}
		}

		// Token: 0x060055F2 RID: 22002 RVA: 0x00168F00 File Offset: 0x00167100
		public virtual void UpdatePosition()
		{
			if (this.UI == null)
			{
				return;
			}
			if (!Singleton<MapPositionUtility>.InstanceExists)
			{
				return;
			}
			this.UI.anchoredPosition = Singleton<MapPositionUtility>.Instance.GetMapPosition(base.transform.position);
			if (this.Rotate)
			{
				this.IconContainer.localEulerAngles = new Vector3(0f, 0f, Vector3.SignedAngle(base.transform.forward, Vector3.forward, Vector3.up));
			}
		}

		// Token: 0x060055F3 RID: 22003 RVA: 0x00168F80 File Offset: 0x00167180
		public virtual void InitializeUI()
		{
			this.mainLabel = this.UI.Find("MainLabel").GetComponent<Text>();
			if (this.mainLabel == null)
			{
				Console.LogError("Failed to find main label", null);
			}
			if (this.MainTextVisibility == POI.TextShowMode.Off || this.MainTextVisibility == POI.TextShowMode.OnHover)
			{
				this.mainLabel.enabled = false;
			}
			else
			{
				this.mainLabel.enabled = true;
			}
			this.eventTrigger = this.UI.GetComponent<EventTrigger>();
			EventTrigger.Entry entry = new EventTrigger.Entry();
			entry.eventID = EventTriggerType.PointerEnter;
			entry.callback.AddListener(delegate(BaseEventData data)
			{
				this.HoverStart();
			});
			this.eventTrigger.triggers.Add(entry);
			entry = new EventTrigger.Entry();
			entry.eventID = EventTriggerType.PointerExit;
			entry.callback.AddListener(delegate(BaseEventData data)
			{
				this.HoverEnd();
			});
			this.eventTrigger.triggers.Add(entry);
			this.button = this.UI.GetComponent<Button>();
			this.button.onClick.AddListener(delegate()
			{
				this.Clicked();
			});
			this.IconContainer = this.UI.Find("IconContainer").GetComponent<RectTransform>();
			if (this.IconContainer == null)
			{
				Console.LogError("Failed to find icon container", null);
			}
			if (!this.mainTextSet)
			{
				this.SetMainText(this.DefaultMainText);
			}
			else
			{
				this.SetMainText(this.MainText);
			}
			if (this.onUICreated != null)
			{
				this.onUICreated.Invoke();
			}
			this.UISetup = true;
			this.UpdatePosition();
		}

		// Token: 0x060055F4 RID: 22004 RVA: 0x0016910A File Offset: 0x0016730A
		protected virtual void HoverStart()
		{
			if (this.MainTextVisibility == POI.TextShowMode.OnHover)
			{
				this.mainLabel.enabled = true;
			}
		}

		// Token: 0x060055F5 RID: 22005 RVA: 0x00169121 File Offset: 0x00167321
		protected virtual void HoverEnd()
		{
			if (this.MainTextVisibility == POI.TextShowMode.OnHover)
			{
				this.mainLabel.enabled = false;
			}
		}

		// Token: 0x060055F6 RID: 22006 RVA: 0x00169138 File Offset: 0x00167338
		protected virtual void Clicked()
		{
			PlayerSingleton<MapApp>.Instance.FocusPosition(this.UI.anchoredPosition);
		}

		// Token: 0x060055F8 RID: 22008 RVA: 0x0016917B File Offset: 0x0016737B
		[CompilerGenerated]
		private IEnumerator <OnEnable>g__Wait|27_0()
		{
			yield return new WaitUntil(() => PlayerSingleton<MapApp>.Instance != null);
			if (!base.enabled)
			{
				yield break;
			}
			if (this.UI == null)
			{
				this.UI = UnityEngine.Object.Instantiate<GameObject>(this.UIPrefab, PlayerSingleton<MapApp>.Instance.PoIContainer).GetComponent<RectTransform>();
				this.InitializeUI();
			}
			yield break;
		}

		// Token: 0x04003FCF RID: 16335
		public POI.TextShowMode MainTextVisibility = POI.TextShowMode.Always;

		// Token: 0x04003FD0 RID: 16336
		public string DefaultMainText = "PoI Main Text";

		// Token: 0x04003FD1 RID: 16337
		public bool AutoUpdatePosition = true;

		// Token: 0x04003FD2 RID: 16338
		public bool Rotate;

		// Token: 0x04003FD4 RID: 16340
		[SerializeField]
		protected GameObject UIPrefab;

		// Token: 0x04003FD7 RID: 16343
		protected Text mainLabel;

		// Token: 0x04003FD8 RID: 16344
		protected Button button;

		// Token: 0x04003FD9 RID: 16345
		protected EventTrigger eventTrigger;

		// Token: 0x04003FDA RID: 16346
		private bool mainTextSet;

		// Token: 0x04003FDB RID: 16347
		public UnityEvent onUICreated;

		// Token: 0x02000C01 RID: 3073
		public enum TextShowMode
		{
			// Token: 0x04003FDD RID: 16349
			Off,
			// Token: 0x04003FDE RID: 16350
			Always,
			// Token: 0x04003FDF RID: 16351
			OnHover
		}
	}
}
