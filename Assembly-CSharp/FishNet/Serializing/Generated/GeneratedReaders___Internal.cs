using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using FishNet.Object;
using ScheduleOne.AvatarFramework;
using ScheduleOne.AvatarFramework.Customization;
using ScheduleOne.Casino;
using ScheduleOne.Clothing;
using ScheduleOne.Combat;
using ScheduleOne.Delivery;
using ScheduleOne.DevUtilities;
using ScheduleOne.Doors;
using ScheduleOne.Economy;
using ScheduleOne.Employees;
using ScheduleOne.GameTime;
using ScheduleOne.ItemFramework;
using ScheduleOne.Law;
using ScheduleOne.Levelling;
using ScheduleOne.Management;
using ScheduleOne.Messaging;
using ScheduleOne.ObjectScripts;
using ScheduleOne.ObjectScripts.WateringCan;
using ScheduleOne.Persistence;
using ScheduleOne.Persistence.Datas;
using ScheduleOne.PlayerScripts;
using ScheduleOne.Product;
using ScheduleOne.Property;
using ScheduleOne.Quests;
using ScheduleOne.Storage;
using ScheduleOne.Tiles;
using ScheduleOne.UI.Handover;
using ScheduleOne.UI.Phone.Messages;
using ScheduleOne.Vehicles;
using ScheduleOne.Vehicles.Modification;
using ScheduleOne.Vision;
using UnityEngine;

