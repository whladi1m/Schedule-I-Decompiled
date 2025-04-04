using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Token: 0x02000040 RID: 64
[ExecuteInEditMode]
public class WarehouseModular : MonoBehaviour
{
	// Token: 0x0600014B RID: 331 RVA: 0x00007524 File Offset: 0x00005724
	private void Start()
	{
		this.myMeshFilter = base.GetComponent<MeshFilter>();
		this.largeWall = (Resources.Load("Models/LargeWall") as GameObject);
		this.mediumWall = (Resources.Load("Models/MediumWall") as GameObject);
		this.smallWall = (Resources.Load("Models/SmallWall") as GameObject);
		this.miniWall = (Resources.Load("Models/Extra_SmallWall") as GameObject);
		this.tinyWall = (Resources.Load("Models/Extra_SmallWall1") as GameObject);
		this.windowWall = (Resources.Load("Models/WindowWall") as GameObject);
		this.smallWindowWall = (Resources.Load("Models/SmallWindowWall") as GameObject);
		this.innerCorner = (Resources.Load("Models/LeftCorner") as GameObject);
		this.outerCorner = (Resources.Load("Models/RightCorner") as GameObject);
		this.garageFrame = (Resources.Load("Models/GarageDoorFrame") as GameObject);
		this.doorFrame = (Resources.Load("Models/DoorWall") as GameObject);
		this.doubleDoorFrame = (Resources.Load("Models/DoubleDoorWall") as GameObject);
	}

	// Token: 0x0600014C RID: 332 RVA: 0x0000763C File Offset: 0x0000583C
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

	// Token: 0x0600014D RID: 333 RVA: 0x000076D8 File Offset: 0x000058D8
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

	// Token: 0x04000115 RID: 277
	[HideInInspector]
	public List<GameObject> itemsList = new List<GameObject>();

	// Token: 0x04000116 RID: 278
	[HideInInspector]
	public GameObject largeWall;

	// Token: 0x04000117 RID: 279
	[HideInInspector]
	public GameObject mediumWall;

	// Token: 0x04000118 RID: 280
	[HideInInspector]
	public GameObject smallWall;

	// Token: 0x04000119 RID: 281
	[HideInInspector]
	public GameObject miniWall;

	// Token: 0x0400011A RID: 282
	[HideInInspector]
	public GameObject tinyWall;

	// Token: 0x0400011B RID: 283
	[HideInInspector]
	public GameObject windowWall;

	// Token: 0x0400011C RID: 284
	[HideInInspector]
	public GameObject smallWindowWall;

	// Token: 0x0400011D RID: 285
	[HideInInspector]
	public GameObject innerCorner;

	// Token: 0x0400011E RID: 286
	[HideInInspector]
	public GameObject outerCorner;

	// Token: 0x0400011F RID: 287
	[HideInInspector]
	public GameObject garageFrame;

	// Token: 0x04000120 RID: 288
	[HideInInspector]
	public GameObject doorFrame;

	// Token: 0x04000121 RID: 289
	[HideInInspector]
	public GameObject doubleDoorFrame;

	// Token: 0x04000122 RID: 290
	private MeshFilter myMeshFilter;
}
