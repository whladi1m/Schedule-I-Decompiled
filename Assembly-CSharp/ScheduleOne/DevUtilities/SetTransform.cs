using System;
using UnityEngine;

namespace ScheduleOne.DevUtilities
{
	// Token: 0x020006E5 RID: 1765
	public class SetTransform : MonoBehaviour
	{
		// Token: 0x06002FFF RID: 12287 RVA: 0x000C80F2 File Offset: 0x000C62F2
		private void Awake()
		{
			if (this.SetOnAwake)
			{
				this.Set();
			}
		}

		// Token: 0x06003000 RID: 12288 RVA: 0x000C8102 File Offset: 0x000C6302
		private void Update()
		{
			if (this.SetOnUpdate)
			{
				this.Set();
			}
		}

		// Token: 0x06003001 RID: 12289 RVA: 0x000C8112 File Offset: 0x000C6312
		private void LateUpdate()
		{
			if (this.SetOnLateUpdate)
			{
				this.Set();
			}
		}

		// Token: 0x06003002 RID: 12290 RVA: 0x000C8124 File Offset: 0x000C6324
		private void Set()
		{
			if (base.gameObject.isStatic)
			{
				Console.LogWarning("SetTransform is being used on a static object.", null);
			}
			if (this.SetPosition)
			{
				base.transform.localPosition = this.LocalPosition;
			}
			if (this.SetRotation)
			{
				base.transform.localRotation = Quaternion.Euler(this.LocalRotation);
			}
			if (this.SetScale)
			{
				base.transform.localScale = this.LocalScale;
			}
		}

		// Token: 0x04002240 RID: 8768
		[Header("Frequency Settings")]
		public bool SetOnAwake = true;

		// Token: 0x04002241 RID: 8769
		public bool SetOnUpdate;

		// Token: 0x04002242 RID: 8770
		public bool SetOnLateUpdate;

		// Token: 0x04002243 RID: 8771
		[Header("Transform Settings")]
		public bool SetPosition;

		// Token: 0x04002244 RID: 8772
		public Vector3 LocalPosition = Vector3.zero;

		// Token: 0x04002245 RID: 8773
		public bool SetRotation;

		// Token: 0x04002246 RID: 8774
		public Vector3 LocalRotation = Vector3.zero;

		// Token: 0x04002247 RID: 8775
		public bool SetScale;

		// Token: 0x04002248 RID: 8776
		public Vector3 LocalScale = Vector3.one;
	}
}