namespace FishNet.Serializing.Generated
{
	// Token: 0x02000C37 RID: 3127
	[StructLayout(LayoutKind.Auto, CharSet = CharSet.Auto)]
	public static class GeneratedReaders___Internal
	{
		// Token: 0x06005756 RID: 22358 RVA: 0x0016F448 File Offset: 0x0016D648
		[RuntimeInitializeOnLoadMethod]
		private static void InitializeOnce()
		{
			GenericReader<ItemInstance>.Read = new Func<Reader, ItemInstance>(ItemSerializers.ReadItemInstance);
			GenericReader<StorableItemInstance>.Read = new Func<Reader, StorableItemInstance>(ItemSerializers.ReadStorableItemInstance);
			GenericReader<CashInstance>.Read = new Func<Reader, CashInstance>(ItemSerializers.ReadCashInstance);
			GenericReader<QualityItemInstance>.Read = new Func<Reader, QualityItemInstance>(ItemSerializers.ReadQualityItemInstance);
			GenericReader<ClothingInstance>.Read = new Func<Reader, ClothingInstance>(ItemSerializers.ReadClothingInstance);
			GenericReader<ProductItemInstance>.Read = new Func<Reader, ProductItemInstance>(ItemSerializers.ReadProductItemInstance);
			GenericReader<WeedInstance>.Read = new Func<Reader, WeedInstance>(ItemSerializers.ReadWeedInstance);
			GenericReader<MethInstance>.Read = new Func<Reader, MethInstance>(ItemSerializers.ReadMethInstance);
			GenericReader<CocaineInstance>.Read = new Func<Reader, CocaineInstance>(ItemSerializers.ReadCocaineInstance);
			GenericReader<IntegerItemInstance>.Read = new Func<Reader, IntegerItemInstance>(ItemSerializers.ReadIntegerItemInstance);
			GenericReader<WateringCanInstance>.Read = new Func<Reader, WateringCanInstance>(ItemSerializers.ReadWateringCanInstance);
			GenericReader<TrashGrabberInstance>.Read = new Func<Reader, TrashGrabberInstance>(ItemSerializers.ReadTrashGrabberInstance);
			GenericReader<VisionEventReceipt>.Read = new Func<Reader, VisionEventReceipt>(GeneratedReaders___Internal.Read___ScheduleOne.Vision.VisionEventReceiptFishNet.Serializing.Generateds);
			GenericReader<PlayerVisualState.EVisualState>.Read = new Func<Reader, PlayerVisualState.EVisualState>(GeneratedReaders___Internal.Read___ScheduleOne.PlayerScripts.PlayerVisualState/EVisualStateFishNet.Serializing.Generateds);
			GenericReader<VisionCone.EEventLevel>.Read = new Func<Reader, VisionCone.EEventLevel>(GeneratedReaders___Internal.Read___ScheduleOne.Vision.VisionCone/EEventLevelFishNet.Serializing.Generateds);
			GenericReader<ContractInfo>.Read = new Func<Reader, ContractInfo>(GeneratedReaders___Internal.Read___ScheduleOne.Quests.ContractInfoFishNet.Serializing.Generateds);
			GenericReader<ProductList>.Read = new Func<Reader, ProductList>(GeneratedReaders___Internal.Read___ScheduleOne.Product.ProductListFishNet.Serializing.Generateds);
			GenericReader<ProductList.Entry>.Read = new Func<Reader, ProductList.Entry>(GeneratedReaders___Internal.Read___ScheduleOne.Product.ProductList/EntryFishNet.Serializing.Generateds);
			GenericReader<EQuality>.Read = new Func<Reader, EQuality>(GeneratedReaders___Internal.Read___ScheduleOne.ItemFramework.EQualityFishNet.Serializing.Generateds);
			GenericReader<List<ProductList.Entry>>.Read = new Func<Reader, List<ProductList.Entry>>(GeneratedReaders___Internal.Read___System.Collections.Generic.List`1<ScheduleOne.Product.ProductList/Entry>FishNet.Serializing.Generateds);
			GenericReader<QuestWindowConfig>.Read = new Func<Reader, QuestWindowConfig>(GeneratedReaders___Internal.Read___ScheduleOne.Quests.QuestWindowConfigFishNet.Serializing.Generateds);
			GenericReader<GameDateTime>.Read = new Func<Reader, GameDateTime>(GeneratedReaders___Internal.Read___ScheduleOne.GameTime.GameDateTimeFishNet.Serializing.Generateds);
			GenericReader<QuestManager.EQuestAction>.Read = new Func<Reader, QuestManager.EQuestAction>(GeneratedReaders___Internal.Read___ScheduleOne.Quests.QuestManager/EQuestActionFishNet.Serializing.Generateds);
			GenericReader<EQuestState>.Read = new Func<Reader, EQuestState>(GeneratedReaders___Internal.Read___ScheduleOne.Quests.EQuestStateFishNet.Serializing.Generateds);
			GenericReader<Impact>.Read = new Func<Reader, Impact>(GeneratedReaders___Internal.Read___ScheduleOne.Combat.ImpactFishNet.Serializing.Generateds);
			GenericReader<EImpactType>.Read = new Func<Reader, EImpactType>(GeneratedReaders___Internal.Read___ScheduleOne.Combat.EImpactTypeFishNet.Serializing.Generateds);
			GenericReader<LandVehicle>.Read = new Func<Reader, LandVehicle>(GeneratedReaders___Internal.Read___ScheduleOne.Vehicles.LandVehicleFishNet.Serializing.Generateds);
			GenericReader<CheckpointManager.ECheckpointLocation>.Read = new Func<Reader, CheckpointManager.ECheckpointLocation>(GeneratedReaders___Internal.Read___ScheduleOne.Law.CheckpointManager/ECheckpointLocationFishNet.Serializing.Generateds);
			GenericReader<Player>.Read = new Func<Reader, Player>(GeneratedReaders___Internal.Read___ScheduleOne.PlayerScripts.PlayerFishNet.Serializing.Generateds);
			GenericReader<List<string>>.Read = new Func<Reader, List<string>>(GeneratedReaders___Internal.Read___System.Collections.Generic.List`1<System.String>FishNet.Serializing.Generateds);
			GenericReader<StringIntPair>.Read = new Func<Reader, StringIntPair>(GeneratedReaders___Internal.Read___ScheduleOne.DevUtilities.StringIntPairFishNet.Serializing.Generateds);
			GenericReader<StringIntPair[]>.Read = new Func<Reader, StringIntPair[]>(GeneratedReaders___Internal.Read___ScheduleOne.DevUtilities.StringIntPair[]FishNet.Serializing.Generateds);
			GenericReader<Message>.Read = new Func<Reader, Message>(GeneratedReaders___Internal.Read___ScheduleOne.Messaging.MessageFishNet.Serializing.Generateds);
			GenericReader<Message.ESenderType>.Read = new Func<Reader, Message.ESenderType>(GeneratedReaders___Internal.Read___ScheduleOne.Messaging.Message/ESenderTypeFishNet.Serializing.Generateds);
			GenericReader<MessageChain>.Read = new Func<Reader, MessageChain>(GeneratedReaders___Internal.Read___ScheduleOne.UI.Phone.Messages.MessageChainFishNet.Serializing.Generateds);
			GenericReader<MSGConversationData>.Read = new Func<Reader, MSGConversationData>(GeneratedReaders___Internal.Read___ScheduleOne.Persistence.Datas.MSGConversationDataFishNet.Serializing.Generateds);
			GenericReader<TextMessageData>.Read = new Func<Reader, TextMessageData>(GeneratedReaders___Internal.Read___ScheduleOne.Persistence.Datas.TextMessageDataFishNet.Serializing.Generateds);
			GenericReader<TextMessageData[]>.Read = new Func<Reader, TextMessageData[]>(GeneratedReaders___Internal.Read___ScheduleOne.Persistence.Datas.TextMessageData[]FishNet.Serializing.Generateds);
			GenericReader<TextResponseData>.Read = new Func<Reader, TextResponseData>(GeneratedReaders___Internal.Read___ScheduleOne.Persistence.Datas.TextResponseDataFishNet.Serializing.Generateds);
			GenericReader<TextResponseData[]>.Read = new Func<Reader, TextResponseData[]>(GeneratedReaders___Internal.Read___ScheduleOne.Persistence.Datas.TextResponseData[]FishNet.Serializing.Generateds);
			GenericReader<Response>.Read = new Func<Reader, Response>(GeneratedReaders___Internal.Read___ScheduleOne.Messaging.ResponseFishNet.Serializing.Generateds);
			GenericReader<List<Response>>.Read = new Func<Reader, List<Response>>(GeneratedReaders___Internal.Read___System.Collections.Generic.List`1<ScheduleOne.Messaging.Response>FishNet.Serializing.Generateds);
			GenericReader<List<NetworkObject>>.Read = new Func<Reader, List<NetworkObject>>(GeneratedReaders___Internal.Read___System.Collections.Generic.List`1<FishNet.Object.NetworkObject>FishNet.Serializing.Generateds);
			GenericReader<AdvancedTransitRouteData>.Read = new Func<Reader, AdvancedTransitRouteData>(GeneratedReaders___Internal.Read___ScheduleOne.Persistence.Datas.AdvancedTransitRouteDataFishNet.Serializing.Generateds);
			GenericReader<ManagementItemFilter.EMode>.Read = new Func<Reader, ManagementItemFilter.EMode>(GeneratedReaders___Internal.Read___ScheduleOne.Management.ManagementItemFilter/EModeFishNet.Serializing.Generateds);
			GenericReader<AdvancedTransitRouteData[]>.Read = new Func<Reader, AdvancedTransitRouteData[]>(GeneratedReaders___Internal.Read___ScheduleOne.Persistence.Datas.AdvancedTransitRouteData[]FishNet.Serializing.Generateds);
			GenericReader<ERank>.Read = new Func<Reader, ERank>(GeneratedReaders___Internal.Read___ScheduleOne.Levelling.ERankFishNet.Serializing.Generateds);
			GenericReader<FullRank>.Read = new Func<Reader, FullRank>(GeneratedReaders___Internal.Read___ScheduleOne.Levelling.FullRankFishNet.Serializing.Generateds);
			GenericReader<PlayerData>.Read = new Func<Reader, PlayerData>(GeneratedReaders___Internal.Read___ScheduleOne.Persistence.Datas.PlayerDataFishNet.Serializing.Generateds);
			GenericReader<VariableData>.Read = new Func<Reader, VariableData>(GeneratedReaders___Internal.Read___ScheduleOne.Persistence.Datas.VariableDataFishNet.Serializing.Generateds);
			GenericReader<VariableData[]>.Read = new Func<Reader, VariableData[]>(GeneratedReaders___Internal.Read___ScheduleOne.Persistence.Datas.VariableData[]FishNet.Serializing.Generateds);
			GenericReader<AvatarSettings>.Read = new Func<Reader, AvatarSettings>(GeneratedReaders___Internal.Read___ScheduleOne.AvatarFramework.AvatarSettingsFishNet.Serializing.Generateds);
			GenericReader<Eye.EyeLidConfiguration>.Read = new Func<Reader, Eye.EyeLidConfiguration>(GeneratedReaders___Internal.Read___ScheduleOne.AvatarFramework.Eye/EyeLidConfigurationFishNet.Serializing.Generateds);
			GenericReader<AvatarSettings.LayerSetting>.Read = new Func<Reader, AvatarSettings.LayerSetting>(GeneratedReaders___Internal.Read___ScheduleOne.AvatarFramework.AvatarSettings/LayerSettingFishNet.Serializing.Generateds);
			GenericReader<List<AvatarSettings.LayerSetting>>.Read = new Func<Reader, List<AvatarSettings.LayerSetting>>(GeneratedReaders___Internal.Read___System.Collections.Generic.List`1<ScheduleOne.AvatarFramework.AvatarSettings/LayerSetting>FishNet.Serializing.Generateds);
			GenericReader<AvatarSettings.AccessorySetting>.Read = new Func<Reader, AvatarSettings.AccessorySetting>(GeneratedReaders___Internal.Read___ScheduleOne.AvatarFramework.AvatarSettings/AccessorySettingFishNet.Serializing.Generateds);
			GenericReader<List<AvatarSettings.AccessorySetting>>.Read = new Func<Reader, List<AvatarSettings.AccessorySetting>>(GeneratedReaders___Internal.Read___System.Collections.Generic.List`1<ScheduleOne.AvatarFramework.AvatarSettings/AccessorySetting>FishNet.Serializing.Generateds);
			GenericReader<BasicAvatarSettings>.Read = new Func<Reader, BasicAvatarSettings>(GeneratedReaders___Internal.Read___ScheduleOne.AvatarFramework.Customization.BasicAvatarSettingsFishNet.Serializing.Generateds);
			GenericReader<PlayerCrimeData.EPursuitLevel>.Read = new Func<Reader, PlayerCrimeData.EPursuitLevel>(GeneratedReaders___Internal.Read___ScheduleOne.PlayerScripts.PlayerCrimeData/EPursuitLevelFishNet.Serializing.Generateds);
			GenericReader<Property>.Read = new Func<Reader, Property>(GeneratedReaders___Internal.Read___ScheduleOne.Property.PropertyFishNet.Serializing.Generateds);
			GenericReader<EEmployeeType>.Read = new Func<Reader, EEmployeeType>(GeneratedReaders___Internal.Read___ScheduleOne.Employees.EEmployeeTypeFishNet.Serializing.Generateds);
			GenericReader<EDealWindow>.Read = new Func<Reader, EDealWindow>(GeneratedReaders___Internal.Read___ScheduleOne.Economy.EDealWindowFishNet.Serializing.Generateds);
			GenericReader<HandoverScreen.EHandoverOutcome>.Read = new Func<Reader, HandoverScreen.EHandoverOutcome>(GeneratedReaders___Internal.Read___ScheduleOne.UI.Handover.HandoverScreen/EHandoverOutcomeFishNet.Serializing.Generateds);
			GenericReader<List<ItemInstance>>.Read = new Func<Reader, List<ItemInstance>>(GeneratedReaders___Internal.Read___System.Collections.Generic.List`1<ScheduleOne.ItemFramework.ItemInstance>FishNet.Serializing.Generateds);
			GenericReader<ScheduleOne.Persistence.Datas.CustomerData>.Read = new Func<Reader, ScheduleOne.Persistence.Datas.CustomerData>(GeneratedReaders___Internal.Read___ScheduleOne.Persistence.Datas.CustomerDataFishNet.Serializing.Generateds);
			GenericReader<string[]>.Read = new Func<Reader, string[]>(GeneratedReaders___Internal.Read___System.String[]FishNet.Serializing.Generateds);
			GenericReader<float[]>.Read = new Func<Reader, float[]>(GeneratedReaders___Internal.Read___System.Single[]FishNet.Serializing.Generateds);
			GenericReader<EDrugType>.Read = new Func<Reader, EDrugType>(GeneratedReaders___Internal.Read___ScheduleOne.Product.EDrugTypeFishNet.Serializing.Generateds);
			GenericReader<GameData>.Read = new Func<Reader, GameData>(GeneratedReaders___Internal.Read___ScheduleOne.Persistence.Datas.GameDataFishNet.Serializing.Generateds);
			GenericReader<GameSettings>.Read = new Func<Reader, GameSettings>(GeneratedReaders___Internal.Read___ScheduleOne.DevUtilities.GameSettingsFishNet.Serializing.Generateds);
			GenericReader<DeliveryInstance>.Read = new Func<Reader, DeliveryInstance>(GeneratedReaders___Internal.Read___ScheduleOne.Delivery.DeliveryInstanceFishNet.Serializing.Generateds);
			GenericReader<EDeliveryStatus>.Read = new Func<Reader, EDeliveryStatus>(GeneratedReaders___Internal.Read___ScheduleOne.Delivery.EDeliveryStatusFishNet.Serializing.Generateds);
			GenericReader<ExplosionData>.Read = new Func<Reader, ExplosionData>(GeneratedReaders___Internal.Read___ScheduleOne.Combat.ExplosionDataFishNet.Serializing.Generateds);
			GenericReader<PlayingCard.ECardSuit>.Read = new Func<Reader, PlayingCard.ECardSuit>(GeneratedReaders___Internal.Read___ScheduleOne.Casino.PlayingCard/ECardSuitFishNet.Serializing.Generateds);
			GenericReader<PlayingCard.ECardValue>.Read = new Func<Reader, PlayingCard.ECardValue>(GeneratedReaders___Internal.Read___ScheduleOne.Casino.PlayingCard/ECardValueFishNet.Serializing.Generateds);
			GenericReader<NetworkObject[]>.Read = new Func<Reader, NetworkObject[]>(GeneratedReaders___Internal.Read___FishNet.Object.NetworkObject[]FishNet.Serializing.Generateds);
			GenericReader<RTBGameController.EStage>.Read = new Func<Reader, RTBGameController.EStage>(GeneratedReaders___Internal.Read___ScheduleOne.Casino.RTBGameController/EStageFishNet.Serializing.Generateds);
			GenericReader<SlotMachine.ESymbol>.Read = new Func<Reader, SlotMachine.ESymbol>(GeneratedReaders___Internal.Read___ScheduleOne.Casino.SlotMachine/ESymbolFishNet.Serializing.Generateds);
			GenericReader<SlotMachine.ESymbol[]>.Read = new Func<Reader, SlotMachine.ESymbol[]>(GeneratedReaders___Internal.Read___ScheduleOne.Casino.SlotMachine/ESymbol[]FishNet.Serializing.Generateds);
			GenericReader<EDoorSide>.Read = new Func<Reader, EDoorSide>(GeneratedReaders___Internal.Read___ScheduleOne.Doors.EDoorSideFishNet.Serializing.Generateds);
			GenericReader<EVehicleColor>.Read = new Func<Reader, EVehicleColor>(GeneratedReaders___Internal.Read___ScheduleOne.Vehicles.Modification.EVehicleColorFishNet.Serializing.Generateds);
			GenericReader<ParkData>.Read = new Func<Reader, ParkData>(GeneratedReaders___Internal.Read___ScheduleOne.Vehicles.ParkDataFishNet.Serializing.Generateds);
			GenericReader<EParkingAlignment>.Read = new Func<Reader, EParkingAlignment>(GeneratedReaders___Internal.Read___ScheduleOne.Vehicles.EParkingAlignmentFishNet.Serializing.Generateds);
			GenericReader<TrashContentData>.Read = new Func<Reader, TrashContentData>(GeneratedReaders___Internal.Read___ScheduleOne.Persistence.TrashContentDataFishNet.Serializing.Generateds);
			GenericReader<int[]>.Read = new Func<Reader, int[]>(GeneratedReaders___Internal.Read___System.Int32[]FishNet.Serializing.Generateds);
			GenericReader<Coordinate>.Read = new Func<Reader, Coordinate>(GeneratedReaders___Internal.Read___ScheduleOne.Tiles.CoordinateFishNet.Serializing.Generateds);
			GenericReader<WeedAppearanceSettings>.Read = new Func<Reader, WeedAppearanceSettings>(GeneratedReaders___Internal.Read___ScheduleOne.Product.WeedAppearanceSettingsFishNet.Serializing.Generateds);
			GenericReader<CocaineAppearanceSettings>.Read = new Func<Reader, CocaineAppearanceSettings>(GeneratedReaders___Internal.Read___ScheduleOne.Product.CocaineAppearanceSettingsFishNet.Serializing.Generateds);
			GenericReader<MethAppearanceSettings>.Read = new Func<Reader, MethAppearanceSettings>(GeneratedReaders___Internal.Read___ScheduleOne.Product.MethAppearanceSettingsFishNet.Serializing.Generateds);
			GenericReader<NewMixOperation>.Read = new Func<Reader, NewMixOperation>(GeneratedReaders___Internal.Read___ScheduleOne.Product.NewMixOperationFishNet.Serializing.Generateds);
			GenericReader<Recycler.EState>.Read = new Func<Reader, Recycler.EState>(GeneratedReaders___Internal.Read___ScheduleOne.ObjectScripts.Recycler/EStateFishNet.Serializing.Generateds);
			GenericReader<CoordinateProceduralTilePair>.Read = new Func<Reader, CoordinateProceduralTilePair>(GeneratedReaders___Internal.Read___ScheduleOne.Tiles.CoordinateProceduralTilePairFishNet.Serializing.Generateds);
			GenericReader<List<CoordinateProceduralTilePair>>.Read = new Func<Reader, List<CoordinateProceduralTilePair>>(GeneratedReaders___Internal.Read___System.Collections.Generic.List`1<ScheduleOne.Tiles.CoordinateProceduralTilePair>FishNet.Serializing.Generateds);
			GenericReader<ChemistryCookOperation>.Read = new Func<Reader, ChemistryCookOperation>(GeneratedReaders___Internal.Read___ScheduleOne.ObjectScripts.ChemistryCookOperationFishNet.Serializing.Generateds);
			GenericReader<DryingOperation>.Read = new Func<Reader, DryingOperation>(GeneratedReaders___Internal.Read___ScheduleOne.ObjectScripts.DryingOperationFishNet.Serializing.Generateds);
			GenericReader<OvenCookOperation>.Read = new Func<Reader, OvenCookOperation>(GeneratedReaders___Internal.Read___ScheduleOne.ObjectScripts.OvenCookOperationFishNet.Serializing.Generateds);
			GenericReader<MixOperation>.Read = new Func<Reader, MixOperation>(GeneratedReaders___Internal.Read___ScheduleOne.ObjectScripts.MixOperationFishNet.Serializing.Generateds);
		}

