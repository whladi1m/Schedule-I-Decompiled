using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200000C RID: 12
public class FrameTimingsHUDDisplay : MonoBehaviour
{
	// Token: 0x06000048 RID: 72 RVA: 0x00004314 File Offset: 0x00002514
	private void Awake()
	{
		this.m_Style = new GUIStyle();
		this.m_Style.fontSize = 15;
		this.m_Style.normal.textColor = Color.white;
	}

	// Token: 0x06000049 RID: 73 RVA: 0x00004344 File Offset: 0x00002544
	private void OnGUI()
	{
		this.CaptureTimings();
		this.frameTimingsHistory.Add(new FrameTimingsHUDDisplay.FrameTimingPoint
		{
			cpuFrameTime = this.m_FrameTimings[0].cpuFrameTime,
			cpuMainThreadFrameTime = this.m_FrameTimings[0].cpuMainThreadFrameTime,
			cpuRenderThreadFrameTime = this.m_FrameTimings[0].cpuRenderThreadFrameTime,
			gpuFrameTime = this.m_FrameTimings[0].gpuFrameTime
		});
		if (this.frameTimingsHistory.Count > 200)
		{
			this.frameTimingsHistory.RemoveAt(0);
		}
		double num = 0.0;
		double num2 = 0.0;
		double num3 = 0.0;
		double num4 = 0.0;
		for (int i = 0; i < this.frameTimingsHistory.Count; i++)
		{
			num += this.frameTimingsHistory[i].cpuFrameTime;
			num2 += this.frameTimingsHistory[i].cpuMainThreadFrameTime;
			num3 += this.frameTimingsHistory[i].cpuRenderThreadFrameTime;
			num4 += this.frameTimingsHistory[i].gpuFrameTime;
		}
		num /= (double)this.frameTimingsHistory.Count;
		num2 /= (double)this.frameTimingsHistory.Count;
		num3 /= (double)this.frameTimingsHistory.Count;
		num4 /= (double)this.frameTimingsHistory.Count;
		string text = string.Format("\nCPU: {0:00.00}", num) + string.Format("\nMain Thread: {0:00.00}", num2) + string.Format("\nRender Thread: {0:00.00}", num3) + string.Format("\nGPU: {0:00.00}", num4);
		Color color = GUI.color;
		GUI.color = new Color(1f, 1f, 1f, 1f);
		float width = 300f;
		float height = 210f;
		GUILayout.BeginArea(new Rect(32f, 50f, width, height), "Frame Stats", GUI.skin.window);
		GUILayout.Label(text, this.m_Style, Array.Empty<GUILayoutOption>());
		GUILayout.EndArea();
		GUI.color = color;
	}

	// Token: 0x0600004A RID: 74 RVA: 0x00004577 File Offset: 0x00002777
	private void CaptureTimings()
	{
		FrameTimingManager.CaptureFrameTimings();
		FrameTimingManager.GetLatestTimings((uint)this.m_FrameTimings.Length, this.m_FrameTimings);
	}

	// Token: 0x04000062 RID: 98
	private GUIStyle m_Style;

	// Token: 0x04000063 RID: 99
	private readonly FrameTiming[] m_FrameTimings = new FrameTiming[1];

	// Token: 0x04000064 RID: 100
	public const int SAMPLE_SIZE = 200;

	// Token: 0x04000065 RID: 101
	public List<FrameTimingsHUDDisplay.FrameTimingPoint> frameTimingsHistory = new List<FrameTimingsHUDDisplay.FrameTimingPoint>();

	// Token: 0x0200000D RID: 13
	public struct FrameTimingPoint
	{
		// Token: 0x04000066 RID: 102
		public double cpuFrameTime;

		// Token: 0x04000067 RID: 103
		public double cpuMainThreadFrameTime;

		// Token: 0x04000068 RID: 104
		public double cpuRenderThreadFrameTime;

		// Token: 0x04000069 RID: 105
		public double gpuFrameTime;
	}
}
