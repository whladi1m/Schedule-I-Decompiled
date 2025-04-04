using System;
using System.Collections.Generic;
using UnityEngine;

namespace ScheduleOne.Property.Utilities.Power
{
	// Token: 0x0200080E RID: 2062
	public class PowerNode : MonoBehaviour
	{
		// Token: 0x17000806 RID: 2054
		// (get) Token: 0x0600387B RID: 14459 RVA: 0x000EEE38 File Offset: 0x000ED038
		public Transform pConnectionPoint
		{
			get
			{
				return this.connectionPoint;
			}
		}

		// Token: 0x0600387C RID: 14460 RVA: 0x000EEE40 File Offset: 0x000ED040
		public bool IsConnectedTo(PowerNode node)
		{
			for (int i = 0; i < this.connections.Count; i++)
			{
				if (this.connections[i].nodeA == this)
				{
					if (this.connections[i].nodeB == node)
					{
						return true;
					}
				}
				else if (this.connections[i].nodeA == node)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600387D RID: 14461 RVA: 0x000EEEB4 File Offset: 0x000ED0B4
		public void RecalculatePowerNetwork()
		{
			List<PowerNode> connectedNodes = this.GetConnectedNodes(new List<PowerNode>());
			bool flag = false;
			using (List<PowerNode>.Enumerator enumerator = connectedNodes.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.poweredNode)
					{
						flag = true;
					}
				}
			}
			foreach (PowerNode powerNode in connectedNodes)
			{
				if (flag)
				{
					powerNode.isConnectedToPower = true;
				}
				else
				{
					powerNode.isConnectedToPower = false;
				}
			}
		}

		// Token: 0x0600387E RID: 14462 RVA: 0x000EEF5C File Offset: 0x000ED15C
		public List<PowerNode> GetConnectedNodes(List<PowerNode> exclusions)
		{
			List<PowerNode> list = new List<PowerNode>();
			list.Add(this);
			exclusions.Add(this);
			for (int i = 0; i < this.connections.Count; i++)
			{
				if (!exclusions.Contains(this.connections[i].GetOtherNode(this)))
				{
					List<PowerNode> connectedNodes = this.connections[i].GetOtherNode(this).GetConnectedNodes(exclusions);
					exclusions.AddRange(connectedNodes);
					list.AddRange(connectedNodes);
				}
			}
			return list;
		}

		// Token: 0x04002909 RID: 10505
		public bool poweredNode;

		// Token: 0x0400290A RID: 10506
		public bool consumptionNode;

		// Token: 0x0400290B RID: 10507
		public bool isConnectedToPower;

		// Token: 0x0400290C RID: 10508
		[Header("References")]
		[SerializeField]
		protected Transform connectionPoint;

		// Token: 0x0400290D RID: 10509
		public List<PowerLine> connections = new List<PowerLine>();
	}
}
