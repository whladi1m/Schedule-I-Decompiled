using System;
using UnityEngine;
using UnityEngine.EventSystems;

// Token: 0x02000028 RID: 40
public class UICharacterRotate : MonoBehaviour, IPointerDownHandler, IEventSystemHandler, IPointerUpHandler
{
	// Token: 0x060000B9 RID: 185 RVA: 0x000058D2 File Offset: 0x00003AD2
	public void OnPointerDown(PointerEventData eventData)
	{
		this.toogle = true;
		Cursor.lockState = CursorLockMode.Locked;
	}

	// Token: 0x060000BA RID: 186 RVA: 0x000058E1 File Offset: 0x00003AE1
	public void OnPointerUp(PointerEventData eventData)
	{
		this.toogle = false;
		Cursor.lockState = CursorLockMode.None;
	}

	// Token: 0x060000BB RID: 187 RVA: 0x000058F0 File Offset: 0x00003AF0
	private void Update()
	{
		if (this.toogle)
		{
			this.uIController.CharacterCustomization.transform.localRotation = Quaternion.Euler(this.uIController.CharacterCustomization.transform.localEulerAngles + Vector3.up * -Input.GetAxis("Mouse X") * this.mouseRotateCharacterPower);
		}
	}

	// Token: 0x040000A9 RID: 169
	public UIControllerDEMO uIController;

	// Token: 0x040000AA RID: 170
	public float mouseRotateCharacterPower = 8f;

	// Token: 0x040000AB RID: 171
	private bool toogle;
}
