using System;
using System.Collections.Generic;
using System.Linq;
using ScheduleOne.DevUtilities;
using ScheduleOne.Interaction;
using ScheduleOne.Packaging;
using ScheduleOne.Properties;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.Product
{
	// Token: 0x020008CF RID: 2255
	public class NewMixDiscoveryBox : MonoBehaviour
	{
		// Token: 0x06003CF7 RID: 15607 RVA: 0x000FFF34 File Offset: 0x000FE134
		public void Start()
		{
			this.closedLidPose = new Pose(this.Lid.localPosition, this.Lid.localRotation);
			this.CloseCase();
			this.IntObj.onInteractStart.AddListener(new UnityAction(this.Interacted));
			this.IntObj.gameObject.SetActive(false);
			bool isMixComplete = NetworkSingleton<ProductManager>.Instance.IsMixComplete;
		}

		// Token: 0x06003CF8 RID: 15608 RVA: 0x000FFFA0 File Offset: 0x000FE1A0
		public void ShowProduct(ProductDefinition baseDefinition, List<Property> properties)
		{
			this.PropertiesText.text = string.Empty;
			foreach (Property property in properties)
			{
				if (this.PropertiesText.text.Length > 0)
				{
					TextMeshPro propertiesText = this.PropertiesText;
					propertiesText.text += "\n";
				}
				TextMeshPro propertiesText2 = this.PropertiesText;
				propertiesText2.text = string.Concat(new string[]
				{
					propertiesText2.text,
					"<color=#",
					ColorUtility.ToHtmlStringRGBA(property.LabelColor),
					">",
					property.Name,
					"</color>"
				});
			}
			for (int i = 0; i < this.Visuals.Length; i++)
			{
				this.Visuals[i].Visuals.gameObject.SetActive(false);
			}
			ProductDefinition productDefinition = UnityEngine.Object.Instantiate<ProductDefinition>(baseDefinition);
			switch (baseDefinition.DrugType)
			{
			case EDrugType.Marijuana:
			{
				WeedDefinition weedDefinition = productDefinition as WeedDefinition;
				WeedAppearanceSettings appearanceSettings = WeedDefinition.GetAppearanceSettings(properties);
				weedDefinition.Initialize(properties, new List<EDrugType>
				{
					EDrugType.Marijuana
				}, appearanceSettings);
				(weedDefinition.GetDefaultInstance(1) as WeedInstance).SetupPackagingVisuals(this.Visuals.First((NewMixDiscoveryBox.DrugTypeVisuals x) => x.DrugType == EDrugType.Marijuana).Visuals);
				this.Visuals.First((NewMixDiscoveryBox.DrugTypeVisuals x) => x.DrugType == EDrugType.Marijuana).Visuals.gameObject.SetActive(true);
				break;
			}
			case EDrugType.Methamphetamine:
			{
				MethDefinition methDefinition = productDefinition as MethDefinition;
				MethAppearanceSettings appearanceSettings2 = MethDefinition.GetAppearanceSettings(properties);
				methDefinition.Initialize(properties, new List<EDrugType>
				{
					EDrugType.Methamphetamine
				}, appearanceSettings2);
				(methDefinition.GetDefaultInstance(1) as MethInstance).SetupPackagingVisuals(this.Visuals.First((NewMixDiscoveryBox.DrugTypeVisuals x) => x.DrugType == EDrugType.Methamphetamine).Visuals);
				this.Visuals.First((NewMixDiscoveryBox.DrugTypeVisuals x) => x.DrugType == EDrugType.Methamphetamine).Visuals.gameObject.SetActive(true);
				break;
			}
			case EDrugType.Cocaine:
			{
				CocaineDefinition cocaineDefinition = productDefinition as CocaineDefinition;
				CocaineAppearanceSettings appearanceSettings3 = CocaineDefinition.GetAppearanceSettings(properties);
				cocaineDefinition.Initialize(properties, new List<EDrugType>
				{
					EDrugType.Cocaine
				}, appearanceSettings3);
				(cocaineDefinition.GetDefaultInstance(1) as CocaineInstance).SetupPackagingVisuals(this.Visuals.First((NewMixDiscoveryBox.DrugTypeVisuals x) => x.DrugType == EDrugType.Cocaine).Visuals);
				this.Visuals.First((NewMixDiscoveryBox.DrugTypeVisuals x) => x.DrugType == EDrugType.Cocaine).Visuals.gameObject.SetActive(true);
				break;
			}
			default:
				Console.LogError("Drug type not supported", null);
				break;
			}
			base.gameObject.SetActive(true);
		}

		// Token: 0x06003CF9 RID: 15609 RVA: 0x001002CC File Offset: 0x000FE4CC
		private void CloseCase()
		{
			this.isOpen = false;
			this.Lid.localPosition = this.closedLidPose.position;
			this.Lid.localRotation = this.closedLidPose.rotation;
		}

		// Token: 0x06003CFA RID: 15610 RVA: 0x00100301 File Offset: 0x000FE501
		private void OpenCase()
		{
			this.isOpen = true;
			this.Animation.Play("New mix box open");
		}

		// Token: 0x06003CFB RID: 15611 RVA: 0x0010031B File Offset: 0x000FE51B
		private void Interacted()
		{
			if (!this.isOpen)
			{
				this.OpenCase();
			}
			Registry.GetItem(this.currentMix.ProductID);
		}

		// Token: 0x04002C14 RID: 11284
		private bool isOpen;

		// Token: 0x04002C15 RID: 11285
		[Header("References")]
		public Transform CameraPosition;

		// Token: 0x04002C16 RID: 11286
		public TextMeshPro PropertiesText;

		// Token: 0x04002C17 RID: 11287
		public NewMixDiscoveryBox.DrugTypeVisuals[] Visuals;

		// Token: 0x04002C18 RID: 11288
		public Animation Animation;

		// Token: 0x04002C19 RID: 11289
		public InteractableObject IntObj;

		// Token: 0x04002C1A RID: 11290
		public Transform Lid;

		// Token: 0x04002C1B RID: 11291
		private Pose closedLidPose;

		// Token: 0x04002C1C RID: 11292
		private NewMixOperation currentMix;

		// Token: 0x020008D0 RID: 2256
		[Serializable]
		public class DrugTypeVisuals
		{
			// Token: 0x04002C1D RID: 11293
			public EDrugType DrugType;

			// Token: 0x04002C1E RID: 11294
			public FilledPackagingVisuals Visuals;
		}
	}
}
