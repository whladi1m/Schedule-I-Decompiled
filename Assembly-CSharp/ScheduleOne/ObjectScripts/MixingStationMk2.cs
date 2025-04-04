using System;
using ScheduleOne.Product;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ScheduleOne.ObjectScripts
{
	// Token: 0x02000BB5 RID: 2997
	public class MixingStationMk2 : MixingStation
	{
		// Token: 0x06005359 RID: 21337 RVA: 0x0015F2FE File Offset: 0x0015D4FE
		protected override void MinPass()
		{
			base.MinPass();
			this.UpdateScreen();
		}

		// Token: 0x0600535A RID: 21338 RVA: 0x0015F30C File Offset: 0x0015D50C
		public override void MixingStart()
		{
			base.MixingStart();
			this.Animation.Play("Mixing station start");
			this.EnableScreen();
		}

		// Token: 0x0600535B RID: 21339 RVA: 0x0015F32B File Offset: 0x0015D52B
		public override void MixingDone()
		{
			base.MixingDone();
			this.Animation.Play("Mixing station end");
			this.DisableScreen();
		}

		// Token: 0x0600535C RID: 21340 RVA: 0x0015F34C File Offset: 0x0015D54C
		private void EnableScreen()
		{
			if (base.CurrentMixOperation == null)
			{
				return;
			}
			this.QuantityLabel.text = base.CurrentMixOperation.Quantity.ToString() + "x";
			ProductDefinition productDefinition;
			if (base.CurrentMixOperation.IsOutputKnown(out productDefinition))
			{
				this.OutputIcon.sprite = productDefinition.Icon;
				this.OutputIcon.color = Color.white;
				this.QuestionMark.gameObject.SetActive(false);
			}
			else
			{
				this.OutputIcon.sprite = Registry.GetItem(base.CurrentMixOperation.ProductID).Icon;
				this.OutputIcon.color = Color.black;
				this.QuestionMark.gameObject.SetActive(true);
			}
			this.UpdateScreen();
			this.ScreenCanvas.enabled = true;
		}

		// Token: 0x0600535D RID: 21341 RVA: 0x0015F420 File Offset: 0x0015D620
		private void UpdateScreen()
		{
			if (base.CurrentMixOperation == null)
			{
				return;
			}
			this.ProgressLabel.text = (base.GetMixTimeForCurrentOperation() - base.CurrentMixTime).ToString() + " mins remaining";
		}

		// Token: 0x0600535E RID: 21342 RVA: 0x0015F460 File Offset: 0x0015D660
		private void DisableScreen()
		{
			this.ScreenCanvas.enabled = false;
		}

		// Token: 0x06005360 RID: 21344 RVA: 0x0015F476 File Offset: 0x0015D676
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.ObjectScripts.MixingStationMk2Assembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.ObjectScripts.MixingStationMk2Assembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
		}

		// Token: 0x06005361 RID: 21345 RVA: 0x0015F48F File Offset: 0x0015D68F
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.ObjectScripts.MixingStationMk2Assembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.ObjectScripts.MixingStationMk2Assembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x06005362 RID: 21346 RVA: 0x0015F4A8 File Offset: 0x0015D6A8
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06005363 RID: 21347 RVA: 0x0015F4B6 File Offset: 0x0015D6B6
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			base.Awake();
			this.NetworkInitialize__Late();
		}

		// Token: 0x04003E1A RID: 15898
		public Animation Animation;

		// Token: 0x04003E1B RID: 15899
		[Header("Screen")]
		public Canvas ScreenCanvas;

		// Token: 0x04003E1C RID: 15900
		public Image OutputIcon;

		// Token: 0x04003E1D RID: 15901
		public RectTransform QuestionMark;

		// Token: 0x04003E1E RID: 15902
		public TextMeshProUGUI QuantityLabel;

		// Token: 0x04003E1F RID: 15903
		public TextMeshProUGUI ProgressLabel;

		// Token: 0x04003E20 RID: 15904
		private bool dll_Excuted;

		// Token: 0x04003E21 RID: 15905
		private bool dll_Excuted;
	}
}
