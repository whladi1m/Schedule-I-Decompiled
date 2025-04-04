using System;
using System.Linq;
using ScheduleOne.DevUtilities;
using ScheduleOne.Economy;
using ScheduleOne.ItemFramework;
using ScheduleOne.NPCs;
using ScheduleOne.Persistence.Datas;
using UnityEngine;

namespace ScheduleOne.Persistence.Loaders
{
	// Token: 0x020003AB RID: 939
	public class NPCLoader : Loader
	{
		// Token: 0x170003D7 RID: 983
		// (get) Token: 0x060014C2 RID: 5314 RVA: 0x0005CFF8 File Offset: 0x0005B1F8
		public virtual string NPCType
		{
			get
			{
				return typeof(NPCData).Name;
			}
		}

		// Token: 0x060014C3 RID: 5315 RVA: 0x0005D009 File Offset: 0x0005B209
		public NPCLoader()
		{
			Singleton<LoadManager>.Instance.NPCLoaders.Add(this);
		}

		// Token: 0x060014C4 RID: 5316 RVA: 0x0005D024 File Offset: 0x0005B224
		public override void Load(string mainPath)
		{
			string json;
			if (base.TryLoadFile(mainPath, "NPC", out json))
			{
				NPCData data = null;
				try
				{
					data = JsonUtility.FromJson<NPCData>(json);
				}
				catch (Exception ex)
				{
					Type type = base.GetType();
					string str = (type != null) ? type.ToString() : null;
					string str2 = " error reading data: ";
					Exception ex2 = ex;
					Console.LogError(str + str2 + ((ex2 != null) ? ex2.ToString() : null), null);
				}
				NPC npc = NPCManager.NPCRegistry.FirstOrDefault((NPC x) => x.ID == data.ID);
				if (npc == null)
				{
					Console.LogWarning("Failed to find NPC with ID: " + data.ID, null);
					return;
				}
				npc.Load(data, mainPath);
				string json2;
				if (base.TryLoadFile(mainPath, "Relationship", out json2))
				{
					RelationshipData relationshipData = null;
					try
					{
						relationshipData = JsonUtility.FromJson<RelationshipData>(json2);
					}
					catch (Exception ex3)
					{
						Type type2 = base.GetType();
						string str3 = (type2 != null) ? type2.ToString() : null;
						string str4 = " error reading relationship data: ";
						Exception ex4 = ex3;
						Console.LogError(str3 + str4 + ((ex4 != null) ? ex4.ToString() : null), null);
					}
					if (relationshipData != null)
					{
						if (!float.IsNaN(relationshipData.RelationDelta) && !float.IsInfinity(relationshipData.RelationDelta))
						{
							npc.RelationData.SetRelationship(relationshipData.RelationDelta);
						}
						if (relationshipData.Unlocked)
						{
							npc.RelationData.Unlock(relationshipData.UnlockType, false);
						}
					}
				}
				this.TryLoadInventory(mainPath, npc);
				string json3;
				if (base.TryLoadFile(mainPath, "Health", out json3))
				{
					NPCHealthData npchealthData = null;
					try
					{
						npchealthData = JsonUtility.FromJson<NPCHealthData>(json3);
					}
					catch (Exception ex5)
					{
						Type type3 = base.GetType();
						string str5 = (type3 != null) ? type3.ToString() : null;
						string str6 = " error reading health data: ";
						Exception ex6 = ex5;
						Console.LogError(str5 + str6 + ((ex6 != null) ? ex6.ToString() : null), null);
					}
					if (npchealthData != null)
					{
						npc.Health.Load(npchealthData);
					}
				}
				string json4;
				if (base.TryLoadFile(mainPath, "MessageConversation", out json4))
				{
					MSGConversationData msgconversationData = null;
					try
					{
						msgconversationData = JsonUtility.FromJson<MSGConversationData>(json4);
					}
					catch (Exception ex7)
					{
						Type type4 = base.GetType();
						string str7 = (type4 != null) ? type4.ToString() : null;
						string str8 = " error reading message data: ";
						Exception ex8 = ex7;
						Console.LogError(str7 + str8 + ((ex8 != null) ? ex8.ToString() : null), null);
					}
					if (msgconversationData != null)
					{
						npc.MSGConversation.Load(msgconversationData);
					}
				}
				string json5;
				if (base.TryLoadFile(mainPath, "CustomerData", out json5))
				{
					ScheduleOne.Persistence.Datas.CustomerData customerData = null;
					try
					{
						customerData = JsonUtility.FromJson<ScheduleOne.Persistence.Datas.CustomerData>(json5);
					}
					catch (Exception ex9)
					{
						Type type5 = base.GetType();
						string str9 = (type5 != null) ? type5.ToString() : null;
						string str10 = " error reading customer data: ";
						Exception ex10 = ex9;
						Console.LogError(str9 + str10 + ((ex10 != null) ? ex10.ToString() : null), null);
					}
					if (customerData != null && npc.GetComponent<Customer>() != null)
					{
						npc.GetComponent<Customer>().Load(customerData);
					}
				}
			}
		}

		// Token: 0x060014C5 RID: 5317 RVA: 0x0005D300 File Offset: 0x0005B500
		protected void TryLoadInventory(string mainPath, NPC npc)
		{
			string json;
			if (base.TryLoadFile(mainPath, "Inventory", out json))
			{
				ItemInstance[] array = ItemSet.Deserialize(json);
				for (int i = 0; i < array.Length; i++)
				{
					if (array[i] != null)
					{
						npc.Inventory.ItemSlots[i].SetStoredItem(array[i], false);
					}
				}
			}
		}
	}
}
