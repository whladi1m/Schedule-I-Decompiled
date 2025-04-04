using System;
using UnityEngine;

namespace ScheduleOne.ScriptableObjects
{
	// Token: 0x02000768 RID: 1896
	[CreateAssetMenu(fileName = "CallerID", menuName = "ScriptableObjects/CallerID", order = 1)]
	[Serializable]
	public class CallerID : ScriptableObject
	{
		// Token: 0x0400253C RID: 9532
		public string Name;

		// Token: 0x0400253D RID: 9533
		public Sprite ProfilePicture;
	}
}
