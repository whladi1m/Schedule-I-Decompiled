using System;
using EasyButtons;
using ScheduleOne.Audio;
using UnityEngine;

namespace ScheduleOne.Storage
{
	// Token: 0x0200089F RID: 2207
	public class StorageDoorAnimation : MonoBehaviour
	{
		// Token: 0x17000878 RID: 2168
		// (get) Token: 0x06003C2F RID: 15407 RVA: 0x000FD4E0 File Offset: 0x000FB6E0
		// (set) Token: 0x06003C30 RID: 15408 RVA: 0x000FD4E8 File Offset: 0x000FB6E8
		public bool IsOpen { get; protected set; }

		// Token: 0x06003C31 RID: 15409 RVA: 0x000FD4F1 File Offset: 0x000FB6F1
		private void Start()
		{
			if (this.ItemContainer != null)
			{
				this.ItemContainer.gameObject.SetActive(false);
			}
		}

		// Token: 0x06003C32 RID: 15410 RVA: 0x000FD512 File Offset: 0x000FB712
		[Button]
		public void Open()
		{
			this.SetIsOpen(true);
		}

		// Token: 0x06003C33 RID: 15411 RVA: 0x000FD51B File Offset: 0x000FB71B
		[Button]
		public void Close()
		{
			this.SetIsOpen(false);
		}

		// Token: 0x06003C34 RID: 15412 RVA: 0x000FD524 File Offset: 0x000FB724
		public void SetIsOpen(bool open)
		{
			if (this.overriddeIsOpen)
			{
				open = this.overrideState;
			}
			if (this.IsOpen == open)
			{
				return;
			}
			if (open && this.ItemContainer != null)
			{
				this.ItemContainer.gameObject.SetActive(true);
			}
			this.IsOpen = open;
			for (int i = 0; i < this.Anims.Length; i++)
			{
				this.Anims[i].Play(this.IsOpen ? this.OpenAnim.name : this.CloseAnim.name);
			}
			if (this.IsOpen)
			{
				if (this.OpenSound != null)
				{
					this.OpenSound.Play();
				}
			}
			else if (this.CloseSound != null)
			{
				this.CloseSound.Play();
			}
			if (!open)
			{
				base.Invoke("DisableItems", this.CloseAnim.length);
			}
		}

		// Token: 0x06003C35 RID: 15413 RVA: 0x000FD60A File Offset: 0x000FB80A
		private void DisableItems()
		{
			if (this.IsOpen)
			{
				return;
			}
			if (this.ItemContainer != null)
			{
				this.ItemContainer.gameObject.SetActive(false);
			}
		}

		// Token: 0x06003C36 RID: 15414 RVA: 0x000FD634 File Offset: 0x000FB834
		public void OverrideState(bool open)
		{
			this.overriddeIsOpen = true;
			this.overrideState = open;
			this.SetIsOpen(open);
		}

		// Token: 0x06003C37 RID: 15415 RVA: 0x000FD64B File Offset: 0x000FB84B
		public void ResetOverride()
		{
			this.overriddeIsOpen = false;
		}

		// Token: 0x04002B56 RID: 11094
		private bool overriddeIsOpen;

		// Token: 0x04002B57 RID: 11095
		private bool overrideState;

		// Token: 0x04002B58 RID: 11096
		public Transform ItemContainer;

		// Token: 0x04002B59 RID: 11097
		[Header("Animations")]
		public Animation[] Anims;

		// Token: 0x04002B5A RID: 11098
		public AnimationClip OpenAnim;

		// Token: 0x04002B5B RID: 11099
		public AnimationClip CloseAnim;

		// Token: 0x04002B5C RID: 11100
		public AudioSourceController OpenSound;

		// Token: 0x04002B5D RID: 11101
		public AudioSourceController CloseSound;
	}
}
