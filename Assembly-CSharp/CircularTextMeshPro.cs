using System;
using TMPro;
using UnityEngine;

// Token: 0x02000018 RID: 24
[ExecuteAlways]
public class CircularTextMeshPro : MonoBehaviour
{
	// Token: 0x06000073 RID: 115 RVA: 0x000049F3 File Offset: 0x00002BF3
	private void Reset()
	{
		this.text = base.gameObject.GetComponent<TMP_Text>();
	}

	// Token: 0x06000074 RID: 116 RVA: 0x00004A06 File Offset: 0x00002C06
	private void Awake()
	{
		if (!this.text)
		{
			this.text = base.gameObject.GetComponent<TMP_Text>();
		}
	}

	// Token: 0x06000075 RID: 117 RVA: 0x00004A26 File Offset: 0x00002C26
	private void OnEnable()
	{
		this.WarpText();
		TMPro_EventManager.TEXT_CHANGED_EVENT.Add(new Action<UnityEngine.Object>(this.ReactToTextChanged));
	}

	// Token: 0x06000076 RID: 118 RVA: 0x00004A44 File Offset: 0x00002C44
	private void OnDisable()
	{
		TMPro_EventManager.TEXT_CHANGED_EVENT.Remove(new Action<UnityEngine.Object>(this.ReactToTextChanged));
		this.text.ForceMeshUpdate(false, false);
	}

	// Token: 0x06000077 RID: 119 RVA: 0x00004A69 File Offset: 0x00002C69
	private void OnValidate()
	{
		this.WarpText();
	}

	// Token: 0x06000078 RID: 120 RVA: 0x00004A74 File Offset: 0x00002C74
	private void ReactToTextChanged(UnityEngine.Object obj)
	{
		TMP_Text tmp_Text = obj as TMP_Text;
		if (tmp_Text && this.text && tmp_Text == this.text && !this.isForceUpdatingMesh)
		{
			this.WarpText();
		}
	}

	// Token: 0x06000079 RID: 121 RVA: 0x00004ABC File Offset: 0x00002CBC
	private void WarpText()
	{
		if (!this.text)
		{
			return;
		}
		this.isForceUpdatingMesh = true;
		this.vertexCurve.preWrapMode = WrapMode.Once;
		this.vertexCurve.postWrapMode = WrapMode.Once;
		this.text.havePropertiesChanged = true;
		this.text.ForceMeshUpdate(false, false);
		TMP_TextInfo textInfo = this.text.textInfo;
		if (textInfo == null)
		{
			return;
		}
		int characterCount = textInfo.characterCount;
		if (characterCount == 0)
		{
			return;
		}
		float x = this.text.bounds.min.x;
		float x2 = this.text.bounds.max.x;
		for (int i = 0; i < characterCount; i++)
		{
			if (textInfo.characterInfo[i].isVisible)
			{
				int vertexIndex = textInfo.characterInfo[i].vertexIndex;
				int materialReferenceIndex = textInfo.characterInfo[i].materialReferenceIndex;
				Vector3[] vertices = textInfo.meshInfo[materialReferenceIndex].vertices;
				Vector3 vector = new Vector2((vertices[vertexIndex].x + vertices[vertexIndex + 2].x) / 2f, textInfo.characterInfo[i].baseLine);
				vertices[vertexIndex] += -vector;
				vertices[vertexIndex + 1] += -vector;
				vertices[vertexIndex + 2] += -vector;
				vertices[vertexIndex + 3] += -vector;
				float num = (vector.x - x) / (x2 - x);
				float num2 = num + 0.0001f;
				float y = this.vertexCurve.Evaluate(num) * this.yCurveScaling;
				float y2 = this.vertexCurve.Evaluate(num2) * this.yCurveScaling;
				Vector3 lhs = new Vector3(1f, 0f, 0f);
				Vector3 rhs = new Vector3(num2 * (x2 - x) + x, y2) - new Vector3(vector.x, y);
				float num3 = Mathf.Acos(Vector3.Dot(lhs, rhs.normalized)) * 57.29578f;
				float z = (Vector3.Cross(lhs, rhs).z > 0f) ? num3 : (360f - num3);
				Matrix4x4 matrix4x = Matrix4x4.TRS(new Vector3(0f, y, 0f), Quaternion.Euler(0f, 0f, z), Vector3.one);
				vertices[vertexIndex] = matrix4x.MultiplyPoint3x4(vertices[vertexIndex]);
				vertices[vertexIndex + 1] = matrix4x.MultiplyPoint3x4(vertices[vertexIndex + 1]);
				vertices[vertexIndex + 2] = matrix4x.MultiplyPoint3x4(vertices[vertexIndex + 2]);
				vertices[vertexIndex + 3] = matrix4x.MultiplyPoint3x4(vertices[vertexIndex + 3]);
				vertices[vertexIndex] += vector;
				vertices[vertexIndex + 1] += vector;
				vertices[vertexIndex + 2] += vector;
				vertices[vertexIndex + 3] += vector;
				this.text.UpdateVertexData();
			}
		}
		this.isForceUpdatingMesh = false;
	}

	// Token: 0x04000076 RID: 118
	[SerializeField]
	private TMP_Text text;

	// Token: 0x04000077 RID: 119
	public AnimationCurve vertexCurve = new AnimationCurve(new Keyframe[]
	{
		new Keyframe(0f, 0f, 0f, 1f, 0f, 0f),
		new Keyframe(0.5f, 1f),
		new Keyframe(1f, 0f, 1f, 0f, 0f, 0f)
	});

	// Token: 0x04000078 RID: 120
	public float yCurveScaling = 50f;

	// Token: 0x04000079 RID: 121
	private bool isForceUpdatingMesh;
}
