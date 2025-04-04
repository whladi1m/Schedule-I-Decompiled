using System;
using System.Collections.Generic;
using ScheduleOne.ConstructableScripts;
using ScheduleOne.Construction.Features;
using ScheduleOne.DevUtilities;
using ScheduleOne.UI.Construction.Features;
using TMPro;
using UnityEngine;

namespace ScheduleOne.UI.Construction
{
	// Token: 0x02000B57 RID: 2903
	public class FeaturesManager : Singleton<FeaturesManager>
	{
		// Token: 0x17000AA2 RID: 2722
		// (get) Token: 0x06004D25 RID: 19749 RVA: 0x00145B22 File Offset: 0x00143D22
		public bool isActive
		{
			get
			{
				return this.activeConstructable != null;
			}
		}

		// Token: 0x06004D26 RID: 19750 RVA: 0x00145B30 File Offset: 0x00143D30
		protected override void Awake()
		{
			base.Awake();
			this.CloseFeatureMenu();
		}

		// Token: 0x06004D27 RID: 19751 RVA: 0x00145B3E File Offset: 0x00143D3E
		private void LateUpdate()
		{
			if (!this.isActive)
			{
				return;
			}
			if (this.featureIcons.Count > 0)
			{
				this.UpdateIconTransforms();
			}
		}

		// Token: 0x06004D28 RID: 19752 RVA: 0x00145B60 File Offset: 0x00143D60
		public void OpenFeatureMenu(Feature feature)
		{
			if (this.selectedFeature != null)
			{
				this.CloseFeatureMenu();
			}
			this.selectedFeature = feature;
			this.featureMenuRect.gameObject.SetActive(true);
			this.featureMenuTitleLabel.text = Singleton<ConstructionMenu>.Instance.SelectedConstructable.ConstructableName + " > " + this.selectedFeature.featureName;
			if (feature.disableRoofDisibility && Singleton<ConstructionMenu>.Instance.SelectedConstructable is Constructable_GridBased)
			{
				(Singleton<ConstructionMenu>.Instance.SelectedConstructable as Constructable_GridBased).SetRoofVisible(false);
				this.roofSetInvisible = true;
			}
			this.currentFeatureInterface = feature.CreateInterface(this.featureInterfaceContainer);
		}

		// Token: 0x06004D29 RID: 19753 RVA: 0x00145C10 File Offset: 0x00143E10
		public void CloseFeatureMenu()
		{
			if (this.currentFeatureInterface != null)
			{
				this.currentFeatureInterface.Close();
			}
			if (this.roofSetInvisible)
			{
				if (Singleton<ConstructionMenu>.Instance.SelectedConstructable is Constructable_GridBased)
				{
					(Singleton<ConstructionMenu>.Instance.SelectedConstructable as Constructable_GridBased).SetRoofVisible(true);
				}
				this.roofSetInvisible = false;
			}
			this.selectedFeature = null;
			this.featureMenuRect.gameObject.SetActive(false);
		}

		// Token: 0x06004D2A RID: 19754 RVA: 0x00145C84 File Offset: 0x00143E84
		public void DeselectFeature()
		{
			if (this.selectedFeature == null)
			{
				return;
			}
			foreach (FeatureIcon featureIcon in this.featureIcons)
			{
				if (featureIcon.isSelected)
				{
					featureIcon.SetIsSelected(false);
				}
			}
			this.CloseFeatureMenu();
			this.selectedFeature = null;
		}

		// Token: 0x06004D2B RID: 19755 RVA: 0x00145CFC File Offset: 0x00143EFC
		public void Activate(Constructable constructable)
		{
			this.Deactivate();
			this.activeConstructable = constructable;
			this.CreateIcons();
		}

		// Token: 0x06004D2C RID: 19756 RVA: 0x00145D11 File Offset: 0x00143F11
		public void Deactivate()
		{
			this.ClearIcons();
			if (this.selectedFeature != null)
			{
				this.CloseFeatureMenu();
			}
			this.activeConstructable = null;
		}

		// Token: 0x06004D2D RID: 19757 RVA: 0x00145D34 File Offset: 0x00143F34
		private void ClearIcons()
		{
			for (int i = 0; i < this.featureIcons.Count; i++)
			{
				UnityEngine.Object.Destroy(this.featureIcons[i].gameObject);
			}
			this.featureIcons.Clear();
		}

		// Token: 0x06004D2E RID: 19758 RVA: 0x00145D78 File Offset: 0x00143F78
		private void CreateIcons()
		{
			foreach (Feature feature in this.activeConstructable.features)
			{
				FeatureIcon component = UnityEngine.Object.Instantiate<GameObject>(this.featureIconPrefab, this.featureIconsContainer).GetComponent<FeatureIcon>();
				component.AssignFeature(feature);
				this.featureIcons.Add(component);
			}
			this.UpdateIconTransforms();
		}

		// Token: 0x06004D2F RID: 19759 RVA: 0x00145DFC File Offset: 0x00143FFC
		private void UpdateIconTransforms()
		{
			foreach (FeatureIcon featureIcon in this.featureIcons)
			{
				featureIcon.UpdateTransform();
			}
		}

		// Token: 0x04003A6A RID: 14954
		public Constructable activeConstructable;

		// Token: 0x04003A6B RID: 14955
		public Feature selectedFeature;

		// Token: 0x04003A6C RID: 14956
		[Header("References")]
		[SerializeField]
		protected RectTransform featureIconsContainer;

		// Token: 0x04003A6D RID: 14957
		[SerializeField]
		protected RectTransform featureMenuRect;

		// Token: 0x04003A6E RID: 14958
		[SerializeField]
		protected TextMeshProUGUI featureMenuTitleLabel;

		// Token: 0x04003A6F RID: 14959
		[SerializeField]
		protected RectTransform featureInterfaceContainer;

		// Token: 0x04003A70 RID: 14960
		[Header("Prefabs")]
		[SerializeField]
		protected GameObject featureIconPrefab;

		// Token: 0x04003A71 RID: 14961
		private FI_Base currentFeatureInterface;

		// Token: 0x04003A72 RID: 14962
		private bool roofSetInvisible;

		// Token: 0x04003A73 RID: 14963
		protected List<FeatureIcon> featureIcons = new List<FeatureIcon>();
	}
}
