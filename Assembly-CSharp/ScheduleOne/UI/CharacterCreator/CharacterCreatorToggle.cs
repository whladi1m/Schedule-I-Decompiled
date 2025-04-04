using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace ScheduleOne.UI.CharacterCreator
{
	// Token: 0x02000B3D RID: 2877
	public class CharacterCreatorToggle : CharacterCreatorField<int>
	{
		// Token: 0x06004C7C RID: 19580 RVA: 0x00141F72 File Offset: 0x00140172
		protected override void Awake()
		{
			base.Awake();
			this.Button1.onClick.AddListener(new UnityAction(this.OnButton1));
			this.Button2.onClick.AddListener(new UnityAction(this.OnButton2));
		}

		// Token: 0x06004C7D RID: 19581 RVA: 0x00141FB2 File Offset: 0x001401B2
		public override void ApplyValue()
		{
			base.ApplyValue();
			this.Button1.interactable = (base.value != 0);
			this.Button2.interactable = (base.value == 0);
		}

		// Token: 0x06004C7E RID: 19582 RVA: 0x00141FE2 File Offset: 0x001401E2
		public void OnButton1()
		{
			base.value = 0;
			this.WriteValue(true);
		}

		// Token: 0x06004C7F RID: 19583 RVA: 0x00141FF2 File Offset: 0x001401F2
		public void OnButton2()
		{
			base.value = 1;
			this.WriteValue(true);
		}

		// Token: 0x040039D2 RID: 14802
		[Header("References")]
		public Button Button1;

		// Token: 0x040039D3 RID: 14803
		public Button Button2;
	}
}
