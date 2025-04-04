using System;
using System.Collections.Generic;
using System.Linq;
using ScheduleOne.DevUtilities;
using ScheduleOne.PlayerScripts;
using UnityEngine;
using UnityEngine.UI;

namespace ScheduleOne.UI
{
	// Token: 0x020009AD RID: 2477
	public class CharacterInterface : MonoBehaviour
	{
		// Token: 0x17000971 RID: 2417
		// (get) Token: 0x060042E1 RID: 17121 RVA: 0x00118403 File Offset: 0x00116603
		// (set) Token: 0x060042E2 RID: 17122 RVA: 0x0011840B File Offset: 0x0011660B
		public bool IsOpen { get; private set; }

		// Token: 0x060042E3 RID: 17123 RVA: 0x00118414 File Offset: 0x00116614
		private void Awake()
		{
			this.Close();
		}

		// Token: 0x060042E4 RID: 17124 RVA: 0x0011841C File Offset: 0x0011661C
		private void LateUpdate()
		{
			if (this.IsOpen)
			{
				foreach (ClothingSlotUI clothingSlotUI in this.ClothingSlots)
				{
					Transform component = clothingSlotUI.GetComponent<RectTransform>();
					Transform transform = this.SlotAlignmentPoints[clothingSlotUI];
					component.position = RectTransformUtility.WorldToScreenPoint(Singleton<GameplayMenu>.Instance.OverlayCamera, transform.position);
				}
			}
		}

		// Token: 0x060042E5 RID: 17125 RVA: 0x0011847C File Offset: 0x0011667C
		public void Open()
		{
			if (this.SlotAlignmentPoints.Count == 0)
			{
				ClothingSlotUI[] clothingSlots = this.ClothingSlots;
				for (int i = 0; i < clothingSlots.Length; i++)
				{
					ClothingSlotUI slotUI = clothingSlots[i];
					slotUI.AssignSlot(Player.Local.Clothing.ClothingSlots[slotUI.SlotType]);
					CharacterDisplay.SlotAlignmentPoint slotAlignmentPoint = Singleton<CharacterDisplay>.Instance.AlignmentPoints.FirstOrDefault((CharacterDisplay.SlotAlignmentPoint x) => x.SlotType == slotUI.SlotType);
					if (slotAlignmentPoint != null)
					{
						this.SlotAlignmentPoints.Add(slotUI, slotAlignmentPoint.Point);
					}
					else
					{
						Console.LogError(string.Format("No alignment point found for slot type {0}", slotUI.SlotType), null);
					}
				}
			}
			this.IsOpen = true;
			this.Container.gameObject.SetActive(true);
			this.LateUpdate();
		}

		// Token: 0x060042E6 RID: 17126 RVA: 0x00118563 File Offset: 0x00116763
		public void Close()
		{
			this.IsOpen = false;
			this.Container.gameObject.SetActive(false);
		}

		// Token: 0x040030D6 RID: 12502
		public ClothingSlotUI[] ClothingSlots;

		// Token: 0x040030D7 RID: 12503
		public RectTransform Container;

		// Token: 0x040030D8 RID: 12504
		public Slider RotationSlider;

		// Token: 0x040030D9 RID: 12505
		private Dictionary<ClothingSlotUI, Transform> SlotAlignmentPoints = new Dictionary<ClothingSlotUI, Transform>();
	}
}
