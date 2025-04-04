using System;
using System.Collections;
using System.Collections.Generic;

namespace FishySteamworks
{
	// Token: 0x02000C0F RID: 3087
	public class BidirectionalDictionary<T1, T2> : IEnumerable
	{
		// Token: 0x17000C1A RID: 3098
		// (get) Token: 0x06005638 RID: 22072 RVA: 0x00169FD7 File Offset: 0x001681D7
		public IEnumerable<T1> FirstTypes
		{
			get
			{
				return this.t1ToT2Dict.Keys;
			}
		}

		// Token: 0x17000C1B RID: 3099
		// (get) Token: 0x06005639 RID: 22073 RVA: 0x00169FE4 File Offset: 0x001681E4
		public IEnumerable<T2> SecondTypes
		{
			get
			{
				return this.t2ToT1Dict.Keys;
			}
		}

		// Token: 0x0600563A RID: 22074 RVA: 0x00169FF1 File Offset: 0x001681F1
		public IEnumerator GetEnumerator()
		{
			return this.t1ToT2Dict.GetEnumerator();
		}

		// Token: 0x17000C1C RID: 3100
		// (get) Token: 0x0600563B RID: 22075 RVA: 0x0016A003 File Offset: 0x00168203
		public int Count
		{
			get
			{
				return this.t1ToT2Dict.Count;
			}
		}

		// Token: 0x17000C1D RID: 3101
		// (get) Token: 0x0600563C RID: 22076 RVA: 0x0016A010 File Offset: 0x00168210
		public Dictionary<T1, T2> First
		{
			get
			{
				return this.t1ToT2Dict;
			}
		}

		// Token: 0x17000C1E RID: 3102
		// (get) Token: 0x0600563D RID: 22077 RVA: 0x0016A018 File Offset: 0x00168218
		public Dictionary<T2, T1> Second
		{
			get
			{
				return this.t2ToT1Dict;
			}
		}

		// Token: 0x0600563E RID: 22078 RVA: 0x0016A020 File Offset: 0x00168220
		public void Add(T1 key, T2 value)
		{
			if (this.t1ToT2Dict.ContainsKey(key))
			{
				this.Remove(key);
			}
			this.t1ToT2Dict[key] = value;
			this.t2ToT1Dict[value] = key;
		}

		// Token: 0x0600563F RID: 22079 RVA: 0x0016A051 File Offset: 0x00168251
		public void Add(T2 key, T1 value)
		{
			if (this.t2ToT1Dict.ContainsKey(key))
			{
				this.Remove(key);
			}
			this.t2ToT1Dict[key] = value;
			this.t1ToT2Dict[value] = key;
		}

		// Token: 0x06005640 RID: 22080 RVA: 0x0016A082 File Offset: 0x00168282
		public T2 Get(T1 key)
		{
			return this.t1ToT2Dict[key];
		}

		// Token: 0x06005641 RID: 22081 RVA: 0x0016A090 File Offset: 0x00168290
		public T1 Get(T2 key)
		{
			return this.t2ToT1Dict[key];
		}

		// Token: 0x06005642 RID: 22082 RVA: 0x0016A09E File Offset: 0x0016829E
		public bool TryGetValue(T1 key, out T2 value)
		{
			return this.t1ToT2Dict.TryGetValue(key, out value);
		}

		// Token: 0x06005643 RID: 22083 RVA: 0x0016A0AD File Offset: 0x001682AD
		public bool TryGetValue(T2 key, out T1 value)
		{
			return this.t2ToT1Dict.TryGetValue(key, out value);
		}

		// Token: 0x06005644 RID: 22084 RVA: 0x0016A0BC File Offset: 0x001682BC
		public bool Contains(T1 key)
		{
			return this.t1ToT2Dict.ContainsKey(key);
		}

		// Token: 0x06005645 RID: 22085 RVA: 0x0016A0CA File Offset: 0x001682CA
		public bool Contains(T2 key)
		{
			return this.t2ToT1Dict.ContainsKey(key);
		}

		// Token: 0x06005646 RID: 22086 RVA: 0x0016A0D8 File Offset: 0x001682D8
		public void Remove(T1 key)
		{
			if (this.Contains(key))
			{
				T2 key2 = this.t1ToT2Dict[key];
				this.t1ToT2Dict.Remove(key);
				this.t2ToT1Dict.Remove(key2);
			}
		}

		// Token: 0x06005647 RID: 22087 RVA: 0x0016A118 File Offset: 0x00168318
		public void Remove(T2 key)
		{
			if (this.Contains(key))
			{
				T1 key2 = this.t2ToT1Dict[key];
				this.t1ToT2Dict.Remove(key2);
				this.t2ToT1Dict.Remove(key);
			}
		}

		// Token: 0x17000C1F RID: 3103
		public T1 this[T2 key]
		{
			get
			{
				return this.t2ToT1Dict[key];
			}
			set
			{
				this.Add(key, value);
			}
		}

		// Token: 0x17000C20 RID: 3104
		public T2 this[T1 key]
		{
			get
			{
				return this.t1ToT2Dict[key];
			}
			set
			{
				this.Add(key, value);
			}
		}

		// Token: 0x0400402F RID: 16431
		private Dictionary<T1, T2> t1ToT2Dict = new Dictionary<T1, T2>();

		// Token: 0x04004030 RID: 16432
		private Dictionary<T2, T1> t2ToT1Dict = new Dictionary<T2, T1>();
	}
}
