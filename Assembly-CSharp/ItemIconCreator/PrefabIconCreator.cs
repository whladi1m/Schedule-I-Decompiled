using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ItemIconCreator
{
	// Token: 0x02000225 RID: 549
	[ExecuteInEditMode]
	public class PrefabIconCreator : IconCreator
	{
		// Token: 0x06000BB7 RID: 2999 RVA: 0x0003678E File Offset: 0x0003498E
		public override void BuildIcons()
		{
			base.StartCoroutine(this.BuildAllIcons());
		}

		// Token: 0x06000BB8 RID: 3000 RVA: 0x0003679D File Offset: 0x0003499D
		public override bool CheckConditions()
		{
			if (!base.CheckConditions())
			{
				return false;
			}
			if (this.itemsToShot.Length == 0)
			{
				Debug.LogError("There's no prefab to shoot");
				return false;
			}
			if (this.itemPosition == null)
			{
				Debug.LogError("Item position is null");
				return false;
			}
			return true;
		}

		// Token: 0x06000BB9 RID: 3001 RVA: 0x000367DC File Offset: 0x000349DC
		protected override void Update()
		{
			if (this.preview && !this.isCreatingIcons)
			{
				if (this.instantiatedItem != null)
				{
					if (this.dynamicFov)
					{
						base.UpdateFOV(this.instantiatedItem);
					}
					if (this.lookAtObjectCenter)
					{
						base.LookAtTargetCenter(this.instantiatedItem);
					}
					this.instantiatedItem.transform.position = this.itemPosition.transform.position;
					this.instantiatedItem.transform.rotation = this.itemPosition.transform.rotation;
				}
				else if (this.instantiatedItem == null && this.itemsToShot.Length != 0)
				{
					this.ClearShit();
					if (this.itemPosition.childCount > 0 && this.itemPosition.GetChild(0).GetComponent<MeshRenderer>() != null)
					{
						this.instantiatedItem = this.itemPosition.GetChild(0).gameObject;
					}
					else
					{
						this.instantiatedItem = UnityEngine.Object.Instantiate<GameObject>(this.itemsToShot[0], this.itemPosition.transform.position, this.itemPosition.transform.rotation, this.itemPosition);
					}
				}
			}
			base.Update();
		}

		// Token: 0x06000BBA RID: 3002 RVA: 0x0003691C File Offset: 0x00034B1C
		private void ClearShit()
		{
			List<Transform> list = new List<Transform>();
			for (int i = 0; i < this.itemPosition.childCount; i++)
			{
				list.Add(this.itemPosition.GetChild(i));
			}
			for (int j = 0; j < list.Count; j++)
			{
				UnityEngine.Object.DestroyImmediate(list[j].gameObject);
			}
		}

		// Token: 0x06000BBB RID: 3003 RVA: 0x00036979 File Offset: 0x00034B79
		public IEnumerator BuildAllIcons()
		{
			base.Initialize();
			int num;
			for (int i = 0; i < this.itemsToShot.Length; i = num + 1)
			{
				this.finalPath = "C:/Users/Tyler/Desktop/";
				if (this.instantiatedItem != null)
				{
					UnityEngine.Object.DestroyImmediate(this.instantiatedItem);
				}
				if (this.whiteCam != null)
				{
					this.whiteCam.enabled = false;
				}
				if (this.blackCam != null)
				{
					this.blackCam.enabled = false;
				}
				this.ClearShit();
				this.instantiatedItem = UnityEngine.Object.Instantiate<GameObject>(this.itemsToShot[i], this.itemPosition.transform.position, this.itemPosition.transform.rotation);
				if (IconCreatorCanvas.instance != null)
				{
					IconCreatorCanvas.instance.SetInfo(this.itemsToShot.Length, i, this.itemsToShot[i].name, true, this.nextIconKey);
				}
				this.currentObject = this.instantiatedItem.transform;
				if (this.dynamicFov)
				{
					base.UpdateFOV(this.instantiatedItem);
				}
				if (this.lookAtObjectCenter)
				{
					base.LookAtTargetCenter(this.instantiatedItem);
				}
				if (this.mode == IconCreator.Mode.Manual)
				{
					this.CanMove = true;
					yield return new WaitUntil(() => Input.GetKeyDown(this.nextIconKey));
					this.CanMove = false;
				}
				if (IconCreatorCanvas.instance != null)
				{
					IconCreatorCanvas.instance.SetTakingPicture();
					yield return null;
					yield return null;
				}
				yield return base.CaptureFrame(this.itemsToShot[i].name, i);
				num = i;
			}
			if (IconCreatorCanvas.instance != null)
			{
				IconCreatorCanvas.instance.SetInfo(0, 0, "", false, this.nextIconKey);
			}
			base.DeleteCameras();
			yield break;
		}

		// Token: 0x04000D19 RID: 3353
		[Header("Items")]
		public GameObject[] itemsToShot;

		// Token: 0x04000D1A RID: 3354
		public Transform itemPosition;

		// Token: 0x04000D1B RID: 3355
		private GameObject instantiatedItem;
	}
}
