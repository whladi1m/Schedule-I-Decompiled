using System;
using EasyButtons;
using UnityEngine;

// Token: 0x0200001A RID: 26
public class BuildingLODMaker : MonoBehaviour
{
	// Token: 0x0600007E RID: 126 RVA: 0x00004F30 File Offset: 0x00003130
	[Button]
	public void CreateLODs()
	{
		foreach (Transform transform in base.GetComponentsInChildren<Transform>())
		{
			foreach (BuildingLODMaker.LODGroupData lodgroupData in this.LODGroups)
			{
				string text = transform.gameObject.name;
				if (text.Contains(" "))
				{
					text = text.Substring(0, text.IndexOf(" "));
				}
				if (text == lodgroupData.ObjectName)
				{
					GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(lodgroupData.LODObject, transform);
					gameObject.transform.localPosition = Vector3.zero;
					gameObject.transform.localRotation = Quaternion.identity;
					gameObject.transform.localScale = Vector3.one;
					Debug.Log("Created LOD object " + lodgroupData.ObjectName + " under " + transform.gameObject.name);
				}
			}
		}
	}

	// Token: 0x0400007C RID: 124
	public BuildingLODMaker.LODGroupData[] LODGroups;

	// Token: 0x0400007D RID: 125
	public LODGroup LodGroup;

	// Token: 0x0200001B RID: 27
	[Serializable]
	public class LODGroupData
	{
		// Token: 0x0400007E RID: 126
		public string ObjectName;

		// Token: 0x0400007F RID: 127
		public GameObject LODObject;
	}
}
