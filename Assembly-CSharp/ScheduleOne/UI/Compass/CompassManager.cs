using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using ScheduleOne.DevUtilities;
using ScheduleOne.PlayerScripts;
using TMPro;
using UnityEngine;

namespace ScheduleOne.UI.Compass
{
	// Token: 0x02000B25 RID: 2853
	public class CompassManager : Singleton<CompassManager>
	{
		// Token: 0x17000A86 RID: 2694
		// (get) Token: 0x06004BF5 RID: 19445 RVA: 0x00044D66 File Offset: 0x00042F66
		private Transform cam
		{
			get
			{
				return PlayerSingleton<PlayerCamera>.Instance.transform;
			}
		}

		// Token: 0x06004BF6 RID: 19446 RVA: 0x00140208 File Offset: 0x0013E408
		protected override void Awake()
		{
			base.Awake();
			this.notchPositions = new List<Transform>(this.NotchPointContainer.GetComponentsInChildren<Transform>());
			this.notchPositions.Remove(this.NotchPointContainer);
			for (int i = 0; i < this.notchPositions.Count; i++)
			{
				GameObject original = this.NotchPrefab;
				int num = Mathf.RoundToInt((float)(i + 1) / (float)this.notchPositions.Count * 360f);
				if (num % 90 == 0)
				{
					original = this.DirectionIndicatorPrefab;
				}
				GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(original, this.NotchUIContainer);
				CompassManager.Notch notch = new CompassManager.Notch();
				notch.Rect = gameObject.GetComponent<RectTransform>();
				notch.Group = gameObject.GetComponent<CanvasGroup>();
				this.notches.Add(notch);
				if (num % 90 == 0)
				{
					string text = "N";
					if (num == 90)
					{
						text = "E";
					}
					else if (num == 180)
					{
						text = "S";
					}
					else if (num == 270)
					{
						text = "W";
					}
					notch.Rect.GetComponentInChildren<TextMeshProUGUI>().text = text;
				}
			}
		}

		// Token: 0x06004BF7 RID: 19447 RVA: 0x00140319 File Offset: 0x0013E519
		private void LateUpdate()
		{
			if (!PlayerSingleton<PlayerCamera>.InstanceExists)
			{
				return;
			}
			this.Canvas.enabled = (Singleton<HUD>.Instance.canvas.enabled && this.CompassEnabled);
		}

		// Token: 0x06004BF8 RID: 19448 RVA: 0x00140348 File Offset: 0x0013E548
		private void FixedUpdate()
		{
			if (!PlayerSingleton<PlayerCamera>.InstanceExists)
			{
				return;
			}
			if (Singleton<HUD>.Instance.canvas.enabled)
			{
				this.UpdateNotches();
				this.UpdateElements();
			}
		}

		// Token: 0x06004BF9 RID: 19449 RVA: 0x0014036F File Offset: 0x0013E56F
		public void SetCompassEnabled(bool enabled)
		{
			this.CompassEnabled = enabled;
		}

		// Token: 0x06004BFA RID: 19450 RVA: 0x00140378 File Offset: 0x0013E578
		public void SetVisible(bool visible)
		{
			if (this.lerpContainerPositionCoroutine != null)
			{
				base.StopCoroutine(this.lerpContainerPositionCoroutine);
			}
			this.lerpContainerPositionCoroutine = base.StartCoroutine(this.<SetVisible>g__LerpContainerPosition|28_0(visible ? this.OpenYPos : this.ClosedYPos, visible));
		}

		// Token: 0x06004BFB RID: 19451 RVA: 0x001403B4 File Offset: 0x0013E5B4
		private void UpdateNotches()
		{
			for (int i = 0; i < this.notchPositions.Count; i++)
			{
				float x;
				float num;
				this.GetCompassData(this.notchPositions[i].position, out x, out num);
				this.notches[i].Rect.anchoredPosition = new Vector2(x, 0f);
				this.notches[i].Group.alpha = num;
				this.notches[i].Rect.gameObject.SetActive(num > 0f);
			}
		}

		// Token: 0x06004BFC RID: 19452 RVA: 0x00140450 File Offset: 0x0013E650
		private void UpdateElements()
		{
			for (int i = 0; i < this.elements.Count; i++)
			{
				this.UpdateElement(this.elements[i]);
			}
		}

		// Token: 0x06004BFD RID: 19453 RVA: 0x00140488 File Offset: 0x0013E688
		private void UpdateElement(CompassManager.Element element)
		{
			if (!element.Visible || element.Transform == null)
			{
				element.Group.alpha = 0f;
			}
			else
			{
				float x;
				float alpha;
				this.GetCompassData(element.Transform.position, out x, out alpha);
				element.Rect.anchoredPosition = new Vector2(x, 0f);
				element.Group.alpha = alpha;
				float num = Vector3.Distance(this.cam.position, element.Transform.position);
				if (num <= 50f)
				{
					element.DistanceLabel.text = Mathf.CeilToInt(num).ToString() + "m";
				}
				else
				{
					element.DistanceLabel.text = string.Empty;
				}
			}
			element.Rect.gameObject.SetActive(element.Group.alpha > 0f);
		}

		// Token: 0x06004BFE RID: 19454 RVA: 0x00140574 File Offset: 0x0013E774
		public void GetCompassData(Vector3 worldPosition, out float xPos, out float alpha)
		{
			Vector3 normalized = Vector3.ProjectOnPlane(this.cam.forward, Vector3.up).normalized;
			Vector3 to = worldPosition - this.cam.position;
			to.y = 0f;
			float num = Vector3.SignedAngle(normalized, to, Vector3.up);
			xPos = Mathf.Clamp(num / this.AngleDivisor, -1f, 1f) * this.CompassUIRange * 0.5f;
			alpha = 1f;
			if (Mathf.Abs(num) > this.FullAlphaRange)
			{
				alpha = 1f - (Mathf.Abs(num) - this.FullAlphaRange) / (this.AngleDivisor - this.FullAlphaRange);
			}
		}

