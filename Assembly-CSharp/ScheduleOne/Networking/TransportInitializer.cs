using System;
using FishNet.Managing.Transporting;
using FishNet.Transporting.Multipass;
using FishNet.Transporting.Yak;
using UnityEngine;

namespace ScheduleOne.Networking
{
	// Token: 0x02000534 RID: 1332
	public class TransportInitializer : MonoBehaviour
	{
		// Token: 0x060020A2 RID: 8354 RVA: 0x00086316 File Offset: 0x00084516
		public void Awake()
		{
			base.GetComponent<TransportManager>().GetTransport<Multipass>().SetClientTransport<Yak>();
		}
	}
}
