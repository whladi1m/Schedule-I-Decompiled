using System;
using System.Collections.Generic;
using EasyButtons;
using EPOOutline;
using FluffyUnderware.DevTools.Extensions;
using ScheduleOne.DevUtilities;
using ScheduleOne.ItemFramework;
using ScheduleOne.Management;
using ScheduleOne.Map;
using ScheduleOne.Property;
using ScheduleOne.Vehicles;
using UnityEngine;

namespace ScheduleOne.Delivery
{
	// Token: 0x02000709 RID: 1801
	public class LoadingDock : MonoBehaviour, IGUIDRegisterable, ITransitEntity
	{
		// Token: 0x1700071D RID: 1821
		// (get) Token: 0x060030BA RID: 12474 RVA: 0x000CAB97 File Offset: 0x000C8D97
		// (set) Token: 0x060030BB RID: 12475 RVA: 0x000CAB9F File Offset: 0x000C8D9F
		public LandVehicle DynamicOccupant { get; private set; }

		// Token: 0x1700071E RID: 1822
		// (get) Token: 0x060030BC RID: 12476 RVA: 0x000CABA8 File Offset: 0x000C8DA8
		// (set) Token: 0x060030BD RID: 12477 RVA: 0x000CABB0 File Offset: 0x000C8DB0
		public LandVehicle StaticOccupant { get; private set; }

		// Token: 0x1700071F RID: 1823
		// (get) Token: 0x060030BE RID: 12478 RVA: 0x000CABB9 File Offset: 0x000C8DB9
		public bool IsInUse
		{
			get
			{
				return this.DynamicOccupant != null || this.StaticOccupant != null;
			}
		}

		// Token: 0x17000720 RID: 1824
		// (get) Token: 0x060030BF RID: 12479 RVA: 0x000CABD7 File Offset: 0x000C8DD7
		// (set) Token: 0x060030C0 RID: 12480 RVA: 0x000CABDF File Offset: 0x000C8DDF
		public Guid GUID { get; protected set; }

		// Token: 0x060030C1 RID: 12481 RVA: 0x000CABE8 File Offset: 0x000C8DE8
		[Button]
		public void RegenerateGUID()
		{
			this.BakedGUID = Guid.NewGuid().ToString();
		}

		// Token: 0x17000721 RID: 1825
		// (get) Token: 0x060030C2 RID: 12482 RVA: 0x000CAC10 File Offset: 0x000C8E10
		public string Name
		{
			get
			{
				return "Loading Dock " + (this.ParentProperty.LoadingDocks.IndexOf(this) + 1).ToString();
			}
		}

		// Token: 0x17000722 RID: 1826
		// (get) Token: 0x060030C3 RID: 12483 RVA: 0x000CAC42 File Offset: 0x000C8E42
		// (set) Token: 0x060030C4 RID: 12484 RVA: 0x000CAC4A File Offset: 0x000C8E4A
		public List<ItemSlot> InputSlots { get; set; } = new List<ItemSlot>();

		// Token: 0x17000723 RID: 1827
		// (get) Token: 0x060030C5 RID: 12485 RVA: 0x000CAC53 File Offset: 0x000C8E53
		// (set) Token: 0x060030C6 RID: 12486 RVA: 0x000CAC5B File Offset: 0x000C8E5B
		public List<ItemSlot> OutputSlots { get; set; } = new List<ItemSlot>();

		// Token: 0x17000724 RID: 1828
		// (get) Token: 0x060030C7 RID: 12487 RVA: 0x000CAC64 File Offset: 0x000C8E64
		public Transform LinkOrigin
		{
			get
			{
				return this.uiPoint;
			}
		}

		// Token: 0x17000725 RID: 1829
		// (get) Token: 0x060030C8 RID: 12488 RVA: 0x000CAC6C File Offset: 0x000C8E6C
		public Transform[] AccessPoints
		{
			get
			{
				return this.accessPoints;
			}
		}

		// Token: 0x17000726 RID: 1830
		// (get) Token: 0x060030C9 RID: 12489 RVA: 0x000CAC74 File Offset: 0x000C8E74
		public bool Selectable { get; } = 1;

		// Token: 0x17000727 RID: 1831
		// (get) Token: 0x060030CA RID: 12490 RVA: 0x000CAC7C File Offset: 0x000C8E7C
		// (set) Token: 0x060030CB RID: 12491 RVA: 0x000CAC84 File Offset: 0x000C8E84
		public bool IsAcceptingItems { get; set; }

		// Token: 0x17000728 RID: 1832
		// (get) Token: 0x060030CC RID: 12492 RVA: 0x000CAC8D File Offset: 0x000C8E8D
		// (set) Token: 0x060030CD RID: 12493 RVA: 0x000CAC95 File Offset: 0x000C8E95
		public bool IsDestroyed { get; set; }

		// Token: 0x060030CE RID: 12494 RVA: 0x000CAC9E File Offset: 0x000C8E9E
		private void Awake()
		{
			this.GUID = new Guid(this.BakedGUID);
			GUIDManager.RegisterObject(this);
		}

