using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Token: 0x02000038 RID: 56
[ExecuteInEditMode]
public class ACpipeModular : MonoBehaviour
{
	// Token: 0x06000129 RID: 297 RVA: 0x00006E70 File Offset: 0x00005070
	private void Start()
	{
		this.largeACPipe = (Resources.Load("Models/AC_Pipe_Long") as GameObject);
		this.smallACpipe = (Resources.Load("Models/AC_Pipe_Medium") as GameObject);
		this.innerCorner = (Resources.Load("Models/AC_Pipe_Side_left") as GameObject);
		this.outerCorner = (Resources.Load("Models/AC_Pipe_Side_Right") as GameObject);
	}

	// Token: 0x0600012A RID: 298 RVA: 0x00006ED4 File Offset: 0x000050D4
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
		MonoBehaviour.print(child.gameObject.transform.parent.gameObject.name);
		GameObject gameObject2 = UnityEngine.Object.Instantiate<GameObject>(item, child.position, child.rotation);
		gameObject2.transform.SetParent(base.transform);
		this.itemsList.Add(gameObject2);
	}

	// Token: 0x0600012B RID: 299 RVA: 0x00006F90 File Offset: 0x00005190
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

	// Token: 0x040000FD RID: 253
	[HideInInspector]
	public List<GameObject> itemsList = new List<GameObject>();

	// Token: 0x040000FE RID: 254
	[HideInInspector]
	public GameObject largeACPipe;

	// Token: 0x040000FF RID: 255
	[HideInInspector]
	public GameObject smallACpipe;

	// Token: 0x04000100 RID: 256
	[HideInInspector]
	public GameObject innerCorner;

	// Token: 0x04000101 RID: 257
	[HideInInspector]
	public GameObject outerCorner;
}