		// Token: 0x06005757 RID: 22359 RVA: 0x0016FAC8 File Offset: 0x0016DCC8
		public static VisionEventReceipt Generateds(Reader reader)
		{
			bool flag = reader.ReadBoolean();
			if (flag)
			{
				return null;
			}
			return new VisionEventReceipt
			{
				TargetPlayer = reader.ReadNetworkObject(),
				State = GeneratedReaders___Internal.Read___ScheduleOne.PlayerScripts.PlayerVisualState/EVisualStateFishNet.Serializing.Generateds(reader)
			};
		}

		// Token: 0x06005758 RID: 22360 RVA: 0x0016FB20 File Offset: 0x0016DD20
		public static PlayerVisualState.EVisualState Generateds(Reader reader)
		{
			return (PlayerVisualState.EVisualState)reader.ReadInt32(AutoPackType.Packed);
		}

		// Token: 0x06005759 RID: 22361 RVA: 0x0016FB3C File Offset: 0x0016DD3C
		public static VisionCone.EEventLevel Generateds(Reader reader)
		{
			return (VisionCone.EEventLevel)reader.ReadInt32(AutoPackType.Packed);
		}

		// Token: 0x0600575A RID: 22362 RVA: 0x0016FB58 File Offset: 0x0016DD58
		public static ContractInfo Generateds(Reader reader)
		{
			bool flag = reader.ReadBoolean();
			if (flag)
			{
				return null;
			}
			return new ContractInfo
			{
				Payment = reader.ReadSingle(AutoPackType.Unpacked),
				Products = GeneratedReaders___Internal.Read___ScheduleOne.Product.ProductListFishNet.Serializing.Generateds(reader),
				DeliveryLocationGUID = reader.ReadString(),
				DeliveryWindow = GeneratedReaders___Internal.Read___ScheduleOne.Quests.QuestWindowConfigFishNet.Serializing.Generateds(reader),
				Expires = reader.ReadBoolean(),
				ExpiresAfter = reader.ReadInt32(AutoPackType.Packed),
				PickupScheduleIndex = reader.ReadInt32(AutoPackType.Packed),
				IsCounterOffer = reader.ReadBoolean()
			};
		}

