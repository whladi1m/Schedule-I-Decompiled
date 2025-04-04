using System;
using System.Collections;
using System.Runtime.CompilerServices;
using EasyButtons;
using UnityEngine;

// Token: 0x02000023 RID: 35
public class TrailerCharacterController : MonoBehaviour
{
	// Token: 0x0600009E RID: 158 RVA: 0x000053A1 File Offset: 0x000035A1
	private void Awake()
	{
		if (Input.GetKeyDown(KeyCode.LeftCurlyBracket))
		{
			this.Play();
		}
		if (Input.GetKeyDown(KeyCode.RightCurlyBracket))
		{
			this.Stop();
		}
	}

	// Token: 0x0600009F RID: 159 RVA: 0x000053C1 File Offset: 0x000035C1
	[Button]
	public void Play()
	{
		this.Stop();
		this.routine = base.StartCoroutine(this.<Play>g__Routine|6_0());
	}

	// Token: 0x060000A0 RID: 160 RVA: 0x000053DB File Offset: 0x000035DB
	[Button]
	public void Stop()
	{
		if (this.routine != null)
		{
			base.StopCoroutine(this.routine);
			this.routine = null;
		}
	}

	// Token: 0x060000A2 RID: 162 RVA: 0x0000540B File Offset: 0x0000360B
	[CompilerGenerated]
	private IEnumerator <Play>g__Routine|6_0()
	{
		float lerpTime = Vector3.Distance(this.StartPos.position, this.EndPos.position) / this.WalkSpeed;
		float t = 0f;
		for (;;)
		{
			this.Character.transform.position = Vector3.Lerp(this.StartPos.position, this.EndPos.position, t / lerpTime);
			this.Character.transform.rotation = this.StartPos.rotation;
			t += Time.deltaTime;
			if (t >= lerpTime)
			{
				break;
			}
			yield return new WaitForEndOfFrame();
		}
		this.Character.transform.position = this.EndPos.position;
		yield break;
	}

	// Token: 0x0400008D RID: 141
	public Transform StartPos;

	// Token: 0x0400008E RID: 142
	public Transform EndPos;

	// Token: 0x0400008F RID: 143
	public Transform Character;

	// Token: 0x04000090 RID: 144
	public float WalkSpeed = 2f;

	// Token: 0x04000091 RID: 145
	private Coroutine routine;
}