		// Token: 0x060030CF RID: 12495 RVA: 0x000CACB7 File Offset: 0x000C8EB7
		private void Start()
		{
			base.InvokeRepeating("RefreshOccupant", UnityEngine.Random.Range(0f, 1f), 1f);
		}

		// Token: 0x060030D0 RID: 12496 RVA: 0x000CACD8 File Offset: 0x000C8ED8
		public void SetGUID(Guid guid)
		{
			this.GUID = guid;
			GUIDManager.RegisterObject(this);
		}

		// Token: 0x060030D1 RID: 12497 RVA: 0x000CACE8 File Offset: 0x000C8EE8
		private void RefreshOccupant()
		{
			LandVehicle closestVehicle = this.VehicleDetector.closestVehicle;
			if (closestVehicle != null && closestVehicle.speed_Kmh < 2f)
			{
				this.SetOccupant(this.VehicleDetector.closestVehicle);
			}
			else
			{
				this.SetOccupant(null);
			}
			if (this.StaticOccupant != null && !this.StaticOccupant.IsVisible)
			{
				this.SetStaticOccupant(null);
			}
			if (this.DynamicOccupant != null)
			{
				Vector3 position = this.DynamicOccupant.transform.position - this.DynamicOccupant.transform.forward * (this.DynamicOccupant.boundingBoxDimensions.z / 2f + 0.6f);
				this.accessPoints[0].transform.position = position;
				this.accessPoints[0].transform.rotation = Quaternion.LookRotation(this.DynamicOccupant.transform.forward, Vector3.up);
				this.accessPoints[0].transform.localPosition = new Vector3(this.accessPoints[0].transform.localPosition.x, 0f, this.accessPoints[0].transform.localPosition.z);
			}
		}

		// Token: 0x060030D2 RID: 12498 RVA: 0x000CAE34 File Offset: 0x000C9034
		private void SetOccupant(LandVehicle occupant)
		{
			if (occupant == this.DynamicOccupant)
			{
				return;
			}
			Console.Log(string.Concat(new string[]
			{
				"Loading dock ",
				base.name,
				" is ",
				(occupant == null) ? "empty" : "occupied",
				"."
			}), null);
			this.DynamicOccupant = occupant;
			this.InputSlots.Clear();
			this.OutputSlots.Clear();
			if (this.DynamicOccupant != null)
			{
				this.OutputSlots.AddRange(this.DynamicOccupant.Storage.ItemSlots);
			}
		}

		// Token: 0x060030D3 RID: 12499 RVA: 0x000CAEE0 File Offset: 0x000C90E0
		public void SetStaticOccupant(LandVehicle vehicle)
		{
			this.StaticOccupant = vehicle;
		}

		// Token: 0x060030D4 RID: 12500 RVA: 0x000CAEEC File Offset: 0x000C90EC
		public virtual void ShowOutline(Color color)
		{
			if (this.OutlineEffect == null)
			{
				this.OutlineEffect = base.gameObject.AddComponent<Outlinable>();
				this.OutlineEffect.OutlineParameters.BlurShift = 0f;
				this.OutlineEffect.OutlineParameters.DilateShift = 0.5f;
				this.OutlineEffect.OutlineParameters.FillPass.Shader = Resources.Load<Shader>("Easy performant outline/Shaders/Fills/ColorFill");
				foreach (GameObject gameObject in this.OutlineRenderers)
				{
					MeshRenderer[] array = new MeshRenderer[0];
					array = new MeshRenderer[]
					{
						gameObject.GetComponent<MeshRenderer>()
					};
					for (int j = 0; j < array.Length; j++)
					{
						OutlineTarget target = new OutlineTarget(array[j], 0);
						this.OutlineEffect.TryAddTarget(target);
					}
				}
			}
			this.OutlineEffect.OutlineParameters.Color = color;
			Color32 c = color;
			c.a = 9;
			this.OutlineEffect.OutlineParameters.FillPass.SetColor("_PublicColor", c);
			this.OutlineEffect.enabled = true;
		}

		// Token: 0x060030D5 RID: 12501 RVA: 0x000CB011 File Offset: 0x000C9211
		public virtual void HideOutline()
		{
			if (this.OutlineEffect != null)
			{
				this.OutlineEffect.enabled = false;
			}
		}

		// Token: 0x040022E0 RID: 8928
		[SerializeField]
		protected string BakedGUID = string.Empty;

		// Token: 0x040022E6 RID: 8934
		public Property ParentProperty;

		// Token: 0x040022E7 RID: 8935
		public VehicleDetector VehicleDetector;

		// Token: 0x040022E8 RID: 8936
		public ParkingLot Parking;

		// Token: 0x040022E9 RID: 8937
		public Transform uiPoint;

		// Token: 0x040022EA RID: 8938
		public Transform[] accessPoints;

		// Token: 0x040022EB RID: 8939
		public GameObject[] OutlineRenderers;

		// Token: 0x040022EC RID: 8940
		private Outlinable OutlineEffect;
	}
}
