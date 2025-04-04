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
	// Token: 0x020008F5 RID: 2293
	[Serializable]
	public class WeedInstance : ProductItemInstance
	{
		// Token: 0x06003E30 RID: 15920 RVA: 0x000FF41B File Offset: 0x000FD61B
		public WeedInstance()
		{
		}

		// Token: 0x06003E31 RID: 15921 RVA: 0x000FF423 File Offset: 0x000FD623
		public WeedInstance(ItemDefinition definition, int quantity, EQuality quality, PackagingDefinition packaging = null) : base(definition, quantity, quality, packaging)
		{
		}

		// Token: 0x06003E32 RID: 15922 RVA: 0x00106708 File Offset: 0x00104908
		public override ItemInstance GetCopy(int overrideQuantity = -1)
		{
			int quantity = this.Quantity;
			if (overrideQuantity != -1)
			{
				quantity = overrideQuantity;
			}
			return new WeedInstance(base.Definition, quantity, this.Quality, base.AppliedPackaging);
		}

		// Token: 0x06003E33 RID: 15923 RVA: 0x0010673C File Offset: 0x0010493C
		public override void SetupPackagingVisuals(FilledPackagingVisuals visuals)
		{
			base.SetupPackagingVisuals(visuals);
			if (visuals == null)
			{
				Console.LogError("WeedInstance: visuals is null!", null);
				return;
			}
			WeedDefinition weedDefinition = base.Definition as WeedDefinition;
			if (weedDefinition == null)
			{
				string str = "WeedInstance: definition is null! Type: ";
				ItemDefinition definition = base.Definition;
				Console.LogError(str + ((definition != null) ? definition.ToString() : null), null);
				return;
			}
			foreach (FilledPackagingVisuals.MeshIndexPair meshIndexPair in visuals.weedVisuals.MainMeshes)
			{
				Material[] materials = meshIndexPair.Mesh.materials;
				materials[meshIndexPair.MaterialIndex] = weedDefinition.MainMat;
				meshIndexPair.Mesh.materials = materials;
			}
			foreach (FilledPackagingVisuals.MeshIndexPair meshIndexPair2 in visuals.weedVisuals.SecondaryMeshes)
			{
				Material[] materials2 = meshIndexPair2.Mesh.materials;
				materials2[meshIndexPair2.MaterialIndex] = weedDefinition.SecondaryMat;
				meshIndexPair2.Mesh.materials = materials2;
			}
			foreach (FilledPackagingVisuals.MeshIndexPair meshIndexPair3 in visuals.weedVisuals.LeafMeshes)
			{
				Material[] materials3 = meshIndexPair3.Mesh.materials;
				materials3[meshIndexPair3.MaterialIndex] = weedDefinition.LeafMat;
				meshIndexPair3.Mesh.materials = materials3;
			}
			foreach (FilledPackagingVisuals.MeshIndexPair meshIndexPair4 in visuals.weedVisuals.StemMeshes)
			{
				Material[] materials4 = meshIndexPair4.Mesh.materials;
				materials4[meshIndexPair4.MaterialIndex] = weedDefinition.StemMat;
				meshIndexPair4.Mesh.materials = materials4;
			}
			visuals.weedVisuals.Container.gameObject.SetActive(true);
		}

		// Token: 0x06003E34 RID: 15924 RVA: 0x001068DB File Offset: 0x00104ADB
		public override ItemData GetItemData()
		{
			return new WeedData(base.Definition.ID, this.Quantity, this.Quality.ToString(), this.PackagingID);
		}

		// Token: 0x06003E35 RID: 15925 RVA: 0x0010690C File Offset: 0x00104B0C
		public override void ApplyEffectsToNPC(NPC npc)
		{
			npc.Avatar.Eyes.OverrideEyeballTint(new Color32(byte.MaxValue, 170, 170, byte.MaxValue));
			npc.Avatar.Eyes.OverrideEyeLids(new Eye.EyeLidConfiguration
			{
				bottomLidOpen = 0.3f,
				topLidOpen = 0.3f
			});
			npc.Avatar.Eyes.ForceBlink();
			base.ApplyEffectsToNPC(npc);
		}

		// Token: 0x06003E36 RID: 15926 RVA: 0x0010698F File Offset: 0x00104B8F
		public override void ClearEffectsFromNPC(NPC npc)
		{
			npc.Avatar.Eyes.ResetEyeballTint();
			npc.Avatar.Eyes.ResetEyeLids();
			npc.Avatar.Eyes.ForceBlink();
			base.ClearEffectsFromNPC(npc);
		}

		// Token: 0x06003E37 RID: 15927 RVA: 0x001069C8 File Offset: 0x00104BC8
		public override void ApplyEffectsToPlayer(Player player)
		{
			player.Avatar.Eyes.OverrideEyeballTint(new Color32(byte.MaxValue, 170, 170, byte.MaxValue));
			player.Avatar.Eyes.OverrideEyeLids(new Eye.EyeLidConfiguration
			{
				bottomLidOpen = 0.3f,
				topLidOpen = 0.3f
			});
			if (player.IsOwner)
			{
				Singleton<PostProcessingManager>.Instance.ChromaticAberrationController.AddOverride(0.2f, 5, "weed");
				Singleton<PostProcessingManager>.Instance.SaturationController.AddOverride(70f, 5, "weed");
				Singleton<PostProcessingManager>.Instance.BloomController.AddOverride(3f, 5, "weed");
				Singleton<MusicPlayer>.Instance.SetMusicDistorted(true, 5f);
				Singleton<AudioManager>.Instance.SetDistorted(true, 5f);
			}
			base.ApplyEffectsToPlayer(player);
		}

		// Token: 0x06003E38 RID: 15928 RVA: 0x00106AB4 File Offset: 0x00104CB4
		public override void ClearEffectsFromPlayer(Player Player)
		{
			Player.Avatar.Eyes.ResetEyeballTint();
			Player.Avatar.Eyes.ResetEyeLids();
			Player.Avatar.Eyes.ForceBlink();
			if (Player.IsOwner)
			{
				Singleton<PostProcessingManager>.Instance.ChromaticAberrationController.RemoveOverride("weed");
				Singleton<PostProcessingManager>.Instance.SaturationController.RemoveOverride("weed");
				Singleton<PostProcessingManager>.Instance.BloomController.RemoveOverride("weed");
				Singleton<MusicPlayer>.Instance.SetMusicDistorted(false, 5f);
				Singleton<AudioManager>.Instance.SetDistorted(false, 5f);
			}
			base.ClearEffectsFromPlayer(Player);
		}
	}
}
