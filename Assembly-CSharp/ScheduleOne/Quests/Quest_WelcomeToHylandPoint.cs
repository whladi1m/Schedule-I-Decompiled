using System;
using System.Collections;
using System.Runtime.CompilerServices;
using EasyButtons;
using FishNet;
using ScheduleOne.DevUtilities;
using ScheduleOne.NPCs.CharacterClasses;
using ScheduleOne.PlayerScripts;
using ScheduleOne.Property;
using ScheduleOne.UI.Input;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace ScheduleOne.Quests
{
	// Token: 0x020002F9 RID: 761
	public class Quest_WelcomeToHylandPoint : Quest
	{
		// Token: 0x060010ED RID: 4333 RVA: 0x0004B984 File Offset: 0x00049B84
		protected override void MinPass()
		{
			base.MinPass();
			if (base.QuestState == EQuestState.Active && this.ReadMessagesQuest.State == EQuestState.Active && this.Nelson.MSGConversation != null && this.Nelson.MSGConversation.read)
			{
				this.ReadMessagesQuest.Complete();
			}
		}

		// Token: 0x060010EE RID: 4334 RVA: 0x0004B9D8 File Offset: 0x00049BD8
		private void Update()
		{
			if (base.QuestState == EQuestState.Active && this.ReturnToRVQuest.State == EQuestState.Active && InstanceFinder.IsServer)
			{
				float num;
				Player closestPlayer = Player.GetClosestPlayer(this.RV.transform.position, out num, null);
				if (num < this.ExplosionMinDist)
				{
					this.ReturnToRVQuest.Complete();
					return;
				}
				if (num < this.ExplosionMaxDist)
				{
					if (Vector3.Angle(closestPlayer.MimicCamera.forward, this.RV.transform.position - closestPlayer.MimicCamera.position) < 60f)
					{
						this.cameraLookTime += Time.deltaTime;
						if (this.cameraLookTime > 0.4f)
						{
							this.ReturnToRVQuest.Complete();
							return;
						}
					}
					else
					{
						this.cameraLookTime = 0f;
					}
				}
			}
		}

		// Token: 0x060010EF RID: 4335 RVA: 0x0004BAB0 File Offset: 0x00049CB0
		[Button]
		public void Explode()
		{
			Console.Log("RV exploding!", null);
			if (this.onExplode != null)
			{
				this.onExplode.Invoke();
			}
			base.StartCoroutine(Quest_WelcomeToHylandPoint.<Explode>g__Shake|11_0());
		}

		// Token: 0x060010F0 RID: 4336 RVA: 0x0004BADC File Offset: 0x00049CDC
		public override void SetQuestState(EQuestState state, bool network = true)
		{
			base.SetQuestState(state, network);
			if (state == EQuestState.Active)
			{
				string text;
				string controlPath;
				Singleton<GameInput>.Instance.GetAction(GameInput.ButtonCode.TogglePhone).GetBindingDisplayString(0, out text, out controlPath, (InputBinding.DisplayStringOptions)0);
				string displayNameForControlPath = Singleton<InputPromptsManager>.Instance.GetDisplayNameForControlPath(controlPath);
				this.ReadMessagesQuest.SetEntryTitle("Open your phone (press " + displayNameForControlPath + ") and read your messages");
			}
		}

		// Token: 0x060010F2 RID: 4338 RVA: 0x0004BB52 File Offset: 0x00049D52
		[CompilerGenerated]
		internal static IEnumerator <Explode>g__Shake|11_0()
		{
			yield return new WaitForSeconds(0.35f);
			PlayerSingleton<PlayerCamera>.Instance.StartCameraShake(2f, 1f, true);
			yield break;
		}

		// Token: 0x0400110E RID: 4366
		public QuestEntry ReturnToRVQuest;

		// Token: 0x0400110F RID: 4367
		public QuestEntry ReadMessagesQuest;

		// Token: 0x04001110 RID: 4368
		public RV RV;

		// Token: 0x04001111 RID: 4369
		public UncleNelson Nelson;

		// Token: 0x04001112 RID: 4370
		[Header("Settings")]
		public float ExplosionMaxDist = 25f;

		// Token: 0x04001113 RID: 4371
		public float ExplosionMinDist = 50f;

		// Token: 0x04001114 RID: 4372
		public UnityEvent onExplode;

		// Token: 0x04001115 RID: 4373
		private bool exploded;

		// Token: 0x04001116 RID: 4374
		private float cameraLookTime;
	}
}
