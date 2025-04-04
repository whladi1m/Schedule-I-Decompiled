using System;
using System.Collections.Generic;
using System.Linq;
using ScheduleOne.ItemFramework;
using ScheduleOne.Persistence.Datas;
using UnityEngine.Events;

namespace ScheduleOne.Management
{
	// Token: 0x0200056D RID: 1389
	public class RouteListField : ConfigField
	{
		// Token: 0x060022B7 RID: 8887 RVA: 0x0008EB96 File Offset: 0x0008CD96
		public RouteListField(EntityConfiguration parentConfig) : base(parentConfig)
		{
		}

		// Token: 0x060022B8 RID: 8888 RVA: 0x0008EBBC File Offset: 0x0008CDBC
		public void SetList(List<AdvancedTransitRoute> list, bool network, bool bypassSequenceCheck = false)
		{
			if (this.Routes.SequenceEqual(list) && !bypassSequenceCheck)
			{
				return;
			}
			this.Routes = new List<AdvancedTransitRoute>();
			this.Routes.AddRange(list);
			if (network)
			{
				base.ParentConfig.ReplicateField(this, null);
			}
			if (this.onListChanged != null)
			{
				this.onListChanged.Invoke(list);
			}
		}

		// Token: 0x060022B9 RID: 8889 RVA: 0x0008EC16 File Offset: 0x0008CE16
		public void Replicate()
		{
			Console.Log("Replicating route list field", null);
			this.SetList(this.Routes, true, true);
		}

		// Token: 0x060022BA RID: 8890 RVA: 0x0008EC34 File Offset: 0x0008CE34
		public void AddItem(AdvancedTransitRoute item)
		{
			if (this.Routes.Contains(item))
			{
				return;
			}
			if (this.Routes.Count >= this.MaxRoutes)
			{
				Console.LogWarning("Route cannot be added to " + base.ParentConfig.GetType().Name + " because the maximum number of routes has been reached", null);
				return;
			}
			this.SetList(new List<AdvancedTransitRoute>(this.Routes)
			{
				item
			}, true, false);
		}

		// Token: 0x060022BB RID: 8891 RVA: 0x0008ECA8 File Offset: 0x0008CEA8
		public void RemoveItem(AdvancedTransitRoute item)
		{
			if (!this.Routes.Contains(item))
			{
				return;
			}
			List<AdvancedTransitRoute> list = new List<AdvancedTransitRoute>(this.Routes);
			list.Remove(item);
			this.SetList(list, true, false);
		}

		// Token: 0x060022BC RID: 8892 RVA: 0x0008ECE1 File Offset: 0x0008CEE1
		public override bool IsValueDefault()
		{
			return this.Routes.Count == 0;
		}

		// Token: 0x060022BD RID: 8893 RVA: 0x0008ECF4 File Offset: 0x0008CEF4
		public RouteListData GetData()
		{
			List<AdvancedTransitRouteData> list = new List<AdvancedTransitRouteData>();
			for (int i = 0; i < this.Routes.Count; i++)
			{
				list.Add(this.Routes[i].GetData());
			}
			return new RouteListData(list);
		}

		// Token: 0x060022BE RID: 8894 RVA: 0x0008ED3C File Offset: 0x0008CF3C
		public void Load(RouteListData data)
		{
			if (data != null)
			{
				List<AdvancedTransitRoute> list = new List<AdvancedTransitRoute>();
				for (int i = 0; i < data.Routes.Count; i++)
				{
					if (string.IsNullOrEmpty(data.Routes[i].SourceGUID) || string.IsNullOrEmpty(data.Routes[i].DestinationGUID))
					{
						Console.LogWarning("Route data is missing source or destination GUID", null);
					}
					else
					{
						ITransitEntity source = null;
						ITransitEntity destination = null;
						try
						{
							source = GUIDManager.GetObject<ITransitEntity>(new Guid(data.Routes[i].SourceGUID));
							destination = GUIDManager.GetObject<ITransitEntity>(new Guid(data.Routes[i].DestinationGUID));
						}
						catch (Exception ex)
						{
							Console.LogError("Error loading route: " + ex.Message, null);
							goto IL_175;
						}
						AdvancedTransitRoute advancedTransitRoute = new AdvancedTransitRoute(source, destination);
						advancedTransitRoute.Filter.SetMode(data.Routes[i].FilterMode);
						for (int j = 0; j < data.Routes[i].FilterItemIDs.Count; j++)
						{
							ItemDefinition @object = GUIDManager.GetObject<ItemDefinition>(new Guid(data.Routes[i].FilterItemIDs[j]));
							if (@object == null)
							{
								Console.LogWarning("Could not find item definition with GUID " + data.Routes[i].FilterItemIDs[j], null);
							}
							else if (@object != null)
							{
								advancedTransitRoute.Filter.AddItem(@object);
							}
						}
						list.Add(advancedTransitRoute);
					}
					IL_175:;
				}
				this.SetList(list, true, false);
			}
		}

		// Token: 0x04001A2A RID: 6698
		public List<AdvancedTransitRoute> Routes = new List<AdvancedTransitRoute>();

		// Token: 0x04001A2B RID: 6699
		public int MaxRoutes = 1;

		// Token: 0x04001A2C RID: 6700
		public UnityEvent<List<AdvancedTransitRoute>> onListChanged = new UnityEvent<List<AdvancedTransitRoute>>();
	}
}
