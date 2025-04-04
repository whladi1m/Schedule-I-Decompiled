using System;
using ScheduleOne.DevUtilities;
using UnityEngine;

namespace ScheduleOne.AvatarFramework
{
	// Token: 0x02000947 RID: 2375
	public class Eye : MonoBehaviour
	{
		// Token: 0x17000932 RID: 2354
		// (get) Token: 0x060040C6 RID: 16582 RVA: 0x001103BD File Offset: 0x0010E5BD
		// (set) Token: 0x060040C7 RID: 16583 RVA: 0x001103C5 File Offset: 0x0010E5C5
		public Eye.EyeLidConfiguration CurrentConfiguration { get; protected set; }

		// Token: 0x17000933 RID: 2355
		// (get) Token: 0x060040C8 RID: 16584 RVA: 0x001103CE File Offset: 0x0010E5CE
		public bool IsBlinking
		{
			get
			{
				return this.blinkRoutine != null;
			}
		}

		// Token: 0x060040C9 RID: 16585 RVA: 0x001103D9 File Offset: 0x0010E5D9
		private void Awake()
		{
			this.avatar = base.GetComponentInParent<Avatar>();
			this.EyeLight.Enabled = false;
		}

		// Token: 0x060040CA RID: 16586 RVA: 0x001103F3 File Offset: 0x0010E5F3
		public void SetSize(float size)
		{
			this.Container.localScale = Eye.defaultScale * size;
		}

		// Token: 0x060040CB RID: 16587 RVA: 0x0011040B File Offset: 0x0010E60B
		public void SetLidColor(Color color)
		{
			this.TopLidRend.material.color = color;
			this.BottomLidRend.material.color = color;
		}

		// Token: 0x060040CC RID: 16588 RVA: 0x0011042F File Offset: 0x0010E62F
		public void SetEyeballMaterial(Material mat, Color col)
		{
			this.EyeBallRend.material = mat;
		}

		// Token: 0x060040CD RID: 16589 RVA: 0x0011043D File Offset: 0x0010E63D
		public void SetEyeballColor(Color col, float emission = 0.115f, bool writeDefault = true)
		{
			this.EyeBallRend.material.color = col;
			this.EyeBallRend.material.SetColor("_EmissionColor", col * emission);
			if (writeDefault)
			{
				this.defaultEyeColor = col;
			}
		}

		// Token: 0x060040CE RID: 16590 RVA: 0x00110476 File Offset: 0x0010E676
		public void ResetEyeballColor()
		{
			this.EyeBallRend.material.color = this.defaultEyeColor;
			this.EyeBallRend.material.SetColor("_EmissionColor", this.defaultEyeColor * 0.115f);
		}

		// Token: 0x060040CF RID: 16591 RVA: 0x001104B4 File Offset: 0x0010E6B4
		public void ConfigureEyeLight(Color color, float intensity)
		{
			if (this.EyeLight == null || this.EyeLight._Light == null)
			{
				return;
			}
			this.EyeLight._Light.color = color;
			this.EyeLight._Light.intensity = intensity;
			this.EyeLight.Enabled = (intensity > 0f);
		}

		// Token: 0x060040D0 RID: 16592 RVA: 0x00110518 File Offset: 0x0010E718
		public void SetDilation(float dil)
		{
			this.PupilRend.SetBlendShapeWeight(0, dil * 100f);
		}

		// Token: 0x060040D1 RID: 16593 RVA: 0x00110530 File Offset: 0x0010E730
		public void SetEyeLidState(Eye.EyeLidConfiguration config, float time)
		{
			Eye.<>c__DisplayClass34_0 CS$<>8__locals1 = new Eye.<>c__DisplayClass34_0();
			CS$<>8__locals1.config = config;
			CS$<>8__locals1.time = time;
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.startConfig = this.CurrentConfiguration;
			this.StopExistingRoutines();
			if (!Singleton<CoroutineService>.InstanceExists)
			{
				return;
			}
			this.stateRoutine = Singleton<CoroutineService>.Instance.StartCoroutine(CS$<>8__locals1.<SetEyeLidState>g__Routine|0());
		}

		// Token: 0x060040D2 RID: 16594 RVA: 0x00110588 File Offset: 0x0010E788
		private void StopExistingRoutines()
		{
			if (this.blinkRoutine != null)
			{
				base.StopCoroutine(this.blinkRoutine);
			}
			if (this.stateRoutine != null)
			{
				base.StopCoroutine(this.stateRoutine);
			}
		}

		// Token: 0x060040D3 RID: 16595 RVA: 0x001105B4 File Offset: 0x0010E7B4
		public void SetEyeLidState(Eye.EyeLidConfiguration config, bool debug = false)
		{
			if (this.TopLidContainer == null || this.BottomLidContainer == null)
			{
				return;
			}
			if (debug)
			{
				string str = "Setting eye lid state: ";
				Eye.EyeLidConfiguration eyeLidConfiguration = config;
				Console.Log(str + eyeLidConfiguration.ToString(), null);
			}
			this.TopLidContainer.localRotation = Quaternion.Lerp(Quaternion.Euler(0f, 0f, 0f), Quaternion.Euler(-90f, 0f, 0f), config.topLidOpen);
			this.BottomLidContainer.localRotation = Quaternion.Lerp(Quaternion.Euler(0f, 0f, 0f), Quaternion.Euler(90f, 0f, 0f), config.bottomLidOpen);
			this.CurrentConfiguration = config;
		}

