using System;
using System.Collections.Generic;
using ScheduleOne.DevUtilities;
using ScheduleOne.ObjectScripts;
using ScheduleOne.Packaging;
using ScheduleOne.PlayerScripts;
using ScheduleOne.Product;
using ScheduleOne.Product.Packaging;
using ScheduleOne.UI;
using ScheduleOne.UI.Stations;
using UnityEngine;

namespace ScheduleOne.PlayerTasks
{
	// Token: 0x0200034D RID: 845
	public class PackageProductTask : Task
	{
		// Token: 0x17000389 RID: 905
		// (get) Token: 0x060012FE RID: 4862 RVA: 0x00053B01 File Offset: 0x00051D01
		// (set) Token: 0x060012FF RID: 4863 RVA: 0x00053B09 File Offset: 0x00051D09
		public override string TaskName { get; protected set; } = "Package product";

		// Token: 0x06001300 RID: 4864 RVA: 0x00053B14 File Offset: 0x00051D14
		public PackageProductTask(PackagingStation _station)
		{
			if (_station == null)
			{
				Console.LogError("Station is null!", null);
				return;
			}
			if (_station.GetState(PackagingStation.EMode.Package) != PackagingStation.EState.CanBegin)
			{
				Console.LogError("Station not ready to begin packaging!", null);
				return;
			}
			this.station = _station;
			this.ClickDetectionRadius = 0.02f;
			this.Packaging = UnityEngine.Object.Instantiate<FunctionalPackaging>((this.station.PackagingSlot.ItemInstance.Definition as PackagingDefinition).FunctionalPackaging, this.station.Container);
			this.Packaging.Initialize(this.station, this.station.ActivePackagingAlignent, true);
			base.EnableMultiDragging(this.station.Container, 0.08f);
			int quantity = (this.station.PackagingSlot.ItemInstance.Definition as PackagingDefinition).Quantity;
			for (int i = 0; i < quantity; i++)
			{
				FunctionalProduct functionalProduct = UnityEngine.Object.Instantiate<FunctionalProduct>((this.station.ProductSlot.ItemInstance.Definition as ProductDefinition).FunctionalProduct, this.station.Container);
				functionalProduct.Initialize(this.station, this.station.ProductSlot.ItemInstance, this.station.ActiveProductAlignments[i], true);
				functionalProduct.ClampZ = true;
				functionalProduct.DragProjectionMode = Draggable.EDragProjectionMode.FlatCameraForward;
				this.Products.Add(functionalProduct);
			}
			FunctionalPackaging packaging = this.Packaging;
			packaging.onFullyPacked = (Action)Delegate.Combine(packaging.onFullyPacked, new Action(this.FullyPacked));
			FunctionalPackaging packaging2 = this.Packaging;
			packaging2.onSealed = (Action)Delegate.Combine(packaging2.onSealed, new Action(this.Sealed));
			FunctionalPackaging packaging3 = this.Packaging;
			packaging3.onReachOutput = (Action)Delegate.Combine(packaging3.onReachOutput, new Action(this.ReachedOutput));
			this.station.UpdatePackagingVisuals(this.station.PackagingSlot.Quantity - 1);
			this.station.UpdateProductVisuals(this.station.ProductSlot.Quantity - this.Packaging.Definition.Quantity);
			this.station.SetVisualsLocked(true);
			PlayerSingleton<PlayerCamera>.Instance.OverrideFOV(65f, 0.2f);
			PlayerSingleton<PlayerCamera>.Instance.OverrideTransform(this.station.CameraPosition_Task.position, this.station.CameraPosition_Task.rotation, 0.2f, false);
			PlayerSingleton<PlayerInventory>.Instance.SetInventoryEnabled(false);
			Singleton<InputPromptsCanvas>.Instance.LoadModule("packaging");
			base.CurrentInstruction = "Place product into packaging";
		}

		// Token: 0x06001301 RID: 4865 RVA: 0x00053DB4 File Offset: 0x00051FB4
		public override void StopTask()
		{
			this.Packaging.Destroy();
			for (int i = 0; i < this.Products.Count; i++)
			{
				UnityEngine.Object.Destroy(this.Products[i].gameObject);
			}
			this.station.SetVisualsLocked(false);
			this.station.SetHatchOpen(false);
			this.station.UpdateProductVisuals();
			this.station.UpdatePackagingVisuals();
			base.StopTask();
			Singleton<InputPromptsCanvas>.Instance.UnloadModule();
			PlayerSingleton<PlayerCamera>.Instance.OverrideTransform(this.station.CameraPosition.position, this.station.CameraPosition.rotation, 0.2f, false);
			PlayerSingleton<PlayerCamera>.Instance.OverrideFOV(70f, 0.2f);
			if (this.Outcome == Task.EOutcome.Success && this.station.GetState(PackagingStation.EMode.Package) == PackagingStation.EState.CanBegin)
			{
				new PackageProductTask(this.station);
				return;
			}
			Singleton<PackagingStationCanvas>.Instance.SetIsOpen(this.station, true, true);
		}

		// Token: 0x06001302 RID: 4866 RVA: 0x00053EB0 File Offset: 0x000520B0
		public override void Success()
		{
			this.station.PackSingleInstance();
			base.Success();
		}

		// Token: 0x06001303 RID: 4867 RVA: 0x00053EC3 File Offset: 0x000520C3
		private void FullyPacked()
		{
			base.CurrentInstruction = this.Packaging.SealInstruction;
		}

		// Token: 0x06001304 RID: 4868 RVA: 0x00053ED6 File Offset: 0x000520D6
		private void Sealed()
		{
			base.CurrentInstruction = "Place packaging in hopper";
			this.station.SetHatchOpen(true);
		}

		// Token: 0x06001305 RID: 4869 RVA: 0x00053EEF File Offset: 0x000520EF
		private void ReachedOutput()
		{
			this.Success();
		}

		// Token: 0x0400124C RID: 4684
		protected PackagingStation station;

		// Token: 0x0400124D RID: 4685
		protected FunctionalPackaging Packaging;

		// Token: 0x0400124E RID: 4686
		protected List<FunctionalProduct> Products = new List<FunctionalProduct>();
	}
}
