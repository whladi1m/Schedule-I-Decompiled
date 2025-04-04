using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000039 RID: 57
public class GarageDoorController : MonoBehaviour
{
	// Token: 0x0600012D RID: 301 RVA: 0x00006FE4 File Offset: 0x000051E4
	private void OnTriggerStay(Collider other)
	{
		if (other.gameObject.CompareTag("MainCamera"))
		{
			if (Input.GetKeyUp(KeyCode.E) && !this.doorStatus.doorIsOpen && this.doorStatus.canRotate)
			{
				this.doorStatus.canRotate = false;
				base.StartCoroutine(this.Rotate(Vector3.right, -80f, 1f));
			}
			if (Input.GetKeyUp(KeyCode.E) && this.doorStatus.doorIsOpen && this.doorStatus.canRotate)
			{
				this.doorStatus.canRotate = false;
				base.StartCoroutine(this.Rotate(Vector3.right, 80f, 1f));
			}
		}
	}

	// Token: 0x0600012E RID: 302 RVA: 0x0000709C File Offset: 0x0000529C
	private IEnumerator Rotate(Vector3 axis, float angle, float duration = 1f)
	{
		Quaternion from = this.garageDoor.rotation;
		Quaternion to = this.garageDoor.rotation;
		to *= Quaternion.Euler(axis * angle);
		float elapsed = 0f;
		while (elapsed < duration)
		{
			this.garageDoor.rotation = Quaternion.Slerp(from, to, elapsed / duration);
			elapsed += Time.deltaTime;
			yield return null;
		}
		this.garageDoor.rotation = to;
		this.doorStatus.doorIsOpen = !this.doorStatus.doorIsOpen;
		this.doorStatus.canRotate = true;
		yield break;
	}

	// Token: 0x04000102 RID: 258
	public GarageDoorStatus doorStatus;

	// Token: 0x04000103 RID: 259
	public Transform garageDoor;

	// Token: 0x04000104 RID: 260
	public Quaternion targetRotation = new Quaternion(80f, 0f, 0f, 0f);
}