		// Token: 0x060040D4 RID: 16596 RVA: 0x00110684 File Offset: 0x0010E884
		public void LookAt(Vector3 position, bool instant = false)
		{
			Vector3 vector = (position - this.EyeLookOrigin.position).normalized;
			vector = this.EyeLookOrigin.InverseTransformDirection(vector);
			vector.z = Mathf.Clamp(vector.z, 0.1f, float.MaxValue);
			vector = this.EyeLookOrigin.TransformDirection(vector);
			Vector3 vector2 = this.EyeLookOrigin.InverseTransformDirection(vector);
			vector2.x = 0f;
			vector2 = this.EyeLookOrigin.TransformDirection(vector2);
			float num = Vector3.SignedAngle(this.EyeLookOrigin.forward, vector2, this.EyeLookOrigin.right);
			Vector3 vector3 = this.EyeLookOrigin.InverseTransformDirection(vector);
			vector3.y = 0f;
			vector3 = this.EyeLookOrigin.TransformDirection(vector3);
			float num2 = Vector3.SignedAngle(this.EyeLookOrigin.forward, vector3, this.EyeLookOrigin.up);
			Vector3 vector4 = new Vector3(Mathf.Clamp(num + this.AngleOffset.x, Eye.minRotation.y, Eye.maxRotation.y), Mathf.Clamp(num2 + this.AngleOffset.y, Eye.minRotation.x, Eye.maxRotation.x), 0f);
			if (instant)
			{
				string str = "instant: ";
				Vector3 vector5 = vector4;
				Debug.Log(str + vector5.ToString());
				this.PupilContainer.localRotation = Quaternion.Euler(vector4);
				return;
			}
			this.PupilContainer.localRotation = Quaternion.Lerp(this.PupilContainer.localRotation, Quaternion.Euler(vector4), Time.deltaTime * 10f);
		}

		// Token: 0x060040D5 RID: 16597 RVA: 0x00110828 File Offset: 0x0010EA28
		public void Blink(float blinkDuration, Eye.EyeLidConfiguration endState, bool debug = false)
		{
			Eye.<>c__DisplayClass38_0 CS$<>8__locals1 = new Eye.<>c__DisplayClass38_0();
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.blinkDuration = blinkDuration;
			CS$<>8__locals1.debug = debug;
			CS$<>8__locals1.endState = endState;
			this.StopExistingRoutines();
			if (this.avatar == null || this.avatar.EmotionManager == null)
			{
				return;
			}
			if (this.avatar.EmotionManager.IsSwitchingEmotion)
			{
				return;
			}
			this.blinkRoutine = Singleton<CoroutineService>.Instance.StartCoroutine(CS$<>8__locals1.<Blink>g__Routine|0());
		}

		// Token: 0x04002E9D RID: 11933
		public const float PupilLookSpeed = 10f;

		// Token: 0x04002E9E RID: 11934
		private static Vector3 defaultScale = new Vector3(0.03f, 0.03f, 0.015f);

		// Token: 0x04002E9F RID: 11935
		private static Vector3 maxRotation = new Vector3(40f, 35f, 0f);

		// Token: 0x04002EA0 RID: 11936
		private static Vector3 minRotation = new Vector3(-40f, -90f, 0f);

		// Token: 0x04002EA2 RID: 11938
		[Header("References")]
		public Transform Container;

		// Token: 0x04002EA3 RID: 11939
		public Transform TopLidContainer;

		// Token: 0x04002EA4 RID: 11940
		public Transform BottomLidContainer;

		// Token: 0x04002EA5 RID: 11941
		public Transform PupilContainer;

		// Token: 0x04002EA6 RID: 11942
		public MeshRenderer TopLidRend;

		// Token: 0x04002EA7 RID: 11943
		public MeshRenderer BottomLidRend;

		// Token: 0x04002EA8 RID: 11944
		public MeshRenderer EyeBallRend;

		// Token: 0x04002EA9 RID: 11945
		public Transform EyeLookOrigin;

		// Token: 0x04002EAA RID: 11946
		public OptimizedLight EyeLight;

		// Token: 0x04002EAB RID: 11947
		public SkinnedMeshRenderer PupilRend;

		// Token: 0x04002EAC RID: 11948
		private Coroutine blinkRoutine;

		// Token: 0x04002EAD RID: 11949
		private Coroutine stateRoutine;

		// Token: 0x04002EAE RID: 11950
		private Avatar avatar;

		// Token: 0x04002EAF RID: 11951
		private Color defaultEyeColor = Color.white;

		// Token: 0x04002EB0 RID: 11952
		public Vector2 AngleOffset = Vector2.zero;

		// Token: 0x02000948 RID: 2376
		[Serializable]
		public struct EyeLidConfiguration
		{
			// Token: 0x060040D8 RID: 16600 RVA: 0x00110920 File Offset: 0x0010EB20
			public override string ToString()
			{
				return "Top: " + this.topLidOpen.ToString() + ", Bottom: " + this.bottomLidOpen.ToString();
			}

			// Token: 0x060040D9 RID: 16601 RVA: 0x00110948 File Offset: 0x0010EB48
			public static Eye.EyeLidConfiguration Lerp(Eye.EyeLidConfiguration start, Eye.EyeLidConfiguration end, float lerp)
			{
				return new Eye.EyeLidConfiguration
				{
					topLidOpen = Mathf.Lerp(start.topLidOpen, end.topLidOpen, lerp),
					bottomLidOpen = Mathf.Lerp(start.bottomLidOpen, end.bottomLidOpen, lerp)
				};
			}

			// Token: 0x04002EB1 RID: 11953
			[Range(0f, 1f)]
			public float topLidOpen;

			// Token: 0x04002EB2 RID: 11954
			[Range(0f, 1f)]
			public float bottomLidOpen;
		}
	}
}