		// Token: 0x0600575B RID: 22363 RVA: 0x0016FC2C File Offset: 0x0016DE2C
		public static ProductList Generateds(Reader reader)
		{
			bool flag = reader.ReadBoolean();
			if (flag)
			{
				return null;
			}
			return new ProductList
			{
				entries = GeneratedReaders___Internal.Read___System.Collections.Generic.List`1<ScheduleOne.Product.ProductList/Entry>FishNet.Serializing.Generateds(reader)
			};
		}

		// Token: 0x0600575C RID: 22364 RVA: 0x0016FC74 File Offset: 0x0016DE74
		public static ProductList.Entry Generateds(Reader reader)
		{
			bool flag = reader.ReadBoolean();
			if (flag)
			{
				return null;
			}
			return new ProductList.Entry
			{
				ProductID = reader.ReadString(),
				Quality = GeneratedReaders___Internal.Read___ScheduleOne.ItemFramework.EQualityFishNet.Serializing.Generateds(reader),
				Quantity = reader.ReadInt32(AutoPackType.Packed)
			};
		}

		// Token: 0x0600575D RID: 22365 RVA: 0x0016FCE4 File Offset: 0x0016DEE4
		public static EQuality Generateds(Reader reader)
		{
			return (EQuality)reader.ReadInt32(AutoPackType.Packed);
		}

		// Token: 0x0600575E RID: 22366 RVA: 0x0016FD00 File Offset: 0x0016DF00
		public static List<ProductList.Entry> List(Reader reader)
		{
			return reader.ReadListAllocated<ProductList.Entry>();
		}

		// Token: 0x0600575F RID: 22367 RVA: 0x0016FD18 File Offset: 0x0016DF18
		public static QuestWindowConfig Generateds(Reader reader)
		{
			bool flag = reader.ReadBoolean();
			if (flag)
			{
				return null;
			}
			return new QuestWindowConfig
			{
				IsEnabled = reader.ReadBoolean(),
				WindowStartTime = reader.ReadInt32(AutoPackType.Packed),
				WindowEndTime = reader.ReadInt32(AutoPackType.Packed)
			};
		}

		// Token: 0x06005760 RID: 22368 RVA: 0x0016FD8C File Offset: 0x0016DF8C
		public static GameDateTime Generateds(Reader reader)
		{
			return new GameDateTime
			{
				elapsedDays = reader.ReadInt32(AutoPackType.Packed),
				time = reader.ReadInt32(AutoPackType.Packed)
			};
		}

		// Token: 0x06005761 RID: 22369 RVA: 0x0016FDD8 File Offset: 0x0016DFD8
		public static QuestManager.EQuestAction Generateds(Reader reader)
		{
			return (QuestManager.EQuestAction)reader.ReadInt32(AutoPackType.Packed);
		}

		// Token: 0x06005762 RID: 22370 RVA: 0x0016FDF4 File Offset: 0x0016DFF4
		public static EQuestState Generateds(Reader reader)
		{
			return (EQuestState)reader.ReadInt32(AutoPackType.Packed);
		}

		// Token: 0x06005763 RID: 22371 RVA: 0x0016FE10 File Offset: 0x0016E010
		public static Impact Generateds(Reader reader)
		{
			bool flag = reader.ReadBoolean();
			if (flag)
			{
				return null;
			}
			return new Impact
			{
				HitPoint = reader.ReadVector3(),
				ImpactForceDirection = reader.ReadVector3(),
				ImpactForce = reader.ReadSingle(AutoPackType.Unpacked),
				ImpactDamage = reader.ReadSingle(AutoPackType.Unpacked),
				ImpactType = GeneratedReaders___Internal.Read___ScheduleOne.Combat.EImpactTypeFishNet.Serializing.Generateds(reader),
				ImpactSource = reader.ReadNetworkObject(),
				ImpactID = reader.ReadInt32(AutoPackType.Packed)
			};
		}

		// Token: 0x06005764 RID: 22372 RVA: 0x0016FED0 File Offset: 0x0016E0D0
		public static EImpactType Generateds(Reader reader)
		{
			return (EImpactType)reader.ReadInt32(AutoPackType.Packed);
		}

		// Token: 0x06005765 RID: 22373 RVA: 0x0016FEEC File Offset: 0x0016E0EC
		public static LandVehicle Generateds(Reader reader)
		{
			return (LandVehicle)reader.ReadNetworkBehaviour();
		}

		// Token: 0x06005766 RID: 22374 RVA: 0x0016FF04 File Offset: 0x0016E104
		public static CheckpointManager.ECheckpointLocation Generateds(Reader reader)
		{
			return (CheckpointManager.ECheckpointLocation)reader.ReadInt32(AutoPackType.Packed);
		}

		// Token: 0x06005767 RID: 22375 RVA: 0x0016FF20 File Offset: 0x0016E120
		public static Player Generateds(Reader reader)
		{
			return (Player)reader.ReadNetworkBehaviour();
		}

		// Token: 0x06005768 RID: 22376 RVA: 0x0016FF38 File Offset: 0x0016E138
		public static List<string> List(Reader reader)
		{
			return reader.ReadListAllocated<string>();
		}

		// Token: 0x06005769 RID: 22377 RVA: 0x0016FF50 File Offset: 0x0016E150
		public static StringIntPair Generateds(Reader reader)
		{
			bool flag = reader.ReadBoolean();
			if (flag)
			{
				return null;
			}
			return new StringIntPair
			{
				String = reader.ReadString(),
				Int = reader.ReadInt32(AutoPackType.Packed)
			};
		}

		// Token: 0x0600576A RID: 22378 RVA: 0x0016FFAC File Offset: 0x0016E1AC
		public static StringIntPair[] Generateds(Reader reader)
		{
			return reader.ReadArrayAllocated<StringIntPair>();
		}

		// Token: 0x0600576B RID: 22379 RVA: 0x0016FFC4 File Offset: 0x0016E1C4
		public static Message Generateds(Reader reader)
		{
			bool flag = reader.ReadBoolean();
			if (flag)
			{
				return null;
			}
			return new Message
			{
				messageId = reader.ReadInt32(AutoPackType.Packed),
				text = reader.ReadString(),
				sender = GeneratedReaders___Internal.Read___ScheduleOne.Messaging.Message/ESenderTypeFishNet.Serializing.Generateds(reader),
				endOfGroup = reader.ReadBoolean()
			};
		}

		// Token: 0x0600576C RID: 22380 RVA: 0x00170044 File Offset: 0x0016E244
		public static Message.ESenderType Generateds(Reader reader)
		{
			return (Message.ESenderType)reader.ReadInt32(AutoPackType.Packed);
		}

		// Token: 0x0600576D RID: 22381 RVA: 0x00170060 File Offset: 0x0016E260
		public static MessageChain Generateds(Reader reader)
		{
			bool flag = reader.ReadBoolean();
			if (flag)
			{
				return null;
			}
			return new MessageChain
			{
				Messages = GeneratedReaders___Internal.Read___System.Collections.Generic.List`1<System.String>FishNet.Serializing.Generateds(reader),
				id = reader.ReadInt32(AutoPackType.Packed)
			};
		}

