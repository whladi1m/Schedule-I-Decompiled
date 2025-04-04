using System;
using UnityEngine;

namespace ScheduleOne.AvatarFramework
{
	// Token: 0x0200094D RID: 2381
	public class Eyebrow : MonoBehaviour
	{
		// Token: 0x060040EA RID: 16618 RVA: 0x00110DA4 File Offset: 0x0010EFA4
		public void SetScale(float _scale)
		{
			this.scale = _scale;
			this.Model.localScale = new Vector3(this.EyebrowDefaultScale.x, this.EyebrowDefaultScale.y, this.EyebrowDefaultScale.z * this.thickness) * this.scale;
		}

		// Token: 0x060040EB RID: 16619 RVA: 0x00110DFB File Offset: 0x0010EFFB
		public void SetThickness(float thickness)
		{
			this.thickness = thickness;
			this.SetScale(this.scale);
		}

		// Token: 0x060040EC RID: 16620 RVA: 0x00110E10 File Offset: 0x0010F010
		public void SetRestingAngle(float _angle)
		{
			this.restingAngle = _angle;
			base.transform.localRotation = Quaternion.Euler(base.transform.localEulerAngles.x, base.transform.localEulerAngles.y, this.restingAngle * ((this.Side == Eyebrow.ESide.Left) ? -1f : 1f));
		}

		// Token: 0x060040ED RID: 16621 RVA: 0x00110E70 File Offset: 0x0010F070
		public void SetRestingHeight(float normalizedHeight)
		{
			normalizedHeight = Mathf.Clamp(normalizedHeight, -1.1f, 1.5f);
			this.Model.transform.localPosition = new Vector3(this.EyebrowDefaultLocalPos.x, this.EyebrowDefaultLocalPos.y + normalizedHeight * 0.01f, this.EyebrowDefaultLocalPos.z);
		}

		// Token: 0x060040EE RID: 16622 RVA: 0x00110ECD File Offset: 0x0010F0CD
		public void SetColor(Color _col)
		{
			this.col = _col;
			this.Rend.material.color = this.col;
		}

		// Token: 0x04002EC7 RID: 11975
		private const float eyebrowHeightMultiplier = 0.01f;

		// Token: 0x04002EC8 RID: 11976
		[SerializeField]
		private Vector3 EyebrowDefaultScale;

		// Token: 0x04002EC9 RID: 11977
		[SerializeField]
		private Vector3 EyebrowDefaultLocalPos;

		// Token: 0x04002ECA RID: 11978
		[SerializeField]
		protected Eyebrow.ESide Side;

		// Token: 0x04002ECB RID: 11979
		[SerializeField]
		protected Transform Model;

		// Token: 0x04002ECC RID: 11980
		[SerializeField]
		protected MeshRenderer Rend;

		// Token: 0x04002ECD RID: 11981
		[Header("Eyebrow Data - Readonly")]
		[SerializeField]
		private Color col;

		// Token: 0x04002ECE RID: 11982
		[SerializeField]
		private float scale = 1f;

		// Token: 0x04002ECF RID: 11983
		[SerializeField]
		private float thickness = 1f;

		// Token: 0x04002ED0 RID: 11984
		[SerializeField]
		private float restingAngle;

		// Token: 0x0200094E RID: 2382
		public enum ESide
		{
			// Token: 0x04002ED2 RID: 11986
			Right,
			// Token: 0x04002ED3 RID: 11987
			Left
		}
	}
}
