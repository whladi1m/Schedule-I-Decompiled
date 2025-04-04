using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200002B RID: 43
public abstract class BaseStarDataRenderer
{
	// Token: 0x14000001 RID: 1
	// (add) Token: 0x060000EF RID: 239 RVA: 0x00006384 File Offset: 0x00004584
	// (remove) Token: 0x060000F0 RID: 240 RVA: 0x000063BC File Offset: 0x000045BC
	public event BaseStarDataRenderer.StarDataProgress progressCallback;

	// Token: 0x14000002 RID: 2
	// (add) Token: 0x060000F1 RID: 241 RVA: 0x000063F4 File Offset: 0x000045F4
	// (remove) Token: 0x060000F2 RID: 242 RVA: 0x0000642C File Offset: 0x0000462C
	public event BaseStarDataRenderer.StarDataComplete completionCallback;

	// Token: 0x060000F3 RID: 243
	public abstract IEnumerator ComputeStarData();

	// Token: 0x060000F4 RID: 244 RVA: 0x00006461 File Offset: 0x00004661
	public virtual void Cancel()
	{
		this.progressCallback = null;
		this.completionCallback = null;
	}

	// Token: 0x060000F5 RID: 245 RVA: 0x00006471 File Offset: 0x00004671
	protected void SendProgress(float progress)
	{
		if (this.progressCallback != null)
		{
			this.progressCallback(this, progress);
		}
	}

	// Token: 0x060000F6 RID: 246 RVA: 0x00006488 File Offset: 0x00004688
	protected void SendCompletion(Texture2D texture, bool success)
	{
		if (this.completionCallback != null)
		{
			this.completionCallback(this, texture, success);
		}
	}

	// Token: 0x040000DF RID: 223
	public float density;

	// Token: 0x040000E0 RID: 224
	public float imageSize;

	// Token: 0x040000E1 RID: 225
	public string layerId;

	// Token: 0x040000E2 RID: 226
	public float maxRadius;

	// Token: 0x040000E3 RID: 227
	protected float sphereRadius = 1f;

	// Token: 0x040000E4 RID: 228
	protected bool isCancelled;

	// Token: 0x0200002C RID: 44
	// (Invoke) Token: 0x060000F9 RID: 249
	public delegate void StarDataProgress(BaseStarDataRenderer renderer, float progress);

	// Token: 0x0200002D RID: 45
	// (Invoke) Token: 0x060000FD RID: 253
	public delegate void StarDataComplete(BaseStarDataRenderer renderer, Texture2D texture, bool success);
}
