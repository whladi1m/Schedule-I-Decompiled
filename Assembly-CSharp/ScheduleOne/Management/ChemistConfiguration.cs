using System;
using System.Collections.Generic;
using ScheduleOne.Employees;
using ScheduleOne.EntityFramework;
using ScheduleOne.ObjectScripts;
using ScheduleOne.Persistence.Datas;
using ScheduleOne.UI.Management;
using UnityEngine.Events;

namespace ScheduleOne.Management
{
	// Token: 0x0200055A RID: 1370
	public class ChemistConfiguration : EntityConfiguration
	{
		// Token: 0x1700050A RID: 1290
		// (get) Token: 0x060021ED RID: 8685 RVA: 0x0008BF67 File Offset: 0x0008A167
		public int TotalStations
		{
			get
			{
				return this.ChemStations.Count + this.LabOvens.Count + this.Cauldrons.Count + this.MixStations.Count;
			}
		}

		// Token: 0x1700050B RID: 1291
		// (get) Token: 0x060021EE RID: 8686 RVA: 0x0008BF98 File Offset: 0x0008A198
		// (set) Token: 0x060021EF RID: 8687 RVA: 0x0008BFA0 File Offset: 0x0008A1A0
		public Chemist chemist { get; protected set; }

		// Token: 0x1700050C RID: 1292
		// (get) Token: 0x060021F0 RID: 8688 RVA: 0x0008BFA9 File Offset: 0x0008A1A9
		// (set) Token: 0x060021F1 RID: 8689 RVA: 0x0008BFB1 File Offset: 0x0008A1B1
		public BedItem bedItem { get; private set; }

		// Token: 0x060021F2 RID: 8690 RVA: 0x0008BFBC File Offset: 0x0008A1BC
		public ChemistConfiguration(ConfigurationReplicator replicator, IConfigurable configurable, Chemist _chemist) : base(replicator, configurable)
		{
			this.chemist = _chemist;
			this.Bed = new ObjectField(this);
			this.Bed.TypeRequirements = new List<Type>
			{
				typeof(BedItem)
			};
			this.Bed.onObjectChanged.AddListener(new UnityAction<BuildableItem>(this.BedChanged));
			this.Bed.objectFilter = new ObjectSelector.ObjectFilter(BedItem.IsBedValid);
			this.Stations = new ObjectListField(this);
			this.Stations.MaxItems = 4;
			this.Stations.TypeRequirements = new List<Type>
			{
				typeof(ChemistryStation),
				typeof(LabOven),
				typeof(Cauldron),
				typeof(MixingStation),
				typeof(MixingStationMk2)
			};
			this.Stations.onListChanged.AddListener(delegate(List<BuildableItem> <p0>)
			{
				base.InvokeChanged();
			});
			this.Stations.onListChanged.AddListener(new UnityAction<List<BuildableItem>>(this.AssignedStationsChanged));
			this.Stations.objectFilter = new ObjectSelector.ObjectFilter(this.IsStationValid);
		}

		// Token: 0x060021F3 RID: 8691 RVA: 0x0008C12C File Offset: 0x0008A32C
		public override void Destroy()
		{
			base.Destroy();
			this.Bed.SetObject(null, false);
			foreach (ChemistryStation chemistryStation in this.ChemStations)
			{
				(chemistryStation.Configuration as ChemistryStationConfiguration).AssignedChemist.SetNPC(null, false);
			}
			foreach (LabOven labOven in this.LabOvens)
			{
				(labOven.Configuration as LabOvenConfiguration).AssignedChemist.SetNPC(null, false);
			}
			foreach (Cauldron cauldron in this.Cauldrons)
			{
				(cauldron.Configuration as CauldronConfiguration).AssignedChemist.SetNPC(null, false);
			}
			foreach (MixingStation mixingStation in this.MixStations)
			{
				(mixingStation.Configuration as MixingStationConfiguration).AssignedChemist.SetNPC(null, false);
			}
		}

