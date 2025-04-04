using System;
using ScheduleOne.Audio;
using ScheduleOne.DevUtilities;
using ScheduleOne.ItemFramework;
using ScheduleOne.NPCs;
using ScheduleOne.Packaging;
using ScheduleOne.Persistence.Datas;
using ScheduleOne.PlayerScripts;
using ScheduleOne.Product.Packaging;
using UnityEngine;

namespace ScheduleOne.Product
{
	// Token: 0x020008C3 RID: 2243
	[Serializable]
	public class CocaineInstance : ProductItemInstance
	{
		// Token: 0x06003CD0 RID: 15568 RVA: 0x000FF41B File Offset: 0x000FD61B
		public CocaineInstance()
		{
		}

		// Token: 0x06003CD1 RID: 15569 RVA: 0x000FF423 File Offset: 0x000FD623
		public CocaineInstance(ItemDefinition definition, int quantity, EQuality quality, PackagingDefinition packaging = null) : base(definition, quantity, quality, packaging)
		{
		}

		// Token: 0x06003CD2 RID: 15570 RVA: 0x000FF430 File Offset: 0x000FD630
		public override ItemInstance GetCopy(int overrideQuantity = -1)
		{
			int quantity = this.Quantity;
			if (overrideQuantity != -1)
			{
				quantity = overrideQuantity;
			}
			return new CocaineInstance(base.Definition, quantity, this.Quality, base.AppliedPackaging);
		}

		// Token: 0x06003CD3 RID: 15571 RVA: 0x000FF464 File Offset: 0x000FD664
		public override void SetupPackagingVisuals(FilledPackagingVisuals visuals)
		{
			base.SetupPackagingVisuals(visuals);
			if (visuals == null)
			{
				Console.LogError("CocaineInstance: visuals is null!", null);
				return;
			}
			CocaineDefinition cocaineDefinition = base.Definition as CocaineDefinition;
			if (cocaineDefinition == null)
			{
				string str = "CocaineInstance: definition is null! Type: ";
				ItemDefinition definition = base.Definition;
				Console.LogError(str + ((definition != null) ? definition.ToString() : null), null);
				return;
			}
			MeshRenderer[] rockMeshes = visuals.cocaineVisuals.RockMeshes;
			for (int i = 0; i < rockMeshes.Length; i++)
			{
				rockMeshes[i].material = cocaineDefinition.RockMaterial;
			}
			visuals.cocaineVisuals.Container.gameObject.SetActive(true);
		}

		// Token: 0x06003CD4 RID: 15572 RVA: 0x000FF503 File Offset: 0x000FD703
		public override ItemData GetItemData()
		{
			return new CocaineData(base.Definition.ID, this.Quantity, this.Quality.ToString(), this.PackagingID);
		}

		// Token: 0x06003CD5 RID: 15573 RVA: 0x000FF534 File Offset: 0x000FD734
		public override void ApplyEffectsToNPC(NPC npc)
		{
			npc.Avatar.EmotionManager.AddEmotionOverride("Cocaine", this.Name, 0f, 0);
			npc.Avatar.Eyes.OverrideEyeballTint(new Color32(200, 240, byte.MaxValue, byte.MaxValue));
			npc.Avatar.Eyes.SetPupilDilation(1f, false);
			npc.Avatar.Eyes.ForceBlink();
			npc.Movement.MoveSpeedMultiplier = 1.25f;
			npc.Avatar.LookController.LookLerpSpeed = 10f;
			base.ApplyEffectsToNPC(npc);
		}

		// Token: 0x06003CD6 RID: 15574 RVA: 0x000FF5E4 File Offset: 0x000FD7E4
		public override void ClearEffectsFromNPC(NPC npc)
		{
			npc.Avatar.EmotionManager.RemoveEmotionOverride(this.Name);
			npc.Avatar.Eyes.ResetEyeballTint();
			npc.Avatar.Eyes.ResetEyeLids();
			npc.Avatar.Eyes.ResetPupilDilation();
			npc.Avatar.Eyes.ForceBlink();
			npc.Movement.MoveSpeedMultiplier = 1f;
			npc.Avatar.LookController.LookLerpSpeed = 3f;
			base.ClearEffectsFromNPC(npc);
		}

		// Token: 0x06003CD7 RID: 15575 RVA: 0x000FF674 File Offset: 0x000FD874
		public override void ApplyEffectsToPlayer(Player player)
		{
			player.Avatar.EmotionManager.AddEmotionOverride("Cocaine", this.Name, 0f, 0);
			player.Avatar.Eyes.OverrideEyeballTint(new Color32(200, 240, byte.MaxValue, byte.MaxValue));
			player.Avatar.Eyes.SetPupilDilation(1f, false);
			player.Avatar.Eyes.ForceBlink();
			player.Avatar.LookController.LookLerpSpeed = 10f;
			if (player.IsOwner)
			{
				PlayerSingleton<PlayerCamera>.Instance.CocaineVisuals = true;
				PlayerSingleton<PlayerCamera>.Instance.FoVChangeSmoother.AddOverride(10f, 6, "Cocaine");
				Singleton<MusicPlayer>.Instance.SetMusicDistorted(true, 5f);
				Singleton<AudioManager>.Instance.SetDistorted(true, 5f);
			}
			base.ApplyEffectsToPlayer(player);
		}

		// Token: 0x06003CD8 RID: 15576 RVA: 0x000FF760 File Offset: 0x000FD960
		public override void ClearEffectsFromPlayer(Player Player)
		{
			Player.Avatar.EmotionManager.RemoveEmotionOverride(this.Name);
			Player.Avatar.Eyes.ResetEyeballTint();
			Player.Avatar.Eyes.ResetEyeLids();
			Player.Avatar.Eyes.ResetPupilDilation();
			Player.Avatar.Eyes.ForceBlink();
			Player.Avatar.LookController.LookLerpSpeed = 3f;
			if (Player.IsOwner)
			{
				PlayerSingleton<PlayerCamera>.Instance.CocaineVisuals = false;
				PlayerSingleton<PlayerCamera>.Instance.FoVChangeSmoother.RemoveOverride("Cocaine");
				Singleton<MusicPlayer>.Instance.SetMusicDistorted(false, 5f);
				Singleton<AudioManager>.Instance.SetDistorted(false, 5f);
			}
			base.ClearEffectsFromPlayer(Player);
		}
	}
}
