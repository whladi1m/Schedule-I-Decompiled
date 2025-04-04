using System;
using UnityEngine;

namespace ScheduleOne.ScriptableObjects
{
	// Token: 0x0200076B RID: 1899
	[CreateAssetMenu(fileName = "StringDatabase", menuName = "ScriptableObjects/StringDatabase", order = 1)]
	[Serializable]
	public class StringDatabase : ScriptableObject
	{
		// Token: 0x04002544 RID: 9540
		[TextArea(2, 10)]
		public string[] Strings;
	}
}
