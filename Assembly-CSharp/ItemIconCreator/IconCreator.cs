using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace ItemIconCreator
{
	// Token: 0x0200021E RID: 542
	[ExecuteInEditMode]
	[RequireComponent(typeof(Camera))]
	[DisallowMultipleComponent]
	public class IconCreator : MonoBehaviour
	{
		// Token: 0x06000B86 RID: 2950 RVA: 0x000358E0 File Offset: 0x00033AE0
		private void Awake()
		{
			this.mainCam = base.gameObject.GetComponent<Camera>();
			this.originalClearFlags = this.mainCam.clearFlags;
			if (IconCreatorCanvas.instance != null)
			{
				IconCreatorCanvas.instance.SetInfo(0, 0, "", false, this.nextIconKey);
			}
		}

		// Token: 0x06000B87 RID: 2951 RVA: 0x00035934 File Offset: 0x00033B34
		protected void Initialize()
		{
			this.mainCam.clearFlags = this.originalClearFlags;
			this.isCreatingIcons = true;
			foreach (Camera camera in UnityEngine.Object.FindObjectsOfType<Camera>())
			{
				if (!(camera == this.mainCam))
				{
					camera.gameObject.SetActive(false);
				}
			}
			if (this.useTransparency)
			{
				this.CreateBlackAndWhiteCameras();
			}
			this.CacheAndInitialiseFields();
		}

		// Token: 0x06000B88 RID: 2952 RVA: 0x000359A0 File Offset: 0x00033BA0
		protected void DeleteCameras()
		{
			if (this.whiteCam != null)
			{
				UnityEngine.Object.Destroy(this.whiteCam.gameObject);
			}
			if (this.blackCam != null)
			{
				UnityEngine.Object.Destroy(this.blackCam.gameObject);
			}
			this.isCreatingIcons = false;
		}

		// Token: 0x06000B89 RID: 2953 RVA: 0x000359F0 File Offset: 0x00033BF0
		public virtual void BuildIcons()
		{
			Debug.LogError("Not implemented");
		}

		// Token: 0x06000B8A RID: 2954 RVA: 0x000359FC File Offset: 0x00033BFC
		protected IEnumerator CaptureFrame(string objectName, int i)
		{
			if (this.whiteCam != null)
			{
				this.whiteCam.enabled = true;
			}
			if (this.blackCam != null)
			{
				this.blackCam.enabled = true;
			}
			yield return new WaitForEndOfFrame();
			if (this.useTransparency)
			{
				this.RenderCamToTexture(this.blackCam, this.texBlack);
				this.RenderCamToTexture(this.whiteCam, this.texWhite);
				this.CalculateOutputTexture();
			}
			else
			{
				this.RenderCamToTexture(this.mainCam, this.finalTexture);
			}
			this.SavePng(objectName, i);
			this.mainCam.enabled = true;
			yield break;
		}

		// Token: 0x06000B8B RID: 2955 RVA: 0x00035A1C File Offset: 0x00033C1C
		protected virtual void Update()
		{
			if (this.mode == IconCreator.Mode.Automatic)
			{
				return;
			}
			if (!this.CanMove)
			{
				return;
			}
			if (Input.GetMouseButtonDown(0))
			{
				this.mousePostion = Input.mousePosition;
			}
			if (Input.GetMouseButton(0))
			{
				Vector3 vector = this.mousePostion - Input.mousePosition;
				this.currentObject.Rotate(new Vector3(-vector.y, vector.x, vector.z) * Time.deltaTime * 40f, Space.World);
				this.mousePostion = Input.mousePosition;
				if (this.dynamicFov)
				{
					this.UpdateFOV(this.currentObject.gameObject);
				}
				if (this.lookAtObjectCenter)
				{
					this.LookAtTargetCenter(this.currentObject.gameObject);
				}
			}
			this.UpdateFOV(Input.mouseScrollDelta.y * -2.5f);
		}

		// Token: 0x06000B8C RID: 2956 RVA: 0x00035AF5 File Offset: 0x00033CF5
		private void RenderCamToTexture(Camera cam, Texture2D tex)
		{
			cam.enabled = true;
			cam.Render();
			this.WriteScreenImageToTexture(tex);
			cam.enabled = false;
		}

		// Token: 0x06000B8D RID: 2957 RVA: 0x00035B14 File Offset: 0x00033D14
		private void CreateBlackAndWhiteCameras()
		{
			this.mainCam.clearFlags = CameraClearFlags.Color;
			GameObject gameObject = new GameObject();
			gameObject.name = "White Background Camera";
			this.whiteCam = gameObject.AddComponent<Camera>();
			this.whiteCam.CopyFrom(this.mainCam);
			this.whiteCam.backgroundColor = Color.white;
			gameObject.transform.SetParent(base.gameObject.transform, true);
			gameObject = new GameObject();
			gameObject.name = "Black Background Camera";
			this.blackCam = gameObject.AddComponent<Camera>();
			this.blackCam.CopyFrom(this.mainCam);
			this.blackCam.backgroundColor = Color.black;
			gameObject.transform.SetParent(base.gameObject.transform, true);
		}

		// Token: 0x06000B8E RID: 2958 RVA: 0x00035BD8 File Offset: 0x00033DD8
		protected void CreateNewFolderForIcons()
		{
			this.finalPath = this.GetFinalFolder();
			if (Directory.Exists(this.finalPath))
			{
				int num = 1;
				while (Directory.Exists(this.finalPath + " " + num.ToString()))
				{
					num++;
				}
				this.finalPath = this.finalPath + " " + num.ToString();
			}
			Directory.CreateDirectory(this.finalPath);
		}

		// Token: 0x06000B8F RID: 2959 RVA: 0x00035C4D File Offset: 0x00033E4D
		public string GetFinalFolder()
		{
			if (!string.IsNullOrWhiteSpace(this.GetBaseLocation()))
			{
				return Path.Combine(this.GetBaseLocation(), this.folderName);
			}
			return this.folderName;
		}

		// Token: 0x06000B90 RID: 2960 RVA: 0x00035C74 File Offset: 0x00033E74
		private void WriteScreenImageToTexture(Texture2D tex)
		{
			tex.ReadPixels(new Rect(0f, 0f, (float)Screen.width, (float)Screen.width), 0, 0);
			tex.Apply();
		}

		// Token: 0x06000B91 RID: 2961 RVA: 0x00035CA0 File Offset: 0x00033EA0
		private void CalculateOutputTexture()
		{
			for (int i = 0; i < this.finalTexture.height; i++)
			{
				for (int j = 0; j < this.finalTexture.width; j++)
				{
					float num = this.texWhite.GetPixel(j, i).r - this.texBlack.GetPixel(j, i).r;
					num = 1f - num;
					Color color;
					if (num == 0f)
					{
						color = Color.clear;
					}
					else
					{
						color = this.texBlack.GetPixel(j, i) / num;
					}
					color.a = num;
					this.finalTexture.SetPixel(j, i, color);
				}
			}
		}

		// Token: 0x06000B92 RID: 2962 RVA: 0x00035D48 File Offset: 0x00033F48
		private void SavePng(string name, int i)
		{
			string fileName = this.GetFileName(name, i);
			string text = this.finalPath + "/" + fileName;
			Debug.Log("Writing to: " + text);
			byte[] bytes = this.finalTexture.EncodeToPNG();
			File.WriteAllBytes(text, bytes);
		}

		// Token: 0x06000B93 RID: 2963 RVA: 0x00035D94 File Offset: 0x00033F94
		public string GetFileName(string name, int i)
		{
			string str;
			if (this.useDafaultName)
			{
				str = name;
			}
			else
			{
				str = this.iconFileName;
			}
			str += " Icon";
			if (this.includeResolutionInFileName)
			{
				str = str + " " + this.mainCam.scaledPixelHeight.ToString() + "x";
			}
			return str + ".png";
		}

		// Token: 0x06000B94 RID: 2964 RVA: 0x00035DF8 File Offset: 0x00033FF8
		private void CacheAndInitialiseFields()
		{
			this.texBlack = new Texture2D(this.mainCam.pixelWidth, this.mainCam.pixelHeight, TextureFormat.RGB24, false);
			this.texWhite = new Texture2D(this.mainCam.pixelWidth, this.mainCam.pixelHeight, TextureFormat.RGB24, false);
			this.finalTexture = new Texture2D(this.mainCam.pixelWidth, this.mainCam.pixelHeight, TextureFormat.ARGB32, false);
		}

		// Token: 0x06000B95 RID: 2965 RVA: 0x00035E70 File Offset: 0x00034070
		protected void UpdateFOV(GameObject targetItem)
		{
			float targetFov = this.GetTargetFov(targetItem);
			if (this.useTransparency && this.isCreatingIcons)
			{
				this.whiteCam.fieldOfView = targetFov;
				this.blackCam.fieldOfView = targetFov;
			}
			this.mainCam.fieldOfView = targetFov;
		}

		// Token: 0x06000B96 RID: 2966 RVA: 0x00035EBC File Offset: 0x000340BC
		protected void UpdateFOV(float value)
		{
			if (value == 0f)
			{
				return;
			}
			value = this.mainCam.fieldOfView * value / 100f;
			this.dynamicFov = false;
			if (this.useTransparency)
			{
				this.whiteCam.fieldOfView += value;
				this.blackCam.fieldOfView += value;
			}
			this.mainCam.fieldOfView += value;
		}

		// Token: 0x06000B97 RID: 2967 RVA: 0x00035F30 File Offset: 0x00034130
		protected void LookAtTargetCenter(GameObject targetItem)
		{
			Vector3 meshCenter = this.GetMeshCenter(targetItem);
			this.mainCam.transform.LookAt(meshCenter);
			if (this.whiteCam != null)
			{
				this.whiteCam.transform.LookAt(meshCenter);
			}
			if (this.blackCam != null)
			{
				this.blackCam.transform.LookAt(meshCenter);
			}
		}

		// Token: 0x06000B98 RID: 2968 RVA: 0x00035F94 File Offset: 0x00034194
		private float GetTargetFov(GameObject a)
		{
			Vector3 vector = Vector3.one * 30000f;
			Vector3 vector2 = Vector3.zero;
			List<Renderer> renderers = this.GetRenderers(a);
			for (int i = 0; i < renderers.Count; i++)
			{
				if (Vector3.Distance(Vector3.zero, renderers[i].bounds.min) < Vector3.Distance(Vector3.zero, vector))
				{
					vector = renderers[i].bounds.min;
				}
				if (Vector3.Distance(Vector3.zero, renderers[i].bounds.max) > Vector3.Distance(Vector3.zero, vector2))
				{
					vector2 = renderers[i].bounds.max;
				}
			}
			Vector3 a2 = (vector + vector2) / 2f;
			float num = (vector2 - vector).magnitude / 2f;
			float num2 = Vector3.Distance(a2, base.transform.position);
			float num3 = Mathf.Sqrt(num * num + num2 * num2);
			return Mathf.Asin(num / num3) * 57.29578f * 2f + this.fovOffset;
		}

		// Token: 0x06000B99 RID: 2969 RVA: 0x000360C8 File Offset: 0x000342C8
		private List<Renderer> GetRenderers(GameObject obj)
		{
			List<Renderer> list = new List<Renderer>();
			if (obj.GetComponents<Renderer>() != null)
			{
				list.AddRange(obj.GetComponents<Renderer>());
			}
			if (obj.GetComponentsInChildren<Renderer>() != null)
			{
				list.AddRange(obj.GetComponentsInChildren<Renderer>());
			}
			return list;
		}

		// Token: 0x06000B9A RID: 2970 RVA: 0x00036104 File Offset: 0x00034304
		private Vector3 GetMeshCenter(GameObject a)
		{
			Vector3 a2 = Vector3.zero;
			List<Renderer> renderers = this.GetRenderers(a);
			if (renderers == null)
			{
				Debug.LogError("No mesh was founded in object " + a.name);
				return a.transform.position;
			}
			for (int i = 0; i < renderers.Count; i++)
			{
				a2 += renderers[i].bounds.center;
			}
			return a2 / (float)renderers.Count;
		}

		// Token: 0x06000B9B RID: 2971 RVA: 0x000045B1 File Offset: 0x000027B1
		protected void RevealInFinder()
		{
		}

		// Token: 0x06000B9C RID: 2972 RVA: 0x00036180 File Offset: 0x00034380
		public virtual bool CheckConditions()
		{
			if (this.pathLocation == IconCreator.SaveLocation.custom && !Directory.Exists(this.folderName))
			{
				Debug.LogError("Folder " + this.folderName + " does not exists");
				return false;
			}
			if (!this.useDafaultName && string.IsNullOrWhiteSpace(this.iconFileName))
			{
				Debug.LogError("Invalid icon file name");
				return false;
			}
			return true;
		}

		// Token: 0x06000B9D RID: 2973 RVA: 0x000361E1 File Offset: 0x000343E1
		private string GetBaseLocation()
		{
			if (this.pathLocation == IconCreator.SaveLocation.dataPath)
			{
				return Application.dataPath;
			}
			if (this.pathLocation == IconCreator.SaveLocation.persistentDataPath)
			{
				return Application.persistentDataPath;
			}
			if (this.pathLocation == IconCreator.SaveLocation.projectFolder)
			{
				return Path.GetDirectoryName(Application.dataPath);
			}
			return "";
		}

		// Token: 0x06000B9E RID: 2974 RVA: 0x00036219 File Offset: 0x00034419
		private void OnValidate()
		{
			if (this.mainCam == null)
			{
				this.mainCam = base.GetComponent<Camera>();
			}
		}

		// Token: 0x04000CEB RID: 3307
		protected bool isCreatingIcons;

		// Token: 0x04000CEC RID: 3308
		public bool useDafaultName;

		// Token: 0x04000CED RID: 3309
		public bool includeResolutionInFileName;

		// Token: 0x04000CEE RID: 3310
		public string iconFileName;

		// Token: 0x04000CEF RID: 3311
		public IconCreator.SaveLocation pathLocation;

		// Token: 0x04000CF0 RID: 3312
		public IconCreator.Mode mode;

		// Token: 0x04000CF1 RID: 3313
		public string folderName = "Screenshots";

		// Token: 0x04000CF2 RID: 3314
		public bool useTransparency = true;

		// Token: 0x04000CF3 RID: 3315
		public bool lookAtObjectCenter;

		// Token: 0x04000CF4 RID: 3316
		public bool dynamicFov;

		// Token: 0x04000CF5 RID: 3317
		public float fovOffset;

		// Token: 0x04000CF6 RID: 3318
		protected string finalPath;

		// Token: 0x04000CF7 RID: 3319
		private Vector3 mousePostion;

		// Token: 0x04000CF8 RID: 3320
		public KeyCode nextIconKey = KeyCode.Space;

		// Token: 0x04000CF9 RID: 3321
		protected bool CanMove;

		// Token: 0x04000CFA RID: 3322
		public bool preview = true;

		// Token: 0x04000CFB RID: 3323
		protected Camera whiteCam;

		// Token: 0x04000CFC RID: 3324
		protected Camera blackCam;

		// Token: 0x04000CFD RID: 3325
		public Camera mainCam;

		// Token: 0x04000CFE RID: 3326
		protected Texture2D texBlack;

		// Token: 0x04000CFF RID: 3327
		protected Texture2D texWhite;

		// Token: 0x04000D00 RID: 3328
		protected Texture2D finalTexture;

		// Token: 0x04000D01 RID: 3329
		private CameraClearFlags originalClearFlags;

		// Token: 0x04000D02 RID: 3330
		protected Transform currentObject;

		// Token: 0x0200021F RID: 543
		public enum SaveLocation
		{
			// Token: 0x04000D04 RID: 3332
			persistentDataPath,
			// Token: 0x04000D05 RID: 3333
			dataPath,
			// Token: 0x04000D06 RID: 3334
			projectFolder,
			// Token: 0x04000D07 RID: 3335
			custom
		}

		// Token: 0x02000220 RID: 544
		public enum Mode
		{
			// Token: 0x04000D09 RID: 3337
			Automatic,
			// Token: 0x04000D0A RID: 3338
			Manual
		}
	}
}