		// Token: 0x060021F4 RID: 8692 RVA: 0x0008C290 File Offset: 0x0008A490
		private bool IsStationValid(BuildableItem obj, out string reason)
		{
			reason = string.Empty;
			if (obj is ChemistryStation)
			{
				ChemistryStationConfiguration chemistryStationConfiguration = (obj as ChemistryStation).Configuration as ChemistryStationConfiguration;
				if (chemistryStationConfiguration.AssignedChemist.SelectedNPC != null && chemistryStationConfiguration.AssignedChemist.SelectedNPC != this.chemist)
				{
					reason = "Already assigned to " + chemistryStationConfiguration.AssignedChemist.SelectedNPC.fullName;
					return false;
				}
				return true;
			}
			else if (obj is LabOven)
			{
				LabOvenConfiguration labOvenConfiguration = (obj as LabOven).Configuration as LabOvenConfiguration;
				if (labOvenConfiguration.AssignedChemist.SelectedNPC != null && labOvenConfiguration.AssignedChemist.SelectedNPC != this.chemist)
				{
					reason = "Already assigned to " + labOvenConfiguration.AssignedChemist.SelectedNPC.fullName;
					return false;
				}
				return true;
			}
			else if (obj is Cauldron)
			{
				CauldronConfiguration cauldronConfiguration = (obj as Cauldron).Configuration as CauldronConfiguration;
				if (cauldronConfiguration.AssignedChemist.SelectedNPC != null && cauldronConfiguration.AssignedChemist.SelectedNPC != this.chemist)
				{
					reason = "Already assigned to " + cauldronConfiguration.AssignedChemist.SelectedNPC.fullName;
					return false;
				}
				return true;
			}
			else
			{
				if (!(obj is MixingStation))
				{
					return false;
				}
				MixingStationConfiguration mixingStationConfiguration = (obj as MixingStation).Configuration as MixingStationConfiguration;
				if (mixingStationConfiguration.AssignedChemist.SelectedNPC != null && mixingStationConfiguration.AssignedChemist.SelectedNPC != this.chemist)
				{
					reason = "Already assigned to " + mixingStationConfiguration.AssignedChemist.SelectedNPC.fullName;
					return false;
				}
				return true;
			}
		}