		// Token: 0x0600576E RID: 22382 RVA: 0x001700BC File Offset: 0x0016E2BC
		public static MSGConversationData Generateds(Reader reader)
		{
			bool flag = reader.ReadBoolean();
			if (flag)
			{
				return null;
			}
			return new MSGConversationData
			{
				ConversationIndex = reader.ReadInt32(AutoPackType.Packed),
				Read = reader.ReadBoolean(),
				MessageHistory = GeneratedReaders___Internal.Read___ScheduleOne.Persistence.Datas.TextMessageData[]FishNet.Serializing.Generateds(reader),
				ActiveResponses = GeneratedReaders___Internal.Read___ScheduleOne.Persistence.Datas.TextResponseData[]FishNet.Serializing.Generateds(reader),
				IsHidden = reader.ReadBoolean(),
				DataType = reader.ReadString(),
				DataVersion = reader.ReadInt32(AutoPackType.Packed),
				GameVersion = reader.ReadString()
			};
		}

		// Token: 0x0600576F RID: 22383 RVA: 0x0017018C File Offset: 0x0016E38C
		public static TextMessageData Generateds(Reader reader)
		{
			bool flag = reader.ReadBoolean();
			if (flag)
			{
				return null;
			}
			return new TextMessageData
			{
				Sender = reader.ReadInt32(AutoPackType.Packed),
				MessageID = reader.ReadInt32(AutoPackType.Packed),
				Text = reader.ReadString(),
				EndOfChain = reader.ReadBoolean()
			};
		}

		// Token: 0x06005770 RID: 22384 RVA: 0x00170214 File Offset: 0x0016E414
		public static TextMessageData[] Generateds(Reader reader)
		{
			return reader.ReadArrayAllocated<TextMessageData>();
		}

		// Token: 0x06005771 RID: 22385 RVA: 0x0017022C File Offset: 0x0016E42C
		public static TextResponseData Generateds(Reader reader)
		{
			bool flag = reader.ReadBoolean();
			if (flag)
			{
				return null;
			}
			return new TextResponseData
			{
				Text = reader.ReadString(),
				Label = reader.ReadString()
			};
		}

		// Token: 0x06005772 RID: 22386 RVA: 0x00170284 File Offset: 0x0016E484
		public static TextResponseData[] Generateds(Reader reader)
		{
			return reader.ReadArrayAllocated<TextResponseData>();
		}

		// Token: 0x06005773 RID: 22387 RVA: 0x0017029C File Offset: 0x0016E49C
		public static Response Generateds(Reader reader)
		{
			bool flag = reader.ReadBoolean();
			if (flag)
			{
				return null;
			}
			return new Response
			{
				text = reader.ReadString(),
				label = reader.ReadString(),
				disableDefaultResponseBehaviour = reader.ReadBoolean()
			};
		}

		// Token: 0x06005774 RID: 22388 RVA: 0x00170308 File Offset: 0x0016E508
		public static List<Response> List(Reader reader)
		{
			return reader.ReadListAllocated<Response>();
		}

		// Token: 0x06005775 RID: 22389 RVA: 0x00170320 File Offset: 0x0016E520
		public static List<NetworkObject> List(Reader reader)
		{
			return reader.ReadListAllocated<NetworkObject>();
		}