		// Token: 0x06004BFF RID: 19455 RVA: 0x00140628 File Offset: 0x0013E828
		public CompassManager.Element AddElement(Transform transform, RectTransform contentPrefab, bool visible = true)
		{
			CompassManager.Element element = new CompassManager.Element();
			element.Transform = transform;
			element.Rect = UnityEngine.Object.Instantiate<GameObject>(this.ElementPrefab, this.ElementUIContainer).GetComponent<RectTransform>();
			element.Group = element.Rect.GetComponent<CanvasGroup>();
			element.DistanceLabel = element.Rect.Find("Text").GetComponent<TextMeshProUGUI>();
			RectTransform component = UnityEngine.Object.Instantiate<RectTransform>(contentPrefab, element.Rect).GetComponent<RectTransform>();
			component.anchoredPosition = Vector2.zero;
			component.sizeDelta = this.ElementContentSize;
			element.Visible = visible;
			this.elements.Add(element);
			this.UpdateElement(element);
			return element;
		}

		// Token: 0x06004C00 RID: 19456 RVA: 0x001406CC File Offset: 0x0013E8CC
		public void RemoveElement(Transform transform, bool alsoDestroyRect = true)
		{
			for (int i = 0; i < this.elements.Count; i++)
			{
				if (this.elements[i].Transform == transform)
				{
					this.RemoveElement(this.elements[i], alsoDestroyRect);
					return;
				}
			}
		}

		// Token: 0x06004C01 RID: 19457 RVA: 0x0014071C File Offset: 0x0013E91C
		public void RemoveElement(CompassManager.Element el, bool alsoDestroyRect = true)
		{
			if (alsoDestroyRect)
			{
				UnityEngine.Object.Destroy(el.Rect.gameObject);
			}
			this.elements.Remove(el);
		}

		// Token: 0x06004C03 RID: 19459 RVA: 0x001407C7 File Offset: 0x0013E9C7
		[CompilerGenerated]
		private IEnumerator <SetVisible>g__LerpContainerPosition|28_0(float yPos, bool visible)
		{
			if (visible)
			{
				this.Container.gameObject.SetActive(true);
			}
			float t = 0f;
			Vector2 startPos = this.Container.anchoredPosition;
			Vector2 endPos = new Vector2(startPos.x, yPos);
			while (t < 1f)
			{
				t += Time.deltaTime * 7f;
				this.Container.anchoredPosition = new Vector2(0f, Mathf.Lerp(startPos.y, endPos.y, t));
				yield return null;
			}
			this.Container.anchoredPosition = endPos;
			this.Container.gameObject.SetActive(visible);
			yield break;
		}

		// Token: 0x04003952 RID: 14674
		public const float DISTANCE_LABEL_THRESHOLD = 50f;

		// Token: 0x04003953 RID: 14675
		[Header("References")]
		public RectTransform Container;

		// Token: 0x04003954 RID: 14676
		public Transform NotchPointContainer;

		// Token: 0x04003955 RID: 14677
		public RectTransform NotchUIContainer;

		// Token: 0x04003956 RID: 14678
		public RectTransform ElementUIContainer;

		// Token: 0x04003957 RID: 14679
		public Canvas Canvas;

		// Token: 0x04003958 RID: 14680
		[Header("Prefabs")]
		public GameObject DirectionIndicatorPrefab;

		// Token: 0x04003959 RID: 14681
		public GameObject NotchPrefab;

		// Token: 0x0400395A RID: 14682
		public GameObject ElementPrefab;

		// Token: 0x0400395B RID: 14683
		[Header("Settings")]
		public bool CompassEnabled = true;

		// Token: 0x0400395C RID: 14684
		public Vector2 ElementContentSize = new Vector2(20f, 20f);

		// Token: 0x0400395D RID: 14685
		public float CompassUIRange = 800f;

		// Token: 0x0400395E RID: 14686
		public float FullAlphaRange = 40f;

		// Token: 0x0400395F RID: 14687
		public float AngleDivisor = 60f;

		// Token: 0x04003960 RID: 14688
		public float ClosedYPos = 30f;

		// Token: 0x04003961 RID: 14689
		public float OpenYPos = -50f;

		// Token: 0x04003962 RID: 14690
		private List<Transform> notchPositions = new List<Transform>();

		// Token: 0x04003963 RID: 14691
		private List<CompassManager.Notch> notches = new List<CompassManager.Notch>();

		// Token: 0x04003964 RID: 14692
		private List<CompassManager.Element> elements = new List<CompassManager.Element>();

		// Token: 0x04003965 RID: 14693
		private Coroutine lerpContainerPositionCoroutine;

		// Token: 0x02000B26 RID: 2854
		public class Notch
		{
			// Token: 0x04003966 RID: 14694
			public RectTransform Rect;

			// Token: 0x04003967 RID: 14695
			public CanvasGroup Group;
		}

		// Token: 0x02000B27 RID: 2855
		public class Element
		{
			// Token: 0x04003968 RID: 14696
			public bool Visible;

			// Token: 0x04003969 RID: 14697
			public RectTransform Rect;

			// Token: 0x0400396A RID: 14698
			public CanvasGroup Group;

			// Token: 0x0400396B RID: 14699
			public TextMeshProUGUI DistanceLabel;

			// Token: 0x0400396C RID: 14700
			public Transform Transform;
		}
	}
}
