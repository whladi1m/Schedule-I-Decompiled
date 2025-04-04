using System;
using System.Collections.Generic;
using System.IO;
using EasyButtons;
using ScheduleOne.ItemFramework;
using ScheduleOne.Packaging;
using ScheduleOne.Product;
using UnityEngine;
using UnityEngine.Rendering;

namespace ScheduleOne.DevUtilities
{
	// Token: 0x020006CB RID: 1739
	public class IconGenerator : Singleton<IconGenerator>
	{
		// Token: 0x06002F75 RID: 12149 RVA: 0x000C5D8C File Offset: 0x000C3F8C
		protected override void Awake()
		{
			base.Awake();
			this.Canvas.gameObject.SetActive(false);
			this.CameraPosition.gameObject.SetActive(false);
			this.CameraPosition.clearFlags = CameraClearFlags.Color;
			if (this.Registry == null)
			{
				this.Registry = Singleton<Registry>.Instance;
			}
		}

		// Token: 0x06002F76 RID: 12150 RVA: 0x000C5DE8 File Offset: 0x000C3FE8
		[Button]
		public void GenerateIcon()
		{
			LayerUtility.SetLayerRecursively(this.ItemContainer.gameObject, LayerMask.NameToLayer("IconGeneration"));
			Transform transform = null;
			for (int i = 0; i < this.ItemContainer.transform.childCount; i++)
			{
				if (this.ItemContainer.transform.GetChild(i).gameObject.activeSelf)
				{
					transform = this.ItemContainer.transform.GetChild(i);
				}
			}
			string text = this.OutputPath + "/" + transform.name + "_Icon.png";
			Texture2D texture = this.GetTexture(transform.transform);
			Debug.Log("Writing to: " + text);
			byte[] bytes = texture.EncodeToPNG();
			File.WriteAllBytes(text, bytes);
		}

		// Token: 0x06002F77 RID: 12151 RVA: 0x000C5EA0 File Offset: 0x000C40A0
		public Texture2D GeneratePackagingIcon(string packagingID, string productID)
		{
			IconGenerator.PackagingVisuals packagingVisuals = this.Visuals.Find((IconGenerator.PackagingVisuals x) => packagingID == x.PackagingID);
			if (packagingVisuals == null)
			{
				Debug.LogError(string.Concat(new string[]
				{
					"Failed to find visuals for packaging (",
					packagingID,
					") containing product (",
					productID,
					")"
				}));
				return null;
			}
			ItemDefinition itemDefinition = this.Registry._GetItem(productID);
			if (Application.isPlaying)
			{
				itemDefinition = Singleton<Registry>.Instance._GetItem(productID);
			}
			ProductDefinition productDefinition = itemDefinition as ProductDefinition;
			if (productDefinition == null)
			{
				Debug.LogError("Failed to find product definition for product (" + productID + ")");
				return null;
			}
			(productDefinition.GetDefaultInstance(1) as ProductItemInstance).SetupPackagingVisuals(packagingVisuals.Visuals);
			packagingVisuals.Visuals.gameObject.SetActive(true);
			Texture2D texture = this.GetTexture(packagingVisuals.Visuals.transform.parent);
			packagingVisuals.Visuals.gameObject.SetActive(false);
			return texture;
		}

		// Token: 0x06002F78 RID: 12152 RVA: 0x000C5FA4 File Offset: 0x000C41A4
		public Texture2D GetTexture(Transform model)
		{
			this.MainContainer.gameObject.SetActive(true);
			bool activeSelf = this.ItemContainer.gameObject.activeSelf;
			this.ItemContainer.gameObject.SetActive(true);
			if (this.ModifyLighting)
			{
				RenderSettings.ambientMode = AmbientMode.Flat;
				RenderSettings.ambientLight = Color.white;
			}
			RuntimePreviewGenerator.CamPos = this.CameraPosition.transform.position;
			RuntimePreviewGenerator.CamRot = this.CameraPosition.transform.rotation;
			RuntimePreviewGenerator.Padding = 0f;
			RuntimePreviewGenerator.UseLocalBounds = true;
			RuntimePreviewGenerator.BackgroundColor = new Color32(0, 0, 0, 0);
			Texture2D result = RuntimePreviewGenerator.GenerateModelPreview(model, this.IconSize, this.IconSize, false, true);
			RenderSettings.ambientMode = AmbientMode.Trilight;
			this.MainContainer.gameObject.SetActive(false);
			this.ItemContainer.gameObject.SetActive(activeSelf);
			return result;
		}

		// Token: 0x040021DD RID: 8669
		public int IconSize = 512;

		// Token: 0x040021DE RID: 8670
		public string OutputPath;

		// Token: 0x040021DF RID: 8671
		public bool ModifyLighting = true;

		// Token: 0x040021E0 RID: 8672
		[Header("References")]
		public Registry Registry;

		// Token: 0x040021E1 RID: 8673
		public Camera CameraPosition;

		// Token: 0x040021E2 RID: 8674
		public Transform MainContainer;

		// Token: 0x040021E3 RID: 8675
		public Transform ItemContainer;

		// Token: 0x040021E4 RID: 8676
		public GameObject Canvas;

		// Token: 0x040021E5 RID: 8677
		public List<IconGenerator.PackagingVisuals> Visuals;

		// Token: 0x020006CC RID: 1740
		[Serializable]
		public class PackagingVisuals
		{
			// Token: 0x040021E6 RID: 8678
			public string PackagingID;

			// Token: 0x040021E7 RID: 8679
			public FilledPackagingVisuals Visuals;
		}
	}
}
