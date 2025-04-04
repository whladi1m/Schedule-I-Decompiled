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
	// Token: 0x02000C36 RID: 3126
	[StructLayout(LayoutKind.Auto, CharSet = CharSet.Auto)]
	public static class GeneratedWriters___Internal
	{
		// Token: 0x06005700 RID: 22272 RVA: 0x0016D18C File Offset: 0x0016B38C
		[RuntimeInitializeOnLoadMethod]
		private static void InitializeOnce()
		{
			GenericWriter<ItemInstance>.Write = new Action<Writer, ItemInstance>(ItemSerializers.WriteItemInstance);
			GenericWriter<StorableItemInstance>.Write = new Action<Writer, StorableItemInstance>(ItemSerializers.WriteStorableItemInstance);
			GenericWriter<CashInstance>.Write = new Action<Writer, CashInstance>(ItemSerializers.WriteCashInstance);
			GenericWriter<QualityItemInstance>.Write = new Action<Writer, QualityItemInstance>(ItemSerializers.WriteQualityItemInstance);
			GenericWriter<ClothingInstance>.Write = new Action<Writer, ClothingInstance>(ItemSerializers.WriteClothingInstance);
			GenericWriter<ProductItemInstance>.Write = new Action<Writer, ProductItemInstance>(ItemSerializers.WriteProductItemInstance);
			GenericWriter<WeedInstance>.Write = new Action<Writer, WeedInstance>(ItemSerializers.WriteWeedInstance);
			GenericWriter<MethInstance>.Write = new Action<Writer, MethInstance>(ItemSerializers.WriteMethInstance);
			GenericWriter<CocaineInstance>.Write = new Action<Writer, CocaineInstance>(ItemSerializers.WriteCocaineInstance);
			GenericWriter<IntegerItemInstance>.Write = new Action<Writer, IntegerItemInstance>(ItemSerializers.WriteIntegerItemInstance);
			GenericWriter<WateringCanInstance>.Write = new Action<Writer, WateringCanInstance>(ItemSerializers.WriteWateringCanInstance);
			GenericWriter<TrashGrabberInstance>.Write = new Action<Writer, TrashGrabberInstance>(ItemSerializers.WriteTrashGrabberInstance);
			GenericWriter<VisionEventReceipt>.Write = new Action<Writer, VisionEventReceipt>(GeneratedWriters___Internal.Write___ScheduleOne.Vision.VisionEventReceiptFishNet.Serializing.Generated);
			GenericWriter<PlayerVisualState.EVisualState>.Write = new Action<Writer, PlayerVisualState.EVisualState>(GeneratedWriters___Internal.Write___ScheduleOne.PlayerScripts.PlayerVisualState/EVisualStateFishNet.Serializing.Generated);
			GenericWriter<VisionCone.EEventLevel>.Write = new Action<Writer, VisionCone.EEventLevel>(GeneratedWriters___Internal.Write___ScheduleOne.Vision.VisionCone/EEventLevelFishNet.Serializing.Generated);
			GenericWriter<ContractInfo>.Write = new Action<Writer, ContractInfo>(GeneratedWriters___Internal.Write___ScheduleOne.Quests.ContractInfoFishNet.Serializing.Generated);
			GenericWriter<ProductList>.Write = new Action<Writer, ProductList>(GeneratedWriters___Internal.Write___ScheduleOne.Product.ProductListFishNet.Serializing.Generated);
			GenericWriter<ProductList.Entry>.Write = new Action<Writer, ProductList.Entry>(GeneratedWriters___Internal.Write___ScheduleOne.Product.ProductList/EntryFishNet.Serializing.Generated);
			GenericWriter<EQuality>.Write = new Action<Writer, EQuality>(GeneratedWriters___Internal.Write___ScheduleOne.ItemFramework.EQualityFishNet.Serializing.Generated);
			GenericWriter<List<ProductList.Entry>>.Write = new Action<Writer, List<ProductList.Entry>>(GeneratedWriters___Internal.Write___System.Collections.Generic.List`1<ScheduleOne.Product.ProductList/Entry>FishNet.Serializing.Generated);
			GenericWriter<QuestWindowConfig>.Write = new Action<Writer, QuestWindowConfig>(GeneratedWriters___Internal.Write___ScheduleOne.Quests.QuestWindowConfigFishNet.Serializing.Generated);
			GenericWriter<GameDateTime>.Write = new Action<Writer, GameDateTime>(GeneratedWriters___Internal.Write___ScheduleOne.GameTime.GameDateTimeFishNet.Serializing.Generated);
			GenericWriter<QuestManager.EQuestAction>.Write = new Action<Writer, QuestManager.EQuestAction>(GeneratedWriters___Internal.Write___ScheduleOne.Quests.QuestManager/EQuestActionFishNet.Serializing.Generated);
			GenericWriter<EQuestState>.Write = new Action<Writer, EQuestState>(GeneratedWriters___Internal.Write___ScheduleOne.Quests.EQuestStateFishNet.Serializing.Generated);
			GenericWriter<Impact>.Write = new Action<Writer, Impact>(GeneratedWriters___Internal.Write___ScheduleOne.Combat.ImpactFishNet.Serializing.Generated);
			GenericWriter<EImpactType>.Write = new Action<Writer, EImpactType>(GeneratedWriters___Internal.Write___ScheduleOne.Combat.EImpactTypeFishNet.Serializing.Generated);
			GenericWriter<LandVehicle>.Write = new Action<Writer, LandVehicle>(GeneratedWriters___Internal.Write___ScheduleOne.Vehicles.LandVehicleFishNet.Serializing.Generated);
			GenericWriter<CheckpointManager.ECheckpointLocation>.Write = new Action<Writer, CheckpointManager.ECheckpointLocation>(GeneratedWriters___Internal.Write___ScheduleOne.Law.CheckpointManager/ECheckpointLocationFishNet.Serializing.Generated);
			GenericWriter<Player>.Write = new Action<Writer, Player>(GeneratedWriters___Internal.Write___ScheduleOne.PlayerScripts.PlayerFishNet.Serializing.Generated);
			GenericWriter<List<string>>.Write = new Action<Writer, List<string>>(GeneratedWriters___Internal.Write___System.Collections.Generic.List`1<System.String>FishNet.Serializing.Generated);
			GenericWriter<StringIntPair>.Write = new Action<Writer, StringIntPair>(GeneratedWriters___Internal.Write___ScheduleOne.DevUtilities.StringIntPairFishNet.Serializing.Generated);
			GenericWriter<StringIntPair[]>.Write = new Action<Writer, StringIntPair[]>(GeneratedWriters___Internal.Write___ScheduleOne.DevUtilities.StringIntPair[]FishNet.Serializing.Generated);
			GenericWriter<Message>.Write = new Action<Writer, Message>(GeneratedWriters___Internal.Write___ScheduleOne.Messaging.MessageFishNet.Serializing.Generated);
			GenericWriter<Message.ESenderType>.Write = new Action<Writer, Message.ESenderType>(GeneratedWriters___Internal.Write___ScheduleOne.Messaging.Message/ESenderTypeFishNet.Serializing.Generated);
			GenericWriter<MessageChain>.Write = new Action<Writer, MessageChain>(GeneratedWriters___Internal.Write___ScheduleOne.UI.Phone.Messages.MessageChainFishNet.Serializing.Generated);
			GenericWriter<MSGConversationData>.Write = new Action<Writer, MSGConversationData>(GeneratedWriters___Internal.Write___ScheduleOne.Persistence.Datas.MSGConversationDataFishNet.Serializing.Generated);
			GenericWriter<TextMessageData>.Write = new Action<Writer, TextMessageData>(GeneratedWriters___Internal.Write___ScheduleOne.Persistence.Datas.TextMessageDataFishNet.Serializing.Generated);
			GenericWriter<TextMessageData[]>.Write = new Action<Writer, TextMessageData[]>(GeneratedWriters___Internal.Write___ScheduleOne.Persistence.Datas.TextMessageData[]FishNet.Serializing.Generated);
			GenericWriter<TextResponseData>.Write = new Action<Writer, TextResponseData>(GeneratedWriters___Internal.Write___ScheduleOne.Persistence.Datas.TextResponseDataFishNet.Serializing.Generated);
			GenericWriter<TextResponseData[]>.Write = new Action<Writer, TextResponseData[]>(GeneratedWriters___Internal.Write___ScheduleOne.Persistence.Datas.TextResponseData[]FishNet.Serializing.Generated);
			GenericWriter<Response>.Write = new Action<Writer, Response>(GeneratedWriters___Internal.Write___ScheduleOne.Messaging.ResponseFishNet.Serializing.Generated);
			GenericWriter<List<Response>>.Write = new Action<Writer, List<Response>>(GeneratedWriters___Internal.Write___System.Collections.Generic.List`1<ScheduleOne.Messaging.Response>FishNet.Serializing.Generated);
			GenericWriter<List<NetworkObject>>.Write = new Action<Writer, List<NetworkObject>>(GeneratedWriters___Internal.Write___System.Collections.Generic.List`1<FishNet.Object.NetworkObject>FishNet.Serializing.Generated);
			GenericWriter<AdvancedTransitRouteData>.Write = new Action<Writer, AdvancedTransitRouteData>(GeneratedWriters___Internal.Write___ScheduleOne.Persistence.Datas.AdvancedTransitRouteDataFishNet.Serializing.Generated);
			GenericWriter<ManagementItemFilter.EMode>.Write = new Action<Writer, ManagementItemFilter.EMode>(GeneratedWriters___Internal.Write___ScheduleOne.Management.ManagementItemFilter/EModeFishNet.Serializing.Generated);
			GenericWriter<AdvancedTransitRouteData[]>.Write = new Action<Writer, AdvancedTransitRouteData[]>(GeneratedWriters___Internal.Write___ScheduleOne.Persistence.Datas.AdvancedTransitRouteData[]FishNet.Serializing.Generated);
			GenericWriter<ERank>.Write = new Action<Writer, ERank>(GeneratedWriters___Internal.Write___ScheduleOne.Levelling.ERankFishNet.Serializing.Generated);
			GenericWriter<FullRank>.Write = new Action<Writer, FullRank>(GeneratedWriters___Internal.Write___ScheduleOne.Levelling.FullRankFishNet.Serializing.Generated);
			GenericWriter<PlayerData>.Write = new Action<Writer, PlayerData>(GeneratedWriters___Internal.Write___ScheduleOne.Persistence.Datas.PlayerDataFishNet.Serializing.Generated);
			GenericWriter<VariableData>.Write = new Action<Writer, VariableData>(GeneratedWriters___Internal.Write___ScheduleOne.Persistence.Datas.VariableDataFishNet.Serializing.Generated);
			GenericWriter<VariableData[]>.Write = new Action<Writer, VariableData[]>(GeneratedWriters___Internal.Write___ScheduleOne.Persistence.Datas.VariableData[]FishNet.Serializing.Generated);
			GenericWriter<AvatarSettings>.Write = new Action<Writer, AvatarSettings>(GeneratedWriters___Internal.Write___ScheduleOne.AvatarFramework.AvatarSettingsFishNet.Serializing.Generated);
			GenericWriter<Eye.EyeLidConfiguration>.Write = new Action<Writer, Eye.EyeLidConfiguration>(GeneratedWriters___Internal.Write___ScheduleOne.AvatarFramework.Eye/EyeLidConfigurationFishNet.Serializing.Generated);
			GenericWriter<AvatarSettings.LayerSetting>.Write = new Action<Writer, AvatarSettings.LayerSetting>(GeneratedWriters___Internal.Write___ScheduleOne.AvatarFramework.AvatarSettings/LayerSettingFishNet.Serializing.Generated);
			GenericWriter<List<AvatarSettings.LayerSetting>>.Write = new Action<Writer, List<AvatarSettings.LayerSetting>>(GeneratedWriters___Internal.Write___System.Collections.Generic.List`1<ScheduleOne.AvatarFramework.AvatarSettings/LayerSetting>FishNet.Serializing.Generated);
			GenericWriter<AvatarSettings.AccessorySetting>.Write = new Action<Writer, AvatarSettings.AccessorySetting>(GeneratedWriters___Internal.Write___ScheduleOne.AvatarFramework.AvatarSettings/AccessorySettingFishNet.Serializing.Generated);
			GenericWriter<List<AvatarSettings.AccessorySetting>>.Write = new Action<Writer, List<AvatarSettings.AccessorySetting>>(GeneratedWriters___Internal.Write___System.Collections.Generic.List`1<ScheduleOne.AvatarFramework.AvatarSettings/AccessorySetting>FishNet.Serializing.Generated);
			GenericWriter<BasicAvatarSettings>.Write = new Action<Writer, BasicAvatarSettings>(GeneratedWriters___Internal.Write___ScheduleOne.AvatarFramework.Customization.BasicAvatarSettingsFishNet.Serializing.Generated);
			GenericWriter<PlayerCrimeData.EPursuitLevel>.Write = new Action<Writer, PlayerCrimeData.EPursuitLevel>(GeneratedWriters___Internal.Write___ScheduleOne.PlayerScripts.PlayerCrimeData/EPursuitLevelFishNet.Serializing.Generated);
			GenericWriter<Property>.Write = new Action<Writer, Property>(GeneratedWriters___Internal.Write___ScheduleOne.Property.PropertyFishNet.Serializing.Generated);
			GenericWriter<EEmployeeType>.Write = new Action<Writer, EEmployeeType>(GeneratedWriters___Internal.Write___ScheduleOne.Employees.EEmployeeTypeFishNet.Serializing.Generated);
			GenericWriter<EDealWindow>.Write = new Action<Writer, EDealWindow>(GeneratedWriters___Internal.Write___ScheduleOne.Economy.EDealWindowFishNet.Serializing.Generated);
			GenericWriter<HandoverScreen.EHandoverOutcome>.Write = new Action<Writer, HandoverScreen.EHandoverOutcome>(GeneratedWriters___Internal.Write___ScheduleOne.UI.Handover.HandoverScreen/EHandoverOutcomeFishNet.Serializing.Generated);
			GenericWriter<List<ItemInstance>>.Write = new Action<Writer, List<ItemInstance>>(GeneratedWriters___Internal.Write___System.Collections.Generic.List`1<ScheduleOne.ItemFramework.ItemInstance>FishNet.Serializing.Generated);
			GenericWriter<ScheduleOne.Persistence.Datas.CustomerData>.Write = new Action<Writer, ScheduleOne.Persistence.Datas.CustomerData>(GeneratedWriters___Internal.Write___ScheduleOne.Persistence.Datas.CustomerDataFishNet.Serializing.Generated);
			GenericWriter<string[]>.Write = new Action<Writer, string[]>(GeneratedWriters___Internal.Write___System.String[]FishNet.Serializing.Generated);
			GenericWriter<float[]>.Write = new Action<Writer, float[]>(GeneratedWriters___Internal.Write___System.Single[]FishNet.Serializing.Generated);
			GenericWriter<EDrugType>.Write = new Action<Writer, EDrugType>(GeneratedWriters___Internal.Write___ScheduleOne.Product.EDrugTypeFishNet.Serializing.Generated);
			GenericWriter<GameData>.Write = new Action<Writer, GameData>(GeneratedWriters___Internal.Write___ScheduleOne.Persistence.Datas.GameDataFishNet.Serializing.Generated);
			GenericWriter<GameSettings>.Write = new Action<Writer, GameSettings>(GeneratedWriters___Internal.Write___ScheduleOne.DevUtilities.GameSettingsFishNet.Serializing.Generated);
			GenericWriter<DeliveryInstance>.Write = new Action<Writer, DeliveryInstance>(GeneratedWriters___Internal.Write___ScheduleOne.Delivery.DeliveryInstanceFishNet.Serializing.Generated);
			GenericWriter<EDeliveryStatus>.Write = new Action<Writer, EDeliveryStatus>(GeneratedWriters___Internal.Write___ScheduleOne.Delivery.EDeliveryStatusFishNet.Serializing.Generated);
			GenericWriter<ExplosionData>.Write = new Action<Writer, ExplosionData>(GeneratedWriters___Internal.Write___ScheduleOne.Combat.ExplosionDataFishNet.Serializing.Generated);
			GenericWriter<PlayingCard.ECardSuit>.Write = new Action<Writer, PlayingCard.ECardSuit>(GeneratedWriters___Internal.Write___ScheduleOne.Casino.PlayingCard/ECardSuitFishNet.Serializing.Generated);
			GenericWriter<PlayingCard.ECardValue>.Write = new Action<Writer, PlayingCard.ECardValue>(GeneratedWriters___Internal.Write___ScheduleOne.Casino.PlayingCard/ECardValueFishNet.Serializing.Generated);
			GenericWriter<NetworkObject[]>.Write = new Action<Writer, NetworkObject[]>(GeneratedWriters___Internal.Write___FishNet.Object.NetworkObject[]FishNet.Serializing.Generated);
			GenericWriter<RTBGameController.EStage>.Write = new Action<Writer, RTBGameController.EStage>(GeneratedWriters___Internal.Write___ScheduleOne.Casino.RTBGameController/EStageFishNet.Serializing.Generated);
			GenericWriter<SlotMachine.ESymbol>.Write = new Action<Writer, SlotMachine.ESymbol>(GeneratedWriters___Internal.Write___ScheduleOne.Casino.SlotMachine/ESymbolFishNet.Serializing.Generated);
			GenericWriter<SlotMachine.ESymbol[]>.Write = new Action<Writer, SlotMachine.ESymbol[]>(GeneratedWriters___Internal.Write___ScheduleOne.Casino.SlotMachine/ESymbol[]FishNet.Serializing.Generated);
			GenericWriter<EDoorSide>.Write = new Action<Writer, EDoorSide>(GeneratedWriters___Internal.Write___ScheduleOne.Doors.EDoorSideFishNet.Serializing.Generated);
			GenericWriter<EVehicleColor>.Write = new Action<Writer, EVehicleColor>(GeneratedWriters___Internal.Write___ScheduleOne.Vehicles.Modification.EVehicleColorFishNet.Serializing.Generated);
			GenericWriter<ParkData>.Write = new Action<Writer, ParkData>(GeneratedWriters___Internal.Write___ScheduleOne.Vehicles.ParkDataFishNet.Serializing.Generated);
			GenericWriter<EParkingAlignment>.Write = new Action<Writer, EParkingAlignment>(GeneratedWriters___Internal.Write___ScheduleOne.Vehicles.EParkingAlignmentFishNet.Serializing.Generated);
			GenericWriter<TrashContentData>.Write = new Action<Writer, TrashContentData>(GeneratedWriters___Internal.Write___ScheduleOne.Persistence.TrashContentDataFishNet.Serializing.Generated);
			GenericWriter<int[]>.Write = new Action<Writer, int[]>(GeneratedWriters___Internal.Write___System.Int32[]FishNet.Serializing.Generated);
			GenericWriter<Coordinate>.Write = new Action<Writer, Coordinate>(GeneratedWriters___Internal.Write___ScheduleOne.Tiles.CoordinateFishNet.Serializing.Generated);
			GenericWriter<WeedAppearanceSettings>.Write = new Action<Writer, WeedAppearanceSettings>(GeneratedWriters___Internal.Write___ScheduleOne.Product.WeedAppearanceSettingsFishNet.Serializing.Generated);
			GenericWriter<CocaineAppearanceSettings>.Write = new Action<Writer, CocaineAppearanceSettings>(GeneratedWriters___Internal.Write___ScheduleOne.Product.CocaineAppearanceSettingsFishNet.Serializing.Generated);
			GenericWriter<MethAppearanceSettings>.Write = new Action<Writer, MethAppearanceSettings>(GeneratedWriters___Internal.Write___ScheduleOne.Product.MethAppearanceSettingsFishNet.Serializing.Generated);
			GenericWriter<NewMixOperation>.Write = new Action<Writer, NewMixOperation>(GeneratedWriters___Internal.Write___ScheduleOne.Product.NewMixOperationFishNet.Serializing.Generated);
			GenericWriter<Recycler.EState>.Write = new Action<Writer, Recycler.EState>(GeneratedWriters___Internal.Write___ScheduleOne.ObjectScripts.Recycler/EStateFishNet.Serializing.Generated);
			GenericWriter<CoordinateProceduralTilePair>.Write = new Action<Writer, CoordinateProceduralTilePair>(GeneratedWriters___Internal.Write___ScheduleOne.Tiles.CoordinateProceduralTilePairFishNet.Serializing.Generated);
			GenericWriter<List<CoordinateProceduralTilePair>>.Write = new Action<Writer, List<CoordinateProceduralTilePair>>(GeneratedWriters___Internal.Write___System.Collections.Generic.List`1<ScheduleOne.Tiles.CoordinateProceduralTilePair>FishNet.Serializing.Generated);
			GenericWriter<ChemistryCookOperation>.Write = new Action<Writer, ChemistryCookOperation>(GeneratedWriters___Internal.Write___ScheduleOne.ObjectScripts.ChemistryCookOperationFishNet.Serializing.Generated);
			GenericWriter<DryingOperation>.Write = new Action<Writer, DryingOperation>(GeneratedWriters___Internal.Write___ScheduleOne.ObjectScripts.DryingOperationFishNet.Serializing.Generated);
			GenericWriter<OvenCookOperation>.Write = new Action<Writer, OvenCookOperation>(GeneratedWriters___Internal.Write___ScheduleOne.ObjectScripts.OvenCookOperationFishNet.Serializing.Generated);
			GenericWriter<MixOperation>.Write = new Action<Writer, MixOperation>(GeneratedWriters___Internal.Write___ScheduleOne.ObjectScripts.MixOperationFishNet.Serializing.Generated);
		}

		// Token: 0x06005701 RID: 22273 RVA: 0x0016D80C File Offset: 0x0016BA0C
		public static void Generated(this Writer writer, VisionEventReceipt value)
		{
			if (value == null)
			{
				writer.WriteBoolean(true);
				return;
			}
			writer.WriteBoolean(false);
			writer.WriteNetworkObject(value.TargetPlayer);
			writer.Write___ScheduleOne.PlayerScripts.PlayerVisualState/EVisualStateFishNet.Serializing.Generated(value.State);
		}

		// Token: 0x06005702 RID: 22274 RVA: 0x0016D864 File Offset: 0x0016BA64
		public static void Generated(this Writer writer, PlayerVisualState.EVisualState value)
		{
			writer.WriteInt32((int)value, AutoPackType.Packed);
		}

		// Token: 0x06005703 RID: 22275 RVA: 0x0016D884 File Offset: 0x0016BA84
		public static void Generated(this Writer writer, VisionCone.EEventLevel value)
		{
			writer.WriteInt32((int)value, AutoPackType.Packed);
		}

		// Token: 0x06005704 RID: 22276 RVA: 0x0016D8A4 File Offset: 0x0016BAA4
		public static void Generated(this Writer writer, ContractInfo value)
		{
			if (value == null)
			{
				writer.WriteBoolean(true);
				return;
			}
			writer.WriteBoolean(false);
			writer.WriteSingle(value.Payment, AutoPackType.Unpacked);
			writer.Write___ScheduleOne.Product.ProductListFishNet.Serializing.Generated(value.Products);
			writer.WriteString(value.DeliveryLocationGUID);
			writer.Write___ScheduleOne.Quests.QuestWindowConfigFishNet.Serializing.Generated(value.DeliveryWindow);
			writer.WriteBoolean(value.Expires);
			writer.WriteInt32(value.ExpiresAfter, AutoPackType.Packed);
			writer.WriteInt32(value.PickupScheduleIndex, AutoPackType.Packed);
			writer.WriteBoolean(value.IsCounterOffer);
		}

		// Token: 0x06005705 RID: 22277 RVA: 0x0016D978 File Offset: 0x0016BB78
		public static void Generated(this Writer writer, ProductList value)
		{
			if (value == null)
			{
				writer.WriteBoolean(true);
				return;
			}
			writer.WriteBoolean(false);
			writer.Write___System.Collections.Generic.List`1<ScheduleOne.Product.ProductList/Entry>FishNet.Serializing.Generated(value.entries);
		}

		// Token: 0x06005706 RID: 22278 RVA: 0x0016D9C0 File Offset: 0x0016BBC0
		public static void Generated(this Writer writer, ProductList.Entry value)
		{
			if (value == null)
			{
				writer.WriteBoolean(true);
				return;
			}
			writer.WriteBoolean(false);
			writer.WriteString(value.ProductID);
			writer.Write___ScheduleOne.ItemFramework.EQualityFishNet.Serializing.Generated(value.Quality);
			writer.WriteInt32(value.Quantity, AutoPackType.Packed);
		}

		// Token: 0x06005707 RID: 22279 RVA: 0x0016DA30 File Offset: 0x0016BC30
		public static void Generated(this Writer writer, EQuality value)
		{
			writer.WriteInt32((int)value, AutoPackType.Packed);
		}

		// Token: 0x06005708 RID: 22280 RVA: 0x0016DA50 File Offset: 0x0016BC50
		public static void List(this Writer writer, List<ProductList.Entry> value)
		{
			writer.WriteList<ProductList.Entry>(value);
		}

		// Token: 0x06005709 RID: 22281 RVA: 0x0016DA6C File Offset: 0x0016BC6C
		public static void Generated(this Writer writer, QuestWindowConfig value)
		{
			if (value == null)
			{
				writer.WriteBoolean(true);
				return;
			}
			writer.WriteBoolean(false);
			writer.WriteBoolean(value.IsEnabled);
			writer.WriteInt32(value.WindowStartTime, AutoPackType.Packed);
			writer.WriteInt32(value.WindowEndTime, AutoPackType.Packed);
		}

		// Token: 0x0600570A RID: 22282 RVA: 0x0016DAE0 File Offset: 0x0016BCE0
		public static void Generated(this Writer writer, GameDateTime value)
		{
			writer.WriteInt32(value.elapsedDays, AutoPackType.Packed);
			writer.WriteInt32(value.time, AutoPackType.Packed);
		}

		// Token: 0x0600570B RID: 22283 RVA: 0x0016DB1C File Offset: 0x0016BD1C
		public static void Generated(this Writer writer, QuestManager.EQuestAction value)
		{
			writer.WriteInt32((int)value, AutoPackType.Packed);
		}

		// Token: 0x0600570C RID: 22284 RVA: 0x0016DB3C File Offset: 0x0016BD3C
		public static void Generated(this Writer writer, EQuestState value)
		{
			writer.WriteInt32((int)value, AutoPackType.Packed);
		}

		// Token: 0x0600570D RID: 22285 RVA: 0x0016DB5C File Offset: 0x0016BD5C
		public static void Generated(this Writer writer, Impact value)
		{
			if (value == null)
			{
				writer.WriteBoolean(true);
				return;
			}
			writer.WriteBoolean(false);
			writer.WriteVector3(value.HitPoint);
			writer.WriteVector3(value.ImpactForceDirection);
			writer.WriteSingle(value.ImpactForce, AutoPackType.Unpacked);
			writer.WriteSingle(value.ImpactDamage, AutoPackType.Unpacked);
			writer.Write___ScheduleOne.Combat.EImpactTypeFishNet.Serializing.Generated(value.ImpactType);
			writer.WriteNetworkObject(value.ImpactSource);
			writer.WriteInt32(value.ImpactID, AutoPackType.Packed);
		}

		// Token: 0x0600570E RID: 22286 RVA: 0x0016DC20 File Offset: 0x0016BE20
		public static void Generated(this Writer writer, EImpactType value)
		{
			writer.WriteInt32((int)value, AutoPackType.Packed);
		}

		// Token: 0x0600570F RID: 22287 RVA: 0x0016DC40 File Offset: 0x0016BE40
		public static void Generated(this Writer writer, LandVehicle value)
		{
			writer.WriteNetworkBehaviour(value);
		}

		// Token: 0x06005710 RID: 22288 RVA: 0x0016DC5C File Offset: 0x0016BE5C
		public static void Generated(this Writer writer, CheckpointManager.ECheckpointLocation value)
		{
			writer.WriteInt32((int)value, AutoPackType.Packed);
		}

		// Token: 0x06005711 RID: 22289 RVA: 0x0016DC7C File Offset: 0x0016BE7C
		public static void Generated(this Writer writer, Player value)
		{
			writer.WriteNetworkBehaviour(value);
		}

		// Token: 0x06005712 RID: 22290 RVA: 0x0016DC98 File Offset: 0x0016BE98
		public static void List(this Writer writer, List<string> value)
		{
			writer.WriteList<string>(value);
		}

		// Token: 0x06005713 RID: 22291 RVA: 0x0016DCB4 File Offset: 0x0016BEB4
		public static void Generated(this Writer writer, StringIntPair value)
		{
			if (value == null)
			{
				writer.WriteBoolean(true);
				return;
			}
			writer.WriteBoolean(false);
			writer.WriteString(value.String);
			writer.WriteInt32(value.Int, AutoPackType.Packed);
		}

		// Token: 0x06005714 RID: 22292 RVA: 0x0016DD14 File Offset: 0x0016BF14
		public static void Generated(this Writer writer, StringIntPair[] value)
		{
			writer.WriteArray<StringIntPair>(value);
		}

		// Token: 0x06005715 RID: 22293 RVA: 0x0016DD30 File Offset: 0x0016BF30
		public static void Generated(this Writer writer, Message value)
		{
			if (value == null)
			{
				writer.WriteBoolean(true);
				return;
			}
			writer.WriteBoolean(false);
			writer.WriteInt32(value.messageId, AutoPackType.Packed);
			writer.WriteString(value.text);
			writer.Write___ScheduleOne.Messaging.Message/ESenderTypeFishNet.Serializing.Generated(value.sender);
			writer.WriteBoolean(value.endOfGroup);
		}

		// Token: 0x06005716 RID: 22294 RVA: 0x0016DDB4 File Offset: 0x0016BFB4
		public static void Generated(this Writer writer, Message.ESenderType value)
		{
			writer.WriteInt32((int)value, AutoPackType.Packed);
		}

		// Token: 0x06005717 RID: 22295 RVA: 0x0016DDD4 File Offset: 0x0016BFD4
		public static void Generated(this Writer writer, MessageChain value)
		{
			if (value == null)
			{
				writer.WriteBoolean(true);
				return;
			}
			writer.WriteBoolean(false);
			writer.Write___System.Collections.Generic.List`1<System.String>FishNet.Serializing.Generated(value.Messages);
			writer.WriteInt32(value.id, AutoPackType.Packed);
		}

		// Token: 0x06005718 RID: 22296 RVA: 0x0016DE34 File Offset: 0x0016C034
		public static void Generated(this Writer writer, MSGConversationData value)
		{
			if (value == null)
			{
				writer.WriteBoolean(true);
				return;
			}
			writer.WriteBoolean(false);
			writer.WriteInt32(value.ConversationIndex, AutoPackType.Packed);
			writer.WriteBoolean(value.Read);
			writer.Write___ScheduleOne.Persistence.Datas.TextMessageData[]FishNet.Serializing.Generated(value.MessageHistory);
			writer.Write___ScheduleOne.Persistence.Datas.TextResponseData[]FishNet.Serializing.Generated(value.ActiveResponses);
			writer.WriteBoolean(value.IsHidden);
			writer.WriteString(value.DataType);
			writer.WriteInt32(value.DataVersion, AutoPackType.Packed);
			writer.WriteString(value.GameVersion);
		}

		// Token: 0x06005719 RID: 22297 RVA: 0x0016DF04 File Offset: 0x0016C104
		public static void Generated(this Writer writer, TextMessageData value)
		{
			if (value == null)
			{
				writer.WriteBoolean(true);
				return;
			}
			writer.WriteBoolean(false);
			writer.WriteInt32(value.Sender, AutoPackType.Packed);
			writer.WriteInt32(value.MessageID, AutoPackType.Packed);
			writer.WriteString(value.Text);
			writer.WriteBoolean(value.EndOfChain);
		}

		// Token: 0x0600571A RID: 22298 RVA: 0x0016DF8C File Offset: 0x0016C18C
		public static void Generated(this Writer writer, TextMessageData[] value)
		{
			writer.WriteArray<TextMessageData>(value);
		}

		// Token: 0x0600571B RID: 22299 RVA: 0x0016DFA8 File Offset: 0x0016C1A8
		public static void Generated(this Writer writer, TextResponseData value)
		{
			if (value == null)
			{
				writer.WriteBoolean(true);
				return;
			}
			writer.WriteBoolean(false);
			writer.WriteString(value.Text);
			writer.WriteString(value.Label);
		}

		// Token: 0x0600571C RID: 22300 RVA: 0x0016E000 File Offset: 0x0016C200
		public static void Generated(this Writer writer, TextResponseData[] value)
		{
			writer.WriteArray<TextResponseData>(value);
		}

		// Token: 0x0600571D RID: 22301 RVA: 0x0016E01C File Offset: 0x0016C21C
		public static void Generated(this Writer writer, Response value)
		{
			if (value == null)
			{
				writer.WriteBoolean(true);
				return;
			}
			writer.WriteBoolean(false);
			writer.WriteString(value.text);
			writer.WriteString(value.label);
			writer.WriteBoolean(value.disableDefaultResponseBehaviour);
		}

		// Token: 0x0600571E RID: 22302 RVA: 0x0016E088 File Offset: 0x0016C288
		public static void List(this Writer writer, List<Response> value)
		{
			writer.WriteList<Response>(value);
		}

		// Token: 0x0600571F RID: 22303 RVA: 0x0016E0A4 File Offset: 0x0016C2A4
		public static void List(this Writer writer, List<NetworkObject> value)
		{
			writer.WriteList<NetworkObject>(value);
		}

		// Token: 0x06005720 RID: 22304 RVA: 0x0016E0C0 File Offset: 0x0016C2C0
		public static void Generated(this Writer writer, AdvancedTransitRouteData value)
		{
			if (value == null)
			{
				writer.WriteBoolean(true);
				return;
			}
			writer.WriteBoolean(false);
			writer.WriteString(value.SourceGUID);
			writer.WriteString(value.DestinationGUID);
			writer.Write___ScheduleOne.Management.ManagementItemFilter/EModeFishNet.Serializing.Generated(value.FilterMode);
			writer.Write___System.Collections.Generic.List`1<System.String>FishNet.Serializing.Generated(value.FilterItemIDs);
		}

		// Token: 0x06005721 RID: 22305 RVA: 0x0016E13C File Offset: 0x0016C33C
		public static void Generated(this Writer writer, ManagementItemFilter.EMode value)
		{
			writer.WriteInt32((int)value, AutoPackType.Packed);
		}

		// Token: 0x06005722 RID: 22306 RVA: 0x0016E15C File Offset: 0x0016C35C
		public static void Generated(this Writer writer, AdvancedTransitRouteData[] value)
		{
			writer.WriteArray<AdvancedTransitRouteData>(value);
		}

		// Token: 0x06005723 RID: 22307 RVA: 0x0016E178 File Offset: 0x0016C378
		public static void Generated(this Writer writer, ERank value)
		{
			writer.WriteInt32((int)value, AutoPackType.Packed);
		}

		// Token: 0x06005724 RID: 22308 RVA: 0x0016E198 File Offset: 0x0016C398
		public static void Generated(this Writer writer, FullRank value)
		{
			writer.Write___ScheduleOne.Levelling.ERankFishNet.Serializing.Generated(value.Rank);
			writer.WriteInt32(value.Tier, AutoPackType.Packed);
		}

		// Token: 0x06005725 RID: 22309 RVA: 0x0016E1D0 File Offset: 0x0016C3D0
		public static void Generated(this Writer writer, PlayerData value)
		{
			if (value == null)
			{
				writer.WriteBoolean(true);
				return;
			}
			writer.WriteBoolean(false);
			writer.WriteString(value.PlayerCode);
			writer.WriteVector3(value.Position);
			writer.WriteSingle(value.Rotation, AutoPackType.Unpacked);
			writer.WriteBoolean(value.IntroCompleted);
			writer.WriteString(value.DataType);
			writer.WriteInt32(value.DataVersion, AutoPackType.Packed);
			writer.WriteString(value.GameVersion);
		}

		// Token: 0x06005726 RID: 22310 RVA: 0x0016E28C File Offset: 0x0016C48C
		public static void Generated(this Writer writer, VariableData value)
		{
			if (value == null)
			{
				writer.WriteBoolean(true);
				return;
			}
			writer.WriteBoolean(false);
			writer.WriteString(value.Name);
			writer.WriteString(value.Value);
			writer.WriteString(value.DataType);
			writer.WriteInt32(value.DataVersion, AutoPackType.Packed);
			writer.WriteString(value.GameVersion);
		}

		// Token: 0x06005727 RID: 22311 RVA: 0x0016E320 File Offset: 0x0016C520
		public static void Generated(this Writer writer, VariableData[] value)
		{
			writer.WriteArray<VariableData>(value);
		}

		// Token: 0x06005728 RID: 22312 RVA: 0x0016E33C File Offset: 0x0016C53C
		public static void Generated(this Writer writer, AvatarSettings value)
		{
			if (value == null)
			{
				writer.WriteBoolean(true);
				return;
			}
			writer.WriteBoolean(false);
			writer.WriteColor(value.SkinColor, AutoPackType.Packed);
			writer.WriteSingle(value.Height, AutoPackType.Unpacked);
			writer.WriteSingle(value.Gender, AutoPackType.Unpacked);
			writer.WriteSingle(value.Weight, AutoPackType.Unpacked);
			writer.WriteString(value.HairPath);
			writer.WriteColor(value.HairColor, AutoPackType.Packed);
			writer.WriteSingle(value.EyebrowScale, AutoPackType.Unpacked);
			writer.WriteSingle(value.EyebrowThickness, AutoPackType.Unpacked);
			writer.WriteSingle(value.EyebrowRestingHeight, AutoPackType.Unpacked);
			writer.WriteSingle(value.EyebrowRestingAngle, AutoPackType.Unpacked);
			writer.WriteColor(value.LeftEyeLidColor, AutoPackType.Packed);
			writer.WriteColor(value.RightEyeLidColor, AutoPackType.Packed);
			writer.Write___ScheduleOne.AvatarFramework.Eye/EyeLidConfigurationFishNet.Serializing.Generated(value.LeftEyeRestingState);
			writer.Write___ScheduleOne.AvatarFramework.Eye/EyeLidConfigurationFishNet.Serializing.Generated(value.RightEyeRestingState);
			writer.WriteString(value.EyeballMaterialIdentifier);
			writer.WriteColor(value.EyeBallTint, AutoPackType.Packed);
			writer.WriteSingle(value.PupilDilation, AutoPackType.Unpacked);
			writer.Write___System.Collections.Generic.List`1<ScheduleOne.AvatarFramework.AvatarSettings/LayerSetting>FishNet.Serializing.Generated(value.FaceLayerSettings);
			writer.Write___System.Collections.Generic.List`1<ScheduleOne.AvatarFramework.AvatarSettings/LayerSetting>FishNet.Serializing.Generated(value.BodyLayerSettings);
			writer.Write___System.Collections.Generic.List`1<ScheduleOne.AvatarFramework.AvatarSettings/AccessorySetting>FishNet.Serializing.Generated(value.AccessorySettings);
			writer.WriteBoolean(value.UseCombinedLayer);
			writer.WriteString(value.CombinedLayerPath);
		}

		// Token: 0x06005729 RID: 22313 RVA: 0x0016E540 File Offset: 0x0016C740
		public static void Generated(this Writer writer, Eye.EyeLidConfiguration value)
		{
			writer.WriteSingle(value.topLidOpen, AutoPackType.Unpacked);
			writer.WriteSingle(value.bottomLidOpen, AutoPackType.Unpacked);
		}

		// Token: 0x0600572A RID: 22314 RVA: 0x0016E57C File Offset: 0x0016C77C
		public static void Generated(this Writer writer, AvatarSettings.LayerSetting value)
		{
			writer.WriteString(value.layerPath);
			writer.WriteColor(value.layerTint, AutoPackType.Packed);
		}

		// Token: 0x0600572B RID: 22315 RVA: 0x0016E5B4 File Offset: 0x0016C7B4
		public static void List(this Writer writer, List<AvatarSettings.LayerSetting> value)
		{
			writer.WriteList<AvatarSettings.LayerSetting>(value);
		}

		// Token: 0x0600572C RID: 22316 RVA: 0x0016E5D0 File Offset: 0x0016C7D0
		public static void Generated(this Writer writer, AvatarSettings.AccessorySetting value)
		{
			if (value == null)
			{
				writer.WriteBoolean(true);
				return;
			}
			writer.WriteBoolean(false);
			writer.WriteString(value.path);
			writer.WriteColor(value.color, AutoPackType.Packed);
		}

		// Token: 0x0600572D RID: 22317 RVA: 0x0016E630 File Offset: 0x0016C830
		public static void List(this Writer writer, List<AvatarSettings.AccessorySetting> value)
		{
			writer.WriteList<AvatarSettings.AccessorySetting>(value);
		}

		// Token: 0x0600572E RID: 22318 RVA: 0x0016E64C File Offset: 0x0016C84C
		public static void Generated(this Writer writer, BasicAvatarSettings value)
		{
			if (value == null)
			{
				writer.WriteBoolean(true);
				return;
			}
			writer.WriteBoolean(false);
			writer.WriteInt32(value.Gender, AutoPackType.Packed);
			writer.WriteSingle(value.Weight, AutoPackType.Unpacked);
			writer.WriteColor(value.SkinColor, AutoPackType.Packed);
			writer.WriteString(value.HairStyle);
			writer.WriteColor(value.HairColor, AutoPackType.Packed);
			writer.WriteString(value.Mouth);
			writer.WriteString(value.FacialHair);
			writer.WriteString(value.FacialDetails);
			writer.WriteSingle(value.FacialDetailsIntensity, AutoPackType.Unpacked);
			writer.WriteColor(value.EyeballColor, AutoPackType.Packed);
			writer.WriteSingle(value.UpperEyeLidRestingPosition, AutoPackType.Unpacked);
			writer.WriteSingle(value.LowerEyeLidRestingPosition, AutoPackType.Unpacked);
			writer.WriteSingle(value.PupilDilation, AutoPackType.Unpacked);
			writer.WriteSingle(value.EyebrowScale, AutoPackType.Unpacked);
			writer.WriteSingle(value.EyebrowThickness, AutoPackType.Unpacked);
			writer.WriteSingle(value.EyebrowRestingHeight, AutoPackType.Unpacked);
			writer.WriteSingle(value.EyebrowRestingAngle, AutoPackType.Unpacked);
			writer.WriteString(value.Top);
			writer.WriteColor(value.TopColor, AutoPackType.Packed);
			writer.WriteString(value.Bottom);
			writer.WriteColor(value.BottomColor, AutoPackType.Packed);
			writer.WriteString(value.Shoes);
			writer.WriteColor(value.ShoesColor, AutoPackType.Packed);
			writer.WriteString(value.Headwear);
			writer.WriteColor(value.HeadwearColor, AutoPackType.Packed);
			writer.WriteString(value.Eyewear);
			writer.WriteColor(value.EyewearColor, AutoPackType.Packed);
			writer.Write___System.Collections.Generic.List`1<System.String>FishNet.Serializing.Generated(value.Tattoos);
		}

		// Token: 0x0600572F RID: 22319 RVA: 0x0016E8D4 File Offset: 0x0016CAD4
		public static void Generated(this Writer writer, PlayerCrimeData.EPursuitLevel value)
		{
			writer.WriteInt32((int)value, AutoPackType.Packed);
		}

		// Token: 0x06005730 RID: 22320 RVA: 0x0016E8F4 File Offset: 0x0016CAF4
		public static void Generated(this Writer writer, Property value)
		{
			writer.WriteNetworkBehaviour(value);
		}

		// Token: 0x06005731 RID: 22321 RVA: 0x0016E910 File Offset: 0x0016CB10
		public static void Generated(this Writer writer, EEmployeeType value)
		{
			writer.WriteInt32((int)value, AutoPackType.Packed);
		}

		// Token: 0x06005732 RID: 22322 RVA: 0x0016E930 File Offset: 0x0016CB30
		public static void Generated(this Writer writer, EDealWindow value)
		{
			writer.WriteInt32((int)value, AutoPackType.Packed);
		}

		// Token: 0x06005733 RID: 22323 RVA: 0x0016E950 File Offset: 0x0016CB50
		public static void Generated(this Writer writer, HandoverScreen.EHandoverOutcome value)
		{
			writer.WriteInt32((int)value, AutoPackType.Packed);
		}

		// Token: 0x06005734 RID: 22324 RVA: 0x0016E970 File Offset: 0x0016CB70
		public static void List(this Writer writer, List<ItemInstance> value)
		{
			writer.WriteList<ItemInstance>(value);
		}

		// Token: 0x06005735 RID: 22325 RVA: 0x0016E98C File Offset: 0x0016CB8C
		public static void Generated(this Writer writer, ScheduleOne.Persistence.Datas.CustomerData value)
		{
			if (value == null)
			{
				writer.WriteBoolean(true);
				return;
			}
			writer.WriteBoolean(false);
			writer.WriteSingle(value.Dependence, AutoPackType.Unpacked);
			writer.Write___System.String[]FishNet.Serializing.Generated(value.PurchaseableProducts);
			writer.Write___System.Single[]FishNet.Serializing.Generated(value.ProductAffinities);
			writer.WriteInt32(value.TimeSinceLastDealCompleted, AutoPackType.Packed);
			writer.WriteInt32(value.TimeSinceLastDealOffered, AutoPackType.Packed);
			writer.WriteInt32(value.OfferedDeals, AutoPackType.Packed);
			writer.WriteInt32(value.CompletedDeals, AutoPackType.Packed);
			writer.WriteBoolean(value.IsContractOffered);
			writer.Write___ScheduleOne.Quests.ContractInfoFishNet.Serializing.Generated(value.OfferedContract);
			writer.Write___ScheduleOne.GameTime.GameDateTimeFishNet.Serializing.Generated(value.OfferedContractTime);
			writer.WriteInt32(value.TimeSincePlayerApproached, AutoPackType.Packed);
			writer.WriteInt32(value.TimeSinceInstantDealOffered, AutoPackType.Packed);
			writer.WriteBoolean(value.HasBeenRecommended);
			writer.WriteString(value.DataType);
			writer.WriteInt32(value.DataVersion, AutoPackType.Packed);
			writer.WriteString(value.GameVersion);
		}

		// Token: 0x06005736 RID: 22326 RVA: 0x0016EB08 File Offset: 0x0016CD08
		public static void Generated(this Writer writer, string[] value)
		{
			writer.WriteArray<string>(value);
		}

		// Token: 0x06005737 RID: 22327 RVA: 0x0016EB24 File Offset: 0x0016CD24
		public static void Generated(this Writer writer, float[] value)
		{
			writer.WriteArray<float>(value);
		}

		// Token: 0x06005738 RID: 22328 RVA: 0x0016EB40 File Offset: 0x0016CD40
		public static void Generated(this Writer writer, EDrugType value)
		{
			writer.WriteInt32((int)value, AutoPackType.Packed);
		}

		// Token: 0x06005739 RID: 22329 RVA: 0x0016EB60 File Offset: 0x0016CD60
		public static void Generated(this Writer writer, GameData value)
		{
			if (value == null)
			{
				writer.WriteBoolean(true);
				return;
			}
			writer.WriteBoolean(false);
			writer.WriteString(value.OrganisationName);
			writer.WriteInt32(value.Seed, AutoPackType.Packed);
			writer.Write___ScheduleOne.DevUtilities.GameSettingsFishNet.Serializing.Generated(value.Settings);
			writer.WriteString(value.DataType);
			writer.WriteInt32(value.DataVersion, AutoPackType.Packed);
			writer.WriteString(value.GameVersion);
		}

		// Token: 0x0600573A RID: 22330 RVA: 0x0016EC0C File Offset: 0x0016CE0C
		public static void Generated(this Writer writer, GameSettings value)
		{
			if (value == null)
			{
				writer.WriteBoolean(true);
				return;
			}
			writer.WriteBoolean(false);
			writer.WriteBoolean(value.ConsoleEnabled);
		}

		// Token: 0x0600573B RID: 22331 RVA: 0x0016EC54 File Offset: 0x0016CE54
		public static void Generated(this Writer writer, DeliveryInstance value)
		{
			if (value == null)
			{
				writer.WriteBoolean(true);
				return;
			}
			writer.WriteBoolean(false);
			writer.WriteString(value.DeliveryID);
			writer.WriteString(value.StoreName);
			writer.WriteString(value.DestinationCode);
			writer.WriteInt32(value.LoadingDockIndex, AutoPackType.Packed);
			writer.Write___ScheduleOne.DevUtilities.StringIntPair[]FishNet.Serializing.Generated(value.Items);
			writer.Write___ScheduleOne.Delivery.EDeliveryStatusFishNet.Serializing.Generated(value.Status);
			writer.WriteInt32(value.TimeUntilArrival, AutoPackType.Packed);
		}

		// Token: 0x0600573C RID: 22332 RVA: 0x0016ED10 File Offset: 0x0016CF10
		public static void Generated(this Writer writer, EDeliveryStatus value)
		{
			writer.WriteInt32((int)value, AutoPackType.Packed);
		}

		// Token: 0x0600573D RID: 22333 RVA: 0x0016ED30 File Offset: 0x0016CF30
		public static void Generated(this Writer writer, ExplosionData value)
		{
			writer.WriteSingle(value.DamageRadius, AutoPackType.Unpacked);
			writer.WriteSingle(value.MaxDamage, AutoPackType.Unpacked);
			writer.WriteSingle(value.PushForceRadius, AutoPackType.Unpacked);
			writer.WriteSingle(value.MaxPushForce, AutoPackType.Unpacked);
		}

		// Token: 0x0600573E RID: 22334 RVA: 0x0016ED9C File Offset: 0x0016CF9C
		public static void Generated(this Writer writer, PlayingCard.ECardSuit value)
		{
			writer.WriteInt32((int)value, AutoPackType.Packed);
		}

		// Token: 0x0600573F RID: 22335 RVA: 0x0016EDBC File Offset: 0x0016CFBC
		public static void Generated(this Writer writer, PlayingCard.ECardValue value)
		{
			writer.WriteInt32((int)value, AutoPackType.Packed);
		}

		// Token: 0x06005740 RID: 22336 RVA: 0x0016EDDC File Offset: 0x0016CFDC
		public static void Generated(this Writer writer, NetworkObject[] value)
		{
			writer.WriteArray<NetworkObject>(value);
		}

		// Token: 0x06005741 RID: 22337 RVA: 0x0016EDF8 File Offset: 0x0016CFF8
		public static void Generated(this Writer writer, RTBGameController.EStage value)
		{
			writer.WriteInt32((int)value, AutoPackType.Packed);
		}

		// Token: 0x06005742 RID: 22338 RVA: 0x0016EE18 File Offset: 0x0016D018
		public static void Generated(this Writer writer, SlotMachine.ESymbol value)
		{
			writer.WriteInt32((int)value, AutoPackType.Packed);
		}

		// Token: 0x06005743 RID: 22339 RVA: 0x0016EE38 File Offset: 0x0016D038
		public static void Generated(this Writer writer, SlotMachine.ESymbol[] value)
		{
			writer.WriteArray<SlotMachine.ESymbol>(value);
		}

		// Token: 0x06005744 RID: 22340 RVA: 0x0016EE54 File Offset: 0x0016D054
		public static void Generated(this Writer writer, EDoorSide value)
		{
			writer.WriteInt32((int)value, AutoPackType.Packed);
		}

		// Token: 0x06005745 RID: 22341 RVA: 0x0016EE74 File Offset: 0x0016D074
		public static void Generated(this Writer writer, EVehicleColor value)
		{
			writer.WriteInt32((int)value, AutoPackType.Packed);
		}

		// Token: 0x06005746 RID: 22342 RVA: 0x0016EE94 File Offset: 0x0016D094
		public static void Generated(this Writer writer, ParkData value)
		{
			if (value == null)
			{
				writer.WriteBoolean(true);
				return;
			}
			writer.WriteBoolean(false);
			writer.WriteGuidAllocated(value.lotGUID);
			writer.WriteInt32(value.spotIndex, AutoPackType.Packed);
			writer.Write___ScheduleOne.Vehicles.EParkingAlignmentFishNet.Serializing.Generated(value.alignment);
		}

		// Token: 0x06005747 RID: 22343 RVA: 0x0016EF04 File Offset: 0x0016D104
		public static void Generated(this Writer writer, EParkingAlignment value)
		{
			writer.WriteInt32((int)value, AutoPackType.Packed);
		}

		// Token: 0x06005748 RID: 22344 RVA: 0x0016EF24 File Offset: 0x0016D124
		public static void Generated(this Writer writer, TrashContentData value)
		{
			if (value == null)
			{
				writer.WriteBoolean(true);
				return;
			}
			writer.WriteBoolean(false);
			writer.Write___System.String[]FishNet.Serializing.Generated(value.TrashIDs);
			writer.Write___System.Int32[]FishNet.Serializing.Generated(value.TrashQuantities);
		}

		// Token: 0x06005749 RID: 22345 RVA: 0x0016EF7C File Offset: 0x0016D17C
		public static void Generated(this Writer writer, int[] value)
		{
			writer.WriteArray<int>(value);
		}

		// Token: 0x0600574A RID: 22346 RVA: 0x0016EF98 File Offset: 0x0016D198
		public static void Generated(this Writer writer, Coordinate value)
		{
			if (value == null)
			{
				writer.WriteBoolean(true);
				return;
			}
			writer.WriteBoolean(false);
			writer.WriteInt32(value.x, AutoPackType.Packed);
			writer.WriteInt32(value.y, AutoPackType.Packed);
		}

		// Token: 0x0600574B RID: 22347 RVA: 0x0016EFFC File Offset: 0x0016D1FC
		public static void Generated(this Writer writer, WeedAppearanceSettings value)
		{
			if (value == null)
			{
				writer.WriteBoolean(true);
				return;
			}
			writer.WriteBoolean(false);
			writer.WriteColor32(value.MainColor);
			writer.WriteColor32(value.SecondaryColor);
			writer.WriteColor32(value.LeafColor);
			writer.WriteColor32(value.StemColor);
		}

		// Token: 0x0600574C RID: 22348 RVA: 0x0016F078 File Offset: 0x0016D278
		public static void Generated(this Writer writer, CocaineAppearanceSettings value)
		{
			if (value == null)
			{
				writer.WriteBoolean(true);
				return;
			}
			writer.WriteBoolean(false);
			writer.WriteColor32(value.MainColor);
			writer.WriteColor32(value.SecondaryColor);
		}

		// Token: 0x0600574D RID: 22349 RVA: 0x0016F0D0 File Offset: 0x0016D2D0
		public static void Generated(this Writer writer, MethAppearanceSettings value)
		{
			if (value == null)
			{
				writer.WriteBoolean(true);
				return;
			}
			writer.WriteBoolean(false);
			writer.WriteColor32(value.MainColor);
			writer.WriteColor32(value.SecondaryColor);
		}

		// Token: 0x0600574E RID: 22350 RVA: 0x0016F128 File Offset: 0x0016D328
		public static void Generated(this Writer writer, NewMixOperation value)
		{
			if (value == null)
			{
				writer.WriteBoolean(true);
				return;
			}
			writer.WriteBoolean(false);
			writer.WriteString(value.ProductID);
			writer.WriteString(value.IngredientID);
		}

		// Token: 0x0600574F RID: 22351 RVA: 0x0016F180 File Offset: 0x0016D380
		public static void Generated(this Writer writer, Recycler.EState value)
		{
			writer.WriteInt32((int)value, AutoPackType.Packed);
		}

		// Token: 0x06005750 RID: 22352 RVA: 0x0016F1A0 File Offset: 0x0016D3A0
		public static void Generated(this Writer writer, CoordinateProceduralTilePair value)
		{
			writer.Write___ScheduleOne.Tiles.CoordinateFishNet.Serializing.Generated(value.coord);
			writer.WriteNetworkObject(value.tileParent);
			writer.WriteInt32(value.tileIndex, AutoPackType.Packed);
		}

		// Token: 0x06005751 RID: 22353 RVA: 0x0016F1E8 File Offset: 0x0016D3E8
		public static void List(this Writer writer, List<CoordinateProceduralTilePair> value)
		{
			writer.WriteList<CoordinateProceduralTilePair>(value);
		}

		// Token: 0x06005752 RID: 22354 RVA: 0x0016F204 File Offset: 0x0016D404
		public static void Generated(this Writer writer, ChemistryCookOperation value)
		{
			if (value == null)
			{
				writer.WriteBoolean(true);
				return;
			}
			writer.WriteBoolean(false);
			writer.WriteString(value.RecipeID);
			writer.Write___ScheduleOne.ItemFramework.EQualityFishNet.Serializing.Generated(value.ProductQuality);
			writer.WriteColor(value.StartLiquidColor, AutoPackType.Packed);
			writer.WriteSingle(value.LiquidLevel, AutoPackType.Unpacked);
			writer.WriteInt32(value.CurrentTime, AutoPackType.Packed);
		}

		// Token: 0x06005753 RID: 22355 RVA: 0x0016F2A4 File Offset: 0x0016D4A4
		public static void Generated(this Writer writer, DryingOperation value)
		{
			if (value == null)
			{
				writer.WriteBoolean(true);
				return;
			}
			writer.WriteBoolean(false);
			writer.WriteString(value.ItemID);
			writer.WriteInt32(value.Quantity, AutoPackType.Packed);
			writer.Write___ScheduleOne.ItemFramework.EQualityFishNet.Serializing.Generated(value.StartQuality);
			writer.WriteInt32(value.Time, AutoPackType.Packed);
		}

		// Token: 0x06005754 RID: 22356 RVA: 0x0016F32C File Offset: 0x0016D52C
		public static void Generated(this Writer writer, OvenCookOperation value)
		{
			if (value == null)
			{
				writer.WriteBoolean(true);
				return;
			}
			writer.WriteBoolean(false);
			writer.WriteString(value.IngredientID);
			writer.Write___ScheduleOne.ItemFramework.EQualityFishNet.Serializing.Generated(value.IngredientQuality);
			writer.WriteInt32(value.IngredientQuantity, AutoPackType.Packed);
			writer.WriteString(value.ProductID);
			writer.WriteInt32(value.CookProgress, AutoPackType.Packed);
		}

		// Token: 0x06005755 RID: 22357 RVA: 0x0016F3C4 File Offset: 0x0016D5C4
		public static void Generated(this Writer writer, MixOperation value)
		{
			if (value == null)
			{
				writer.WriteBoolean(true);
				return;
			}
			writer.WriteBoolean(false);
			writer.WriteString(value.ProductID);
			writer.Write___ScheduleOne.ItemFramework.EQualityFishNet.Serializing.Generated(value.ProductQuality);
			writer.WriteString(value.IngredientID);
			writer.WriteInt32(value.Quantity, AutoPackType.Packed);
		}
	}
}
