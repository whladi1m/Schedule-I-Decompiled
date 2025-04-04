using System;
using System.Collections;
using System.Collections.Generic;
using ScheduleOne.Audio;
using ScheduleOne.DevUtilities;
using ScheduleOne.PlayerScripts;
using ScheduleOne.ScriptableObjects;
using ScheduleOne.Vision;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ScheduleOne.UI.Phone
{
	// Token: 0x02000A95 RID: 2709
	public class Phone : PlayerSingleton<Phone>
	{
		// Token: 0x17000A3A RID: 2618
		// (get) Token: 0x060048F2 RID: 18674 RVA: 0x001313C0 File Offset: 0x0012F5C0
		// (set) Token: 0x060048F3 RID: 18675 RVA: 0x001313C8 File Offset: 0x0012F5C8
		public bool IsOpen { get; protected set; }

		// Token: 0x17000A3B RID: 2619
		// (get) Token: 0x060048F4 RID: 18676 RVA: 0x001313D1 File Offset: 0x0012F5D1
		// (set) Token: 0x060048F5 RID: 18677 RVA: 0x001313D9 File Offset: 0x0012F5D9
		public bool isHorizontal { get; protected set; }

		// Token: 0x17000A3C RID: 2620
		// (get) Token: 0x060048F6 RID: 18678 RVA: 0x001313E2 File Offset: 0x0012F5E2
		// (set) Token: 0x060048F7 RID: 18679 RVA: 0x001313EA File Offset: 0x0012F5EA
		public bool isOpenable { get; protected set; } = true;

		// Token: 0x17000A3D RID: 2621
		// (get) Token: 0x060048F8 RID: 18680 RVA: 0x001313F3 File Offset: 0x0012F5F3
		// (set) Token: 0x060048F9 RID: 18681 RVA: 0x001313FB File Offset: 0x0012F5FB
		public bool FlashlightOn { get; protected set; }

		// Token: 0x17000A3E RID: 2622
		// (get) Token: 0x060048FA RID: 18682 RVA: 0x00131404 File Offset: 0x0012F604
		public float ScaledLookOffset
		{
			get
			{
				return Mathf.Lerp(this.LookOffsetMax, this.LookOffsetMin, CanvasScaler.NormalizedCanvasScaleFactor);
			}
		}

		// Token: 0x060048FB RID: 18683 RVA: 0x0013141C File Offset: 0x0012F61C
		protected override void Awake()
		{
			base.Awake();
			this.eventSystem = EventSystem.current;
		}

		// Token: 0x060048FC RID: 18684 RVA: 0x0013142F File Offset: 0x0012F62F
		public override void OnStartClient(bool IsOwner)
		{
			base.OnStartClient(IsOwner);
			if (!IsOwner)
			{
				UnityEngine.Object.Destroy(base.gameObject);
			}
		}

		// Token: 0x060048FD RID: 18685 RVA: 0x00131448 File Offset: 0x0012F648
		protected override void Start()
		{
			base.Start();
			if (this.flashlightVisibility == null)
			{
				this.flashlightVisibility = new VisibilityAttribute("Flashlight", 0f, 1f, -1);
			}
			base.transform.localRotation = this.orientation_Vertical.localRotation;
		}

		// Token: 0x060048FE RID: 18686 RVA: 0x00131494 File Offset: 0x0012F694
		protected virtual void Update()
		{
			if (this.IsOpen)
			{
				Singleton<HUD>.Instance.OnlineBalanceDisplay.Show();
			}
			if (!GameInput.IsTyping && !Singleton<PauseMenu>.Instance.IsPaused && (PlayerSingleton<PlayerCamera>.Instance.activeUIElementCount == 0 || this.IsOpen) && GameInput.GetButtonDown(GameInput.ButtonCode.ToggleFlashlight))
			{
				this.ToggleFlashlight();
			}
		}

		// Token: 0x060048FF RID: 18687 RVA: 0x001314EE File Offset: 0x0012F6EE
		protected override void OnDestroy()
		{
			base.OnDestroy();
			Phone.ActiveApp = null;
		}

		// Token: 0x06004900 RID: 18688 RVA: 0x001314FC File Offset: 0x0012F6FC
		private void ToggleFlashlight()
		{
			this.FlashlightOn = !this.FlashlightOn;
			this.PhoneFlashlight.SetActive(this.FlashlightOn);
			this.FlashlightToggleSound.PitchMultiplier = (this.FlashlightOn ? 1f : 0.9f);
			this.FlashlightToggleSound.Play();
			this.flashlightVisibility.pointsChange = (this.FlashlightOn ? 10f : 0f);
			this.flashlightVisibility.multiplier = (this.FlashlightOn ? 1.5f : 1f);
			Player.Local.SendFlashlightOn(this.FlashlightOn);
		}

		// Token: 0x06004901 RID: 18689 RVA: 0x001315A1 File Offset: 0x0012F7A1
		public void SetOpenable(bool o)
		{
			this.isOpenable = o;
		}

		// Token: 0x06004902 RID: 18690 RVA: 0x001315AC File Offset: 0x0012F7AC
		public void SetIsOpen(bool o)
		{
			this.IsOpen = o;
			if (this.IsOpen)
			{
				if (this.onPhoneOpened != null)
				{
					this.onPhoneOpened();
				}
				if (Phone.ActiveApp == null)
				{
					this.SetLookOffsetMultiplier(1f);
					return;
				}
			}
			else if (this.onPhoneClosed != null)
			{
				this.onPhoneClosed();
			}
		}

		// Token: 0x06004903 RID: 18691 RVA: 0x00131607 File Offset: 0x0012F807
		public void SetIsHorizontal(bool h)
		{
			this.isHorizontal = h;
			if (this.rotationCoroutine != null)
			{
				base.StopCoroutine(this.rotationCoroutine);
			}
			this.rotationCoroutine = base.StartCoroutine(this.SetIsHorizontal_Process(h));
		}

		// Token: 0x06004904 RID: 18692 RVA: 0x00131637 File Offset: 0x0012F837
		protected IEnumerator SetIsHorizontal_Process(bool h)
		{
			float adjustedRotationTime = this.rotationTime;
			Quaternion startRotation = base.transform.localRotation;
			Quaternion endRotation = Quaternion.identity;
			if (h)
			{
				endRotation = this.orientation_Horizontal.localRotation;
				adjustedRotationTime *= Quaternion.Angle(base.transform.localRotation, this.orientation_Horizontal.localRotation) / 90f;
			}
			else
			{
				endRotation = this.orientation_Vertical.localRotation;
				adjustedRotationTime *= Quaternion.Angle(base.transform.localRotation, this.orientation_Vertical.localRotation) / 90f;
			}
			for (float i = 0f; i < adjustedRotationTime; i += Time.deltaTime)
			{
				base.transform.localRotation = Quaternion.Lerp(startRotation, endRotation, i / adjustedRotationTime);
				yield return new WaitForEndOfFrame();
			}
			base.transform.localRotation = endRotation;
			this.rotationCoroutine = null;
			yield break;
		}

		// Token: 0x06004905 RID: 18693 RVA: 0x00131650 File Offset: 0x0012F850
		public void SetLookOffsetMultiplier(float multiplier)
		{
			float lookOffset_Process = this.ScaledLookOffset * multiplier;
			if (this.lookOffsetCoroutine != null)
			{
				base.StopCoroutine(this.lookOffsetCoroutine);
			}
			this.lookOffsetCoroutine = base.StartCoroutine(this.SetLookOffset_Process(lookOffset_Process));
		}

		// Token: 0x06004906 RID: 18694 RVA: 0x0013168D File Offset: 0x0012F88D
		public void RequestCloseApp()
		{
			if (Phone.ActiveApp != null && this.closeApps != null)
			{
				this.closeApps();
			}
		}

		// Token: 0x06004907 RID: 18695 RVA: 0x001316AF File Offset: 0x0012F8AF
		protected IEnumerator SetLookOffset_Process(float lookOffset)
		{
			float startOffset = base.transform.localPosition.z;
			float moveTime = 0.1f;
			for (float i = 0f; i < moveTime; i += Time.deltaTime)
			{
				base.transform.localPosition = new Vector3(base.transform.localPosition.x, base.transform.localPosition.y, Mathf.Lerp(startOffset, lookOffset, i / moveTime));
				yield return new WaitForEndOfFrame();
			}
			base.transform.localPosition = new Vector3(base.transform.localPosition.x, base.transform.localPosition.y, lookOffset);
			this.rotationCoroutine = null;
			yield break;
		}

		// Token: 0x06004908 RID: 18696 RVA: 0x001316C8 File Offset: 0x0012F8C8
		public bool MouseRaycast(out RaycastResult result)
		{
			PointerEventData pointerEventData = new PointerEventData(this.eventSystem);
			pointerEventData.position = Input.mousePosition;
			List<RaycastResult> list = new List<RaycastResult>();
			this.raycaster.Raycast(pointerEventData, list);
			if (list.Count > 0)
			{
				result = list[0];
			}
			else
			{
				result = default(RaycastResult);
			}
			return list.Count > 0;
		}

		// Token: 0x0400364E RID: 13902
		public static GameObject ActiveApp;

		// Token: 0x04003653 RID: 13907
		public PhoneCallData testData;

		// Token: 0x04003654 RID: 13908
		public CallerID testCalller;

		// Token: 0x04003655 RID: 13909
		[Header("References")]
		[SerializeField]
		protected GameObject phoneModel;

		// Token: 0x04003656 RID: 13910
		[SerializeField]
		protected Transform orientation_Vertical;

		// Token: 0x04003657 RID: 13911
		[SerializeField]
		protected Transform orientation_Horizontal;

		// Token: 0x04003658 RID: 13912
		[SerializeField]
		protected GraphicRaycaster raycaster;

		// Token: 0x04003659 RID: 13913
		[SerializeField]
		protected GameObject PhoneFlashlight;

		// Token: 0x0400365A RID: 13914
		[SerializeField]
		protected AudioSourceController FlashlightToggleSound;

		// Token: 0x0400365B RID: 13915
		[Header("Settings")]
		public float rotationTime = 0.1f;

		// Token: 0x0400365C RID: 13916
		public float LookOffsetMax = 0.45f;

		// Token: 0x0400365D RID: 13917
		public float LookOffsetMin = 0.29f;

		// Token: 0x0400365E RID: 13918
		public float OpenVerticalOffset = 0.1f;

		// Token: 0x0400365F RID: 13919
		public Action onPhoneOpened;

		// Token: 0x04003660 RID: 13920
		public Action onPhoneClosed;

		// Token: 0x04003661 RID: 13921
		public Action closeApps;

		// Token: 0x04003662 RID: 13922
		private EventSystem eventSystem;

		// Token: 0x04003663 RID: 13923
		private VisibilityAttribute flashlightVisibility;

		// Token: 0x04003664 RID: 13924
		private Coroutine rotationCoroutine;

		// Token: 0x04003665 RID: 13925
		private Coroutine lookOffsetCoroutine;
	}
}
