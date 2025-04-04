using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace Funly.SkyStudio
{
	// Token: 0x020001D6 RID: 470
	public abstract class BaseSpriteInstancedRenderer : MonoBehaviour
	{
		// Token: 0x17000248 RID: 584
		// (get) Token: 0x06000A52 RID: 2642 RVA: 0x0002DEE8 File Offset: 0x0002C0E8
		// (set) Token: 0x06000A53 RID: 2643 RVA: 0x0002DEF0 File Offset: 0x0002C0F0
		public int maxSprites { get; protected set; }

		// Token: 0x17000249 RID: 585
		// (get) Token: 0x06000A54 RID: 2644 RVA: 0x0002DEF9 File Offset: 0x0002C0F9
		// (set) Token: 0x06000A55 RID: 2645 RVA: 0x0002DF01 File Offset: 0x0002C101
		protected Camera m_ViewerCamera { get; set; }

		// Token: 0x06000A56 RID: 2646 RVA: 0x0002DF0A File Offset: 0x0002C10A
		private void Start()
		{
			if (!SystemInfo.supportsInstancing)
			{
				Debug.LogError("Can't render since GPU instancing isn't supported on this device");
				base.enabled = false;
				return;
			}
			this.m_ViewerCamera = Camera.main;
		}

		// Token: 0x06000A57 RID: 2647
		protected abstract Bounds CalculateMeshBounds();

		// Token: 0x06000A58 RID: 2648
		protected abstract BaseSpriteItemData CreateSpriteItemData();

		// Token: 0x06000A59 RID: 2649
		protected abstract bool IsRenderingEnabled();

		// Token: 0x06000A5A RID: 2650
		protected abstract int GetNextSpawnCount();

		// Token: 0x06000A5B RID: 2651
		protected abstract void CalculateSpriteTRS(BaseSpriteItemData data, out Vector3 spritePosition, out Quaternion spriteRotation, out Vector3 spriteScale);

		// Token: 0x06000A5C RID: 2652
		protected abstract void ConfigureSpriteItemData(BaseSpriteItemData data);

		// Token: 0x06000A5D RID: 2653
		protected abstract void PrepareDataArraysForRendering(int instanceId, BaseSpriteItemData data);

		// Token: 0x06000A5E RID: 2654
		protected abstract void PopulatePropertyBlockForRendering(ref MaterialPropertyBlock propertyBlock);

		// Token: 0x06000A5F RID: 2655 RVA: 0x0002DF30 File Offset: 0x0002C130
		private BaseSpriteItemData DequeueNextSpriteItemData()
		{
			BaseSpriteItemData baseSpriteItemData;
			if (this.m_Available.Count == 0)
			{
				baseSpriteItemData = this.CreateSpriteItemData();
			}
			else
			{
				baseSpriteItemData = this.m_Available.Dequeue();
			}
			this.m_Active.Add(baseSpriteItemData);
			return baseSpriteItemData;
		}

		// Token: 0x06000A60 RID: 2656 RVA: 0x0002DF6F File Offset: 0x0002C16F
		private void ReturnSpriteItemData(BaseSpriteItemData splash)
		{
			splash.Reset();
			this.m_Active.Remove(splash);
			this.m_Available.Enqueue(splash);
		}

		// Token: 0x06000A61 RID: 2657 RVA: 0x0002DF90 File Offset: 0x0002C190
		protected virtual void LateUpdate()
		{
			this.m_ViewerCamera = Camera.main;
			if (!this.IsRenderingEnabled())
			{
				return;
			}
			this.GenerateNewSprites();
			this.AdvanceAllSprites();
			this.RenderAllSprites();
		}

		// Token: 0x06000A62 RID: 2658 RVA: 0x0002DFB8 File Offset: 0x0002C1B8
		private void GenerateNewSprites()
		{
			int nextSpawnCount = this.GetNextSpawnCount();
			for (int i = 0; i < nextSpawnCount; i++)
			{
				BaseSpriteItemData baseSpriteItemData = this.DequeueNextSpriteItemData();
				baseSpriteItemData.spriteSheetData = this.m_SpriteSheetLayout;
				this.ConfigureSpriteItemData(baseSpriteItemData);
				Vector3 worldPosition;
				Quaternion rotation;
				Vector3 scale;
				this.CalculateSpriteTRS(baseSpriteItemData, out worldPosition, out rotation, out scale);
				baseSpriteItemData.SetTRSMatrix(worldPosition, rotation, scale);
				baseSpriteItemData.Start();
			}
		}

		// Token: 0x06000A63 RID: 2659 RVA: 0x0002E018 File Offset: 0x0002C218
		private void AdvanceAllSprites()
		{
			foreach (BaseSpriteItemData baseSpriteItemData in new HashSet<BaseSpriteItemData>(this.m_Active))
			{
				baseSpriteItemData.Continue();
				if (baseSpriteItemData.state == BaseSpriteItemData.SpriteState.Complete)
				{
					this.ReturnSpriteItemData(baseSpriteItemData);
				}
			}
		}

		// Token: 0x06000A64 RID: 2660 RVA: 0x0002E080 File Offset: 0x0002C280
		private void RenderAllSprites()
		{
			if (this.m_Active.Count == 0)
			{
				return;
			}
			if (this.renderMaterial == null)
			{
				Debug.LogError("Can't render sprite without a material.");
				return;
			}
			if (this.m_PropertyBlock == null)
			{
				this.m_PropertyBlock = new MaterialPropertyBlock();
			}
			int num = 0;
			foreach (BaseSpriteItemData baseSpriteItemData in this.m_Active)
			{
				if (num >= 1000)
				{
					Debug.LogError("Can't render any more sprites...");
					break;
				}
				if (baseSpriteItemData.state == BaseSpriteItemData.SpriteState.Animating && baseSpriteItemData.startTime <= Time.time)
				{
					this.m_ModelMatrices[num] = baseSpriteItemData.modelMatrix;
					this.m_StartTimes[num] = baseSpriteItemData.startTime;
					this.m_EndTimes[num] = baseSpriteItemData.endTime;
					this.PrepareDataArraysForRendering(num, baseSpriteItemData);
					num++;
				}
			}
			if (num == 0)
			{
				return;
			}
			this.m_PropertyBlock.Clear();
			this.m_PropertyBlock.SetFloatArray("_StartTime", this.m_StartTimes);
			this.m_PropertyBlock.SetFloatArray("_EndTime", this.m_EndTimes);
			this.m_PropertyBlock.SetFloat("_SpriteColumnCount", (float)this.m_SpriteSheetLayout.columns);
			this.m_PropertyBlock.SetFloat("_SpriteRowCount", (float)this.m_SpriteSheetLayout.rows);
			this.m_PropertyBlock.SetFloat("_SpriteItemCount", (float)this.m_SpriteSheetLayout.frameCount);
			this.m_PropertyBlock.SetFloat("_AnimationSpeed", (float)this.m_SpriteSheetLayout.frameRate);
			this.m_PropertyBlock.SetVector("_TintColor", this.m_TintColor);
			this.PopulatePropertyBlockForRendering(ref this.m_PropertyBlock);
			Mesh mesh = this.GetMesh();
			mesh.bounds = this.CalculateMeshBounds();
			Graphics.DrawMeshInstanced(mesh, 0, this.renderMaterial, this.m_ModelMatrices, num, this.m_PropertyBlock, ShadowCastingMode.Off, false, LayerMask.NameToLayer("TransparentFX"));
		}

		// Token: 0x06000A65 RID: 2661 RVA: 0x0002E278 File Offset: 0x0002C478
		protected Mesh GetMesh()
		{
			if (this.modelMesh)
			{
				return this.modelMesh;
			}
			if (this.m_DefaltModelMesh)
			{
				return this.m_DefaltModelMesh;
			}
			this.m_DefaltModelMesh = this.GenerateMesh();
			return this.m_DefaltModelMesh;
		}

		// Token: 0x06000A66 RID: 2662 RVA: 0x0002E2B4 File Offset: 0x0002C4B4
		protected virtual Mesh GenerateMesh()
		{
			Mesh mesh = new Mesh();
			Vector3[] vertices = new Vector3[]
			{
				new Vector3(-1f, -1f, 0f),
				new Vector3(-1f, 1f, 0f),
				new Vector3(1f, 1f, 0f),
				new Vector3(1f, -1f, 0f)
			};
			Vector2[] uv = new Vector2[]
			{
				new Vector2(0f, 0f),
				new Vector2(0f, 1f),
				new Vector2(1f, 1f),
				new Vector2(1f, 0f)
			};
			int[] triangles = new int[]
			{
				0,
				1,
				2,
				0,
				2,
				3
			};
			mesh.vertices = vertices;
			mesh.uv = uv;
			mesh.triangles = triangles;
			mesh.bounds = new Bounds(Vector3.zero, new Vector3(500f, 500f, 500f));
			return mesh;
		}

		// Token: 0x04000B4F RID: 2895
		public const int kArrayMaxSprites = 1000;

		// Token: 0x04000B51 RID: 2897
		[Tooltip("Mesh used to render the instances onto. If empty, a quad will be used.")]
		public Mesh modelMesh;

		// Token: 0x04000B52 RID: 2898
		[Tooltip("Sky Studio sprite sheet animated shader material.")]
		public Material renderMaterial;

		// Token: 0x04000B53 RID: 2899
		protected Queue<BaseSpriteItemData> m_Available = new Queue<BaseSpriteItemData>();

		// Token: 0x04000B54 RID: 2900
		protected HashSet<BaseSpriteItemData> m_Active = new HashSet<BaseSpriteItemData>();

		// Token: 0x04000B55 RID: 2901
		private MaterialPropertyBlock m_PropertyBlock;

		// Token: 0x04000B56 RID: 2902
		private Matrix4x4[] m_ModelMatrices = new Matrix4x4[1000];

		// Token: 0x04000B57 RID: 2903
		private float[] m_StartTimes = new float[1000];

		// Token: 0x04000B58 RID: 2904
		private float[] m_EndTimes = new float[1000];

		// Token: 0x04000B59 RID: 2905
		protected SpriteSheetData m_SpriteSheetLayout = new SpriteSheetData();

		// Token: 0x04000B5A RID: 2906
		protected Texture m_SpriteTexture;

		// Token: 0x04000B5B RID: 2907
		protected Color m_TintColor = Color.white;

		// Token: 0x04000B5D RID: 2909
		protected Mesh m_DefaltModelMesh;
	}
}