		// Token: 0x06005776 RID: 22390 RVA: 0x00170338 File Offset: 0x0016E538
		public static AdvancedTransitRouteData Generateds(Reader reader)
		{
			bool flag = reader.ReadBoolean();
			if (flag)
			{
				return null;
			}
			return new AdvancedTransitRouteData
			{
				SourceGUID = reader.ReadString(),
				DestinationGUID = reader.ReadString(),
				FilterMode = GeneratedReaders___Internal.Read___ScheduleOne.Management.ManagementItemFilter/EModeFishNet.Serializing.Generateds(reader),
				FilterItemIDs = GeneratedReaders___Internal.Read___System.Collections.Generic.List`1<System.String>FishNet.Serializing.Generateds(reader)
			};
		}

		// Token: 0x06005777 RID: 22391 RVA: 0x001703B4 File Offset: 0x0016E5B4
		public static ManagementItemFilter.EMode Generateds(Reader reader)
		{
			return (ManagementItemFilter.EMode)reader.ReadInt32(AutoPackType.Packed);
		}

		// Token: 0x06005778 RID: 22392 RVA: 0x001703D0 File Offset: 0x0016E5D0
		public static AdvancedTransitRouteData[] Generateds(Reader reader)
		{
			return reader.ReadArrayAllocated<AdvancedTransitRouteData>();
		}

		// Token: 0x06005779 RID: 22393 RVA: 0x001703E8 File Offset: 0x0016E5E8
		public static ERank Generateds(Reader reader)
		{
			return (ERank)reader.ReadInt32(AutoPackType.Packed);
		}

		// Token: 0x0600577A RID: 22394 RVA: 0x00170404 File Offset: 0x0016E604
		public static FullRank Generateds(Reader reader)
		{
			return new FullRank
			{
				Rank = GeneratedReaders___Internal.Read___ScheduleOne.Levelling.ERankFishNet.Serializing.Generateds(reader),
				Tier = reader.ReadInt32(AutoPackType.Packed)
			};
		}

		// Token: 0x0600577B RID: 22395 RVA: 0x00170448 File Offset: 0x0016E648
		public static PlayerData Generateds(Reader reader)
		{
			bool flag = reader.ReadBoolean();
			if (flag)
			{
				return null;
			}
			return new PlayerData
			{
				PlayerCode = reader.ReadString(),
				Position = reader.ReadVector3(),
				Rotation = reader.ReadSingle(AutoPackType.Unpacked),
				IntroCompleted = reader.ReadBoolean(),
				DataType = reader.ReadString(),
				DataVersion = reader.ReadInt32(AutoPackType.Packed),
				GameVersion = reader.ReadString()
			};
		}

		// Token: 0x0600577C RID: 22396 RVA: 0x00170504 File Offset: 0x0016E704
		public static VariableData Generateds(Reader reader)
		{
			bool flag = reader.ReadBoolean();
			if (flag)
			{
				return null;
			}
			return new VariableData
			{
				Name = reader.ReadString(),
				Value = reader.ReadString(),
				DataType = reader.ReadString(),
				DataVersion = reader.ReadInt32(AutoPackType.Packed),
				GameVersion = reader.ReadString()
			};
		}

		// Token: 0x0600577D RID: 22397 RVA: 0x00170598 File Offset: 0x0016E798
		public static VariableData[] Generateds(Reader reader)
		{
			return reader.ReadArrayAllocated<VariableData>();
		}

		// Token: 0x0600577E RID: 22398 RVA: 0x001705B0 File Offset: 0x0016E7B0
		public static AvatarSettings Generateds(Reader reader)
		{
			bool flag = reader.ReadBoolean();
			if (flag)
			{
				return null;
			}
			AvatarSettings avatarSettings = ScriptableObject.CreateInstance<AvatarSettings>();
			avatarSettings.SkinColor = reader.ReadColor(AutoPackType.Packed);
			avatarSettings.Height = reader.ReadSingle(AutoPackType.Unpacked);
			avatarSettings.Gender = reader.ReadSingle(AutoPackType.Unpacked);
			avatarSettings.Weight = reader.ReadSingle(AutoPackType.Unpacked);
			avatarSettings.HairPath = reader.ReadString();
			avatarSettings.HairColor = reader.ReadColor(AutoPackType.Packed);
			avatarSettings.EyebrowScale = reader.ReadSingle(AutoPackType.Unpacked);
			avatarSettings.EyebrowThickness = reader.ReadSingle(AutoPackType.Unpacked);
			avatarSettings.EyebrowRestingHeight = reader.ReadSingle(AutoPackType.Unpacked);
			avatarSettings.EyebrowRestingAngle = reader.ReadSingle(AutoPackType.Unpacked);
			avatarSettings.LeftEyeLidColor = reader.ReadColor(AutoPackType.Packed);
			avatarSettings.RightEyeLidColor = reader.ReadColor(AutoPackType.Packed);
			avatarSettings.LeftEyeRestingState = GeneratedReaders___Internal.Read___ScheduleOne.AvatarFramework.Eye/EyeLidConfigurationFishNet.Serializing.Generateds(reader);
			avatarSettings.RightEyeRestingState = GeneratedReaders___Internal.Read___ScheduleOne.AvatarFramework.Eye/EyeLidConfigurationFishNet.Serializing.Generateds(reader);
			avatarSettings.EyeballMaterialIdentifier = reader.ReadString();
			avatarSettings.EyeBallTint = reader.ReadColor(AutoPackType.Packed);
			avatarSettings.PupilDilation = reader.ReadSingle(AutoPackType.Unpacked);
			avatarSettings.FaceLayerSettings = GeneratedReaders___Internal.Read___System.Collections.Generic.List`1<ScheduleOne.AvatarFramework.AvatarSettings/LayerSetting>FishNet.Serializing.Generateds(reader);
			avatarSettings.BodyLayerSettings = GeneratedReaders___Internal.Read___System.Collections.Generic.List`1<ScheduleOne.AvatarFramework.AvatarSettings/LayerSetting>FishNet.Serializing.Generateds(reader);
			avatarSettings.AccessorySettings = GeneratedReaders___Internal.Read___System.Collections.Generic.List`1<ScheduleOne.AvatarFramework.AvatarSettings/AccessorySetting>FishNet.Serializing.Generateds(reader);
			avatarSettings.UseCombinedLayer = reader.ReadBoolean();
			avatarSettings.CombinedLayerPath = reader.ReadString();
			return avatarSettings;
		}

		// Token: 0x0600577F RID: 22399 RVA: 0x001707B0 File Offset: 0x0016E9B0
		public static Eye.EyeLidConfiguration Generateds(Reader reader)
		{
			return new Eye.EyeLidConfiguration
			{
				topLidOpen = reader.ReadSingle(AutoPackType.Unpacked),
				bottomLidOpen = reader.ReadSingle(AutoPackType.Unpacked)
			};
		}

		// Token: 0x06005780 RID: 22400 RVA: 0x001707FC File Offset: 0x0016E9FC
		public static AvatarSettings.LayerSetting Generateds(Reader reader)
		{
			return new AvatarSettings.LayerSetting
			{
				layerPath = reader.ReadString(),
				layerTint = reader.ReadColor(AutoPackType.Packed)
			};
		}

		// Token: 0x06005781 RID: 22401 RVA: 0x00170840 File Offset: 0x0016EA40
		public static List<AvatarSettings.LayerSetting> List(Reader reader)
		{
			return reader.ReadListAllocated<AvatarSettings.LayerSetting>();
		}

		// Token: 0x06005782 RID: 22402 RVA: 0x00170858 File Offset: 0x0016EA58
		public static AvatarSettings.AccessorySetting Generateds(Reader reader)
		{
			bool flag = reader.ReadBoolean();
			if (flag)
			{
				return null;
			}
			return new AvatarSettings.AccessorySetting
			{
				path = reader.ReadString(),
				color = reader.ReadColor(AutoPackType.Packed)
			};
		}

		// Token: 0x06005783 RID: 22403 RVA: 0x001708B4 File Offset: 0x0016EAB4
		public static List<AvatarSettings.AccessorySetting> List(Reader reader)
		{
			return reader.ReadListAllocated<AvatarSettings.AccessorySetting>();
		}

		// Token: 0x06005784 RID: 22404 RVA: 0x001708CC File Offset: 0x0016EACC
		public static BasicAvatarSettings Generateds(Reader reader)
		{
			bool flag = reader.ReadBoolean();
			if (flag)
			{
				return null;
			}
			BasicAvatarSettings basicAvatarSettings = ScriptableObject.CreateInstance<BasicAvatarSettings>();
			basicAvatarSettings.Gender = reader.ReadInt32(AutoPackType.Packed);
			basicAvatarSettings.Weight = reader.ReadSingle(AutoPackType.Unpacked);
			basicAvatarSettings.SkinColor = reader.ReadColor(AutoPackType.Packed);
			basicAvatarSettings.HairStyle = reader.ReadString();
			basicAvatarSettings.HairColor = reader.ReadColor(AutoPackType.Packed);
			basicAvatarSettings.Mouth = reader.ReadString();
			basicAvatarSettings.FacialHair = reader.ReadString();
			basicAvatarSettings.FacialDetails = reader.ReadString();
			basicAvatarSettings.FacialDetailsIntensity = reader.ReadSingle(AutoPackType.Unpacked);
			basicAvatarSettings.EyeballColor = reader.ReadColor(AutoPackType.Packed);
			basicAvatarSettings.UpperEyeLidRestingPosition = reader.ReadSingle(AutoPackType.Unpacked);
			basicAvatarSettings.LowerEyeLidRestingPosition = reader.ReadSingle(AutoPackType.Unpacked);
			basicAvatarSettings.PupilDilation = reader.ReadSingle(AutoPackType.Unpacked);
			basicAvatarSettings.EyebrowScale = reader.ReadSingle(AutoPackType.Unpacked);
			basicAvatarSettings.EyebrowThickness = reader.ReadSingle(AutoPackType.Unpacked);
			basicAvatarSettings.EyebrowRestingHeight = reader.ReadSingle(AutoPackType.Unpacked);
			basicAvatarSettings.EyebrowRestingAngle = reader.ReadSingle(AutoPackType.Unpacked);
			basicAvatarSettings.Top = reader.ReadString();
			basicAvatarSettings.TopColor = reader.ReadColor(AutoPackType.Packed);
			basicAvatarSettings.Bottom = reader.ReadString();
			basicAvatarSettings.BottomColor = reader.ReadColor(AutoPackType.Packed);
			basicAvatarSettings.Shoes = reader.ReadString();
			basicAvatarSettings.ShoesColor = reader.ReadColor(AutoPackType.Packed);
			basicAvatarSettings.Headwear = reader.ReadString();
			basicAvatarSettings.HeadwearColor = reader.ReadColor(AutoPackType.Packed);
			basicAvatarSettings.Eyewear = reader.ReadString();
			basicAvatarSettings.EyewearColor = reader.ReadColor(AutoPackType.Packed);
			basicAvatarSettings.Tattoos = GeneratedReaders___Internal.Read___System.Collections.Generic.List`1<System.String>FishNet.Serializing.Generateds(reader);
			return basicAvatarSettings;
		}

		// Token: 0x06005785 RID: 22405 RVA: 0x00170B54 File Offset: 0x0016ED54
		public static PlayerCrimeData.EPursuitLevel Generateds(Reader reader)
		{
			return (PlayerCrimeData.EPursuitLevel)reader.ReadInt32(AutoPackType.Packed);
		}

		// Token: 0x06005786 RID: 22406 RVA: 0x00170B70 File Offset: 0x0016ED70
		public static Property Generateds(Reader reader)
		{
			return (Property)reader.ReadNetworkBehaviour();
		}

		// Token: 0x06005787 RID: 22407 RVA: 0x00170B88 File Offset: 0x0016ED88
		public static EEmployeeType Generateds(Reader reader)
		{
			return (EEmployeeType)reader.ReadInt32(AutoPackType.Packed);
		}

		// Token: 0x06005788 RID: 22408 RVA: 0x00170BA4 File Offset: 0x0016EDA4
		public static EDealWindow Generateds(Reader reader)
		{
			return (EDealWindow)reader.ReadInt32(AutoPackType.Packed);
		}

		// Token: 0x06005789 RID: 22409 RVA: 0x00170BC0 File Offset: 0x0016EDC0
		public static HandoverScreen.EHandoverOutcome Generateds(Reader reader)
		{
			return (HandoverScreen.EHandoverOutcome)reader.ReadInt32(AutoPackType.Packed);
		}

		// Token: 0x0600578A RID: 22410 RVA: 0x00170BDC File Offset: 0x0016EDDC
		public static List<ItemInstance> List(Reader reader)
		{
			return reader.ReadListAllocated<ItemInstance>();
		}

		// Token: 0x0600578B RID: 22411 RVA: 0x00170BF4 File Offset: 0x0016EDF4
		public static ScheduleOne.Persistence.Datas.CustomerData Generateds(Reader reader)
		{
			bool flag = reader.ReadBoolean();
			if (flag)
			{
				return null;
			}
			return new ScheduleOne.Persistence.Datas.CustomerData
			{
				Dependence = reader.ReadSingle(AutoPackType.Unpacked),
				PurchaseableProducts = GeneratedReaders___Internal.Read___System.String[]FishNet.Serializing.Generateds(reader),
				ProductAffinities = GeneratedReaders___Internal.Read___System.Single[]FishNet.Serializing.Generateds(reader),
				TimeSinceLastDealCompleted = reader.ReadInt32(AutoPackType.Packed),
				TimeSinceLastDealOffered = reader.ReadInt32(AutoPackType.Packed),
				OfferedDeals = reader.ReadInt32(AutoPackType.Packed),
				CompletedDeals = reader.ReadInt32(AutoPackType.Packed),
				IsContractOffered = reader.ReadBoolean(),
				OfferedContract = GeneratedReaders___Internal.Read___ScheduleOne.Quests.ContractInfoFishNet.Serializing.Generateds(reader),
				OfferedContractTime = GeneratedReaders___Internal.Read___ScheduleOne.GameTime.GameDateTimeFishNet.Serializing.Generateds(reader),
				TimeSincePlayerApproached = reader.ReadInt32(AutoPackType.Packed),
				TimeSinceInstantDealOffered = reader.ReadInt32(AutoPackType.Packed),
				HasBeenRecommended = reader.ReadBoolean(),
				DataType = reader.ReadString(),
				DataVersion = reader.ReadInt32(AutoPackType.Packed),
				GameVersion = reader.ReadString()
			};
		}

		// Token: 0x0600578C RID: 22412 RVA: 0x00170D70 File Offset: 0x0016EF70
		public static string[] Generateds(Reader reader)
		{
			return reader.ReadArrayAllocated<string>();
		}

		// Token: 0x0600578D RID: 22413 RVA: 0x00170D88 File Offset: 0x0016EF88
		public static float[] Generateds(Reader reader)
		{
			return reader.ReadArrayAllocated<float>();
		}

		// Token: 0x0600578E RID: 22414 RVA: 0x00170DA0 File Offset: 0x0016EFA0
		public static EDrugType Generateds(Reader reader)
		{
			return (EDrugType)reader.ReadInt32(AutoPackType.Packed);
		}

		// Token: 0x0600578F RID: 22415 RVA: 0x00170DBC File Offset: 0x0016EFBC
		public static GameData Generateds(Reader reader)
		{
			bool flag = reader.ReadBoolean();
			if (flag)
			{
				return null;
			}
			return new GameData
			{
				OrganisationName = reader.ReadString(),
				Seed = reader.ReadInt32(AutoPackType.Packed),
				Settings = GeneratedReaders___Internal.Read___ScheduleOne.DevUtilities.GameSettingsFishNet.Serializing.Generateds(reader),
				DataType = reader.ReadString(),
				DataVersion = reader.ReadInt32(AutoPackType.Packed),
				GameVersion = reader.ReadString()
			};
		}

		// Token: 0x06005790 RID: 22416 RVA: 0x00170E68 File Offset: 0x0016F068
		public static GameSettings Generateds(Reader reader)
		{
			bool flag = reader.ReadBoolean();
			if (flag)
			{
				return null;
			}
			return new GameSettings
			{
				ConsoleEnabled = reader.ReadBoolean()
			};
		}

		// Token: 0x06005791 RID: 22417 RVA: 0x00170EB0 File Offset: 0x0016F0B0
		public static DeliveryInstance Generateds(Reader reader)
		{
			bool flag = reader.ReadBoolean();
			if (flag)
			{
				return null;
			}
			return new DeliveryInstance
			{
				DeliveryID = reader.ReadString(),
				StoreName = reader.ReadString(),
				DestinationCode = reader.ReadString(),
				LoadingDockIndex = reader.ReadInt32(AutoPackType.Packed),
				Items = GeneratedReaders___Internal.Read___ScheduleOne.DevUtilities.StringIntPair[]FishNet.Serializing.Generateds(reader),
				Status = GeneratedReaders___Internal.Read___ScheduleOne.Delivery.EDeliveryStatusFishNet.Serializing.Generateds(reader),
				TimeUntilArrival = reader.ReadInt32(AutoPackType.Packed)
			};
		}

		// Token: 0x06005792 RID: 22418 RVA: 0x00170F6C File Offset: 0x0016F16C
		public static EDeliveryStatus Generateds(Reader reader)
		{
			return (EDeliveryStatus)reader.ReadInt32(AutoPackType.Packed);
		}

		// Token: 0x06005793 RID: 22419 RVA: 0x00170F88 File Offset: 0x0016F188
		public static ExplosionData Generateds(Reader reader)
		{
			return new ExplosionData
			{
				DamageRadius = reader.ReadSingle(AutoPackType.Unpacked),
				MaxDamage = reader.ReadSingle(AutoPackType.Unpacked),
				PushForceRadius = reader.ReadSingle(AutoPackType.Unpacked),
				MaxPushForce = reader.ReadSingle(AutoPackType.Unpacked)
			};
		}

		// Token: 0x06005794 RID: 22420 RVA: 0x00171000 File Offset: 0x0016F200
		public static PlayingCard.ECardSuit Generateds(Reader reader)
		{
			return (PlayingCard.ECardSuit)reader.ReadInt32(AutoPackType.Packed);
		}

		// Token: 0x06005795 RID: 22421 RVA: 0x0017101C File Offset: 0x0016F21C
		public static PlayingCard.ECardValue Generateds(Reader reader)
		{
			return (PlayingCard.ECardValue)reader.ReadInt32(AutoPackType.Packed);
		}

		// Token: 0x06005796 RID: 22422 RVA: 0x00171038 File Offset: 0x0016F238
		public static NetworkObject[] Generateds(Reader reader)
		{
			return reader.ReadArrayAllocated<NetworkObject>();
		}

		// Token: 0x06005797 RID: 22423 RVA: 0x00171050 File Offset: 0x0016F250
		public static RTBGameController.EStage Generateds(Reader reader)
		{
			return (RTBGameController.EStage)reader.ReadInt32(AutoPackType.Packed);
		}

		// Token: 0x06005798 RID: 22424 RVA: 0x0017106C File Offset: 0x0016F26C
		public static SlotMachine.ESymbol Generateds(Reader reader)
		{
			return (SlotMachine.ESymbol)reader.ReadInt32(AutoPackType.Packed);
		}

		// Token: 0x06005799 RID: 22425 RVA: 0x00171088 File Offset: 0x0016F288
		public static SlotMachine.ESymbol[] Generateds(Reader reader)
		{
			return reader.ReadArrayAllocated<SlotMachine.ESymbol>();
		}

		// Token: 0x0600579A RID: 22426 RVA: 0x001710A0 File Offset: 0x0016F2A0
		public static EDoorSide Generateds(Reader reader)
		{
			return (EDoorSide)reader.ReadInt32(AutoPackType.Packed);
		}

		// Token: 0x0600579B RID: 22427 RVA: 0x001710BC File Offset: 0x0016F2BC
		public static EVehicleColor Generateds(Reader reader)
		{
			return (EVehicleColor)reader.ReadInt32(AutoPackType.Packed);
		}

		// Token: 0x0600579C RID: 22428 RVA: 0x001710D8 File Offset: 0x0016F2D8
		public static ParkData Generateds(Reader reader)
		{
			bool flag = reader.ReadBoolean();
			if (flag)
			{
				return null;
			}
			return new ParkData
			{
				lotGUID = reader.ReadGuid(),
				spotIndex = reader.ReadInt32(AutoPackType.Packed),
				alignment = GeneratedReaders___Internal.Read___ScheduleOne.Vehicles.EParkingAlignmentFishNet.Serializing.Generateds(reader)
			};
		}

		// Token: 0x0600579D RID: 22429 RVA: 0x00171148 File Offset: 0x0016F348
		public static EParkingAlignment Generateds(Reader reader)
		{
			return (EParkingAlignment)reader.ReadInt32(AutoPackType.Packed);
		}

		// Token: 0x0600579E RID: 22430 RVA: 0x00171164 File Offset: 0x0016F364
		public static TrashContentData Generateds(Reader reader)
		{
			bool flag = reader.ReadBoolean();
			if (flag)
			{
				return null;
			}
			return new TrashContentData
			{
				TrashIDs = GeneratedReaders___Internal.Read___System.String[]FishNet.Serializing.Generateds(reader),
				TrashQuantities = GeneratedReaders___Internal.Read___System.Int32[]FishNet.Serializing.Generateds(reader)
			};
		}

		// Token: 0x0600579F RID: 22431 RVA: 0x001711BC File Offset: 0x0016F3BC
		public static int[] Generateds(Reader reader)
		{
			return reader.ReadArrayAllocated<int>();
		}

		// Token: 0x060057A0 RID: 22432 RVA: 0x001711D4 File Offset: 0x0016F3D4
		public static Coordinate Generateds(Reader reader)
		{
			bool flag = reader.ReadBoolean();
			if (flag)
			{
				return null;
			}
			return new Coordinate
			{
				x = reader.ReadInt32(AutoPackType.Packed),
				y = reader.ReadInt32(AutoPackType.Packed)
			};
		}

		// Token: 0x060057A1 RID: 22433 RVA: 0x00171238 File Offset: 0x0016F438
		public static WeedAppearanceSettings Generateds(Reader reader)
		{
			bool flag = reader.ReadBoolean();
			if (flag)
			{
				return null;
			}
			return new WeedAppearanceSettings
			{
				MainColor = reader.ReadColor32(),
				SecondaryColor = reader.ReadColor32(),
				LeafColor = reader.ReadColor32(),
				StemColor = reader.ReadColor32()
			};
		}

		// Token: 0x060057A2 RID: 22434 RVA: 0x001712B4 File Offset: 0x0016F4B4
		public static CocaineAppearanceSettings Generateds(Reader reader)
		{
			bool flag = reader.ReadBoolean();
			if (flag)
			{
				return null;
			}
			return new CocaineAppearanceSettings
			{
				MainColor = reader.ReadColor32(),
				SecondaryColor = reader.ReadColor32()
			};
		}

		// Token: 0x060057A3 RID: 22435 RVA: 0x0017130C File Offset: 0x0016F50C
		public static MethAppearanceSettings Generateds(Reader reader)
		{
			bool flag = reader.ReadBoolean();
			if (flag)
			{
				return null;
			}
			return new MethAppearanceSettings
			{
				MainColor = reader.ReadColor32(),
				SecondaryColor = reader.ReadColor32()
			};
		}

		// Token: 0x060057A4 RID: 22436 RVA: 0x00171364 File Offset: 0x0016F564
		public static NewMixOperation Generateds(Reader reader)
		{
			bool flag = reader.ReadBoolean();
			if (flag)
			{
				return null;
			}
			return new NewMixOperation
			{
				ProductID = reader.ReadString(),
				IngredientID = reader.ReadString()
			};
		}

		// Token: 0x060057A5 RID: 22437 RVA: 0x001713BC File Offset: 0x0016F5BC
		public static Recycler.EState Generateds(Reader reader)
		{
			return (Recycler.EState)reader.ReadInt32(AutoPackType.Packed);
		}

		// Token: 0x060057A6 RID: 22438 RVA: 0x001713D8 File Offset: 0x0016F5D8
		public static CoordinateProceduralTilePair Generateds(Reader reader)
		{
			return new CoordinateProceduralTilePair
			{
				coord = GeneratedReaders___Internal.Read___ScheduleOne.Tiles.CoordinateFishNet.Serializing.Generateds(reader),
				tileParent = reader.ReadNetworkObject(),
				tileIndex = reader.ReadInt32(AutoPackType.Packed)
			};
		}

		// Token: 0x060057A7 RID: 22439 RVA: 0x00171430 File Offset: 0x0016F630
		public static List<CoordinateProceduralTilePair> List(Reader reader)
		{
			return reader.ReadListAllocated<CoordinateProceduralTilePair>();
		}

		// Token: 0x060057A8 RID: 22440 RVA: 0x00171448 File Offset: 0x0016F648
		public static ChemistryCookOperation Generateds(Reader reader)
		{
			bool flag = reader.ReadBoolean();
			if (flag)
			{
				return null;
			}
			return new ChemistryCookOperation
			{
				RecipeID = reader.ReadString(),
				ProductQuality = GeneratedReaders___Internal.Read___ScheduleOne.ItemFramework.EQualityFishNet.Serializing.Generateds(reader),
				StartLiquidColor = reader.ReadColor(AutoPackType.Packed),
				LiquidLevel = reader.ReadSingle(AutoPackType.Unpacked),
				CurrentTime = reader.ReadInt32(AutoPackType.Packed)
			};
		}

		// Token: 0x060057A9 RID: 22441 RVA: 0x001714E4 File Offset: 0x0016F6E4
		public static DryingOperation Generateds(Reader reader)
		{
			bool flag = reader.ReadBoolean();
			if (flag)
			{
				return null;
			}
			return new DryingOperation
			{
				ItemID = reader.ReadString(),
				Quantity = reader.ReadInt32(AutoPackType.Packed),
				StartQuality = GeneratedReaders___Internal.Read___ScheduleOne.ItemFramework.EQualityFishNet.Serializing.Generateds(reader),
				Time = reader.ReadInt32(AutoPackType.Packed)
			};
		}

		// Token: 0x060057AA RID: 22442 RVA: 0x0017156C File Offset: 0x0016F76C
		public static OvenCookOperation Generateds(Reader reader)
		{
			bool flag = reader.ReadBoolean();
			if (flag)
			{
				return null;
			}
			return new OvenCookOperation
			{
				IngredientID = reader.ReadString(),
				IngredientQuality = GeneratedReaders___Internal.Read___ScheduleOne.ItemFramework.EQualityFishNet.Serializing.Generateds(reader),
				IngredientQuantity = reader.ReadInt32(AutoPackType.Packed),
				ProductID = reader.ReadString(),
				CookProgress = reader.ReadInt32(AutoPackType.Packed)
			};
		}

		// Token: 0x060057AB RID: 22443 RVA: 0x00171604 File Offset: 0x0016F804
		public static MixOperation Generateds(Reader reader)
		{
			bool flag = reader.ReadBoolean();
			if (flag)
			{
				return null;
			}
			return new MixOperation
			{
				ProductID = reader.ReadString(),
				ProductQuality = GeneratedReaders___Internal.Read___ScheduleOne.ItemFramework.EQualityFishNet.Serializing.Generateds(reader),
				IngredientID = reader.ReadString(),
				Quantity = reader.ReadInt32(AutoPackType.Packed)
			};
		}
	}
}
