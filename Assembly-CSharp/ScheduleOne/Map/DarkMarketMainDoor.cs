using System;
using System.Collections;
using System.Runtime.CompilerServices;
using ScheduleOne.DevUtilities;
using ScheduleOne.Dialogue;
using ScheduleOne.Doors;
using ScheduleOne.Interaction;
using ScheduleOne.Levelling;
using ScheduleOne.NPCs.CharacterClasses;
using ScheduleOne.PlayerScripts;
using ScheduleOne.UI;
using UnityEngine;

namespace ScheduleOne.Map
{
	// Token: 0x02000BE7 RID: 3047
	public class DarkMarketMainDoor : MonoBehaviour
	{
		// Token: 0x17000C01 RID: 3073
		// (get) Token: 0x0600556E RID: 21870 RVA: 0x001676FC File Offset: 0x001658FC
		// (set) Token: 0x0600556F RID: 21871 RVA: 0x00167704 File Offset: 0x00165904
		public bool KnockingEnabled { get; private set; } = true;

		// Token: 0x06005570 RID: 21872 RVA: 0x0016770D File Offset: 0x0016590D
		private void Start()
		{
			this.Igor.gameObject.SetActive(false);
		}

		// Token: 0x06005571 RID: 21873 RVA: 0x00167720 File Offset: 0x00165920
		public void SetKnockingEnabled(bool enabled)
		{
			this.InteractableObject.gameObject.SetActive(enabled);
			this.KnockingEnabled = enabled;
		}

		// Token: 0x06005572 RID: 21874 RVA: 0x0016773C File Offset: 0x0016593C
		public void Hovered()
		{
			if (this.KnockingEnabled && this.knockRoutine == null && Player.Local.CrimeData.CurrentPursuitLevel == PlayerCrimeData.EPursuitLevel.None)
			{
				this.InteractableObject.SetMessage("Knock");
				this.InteractableObject.SetInteractableState(InteractableObject.EInteractableState.Default);
				return;
			}
			this.InteractableObject.SetInteractableState(InteractableObject.EInteractableState.Disabled);
		}

		// Token: 0x06005573 RID: 21875 RVA: 0x00167793 File Offset: 0x00165993
		public void Interacted()
		{
			this.Knocked();
		}

		// Token: 0x06005574 RID: 21876 RVA: 0x0016779B File Offset: 0x0016599B
		private void Knocked()
		{
			this.knockRoutine = base.StartCoroutine(this.<Knocked>g__Knock|16_0());
		}

		// Token: 0x06005576 RID: 21878 RVA: 0x001677BE File Offset: 0x001659BE
		[CompilerGenerated]
		private IEnumerator <Knocked>g__Knock|16_0()
		{
			this.KnockSound.Play();
			this.Igor.gameObject.SetActive(true);
			this.Igor.Avatar.LookController.ForceLookTarget = PlayerSingleton<PlayerCamera>.Instance.transform;
			yield return new WaitForSeconds(0.75f);
			this.Igor.gameObject.SetActive(true);
			this.Peephole.Open();
			yield return new WaitForSeconds(0.3f);
			bool shouldUnlock = false;
			if (Vector3.Distance(Player.Local.transform.position, base.transform.position) < 3f)
			{
				shouldUnlock = (NetworkSingleton<LevelManager>.Instance.GetFullRank() >= NetworkSingleton<DarkMarket>.Instance.UnlockRank);
				DialogueContainer container = shouldUnlock ? (NetworkSingleton<DarkMarket>.Instance.IsOpen ? this.SuccessDialogue : this.SuccessDialogueNotOpen) : this.FailDialogue;
				this.Igor.dialogueHandler.InitializeDialogue(container);
				yield return new WaitUntil(() => !this.Igor.dialogueHandler.IsPlaying);
			}
			else
			{
				yield return new WaitForSeconds(1f);
			}
			yield return new WaitForSeconds(0.2f);
			this.Peephole.Close();
			yield return new WaitForSeconds(0.2f);
			if (shouldUnlock)
			{
				NetworkSingleton<DarkMarket>.Instance.SendUnlocked();
			}
			else
			{
				HintDisplay instance = Singleton<HintDisplay>.Instance;
				string str = "Reach the rank of <h1>";
				FullRank unlockRank = NetworkSingleton<DarkMarket>.Instance.UnlockRank;
				instance.ShowHint(str + unlockRank.ToString() + "</h> to access this area.", 15f);
			}
			yield return new WaitForSeconds(0.5f);
			this.Igor.gameObject.SetActive(false);
			this.knockRoutine = null;
			yield break;
		}

		// Token: 0x04003F63 RID: 16227
		public AudioSource KnockSound;

		// Token: 0x04003F64 RID: 16228
		public InteractableObject InteractableObject;

		// Token: 0x04003F65 RID: 16229
		public Peephole Peephole;

		// Token: 0x04003F66 RID: 16230
		public Igor Igor;

		// Token: 0x04003F67 RID: 16231
		public DialogueContainer FailDialogue;

		// Token: 0x04003F68 RID: 16232
		public DialogueContainer SuccessDialogue;

		// Token: 0x04003F69 RID: 16233
		public DialogueContainer SuccessDialogueNotOpen;

		// Token: 0x04003F6A RID: 16234
		private Coroutine knockRoutine;
	}
}
