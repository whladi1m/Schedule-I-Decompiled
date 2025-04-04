using System;
using ScheduleOne;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000027 RID: 39
public class ColorPicker : MonoBehaviour
{
	// Token: 0x060000AF RID: 175 RVA: 0x000056B3 File Offset: 0x000038B3
	private void Awake()
	{
		this.mainImage = base.GetComponent<Image>();
	}

	// Token: 0x060000B0 RID: 176 RVA: 0x000056C1 File Offset: 0x000038C1
	public void CursorEnter()
	{
		if (!this.pickerIcon.gameObject.activeSelf)
		{
			this.pickerIcon.gameObject.SetActive(true);
			this._activeCursor = true;
		}
	}

	// Token: 0x060000B1 RID: 177 RVA: 0x000056ED File Offset: 0x000038ED
	private void Update()
	{
		if (this._activeCursor)
		{
			this.CursorMove();
		}
	}

	// Token: 0x060000B2 RID: 178 RVA: 0x00005700 File Offset: 0x00003900
	public void CursorMove()
	{
		this.pickerIcon.position = GameInput.MousePosition;
		this.realSize = this.mainImage.rectTransform.rect.size * this.Canvas.scaleFactor;
		this.offset = this.mainImage.rectTransform.position - GameInput.MousePosition;
		this.offset = new Vector2(256f / this.realSize.x * this.offset.x, 256f / this.realSize.y * this.offset.y);
		this._findColor = this.mainImage.sprite.texture.GetPixel((int)(-(int)this.offset.x), 256 - (int)this.offset.y);
		if (this._findColor.a == 1f)
		{
			this.colorPreview.color = this._findColor;
		}
	}

	// Token: 0x060000B3 RID: 179 RVA: 0x00005812 File Offset: 0x00003A12
	public void CursorPickSkin()
	{
		if (this._findColor.a == 1f)
		{
			this.UIControllerDEMO.SetNewSkinColor(this._findColor);
		}
	}

	// Token: 0x060000B4 RID: 180 RVA: 0x00005837 File Offset: 0x00003A37
	public void CursorPickEye()
	{
		if (this._findColor.a == 1f)
		{
			this.UIControllerDEMO.SetNewEyeColor(this._findColor);
		}
	}

	// Token: 0x060000B5 RID: 181 RVA: 0x0000585C File Offset: 0x00003A5C
	public void CursorPickHair()
	{
		if (this._findColor.a == 1f)
		{
			this.UIControllerDEMO.SetNewHairColor(this._findColor);
		}
	}

	// Token: 0x060000B6 RID: 182 RVA: 0x00005881 File Offset: 0x00003A81
	public void CursorPickUnderpants()
	{
		if (this._findColor.a == 1f)
		{
			this.UIControllerDEMO.SetNewUnderpantsColor(this._findColor);
		}
	}

	// Token: 0x060000B7 RID: 183 RVA: 0x000058A6 File Offset: 0x00003AA6
	public void CursorExit()
	{
		if (this.pickerIcon.gameObject.activeSelf)
		{
			this.pickerIcon.gameObject.SetActive(false);
			this._activeCursor = false;
		}
	}

	// Token: 0x040000A0 RID: 160
	private Image mainImage;

	// Token: 0x040000A1 RID: 161
	public RectTransform pickerIcon;

	// Token: 0x040000A2 RID: 162
	public Image colorPreview;

	// Token: 0x040000A3 RID: 163
	private bool _activeCursor;

	// Token: 0x040000A4 RID: 164
	public Vector2 offset;

	// Token: 0x040000A5 RID: 165
	public UIControllerDEMO UIControllerDEMO;

	// Token: 0x040000A6 RID: 166
	public Canvas Canvas;

	// Token: 0x040000A7 RID: 167
	private Color _findColor;

	// Token: 0x040000A8 RID: 168
	public Vector2 realSize;
}
