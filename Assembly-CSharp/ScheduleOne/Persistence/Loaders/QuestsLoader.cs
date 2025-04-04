using System;
using System.IO;
using ScheduleOne.DevUtilities;
using ScheduleOne.Economy;
using ScheduleOne.GameTime;
using ScheduleOne.NPCs;
using ScheduleOne.Persistence.Datas;
using ScheduleOne.Quests;
using UnityEngine;

namespace ScheduleOne.Persistence.Loaders
{
	// Token: 0x02000395 RID: 917
	public class QuestsLoader : Loader
	{
		// Token: 0x06001496 RID: 5270 RVA: 0x0005BEB0 File Offset: 0x0005A0B0
		public override void Load(string mainPath)
		{
			if (!Directory.Exists(mainPath))
			{
				return;
			}
			string[] files = Directory.GetFiles(mainPath);
			for (int i = 0; i < files.Length; i++)
			{
				string json;
				if (base.TryLoadFile(files[i], out json, false))
				{
					QuestData questData = null;
					try
					{
						questData = JsonUtility.FromJson<QuestData>(json);
					}
					catch (Exception ex)
					{
						Debug.LogError("Error loading quest data: " + ex.Message);
					}
					if (questData != null)
					{
						Quest quest;
						if (questData.DataType == "DeaddropQuestData")
						{
							DeaddropQuestData deaddropQuestData = null;
							try
							{
								deaddropQuestData = JsonUtility.FromJson<DeaddropQuestData>(json);
							}
							catch (Exception ex2)
							{
								Debug.LogError("Error loading quest data: " + ex2.Message);
							}
							if (deaddropQuestData == null)
							{
								goto IL_13D;
							}
							DeadDrop @object = GUIDManager.GetObject<DeadDrop>(new Guid(deaddropQuestData.DeaddropGUID));
							if (@object == null)
							{
								Console.LogWarning("Failed to find deaddrop with GUID: " + deaddropQuestData.DeaddropGUID, null);
								goto IL_13D;
							}
							quest = NetworkSingleton<QuestManager>.Instance.CreateDeaddropCollectionQuest(@object.GUID.ToString(), questData.GUID);
						}
						else
						{
							quest = GUIDManager.GetObject<Quest>(new Guid(questData.GUID));
						}
						if (quest == null)
						{
							Console.LogWarning("Failed to find quest with GUID: " + questData.GUID, null);
						}
						else
						{
							quest.Load(questData);
						}
					}
				}
				IL_13D:;
			}
			string path = Path.Combine(mainPath, "Contracts");
			if (Directory.Exists(path))
			{
				string[] files2 = Directory.GetFiles(path);
				for (int j = 0; j < files2.Length; j++)
				{
					string json2;
					if (base.TryLoadFile(files2[j], out json2, false))
					{
						ContractData contractData = null;
						try
						{
							contractData = JsonUtility.FromJson<ContractData>(json2);
						}
						catch (Exception ex3)
						{
							Debug.LogError("Error loading contract data: " + ex3.Message);
						}
						if (contractData != null)
						{
							NPC object2 = GUIDManager.GetObject<NPC>(new Guid(contractData.CustomerGUID));
							if (object2 == null)
							{
								Console.LogWarning("Failed to find customer with GUID: " + contractData.CustomerGUID, null);
							}
							else
							{
								NetworkSingleton<QuestManager>.Instance.CreateContract_Local(contractData.Title, contractData.Description, contractData.Entries, contractData.GUID, contractData.IsTracked, object2.NetworkObject, contractData.Payment, contractData.ProductList, contractData.DeliveryLocationGUID, contractData.DeliveryWindow, contractData.Expires, new GameDateTime(contractData.ExpiryDate), contractData.PickupScheduleIndex, new GameDateTime(contractData.AcceptTime), null);
							}
						}
					}
				}
			}
		}
	}
}
