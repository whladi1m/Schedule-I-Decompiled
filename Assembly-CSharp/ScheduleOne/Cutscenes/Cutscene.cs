using System;
using ScheduleOne.DevUtilities;
using ScheduleOne.PlayerScripts;
using ScheduleOne.UI;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.Cutscenes
{
	// Token: 0x0200070A RID: 1802
	[RequireComponent(typeof(Animation))]
	public class Cutscene : MonoBehaviour
	{
		// Token: 0x17000729 RID: 1833
		// (get) Token: 0x060030D7 RID: 12503 RVA: 0x000CB05D File Offset: 0x000C925D
		// (set) Token: 0x060030D8 RID: 12504 RVA: 0x000CB065 File Offset: 0x000C9265
		public bool IsPlaying { get; private set; }

		// Token: 0x060030D9 RID: 12505 RVA: 0x000CB06E File Offset: 0x000C926E
		protected virtual void Awake()
		{
			this.animation = base.GetComponent<Animation>();
		}

		// Token: 0x060030DA RID: 12506 RVA: 0x000CB07C File Offset: 0x000C927C
		private void LateUpdate()
		{
			if (this.IsPlaying)
			{
				PlayerSingleton<PlayerCamera>.Instance.transform.position = this.CameraControl.position;
				PlayerSingleton<PlayerCamera>.Instance.transform.rotation = this.CameraControl.rotation;
			}
		}

		// Token: 0x060030DB RID: 12507 RVA: 0x000CB0BC File Offset: 0x000C92BC
		public virtual void Play()
		{
			Console.Log("Playing cutscene: " + this.Name, null);
			this.animation.Play();
			this.IsPlaying = true;
			if (this.onPlay != null)
			{
				this.onPlay.Invoke();
			}
			PlayerSingleton<PlayerCamera>.Instance.AddActiveUIElement("Cutscene (" + this.Name + ")");
			PlayerSingleton<PlayerCamera>.Instance.OverrideTransform(this.CameraControl.position, this.CameraControl.rotation, 0f, false);
			Singleton<HUD>.Instance.canvas.enabled = false;
			if (this.DisablePlayerControl)
			{
				PlayerSingleton<PlayerMovement>.Instance.canMove = false;
				PlayerSingleton<PlayerInventory>.Instance.SetInventoryEnabled(false);
			}
			if (this.OverrideFOV)
			{
				PlayerSingleton<PlayerCamera>.Instance.OverrideFOV(this.CameraFOV, 0f);
			}
		}

		// Token: 0x060030DC RID: 12508 RVA: 0x000CB198 File Offset: 0x000C9398
		public void InvokeEnd()
		{
			Console.Log("Cutscene ended: " + this.Name, null);
			this.animation.Stop();
			this.IsPlaying = false;
			if (this.onEnd != null)
			{
				this.onEnd.Invoke();
			}
			PlayerSingleton<PlayerCamera>.Instance.RemoveActiveUIElement("Cutscene (" + this.Name + ")");
			PlayerSingleton<PlayerCamera>.Instance.StopFOVOverride(0.25f);
			PlayerSingleton<PlayerCamera>.Instance.StopTransformOverride(0f, true, false);
			PlayerSingleton<PlayerCamera>.Instance.LockMouse();
			Singleton<HUD>.Instance.canvas.enabled = true;
			PlayerSingleton<PlayerMovement>.Instance.canMove = true;
			PlayerSingleton<PlayerInventory>.Instance.SetInventoryEnabled(true);
		}

		// Token: 0x040022EE RID: 8942
		[Header("Settings")]
		public string Name = "Cutscene";

		// Token: 0x040022EF RID: 8943
		public bool DisablePlayerControl = true;

		// Token: 0x040022F0 RID: 8944
		public bool OverrideFOV;

		// Token: 0x040022F1 RID: 8945
		public float CameraFOV = 70f;

		// Token: 0x040022F2 RID: 8946
		[Header("References")]
		public Transform CameraControl;

		// Token: 0x040022F3 RID: 8947
		[Header("Events")]
		public UnityEvent onPlay;

		// Token: 0x040022F4 RID: 8948
		public UnityEvent onEnd;

		// Token: 0x040022F5 RID: 8949
		private Animation animation;
	}
}