		// Token: 0x060021F5 RID: 8693 RVA: 0x0008C438 File Offset: 0x0008A638
		public void AssignedStationsChanged(List<BuildableItem> objects)
		{
			for (int i = 0; i < this.ChemStations.Count; i++)
			{
				if (!objects.Contains(this.ChemStations[i]))
				{
					ChemistryStation chemistryStation = this.ChemStations[i];
					this.ChemStations.RemoveAt(i);
					i--;
					if ((chemistryStation.Configuration as ChemistryStationConfiguration).AssignedChemist.SelectedNPC == this.chemist)
					{
						(chemistryStation.Configuration as ChemistryStationConfiguration).AssignedChemist.SetNPC(null, false);
					}
				}
			}
			for (int j = 0; j < this.LabOvens.Count; j++)
			{
				if (!objects.Contains(this.LabOvens[j]))
				{
					LabOven labOven = this.LabOvens[j];
					this.LabOvens.RemoveAt(j);
					j--;
					if ((labOven.Configuration as LabOvenConfiguration).AssignedChemist.SelectedNPC == this.chemist)
					{
						(labOven.Configuration as LabOvenConfiguration).AssignedChemist.SetNPC(null, false);
					}
				}
			}
			for (int k = 0; k < this.Cauldrons.Count; k++)
			{
				if (!objects.Contains(this.Cauldrons[k]))
				{
					Cauldron cauldron = this.Cauldrons[k];
					this.Cauldrons.RemoveAt(k);
					k--;
					if ((cauldron.Configuration as CauldronConfiguration).AssignedChemist.SelectedNPC == this.chemist)
					{
						(cauldron.Configuration as CauldronConfiguration).AssignedChemist.SetNPC(null, false);
					}
				}
			}
			for (int l = 0; l < this.MixStations.Count; l++)
			{
				if (!objects.Contains(this.MixStations[l]))
				{
					MixingStation mixingStation = this.MixStations[l];
					this.MixStations.RemoveAt(l);
					l--;
					if ((mixingStation.Configuration as MixingStationConfiguration).AssignedChemist.SelectedNPC == this.chemist)
					{
						(mixingStation.Configuration as MixingStationConfiguration).AssignedChemist.SetNPC(null, false);
					}
				}
			}
			for (int m = 0; m < objects.Count; m++)
			{
				if (objects[m] is ChemistryStation && !this.ChemStations.Contains(objects[m] as ChemistryStation))
				{
					ChemistryStation chemistryStation2 = objects[m] as ChemistryStation;
					this.ChemStations.Add(chemistryStation2);
					if ((chemistryStation2.Configuration as ChemistryStationConfiguration).AssignedChemist.SelectedNPC != this.chemist)
					{
						(chemistryStation2.Configuration as ChemistryStationConfiguration).AssignedChemist.SetNPC(this.chemist, false);
					}
				}
				if (objects[m] is LabOven && !this.LabOvens.Contains(objects[m] as LabOven))
				{
					LabOven labOven2 = objects[m] as LabOven;
					this.LabOvens.Add(labOven2);
					if ((labOven2.Configuration as LabOvenConfiguration).AssignedChemist.SelectedNPC != this.chemist)
					{
						(labOven2.Configuration as LabOvenConfiguration).AssignedChemist.SetNPC(this.chemist, false);
					}
				}
				if (objects[m] is Cauldron && !this.Cauldrons.Contains(objects[m] as Cauldron))
				{
					Cauldron cauldron2 = objects[m] as Cauldron;
					this.Cauldrons.Add(cauldron2);
					if ((cauldron2.Configuration as CauldronConfiguration).AssignedChemist.SelectedNPC != this.chemist)
					{
						(cauldron2.Configuration as CauldronConfiguration).AssignedChemist.SetNPC(this.chemist, false);
					}
				}
				if (objects[m] is MixingStation && !this.MixStations.Contains(objects[m] as MixingStation))
				{
					MixingStation mixingStation2 = objects[m] as MixingStation;
					this.MixStations.Add(mixingStation2);
					if ((mixingStation2.Configuration as MixingStationConfiguration).AssignedChemist.SelectedNPC != this.chemist)
					{
						(mixingStation2.Configuration as MixingStationConfiguration).AssignedChemist.SetNPC(this.chemist, false);
					}
				}
			}
		}

		// Token: 0x060021F6 RID: 8694 RVA: 0x0008C894 File Offset: 0x0008AA94
		public override bool ShouldSave()
		{
			return this.Bed.SelectedObject != null || this.ChemStations.Count > 0 || this.LabOvens.Count > 0 || this.Cauldrons.Count > 0 || base.ShouldSave();
		}

		// Token: 0x060021F7 RID: 8695 RVA: 0x0008C8EC File Offset: 0x0008AAEC
		public override string GetSaveString()
		{
			return new ChemistConfigurationData(this.Bed.GetData(), this.Stations.GetData()).GetJson(true);
		}

		// Token: 0x060021F8 RID: 8696 RVA: 0x0008C910 File Offset: 0x0008AB10
		private void BedChanged(BuildableItem newItem)
		{
			BedItem bedItem = this.bedItem;
			if (bedItem != null)
			{
				bedItem.Bed.SetAssignedEmployee(null);
			}
			this.bedItem = ((newItem != null) ? (newItem as BedItem) : null);
			if (this.bedItem != null)
			{
				this.bedItem.Bed.SetAssignedEmployee(this.chemist);
			}
			base.InvokeChanged();
		}

		// Token: 0x040019CC RID: 6604
		public ObjectField Bed;

		// Token: 0x040019CD RID: 6605
		public ObjectListField Stations;

		// Token: 0x040019CE RID: 6606
		public List<ChemistryStation> ChemStations = new List<ChemistryStation>();

		// Token: 0x040019CF RID: 6607
		public List<LabOven> LabOvens = new List<LabOven>();

		// Token: 0x040019D0 RID: 6608
		public List<Cauldron> Cauldrons = new List<Cauldron>();

		// Token: 0x040019D1 RID: 6609
		public List<MixingStation> MixStations = new List<MixingStation>();
	}
}
