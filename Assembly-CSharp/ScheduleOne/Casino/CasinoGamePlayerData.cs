using System;
using System.Collections.Generic;
using ScheduleOne.PlayerScripts;

namespace ScheduleOne.Casino
{
	// Token: 0x02000749 RID: 1865
	public class CasinoGamePlayerData
	{
		// Token: 0x1700074A RID: 1866
		// (get) Token: 0x060032A1 RID: 12961 RVA: 0x000D3243 File Offset: 0x000D1443
		// (set) Token: 0x060032A2 RID: 12962 RVA: 0x000D324B File Offset: 0x000D144B
		public CasinoGamePlayers Parent { get; private set; }

		// Token: 0x1700074B RID: 1867
		// (get) Token: 0x060032A3 RID: 12963 RVA: 0x000D3254 File Offset: 0x000D1454
		// (set) Token: 0x060032A4 RID: 12964 RVA: 0x000D325C File Offset: 0x000D145C
		public Player Player { get; private set; }

		// Token: 0x060032A5 RID: 12965 RVA: 0x000D3268 File Offset: 0x000D1468
		public CasinoGamePlayerData(CasinoGamePlayers parent, Player player)
		{
			this.Parent = parent;
			this.Player = player;
			this.bools = new Dictionary<string, bool>();
			this.floats = new Dictionary<string, float>();
		}

		// Token: 0x060032A6 RID: 12966 RVA: 0x000D32B8 File Offset: 0x000D14B8
		public T GetData<T>(string key)
		{
			if (typeof(T) == typeof(bool))
			{
				if (this.bools.ContainsKey(key))
				{
					return (T)((object)this.bools[key]);
				}
			}
			else if (typeof(T) == typeof(float) && this.floats.ContainsKey(key))
			{
				return (T)((object)this.floats[key]);
			}
			return default(T);
		}

		// Token: 0x060032A7 RID: 12967 RVA: 0x000D3350 File Offset: 0x000D1550
		public void SetData<T>(string key, T value, bool network = true)
		{
			if (network)
			{
				if (typeof(T) == typeof(bool))
				{
					this.Parent.SendPlayerBool(this.Player.NetworkObject, key, (bool)((object)value));
				}
				else if (typeof(T) == typeof(float))
				{
					this.Parent.SendPlayerFloat(this.Player.NetworkObject, key, (float)((object)value));
				}
			}
			if (!(typeof(T) == typeof(bool)))
			{
				if (typeof(T) == typeof(float))
				{
					if (this.floats.ContainsKey(key))
					{
						this.floats[key] = (float)((object)value);
						return;
					}
					this.floats.Add(key, (float)((object)value));
				}
				return;
			}
			if (this.bools.ContainsKey(key))
			{
				this.bools[key] = (bool)((object)value);
				return;
			}
			this.bools.Add(key, (bool)((object)value));
		}

		// Token: 0x04002450 RID: 9296
		protected Dictionary<string, bool> bools = new Dictionary<string, bool>();

		// Token: 0x04002451 RID: 9297
		protected Dictionary<string, float> floats = new Dictionary<string, float>();
	}
}
