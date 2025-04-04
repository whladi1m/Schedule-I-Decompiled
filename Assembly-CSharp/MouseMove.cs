using System;
using UnityEngine;

// Token: 0x02000052 RID: 82
public class MouseMove : MonoBehaviour
{
	// Token: 0x060001B9 RID: 441 RVA: 0x0000AB24 File Offset: 0x00008D24
	private void Start()
	{
		this._originalPos = base.transform.position;
	}

	// Token: 0x060001BA RID: 442 RVA: 0x0000AB38 File Offset: 0x00008D38
	private void Update()
	{
		Vector3 vector = Input.mousePosition;
		vector.x /= (float)Screen.width;
		vector.y /= (float)Screen.height;
		vector.x -= 0.5f;
		vector.y -= 0.5f;
		vector *= 2f * this._sensitivity;
		base.transform.position = this._originalPos + vector;
	}

	// Token: 0x040001D6 RID: 470
	[SerializeField]
	private float _sensitivity = 0.5f;

	// Token: 0x040001D7 RID: 471
	private Vector3 _originalPos;
}
