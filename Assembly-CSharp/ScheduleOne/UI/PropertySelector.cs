using System;
using ScheduleOne.DevUtilities;
using ScheduleOne.PlayerScripts;
using ScheduleOne.Property;
using UnityEngine;

namespace ScheduleOne.UI
{
	// Token: 0x02000A18 RID: 2584
	public class PropertySelector : MonoBehaviour
	{
		// Token: 0x170009DF RID: 2527
		// (get) Token: 0x060045B7 RID: 17847 RVA: 0x00124535 File Offset: 0x00122735
		public bool isOpen
		{
			get
			{
				return this.container.activeSelf;
			}
		}

		// Token: 0x060045B8 RID: 17848 RVA: 0x00124542 File Offset: 0x00122742
		protected virtual void Awake()
		{
			Property.onPropertyAcquired = (Property.PropertyChange)Delegate.Combine(Property.onPropertyAcquired, new Property.PropertyChange(this.PropertyAcquired));
			this.container.SetActive(false);
		}

		// Token: 0x060045B9 RID: 17849 RVA: 0x00124570 File Offset: 0x00122770
		protected virtual void Start()
		{
			GameInput.RegisterExitListener(new GameInput.ExitDelegate(this.Exit), 5);
		}

		// Token: 0x060045BA RID: 17850 RVA: 0x00124585 File Offset: 0x00122785
		public virtual void Exit(ExitAction exit)
		{
			if (exit.used)
			{
				return;
			}
			if (exit.exitType == ExitType.RightClick)
			{
				return;
			}
			if (this.container.activeSelf)
			{
				exit.used = true;
				this.Close(true);
			}
		}

		// Token: 0x060045BB RID: 17851 RVA: 0x001245B4 File Offset: 0x001227B4
		public void OpenSelector(PropertySelector.PropertySelected p)
		{
			this.pCallback = p;
			this.container.SetActive(true);
			PlayerSingleton<PlayerCamera>.Instance.AddActiveUIElement(base.name);
			PlayerSingleton<PlayerMovement>.Instance.canMove = false;
			PlayerSingleton<PlayerCamera>.Instance.SetCanLook(false);
			PlayerSingleton<PlayerCamera>.Instance.FreeMouse();
			PlayerSingleton<PlayerInventory>.Instance.SetInventoryEnabled(false);
			Singleton<HUD>.Instance.SetCrosshairVisible(false);
		}

		// Token: 0x060045BC RID: 17852 RVA: 0x000045B1 File Offset: 0x000027B1
		private void PropertyAcquired(Property p)
		{
		}

		// Token: 0x060045BD RID: 17853 RVA: 0x0012461A File Offset: 0x0012281A
		private void SelectProperty(Property p)
		{
			this.pCallback(p);
			this.Close(false);
		}

		// Token: 0x060045BE RID: 17854 RVA: 0x00124630 File Offset: 0x00122830
		private void Close(bool reenableShit)
		{
			this.container.SetActive(false);
			PlayerSingleton<PlayerCamera>.Instance.RemoveActiveUIElement(base.name);
			if (reenableShit)
			{
				PlayerSingleton<PlayerMovement>.Instance.canMove = true;
				PlayerSingleton<PlayerCamera>.Instance.SetCanLook(true);
				PlayerSingleton<PlayerCamera>.Instance.LockMouse();
				PlayerSingleton<PlayerInventory>.Instance.SetInventoryEnabled(true);
				Singleton<HUD>.Instance.SetCrosshairVisible(true);
			}
		}

		// Token: 0x0400338F RID: 13199
		[Header("References")]
		[SerializeField]
		protected GameObject container;

		// Token: 0x04003390 RID: 13200
		[SerializeField]
		protected RectTransform buttonContainer;

		// Token: 0x04003391 RID: 13201
		[Header("Prefabs")]
		[SerializeField]
		protected GameObject buttonPrefab;

		// Token: 0x04003392 RID: 13202
		private PropertySelector.PropertySelected pCallback;

		// Token: 0x02000A19 RID: 2585
		// (Invoke) Token: 0x060045C1 RID: 17857
		public delegate void PropertySelected(Property p);
	}
}
