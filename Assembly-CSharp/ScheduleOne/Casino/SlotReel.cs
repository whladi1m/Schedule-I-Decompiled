using System;
using ScheduleOne.Audio;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.Casino
{
	// Token: 0x0200075F RID: 1887
	public class SlotReel : MonoBehaviour
	{
		// Token: 0x17000762 RID: 1890
		// (get) Token: 0x06003391 RID: 13201 RVA: 0x000D7829 File Offset: 0x000D5A29
		// (set) Token: 0x06003392 RID: 13202 RVA: 0x000D7831 File Offset: 0x000D5A31
		public bool IsSpinning { get; private set; }

		// Token: 0x17000763 RID: 1891
		// (get) Token: 0x06003393 RID: 13203 RVA: 0x000D783A File Offset: 0x000D5A3A
		// (set) Token: 0x06003394 RID: 13204 RVA: 0x000D7842 File Offset: 0x000D5A42
		public SlotMachine.ESymbol CurrentSymbol { get; private set; } = SlotMachine.ESymbol.Seven;

		// Token: 0x17000764 RID: 1892
		// (get) Token: 0x06003395 RID: 13205 RVA: 0x000D784B File Offset: 0x000D5A4B
		// (set) Token: 0x06003396 RID: 13206 RVA: 0x000D7853 File Offset: 0x000D5A53
		public float CurrentRotation { get; private set; }

		// Token: 0x06003397 RID: 13207 RVA: 0x000D785C File Offset: 0x000D5A5C
		private void Awake()
		{
			this.SetSymbol(SlotMachine.GetRandomSymbol());
		}

		// Token: 0x06003398 RID: 13208 RVA: 0x000D7869 File Offset: 0x000D5A69
		private void Update()
		{
			if (this.IsSpinning)
			{
				this.SetReelRotation(this.CurrentRotation + this.SpinSpeed * Time.deltaTime);
				return;
			}
			this.SetReelRotation(this.GetSymbolRotation(this.CurrentSymbol));
		}

		// Token: 0x06003399 RID: 13209 RVA: 0x000D789F File Offset: 0x000D5A9F
		public void Spin()
		{
			this.IsSpinning = true;
			if (this.onStart != null)
			{
				this.onStart.Invoke();
			}
		}

		// Token: 0x0600339A RID: 13210 RVA: 0x000D78BB File Offset: 0x000D5ABB
		public void Stop(SlotMachine.ESymbol endSymbol)
		{
			this.CurrentSymbol = endSymbol;
			this.IsSpinning = false;
			this.StopSound.Play();
			if (this.onStop != null)
			{
				this.onStop.Invoke();
			}
		}

		// Token: 0x0600339B RID: 13211 RVA: 0x000D78E9 File Offset: 0x000D5AE9
		public void SetSymbol(SlotMachine.ESymbol symbol)
		{
			this.CurrentSymbol = symbol;
		}

		// Token: 0x0600339C RID: 13212 RVA: 0x000D78F2 File Offset: 0x000D5AF2
		private void SetReelRotation(float rotation)
		{
			base.transform.localRotation = Quaternion.Euler(rotation, 0f, 0f);
			this.CurrentRotation = rotation % 360f;
		}

		// Token: 0x0600339D RID: 13213 RVA: 0x000D791C File Offset: 0x000D5B1C
		private float GetSymbolRotation(SlotMachine.ESymbol symbol)
		{
			foreach (SlotReel.SymbolRotation symbolRotation in this.SymbolRotations)
			{
				if (symbolRotation.Symbol == symbol)
				{
					return symbolRotation.Rotation;
				}
			}
			Console.LogWarning("SlotReel.GetSymbolRotation: Symbol not found: " + symbol.ToString(), null);
			return 0f;
		}

		// Token: 0x040024ED RID: 9453
		[Header("Settings")]
		public SlotReel.SymbolRotation[] SymbolRotations;

		// Token: 0x040024EE RID: 9454
		public float SpinSpeed = 1000f;

		// Token: 0x040024EF RID: 9455
		[Header("References")]
		public AudioSourceController StopSound;

		// Token: 0x040024F0 RID: 9456
		public UnityEvent onStart;

		// Token: 0x040024F1 RID: 9457
		public UnityEvent onStop;

		// Token: 0x02000760 RID: 1888
		[Serializable]
		public class SymbolRotation
		{
			// Token: 0x040024F2 RID: 9458
			public SlotMachine.ESymbol Symbol;

			// Token: 0x040024F3 RID: 9459
			public float Rotation;
		}
	}
}
