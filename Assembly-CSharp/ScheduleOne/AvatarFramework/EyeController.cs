using System;
using System.Collections;
using EasyButtons;
using ScheduleOne.DevUtilities;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.AvatarFramework
{
	// Token: 0x02000950 RID: 2384
	[ExecuteInEditMode]
	public class EyeController : MonoBehaviour
	{
		// Token: 0x17000938 RID: 2360
		// (get) Token: 0x060040F4 RID: 16628 RVA: 0x00110FD5 File Offset: 0x0010F1D5
		// (set) Token: 0x060040F5 RID: 16629 RVA: 0x00110FDD File Offset: 0x0010F1DD
		public bool EyesOpen { get; protected set; } = true;

		// Token: 0x060040F6 RID: 16630 RVA: 0x00110FE6 File Offset: 0x0010F1E6
		protected virtual void Awake()
		{
			this.avatar = base.GetComponentInParent<Avatar>();
			this.avatar.onRagdollChange.AddListener(new UnityAction<bool, bool, bool>(this.RagdollChange));
			this.SetEyesOpen(true);
			this.ApplyDilation();
		}

		// Token: 0x060040F7 RID: 16631 RVA: 0x00111020 File Offset: 0x0010F220
		protected void Update()
		{
			if (!Application.isPlaying)
			{
				return;
			}
			if (this.BlinkingEnabled && this.blinkRoutine == null)
			{
				this.blinkRoutine = Singleton<CoroutineService>.Instance.StartCoroutine(this.BlinkRoutine());
			}
			if (this.BlinkingEnabled)
			{
				this.timeUntilNextBlink -= Time.deltaTime;
			}
		}

		// Token: 0x060040F8 RID: 16632 RVA: 0x00111075 File Offset: 0x0010F275
		private void OnEnable()
		{
			this.ApplyRestingEyeLidState();
		}

		// Token: 0x060040F9 RID: 16633 RVA: 0x00111080 File Offset: 0x0010F280
		[Button]
		public void ApplySettings()
		{
			this.leftEye.transform.localEulerAngles = new Vector3(0f, -this.eyeSpacing, 0f);
			this.rightEye.transform.localEulerAngles = new Vector3(0f, this.eyeSpacing, 0f);
			this.rightEye.transform.localPosition = new Vector3(0f, this.eyeHeight * EyeController.eyeHeightMultiplier, 0f);
			this.leftEye.transform.localPosition = new Vector3(0f, this.eyeHeight * EyeController.eyeHeightMultiplier, 0f);
			this.leftEye.SetSize(this.eyeSize);
			this.rightEye.SetSize(this.eyeSize);
			this.leftEye.SetLidColor(this.leftEyeLidColor);
			this.rightEye.SetLidColor(this.rightEyeLidColor);
			this.leftEye.SetEyeballMaterial(this.eyeBallMaterial, this.eyeBallColor);
			this.rightEye.SetEyeballMaterial(this.eyeBallMaterial, this.eyeBallColor);
			this.ApplyDilation();
			this.ApplyRestingEyeLidState();
		}

		// Token: 0x060040FA RID: 16634 RVA: 0x001111AC File Offset: 0x0010F3AC
		public void SetEyeballTint(Color col)
		{
			this.leftEye.SetEyeballColor(col, 0.115f, true);
			this.rightEye.SetEyeballColor(col, 0.115f, true);
		}

		// Token: 0x060040FB RID: 16635 RVA: 0x001111D2 File Offset: 0x0010F3D2
		public void OverrideEyeballTint(Color col)
		{
			this.leftEye.SetEyeballColor(col, 0.115f, true);
			this.rightEye.SetEyeballColor(col, 0.115f, true);
			this.eyeBallTintOverridden = true;
		}

		// Token: 0x060040FC RID: 16636 RVA: 0x001111FF File Offset: 0x0010F3FF
		public void ResetEyeballTint()
		{
			this.leftEye.SetEyeballColor(this.eyeBallColor, 0.115f, true);
			this.rightEye.SetEyeballColor(this.eyeBallColor, 0.115f, true);
			this.eyeBallTintOverridden = false;
		}

		// Token: 0x060040FD RID: 16637 RVA: 0x00111236 File Offset: 0x0010F436
		public void OverrideEyeLids(Eye.EyeLidConfiguration eyeLidConfiguration)
		{
			if (!this.eyeLidOverridden)
			{
				this.defaultLeftEyeRestingState = this.LeftRestingEyeState;
				this.defaultRightEyeRestingState = this.RightRestingEyeState;
			}
			this.LeftRestingEyeState = eyeLidConfiguration;
			this.RightRestingEyeState = eyeLidConfiguration;
			this.eyeLidOverridden = true;
		}

		// Token: 0x060040FE RID: 16638 RVA: 0x0011126D File Offset: 0x0010F46D
		public void ResetEyeLids()
		{
			this.LeftRestingEyeState = this.defaultLeftEyeRestingState;
			this.RightRestingEyeState = this.defaultRightEyeRestingState;
			this.eyeLidOverridden = false;
		}

		// Token: 0x060040FF RID: 16639 RVA: 0x0011128E File Offset: 0x0010F48E
		private void RagdollChange(bool oldValue, bool newValue, bool playStandUpAnim)
		{
			if (newValue)
			{
				this.ForceBlink();
			}
		}

		// Token: 0x06004100 RID: 16640 RVA: 0x0011129C File Offset: 0x0010F49C
		public void SetEyesOpen(bool open)
		{
			if (this.DEBUG)
			{
				Debug.Log("Setting eyes open: " + open.ToString());
			}
			this.EyesOpen = open;
			this.leftEye.SetEyeLidState(open ? this.LeftRestingEyeState : new Eye.EyeLidConfiguration
			{
				bottomLidOpen = 0f,
				topLidOpen = 0f
			}, 0.1f);
			this.rightEye.SetEyeLidState(open ? this.RightRestingEyeState : new Eye.EyeLidConfiguration
			{
				bottomLidOpen = 0f,
				topLidOpen = 0f
			}, 0.1f);
		}

		// Token: 0x06004101 RID: 16641 RVA: 0x00111346 File Offset: 0x0010F546
		private void ApplyDilation()
		{
			this.leftEye.SetDilation(this.PupilDilation);
			this.rightEye.SetDilation(this.PupilDilation);
		}

		// Token: 0x06004102 RID: 16642 RVA: 0x0011136A File Offset: 0x0010F56A
		public void SetPupilDilation(float dilation, bool writeDefault = true)
		{
			this.PupilDilation = dilation;
			this.ApplyDilation();
			this.defaultDilation = this.PupilDilation;
		}

		// Token: 0x06004103 RID: 16643 RVA: 0x00111385 File Offset: 0x0010F585
		public void ResetPupilDilation()
		{
			this.SetPupilDilation(this.defaultDilation, true);
		}

		// Token: 0x06004104 RID: 16644 RVA: 0x00111394 File Offset: 0x0010F594
		private void ApplyRestingEyeLidState()
		{
			this.leftEye.SetEyeLidState(this.LeftRestingEyeState, false);
			this.rightEye.SetEyeLidState(this.RightRestingEyeState, false);
		}

		// Token: 0x06004105 RID: 16645 RVA: 0x001113BA File Offset: 0x0010F5BA
		public void ForceBlink()
		{
			this.leftEye.Blink(this.blinkDuration, this.LeftRestingEyeState, false);
			this.rightEye.Blink(this.blinkDuration, this.RightRestingEyeState, false);
			this.ResetBlinkCounter();
		}

		// Token: 0x06004106 RID: 16646 RVA: 0x001113F2 File Offset: 0x0010F5F2
		public void SetLeftEyeRestingLidState(Eye.EyeLidConfiguration config)
		{
			this.LeftRestingEyeState = config;
			if (!this.leftEye.IsBlinking)
			{
				this.leftEye.SetEyeLidState(config, false);
			}
		}

		// Token: 0x06004107 RID: 16647 RVA: 0x00111415 File Offset: 0x0010F615
		public void SetRightEyeRestingLidState(Eye.EyeLidConfiguration config)
		{
			this.RightRestingEyeState = config;
			if (!this.rightEye.IsBlinking)
			{
				this.rightEye.SetEyeLidState(config, false);
			}
		}

		// Token: 0x06004108 RID: 16648 RVA: 0x00111438 File Offset: 0x0010F638
		private IEnumerator BlinkRoutine()
		{
			while (this.BlinkingEnabled)
			{
				if (this.EyesOpen)
				{
					if (this.DEBUG)
					{
						Debug.Log("Blinking");
					}
					this.leftEye.Blink(this.blinkDuration, this.LeftRestingEyeState, this.DEBUG);
					this.rightEye.Blink(this.blinkDuration, this.RightRestingEyeState, this.DEBUG);
				}
				this.ResetBlinkCounter();
				yield return new WaitUntil(() => this.timeUntilNextBlink <= 0f);
			}
			this.blinkRoutine = null;
			yield break;
		}

		// Token: 0x06004109 RID: 16649 RVA: 0x00111447 File Offset: 0x0010F647
		private void ResetBlinkCounter()
		{
			this.timeUntilNextBlink = UnityEngine.Random.Range(Mathf.Clamp(this.blinkInterval - this.blinkIntervalSpread, this.blinkDuration, float.MaxValue), this.blinkInterval + this.blinkIntervalSpread);
		}

		// Token: 0x0600410A RID: 16650 RVA: 0x0011147E File Offset: 0x0010F67E
		public void LookAt(Vector3 position, bool instant = false)
		{
			bool debug = this.DEBUG;
			this.leftEye.LookAt(position, instant);
			this.rightEye.LookAt(position, instant);
		}

		// Token: 0x04002ED6 RID: 11990
		private static float eyeHeightMultiplier = 0.03f;

		// Token: 0x04002ED7 RID: 11991
		public bool DEBUG;

		// Token: 0x04002ED9 RID: 11993
		[Header("References")]
		[SerializeField]
		public Eye leftEye;

		// Token: 0x04002EDA RID: 11994
		[SerializeField]
		public Eye rightEye;

		// Token: 0x04002EDB RID: 11995
		[Header("Location Settings")]
		[Range(0f, 45f)]
		[SerializeField]
		protected float eyeSpacing = 20f;

		// Token: 0x04002EDC RID: 11996
		[Range(-1f, 1f)]
		[SerializeField]
		protected float eyeHeight;

		// Token: 0x04002EDD RID: 11997
		[Range(0.5f, 1.5f)]
		[SerializeField]
		protected float eyeSize = 1f;

		// Token: 0x04002EDE RID: 11998
		[Header("Eyelid Settings")]
		[SerializeField]
		protected Color leftEyeLidColor = Color.white;

		// Token: 0x04002EDF RID: 11999
		[SerializeField]
		protected Color rightEyeLidColor = Color.white;

		// Token: 0x04002EE0 RID: 12000
		public Eye.EyeLidConfiguration LeftRestingEyeState;

		// Token: 0x04002EE1 RID: 12001
		public Eye.EyeLidConfiguration RightRestingEyeState;

		// Token: 0x04002EE2 RID: 12002
		[Header("Eyeball Settings")]
		[SerializeField]
		protected Material eyeBallMaterial;

		// Token: 0x04002EE3 RID: 12003
		[SerializeField]
		protected Color eyeBallColor;

		// Token: 0x04002EE4 RID: 12004
		[Header("Pupil State")]
		[Range(0f, 1f)]
		public float PupilDilation = 0.5f;

		// Token: 0x04002EE5 RID: 12005
		[Header("Blinking Settings")]
		public bool BlinkingEnabled = true;

		// Token: 0x04002EE6 RID: 12006
		[SerializeField]
		[Range(0f, 10f)]
		protected float blinkInterval = 3.5f;

		// Token: 0x04002EE7 RID: 12007
		[SerializeField]
		[Range(0f, 2f)]
		protected float blinkIntervalSpread = 0.5f;

		// Token: 0x04002EE8 RID: 12008
		[SerializeField]
		[Range(0f, 1f)]
		protected float blinkDuration = 0.2f;

		// Token: 0x04002EE9 RID: 12009
		private Avatar avatar;

		// Token: 0x04002EEA RID: 12010
		private Coroutine blinkRoutine;

		// Token: 0x04002EEB RID: 12011
		private float timeUntilNextBlink;

		// Token: 0x04002EEC RID: 12012
		private bool eyeBallTintOverridden;

		// Token: 0x04002EED RID: 12013
		private bool eyeLidOverridden;

		// Token: 0x04002EEE RID: 12014
		private Eye.EyeLidConfiguration defaultLeftEyeRestingState;

		// Token: 0x04002EEF RID: 12015
		private Eye.EyeLidConfiguration defaultRightEyeRestingState;

		// Token: 0x04002EF0 RID: 12016
		private float defaultDilation = 0.5f;
	}
}
