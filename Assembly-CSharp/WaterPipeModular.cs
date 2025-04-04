using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Token: 0x02000041 RID: 65
[ExecuteInEditMode]
public class WaterPipeModular : MonoBehaviour
{
	// Token: 0x0600014F RID: 335 RVA: 0x0000772C File Offset: 0x0000592C
	private void Start()
	{
		this.largeWaterPipe = (Resources.Load("Models/Water_Pipe_Long") as GameObject);
		this.mediumWaterPipe = (Resources.Load("Models/Water_Pipe_Medium") as GameObject);
		this.smallWaterpipe = (Resources.Load("Models/Water_Pipe_Small") as GameObject);
		this.innerCorner = (Resources.Load("Models/Water_Pipe_left") as GameObject);
		this.outerCorner = (Resources.Load("Models/Water_Pipe_right") as GameObject);
	}

	// Token: 0x06000150 RID: 336 RVA: 0x000077A4 File Offset: 0x000059A4
	public void BuildNextItem(GameObject item)
	{
		if (this.itemsList.Count == 0)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(item, base.transform.position, item.transform.rotation);
			gameObject.transform.SetParent(base.transform);
			this.itemsList.Add(gameObject);
			return;
		}
		Transform child = this.itemsList.Last<GameObject>().transform.GetChild(0);
		GameObject gameObject2 = UnityEngine.Object.Instantiate<GameObject>(item, child.position, child.rotation);
		gameObject2.transform.SetParent(base.transform);
		this.itemsList.Add(gameObject2);
	}

	// Token: 0x06000151 RID: 337 RVA: 0x00007840 File Offset: 0x00005A40
	public void DeleteLastItem()
	{
		GameObject gameObject = this.itemsList.Last<GameObject>();
		if (Application.isPlaying)
		{
			UnityEngine.Object.Destroy(gameObject);
		}
		if (Application.isEditor)
		{
			UnityEngine.Object.DestroyImmediate(gameObject);
		}
		this.itemsList.Remove(gameObject);
	}

	// Token: 0x04000123 RID: 291
	[HideInInspector]
	public List<GameObject> itemsList = new List<GameObject>();

	// Token: 0x04000124 RID: 292
	[HideInInspector]
	public GameObject largeWaterPipe;

	// Token: 0x04000125 RID: 293
	[HideInInspector]
	public GameObject mediumWaterPipe;

	// Token: 0x04000126 RID: 294
	[HideInInspector]
	public GameObject smallWaterpipe;

	// Token: 0x04000127 RID: 295
	[HideInInspector]
	public GameObject innerCorner;

	// Token: 0x04000128 RID: 296
	[HideInInspector]
	public GameObject outerCorner;
}
