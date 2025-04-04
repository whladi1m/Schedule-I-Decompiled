using System;
using ScheduleOne.Audio;
using ScheduleOne.AvatarFramework;
using ScheduleOne.DevUtilities;
using ScheduleOne.FX;
using ScheduleOne.ItemFramework;
using ScheduleOne.NPCs;
using ScheduleOne.Packaging;
using ScheduleOne.Persistence.Datas;
using ScheduleOne.PlayerScripts;
using ScheduleOne.Product.Packaging;
using UnityEngine;

namespace ScheduleOne.Product
{
	// Token: 0x020008CD RID: 2253
	[Serializable]
	public class MethInstance : ProductItemInstance
	{
		// Token: 0x06003CED RID: 15597 RVA: 0x000FF41B File Offset: 0x000FD61B
		public MethInstance()
		{
		}

		// Token: 0x06003CEE RID: 15598 RVA: 0x000FF423 File Offset: 0x000FD623
		public MethInstance(ItemDefinition definition, int quantity, EQuality quality, PackagingDefinition packaging = null) : base(definition, quantity, quality, packaging)
		{
		}

		// Token: 0x06003CEF RID: 15599 RVA: 0x000FFB3C File Offset: 0x000FDD3C
		public override ItemInstance GetCopy(int overrideQuantity = -1)
		{
			int quantity = this.Quantity;
			if (overrideQuantity != -1)
			{
				quantity = overrideQuantity;
			}
			return new MethInstance(base.Definition, quantity, this.Quality, base.AppliedPackaging);
		}

		// Token: 0x06003CF0 RID: 15600 RVA: 0x000FFB70 File Offset: 0x000FDD70
		public override void SetupPackagingVisuals(FilledPackagingVisuals visuals)
		{
			base.SetupPackagingVisuals(visuals);
			if (visuals == null)
			{
				Console.LogError("MethInstance: visuals is null!", null);
				return;
			}
			MethDefinition methDefinition = base.Definition as MethDefinition;
			if (methDefinition == null)
			{
				string str = "MethInstance: definition is null! Type: ";
				ItemDefinition definition = base.Definition;
				Console.LogError(str + ((definition != null) ? definition.ToString() : null), null);
				return;
			}
			MeshRenderer[] crystalMeshes = visuals.methVisuals.CrystalMeshes;
			for (int i = 0; i < crystalMeshes.Length; i++)
			{
				crystalMeshes[i].material = methDefinition.CrystalMaterial;
			}
			visuals.methVisuals.Container.gameObject.SetActive(true);
		}

		// Token: 0x06003CF1 RID: 15601 RVA: 0x000FFC0F File Offset: 0x000FDE0F
		public override ItemData GetItemData()
		{
			return new MethData(base.Definition.ID, this.Quantity, this.Quality.ToString(), this.PackagingID);
		}

		// Token: 0x06003CF2 RID: 15602 RVA: 0x000FFC40 File Offset: 0x000FDE40
		public override void ApplyEffectsToNPC(NPC npc)
		{
			Console.Log("Applying meth effects to NPC: " + npc.fullName, null);
			npc.Avatar.EmotionManager.AddEmotionOverride("Meth", this.Name, 0f, 0);
			npc.Avatar.Eyes.OverrideEyeballTint(new Color32(165, 112, 86, byte.MaxValue));
			npc.Avatar.Eyes.OverrideEyeLids(new Eye.EyeLidConfiguration
			{
				bottomLidOpen = 0.5f,
				topLidOpen = 0.1f
			});
			npc.Avatar.Eyes.SetPupilDilation(0.1f, false);
			npc.Avatar.Eyes.ForceBlink();
			npc.OverrideAggression(1f);
			base.ApplyEffectsToNPC(npc);
		}

		// Token: 0x06003CF3 RID: 15603 RVA: 0x000FFD18 File Offset: 0x000FDF18
		public override void ClearEffectsFromNPC(NPC npc)
		{
			npc.Avatar.EmotionManager.RemoveEmotionOverride(this.Name);
			npc.Avatar.Eyes.ResetEyeballTint();
			npc.Avatar.Eyes.ResetEyeLids();
			npc.Avatar.Eyes.ResetPupilDilation();
			npc.Avatar.Eyes.ForceBlink();
			npc.ResetAggression();
			base.ClearEffectsFromNPC(npc);
		}

		// Token: 0x06003CF4 RID: 15604 RVA: 0x000FFD88 File Offset: 0x000FDF88
		public override void ApplyEffectsToPlayer(Player player)
		{
			player.Avatar.EmotionManager.AddEmotionOverride("Meth", this.Name, 0f, 0);
			player.Avatar.Eyes.OverrideEyeballTint(new Color32(165, 112, 86, byte.MaxValue));
			player.Avatar.Eyes.SetPupilDilation(0.1f, false);
			player.Avatar.Eyes.ForceBlink();
			if (player.IsOwner)
			{
				PlayerSingleton<PlayerCamera>.Instance.MethVisuals = true;
				Singleton<PostProcessingManager>.Instance.ColorFilterController.AddOverride((this.definition as MethDefinition).TintColor, 1, "Meth");
				Singleton<MusicPlayer>.Instance.SetMusicDistorted(true, 5f);
				Singleton<AudioManager>.Instance.SetDistorted(true, 5f);
			}
			base.ApplyEffectsToPlayer(player);
		}

		// Token: 0x06003CF5 RID: 15605 RVA: 0x000FFE64 File Offset: 0x000FE064
		public override void ClearEffectsFromPlayer(Player Player)
		{
			Player.Avatar.EmotionManager.RemoveEmotionOverride(this.Name);
			Player.Avatar.Eyes.ResetEyeballTint();
			Player.Avatar.Eyes.ResetEyeLids();
			Player.Avatar.Eyes.ResetPupilDilation();
			Player.Avatar.Eyes.ForceBlink();
			if (Player.IsOwner)
			{
				PlayerSingleton<PlayerCamera>.Instance.MethVisuals = false;
				Singleton<PostProcessingManager>.Instance.ColorFilterController.RemoveOverride("Meth");
				Singleton<MusicPlayer>.Instance.SetMusicDistorted(false, 5f);
				Singleton<AudioManager>.Instance.SetDistorted(false, 5f);
			}
			base.ClearEffectsFromPlayer(Player);
		}
	}
}
