using System;
using System.Collections.Generic;
using UnityEngine;

namespace Funly.SkyStudio
{
	// Token: 0x020001D2 RID: 466
	[Serializable]
	public class SerializableDictionary<K, V> : ISerializationCallbackReceiver
	{
		// Token: 0x06000A34 RID: 2612 RVA: 0x0002D9BD File Offset: 0x0002BBBD
		public void Clear()
		{
			this.dict.Clear();
		}

		// Token: 0x17000247 RID: 583
		public V this[K aKey]
		{
			get
			{
				return this.dict[aKey];
			}
			set
			{
				this.dict[aKey] = value;
			}
		}

		// Token: 0x06000A37 RID: 2615 RVA: 0x0002D9E8 File Offset: 0x0002BBE8
		public void OnBeforeSerialize()
		{
			this.m_Keys.Clear();
			this.m_Values.Clear();
			foreach (K k in this.dict.Keys)
			{
				this.m_Keys.Add(k);
				this.m_Values.Add(this.dict[k]);
			}
		}

		// Token: 0x06000A38 RID: 2616 RVA: 0x0002DA74 File Offset: 0x0002BC74
		public void OnAfterDeserialize()
		{
			if (this.m_Keys.Count != this.m_Values.Count)
			{
				Debug.LogError("Can't restore dictionry with unbalaned key/values");
				return;
			}
			this.dict.Clear();
			for (int i = 0; i < this.m_Keys.Count; i++)
			{
				this.dict[this.m_Keys[i]] = this.m_Values[i];
			}
		}

		// Token: 0x04000B41 RID: 2881
		[NonSerialized]
		public Dictionary<K, V> dict = new Dictionary<K, V>();

		// Token: 0x04000B42 RID: 2882
		[SerializeField]
		public List<K> m_Keys = new List<K>();

		// Token: 0x04000B43 RID: 2883
		[SerializeField]
		public List<V> m_Values = new List<V>();
	}
}
