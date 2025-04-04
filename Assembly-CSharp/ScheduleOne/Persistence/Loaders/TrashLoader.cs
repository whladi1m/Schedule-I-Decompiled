using System;
using System.IO;
using ScheduleOne.DevUtilities;
using ScheduleOne.Persistence.Datas;
using ScheduleOne.Trash;
using UnityEngine;

namespace ScheduleOne.Persistence.Loaders
{
	// Token: 0x02000399 RID: 921
	public class TrashLoader : Loader
	{
		// Token: 0x0600149E RID: 5278 RVA: 0x0005C2BC File Offset: 0x0005A4BC
		public override void Load(string mainPath)
		{
			if (!Directory.Exists(mainPath))
			{
				return;
			}
			string json;
			if (base.TryLoadFile(Path.Combine(mainPath, "Trash"), out json, true))
			{
				TrashData trashData = null;
				try
				{
					trashData = JsonUtility.FromJson<TrashData>(json);
				}
				catch (Exception ex)
				{
					Debug.LogError("Error loading data: " + ex.Message);
				}
				if (trashData != null && trashData.Items != null)
				{
					Console.Log("Loading trash items: " + trashData.Items.Length.ToString(), null);
					foreach (TrashItemData trashItemData in trashData.Items)
					{
						TrashItem trashItem;
						if (trashItemData.DataType == "TrashBagData")
						{
							trashItem = NetworkSingleton<TrashManager>.Instance.CreateTrashBag(trashItemData.TrashID, trashItemData.Position, trashItemData.Rotation, trashItemData.Contents, Vector3.zero, trashItemData.GUID, true);
						}
						else
						{
							trashItem = NetworkSingleton<TrashManager>.Instance.CreateTrashItem(trashItemData.TrashID, trashItemData.Position, trashItemData.Rotation, Vector3.zero, trashItemData.GUID, true);
						}
						if (trashItem != null)
						{
							trashItem.HasChanged = false;
						}
					}
				}
			}
			else
			{
				string path = Path.Combine(mainPath, "Items");
				if (Directory.Exists(path))
				{
					string[] files = Directory.GetFiles(path);
					for (int j = 0; j < files.Length; j++)
					{
						string json2;
						if (base.TryLoadFile(files[j], out json2, false))
						{
							TrashItemData trashItemData2 = null;
							try
							{
								trashItemData2 = JsonUtility.FromJson<TrashItemData>(json2);
							}
							catch (Exception ex2)
							{
								Debug.LogError("Error loading data: " + ex2.Message);
							}
							if (trashItemData2 != null)
							{
								TrashItem trashItem2 = null;
								if (trashItemData2.DataType == "TrashBagData")
								{
									TrashBagData trashBagData = null;
									try
									{
										trashBagData = JsonUtility.FromJson<TrashBagData>(json2);
									}
									catch (Exception ex3)
									{
										Debug.LogError("Error loading data: " + ex3.Message);
									}
									if (trashBagData != null)
									{
										trashItem2 = NetworkSingleton<TrashManager>.Instance.CreateTrashBag(trashBagData.TrashID, trashBagData.Position, trashBagData.Rotation, trashBagData.Contents, Vector3.zero, trashBagData.GUID, true);
									}
								}
								else
								{
									trashItem2 = NetworkSingleton<TrashManager>.Instance.CreateTrashItem(trashItemData2.TrashID, trashItemData2.Position, trashItemData2.Rotation, Vector3.zero, trashItemData2.GUID, true);
								}
								if (trashItem2 != null)
								{
									trashItem2.HasChanged = false;
								}
							}
						}
					}
				}
			}
			string path2 = Path.Combine(mainPath, "Generators");
			if (Directory.Exists(path2))
			{
				string[] files2 = Directory.GetFiles(path2);
				for (int k = 0; k < files2.Length; k++)
				{
					string json3;
					if (base.TryLoadFile(files2[k], out json3, false))
					{
						TrashGeneratorData trashGeneratorData = null;
						try
						{
							trashGeneratorData = JsonUtility.FromJson<TrashGeneratorData>(json3);
						}
						catch (Exception ex4)
						{
							Debug.LogError("Error loading data: " + ex4.Message);
						}
						if (trashGeneratorData != null)
						{
							TrashGenerator @object = GUIDManager.GetObject<TrashGenerator>(new Guid(trashGeneratorData.GUID));
							if (@object != null)
							{
								for (int l = 0; l < trashGeneratorData.GeneratedItems.Length; l++)
								{
									TrashItem object2 = GUIDManager.GetObject<TrashItem>(new Guid(trashGeneratorData.GeneratedItems[l]));
									if (object2 != null)
									{
										@object.AddGeneratedTrash(object2);
									}
								}
								@object.HasChanged = false;
							}
						}
					}
				}
			}
		}
	}
}
