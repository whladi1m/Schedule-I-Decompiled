using System;
using System.Collections.Generic;
using System.Linq;
using ScheduleOne.DevUtilities;
using ScheduleOne.PlayerScripts;
using UnityEngine;

namespace ScheduleOne.AvatarFramework.Emotions
{
	// Token: 0x0200096A RID: 2410
	public class AvatarEmotionManager : MonoBehaviour
	{
		// Token: 0x1700094A RID: 2378
		// (get) Token: 0x0600417B RID: 16763 RVA: 0x001129A2 File Offset: 0x00110BA2
		// (set) Token: 0x0600417C RID: 16764 RVA: 0x001129AA File Offset: 0x00110BAA
		public string CurrentEmotion { get; protected set; } = "Neutral";

		// Token: 0x1700094B RID: 2379
		// (get) Token: 0x0600417D RID: 16765 RVA: 0x001129B3 File Offset: 0x00110BB3
		// (set) Token: 0x0600417E RID: 16766 RVA: 0x001129BB File Offset: 0x00110BBB
		public AvatarEmotionPreset CurrentEmotionPreset { get; protected set; }

		// Token: 0x1700094C RID: 2380
		// (get) Token: 0x0600417F RID: 16767 RVA: 0x001129C4 File Offset: 0x00110BC4
		public bool IsSwitchingEmotion
		{
			get
			{
				return this.emotionLerpRoutine != null;
			}
		}

		// Token: 0x06004180 RID: 16768 RVA: 0x001129D0 File Offset: 0x00110BD0
		private void Start()
		{
			this.neutralPreset = this.EmotionPresetList.Find((AvatarEmotionPreset x) => x.PresetName == "Neutral");
			this.AddEmotionOverride("Neutral", "base_emotion", 0f, -1);
			base.InvokeRepeating("UpdateEmotion", 0f, 0.25f);
		}

		// Token: 0x06004181 RID: 16769 RVA: 0x000045B1 File Offset: 0x000027B1
		private void Update()
		{
		}

		// Token: 0x06004182 RID: 16770 RVA: 0x00112A38 File Offset: 0x00110C38
		public void UpdateEmotion()
		{
			if (PlayerSingleton<PlayerCamera>.InstanceExists && Vector3.Distance(base.transform.position, PlayerSingleton<PlayerCamera>.Instance.transform.position) > 30f)
			{
				return;
			}
			EmotionOverride highestPriorityOverride = this.GetHighestPriorityOverride();
			if (highestPriorityOverride == null)
			{
				return;
			}
			if (highestPriorityOverride != this.activeEmotionOverride)
			{
				this.activeEmotionOverride = highestPriorityOverride;
				this.LerpEmotion(this.GetEmotion(highestPriorityOverride.Emotion), 0.2f);
			}
		}

		// Token: 0x06004183 RID: 16771 RVA: 0x00112AA8 File Offset: 0x00110CA8
		public void ConfigureNeutralFace(Texture2D faceTex, float restingBrowHeight, float restingBrowAngle, Eye.EyeLidConfiguration leftEyelidConfig, Eye.EyeLidConfiguration rightEyelidConfig)
		{
			this.neutralPreset = this.EmotionPresetList.Find((AvatarEmotionPreset x) => x.PresetName == "Neutral");
			if (this.neutralPreset == null)
			{
				Debug.LogError("Could not find neutral preset");
				return;
			}
			this.neutralPreset.FaceTexture = faceTex;
			this.neutralPreset.BrowAngleChange_R = restingBrowAngle;
			this.neutralPreset.BrowAngleChange_L = restingBrowAngle;
			this.neutralPreset.BrowHeightChange_L = restingBrowHeight;
			this.neutralPreset.BrowHeightChange_R = restingBrowHeight;
			this.neutralPreset.LeftEyeRestingState = leftEyelidConfig;
			this.neutralPreset.RightEyeRestingState = rightEyelidConfig;
			if (this.CurrentEmotionPreset == this.neutralPreset)
			{
				this.SetEmotion(this.neutralPreset);
			}
		}

		// Token: 0x06004184 RID: 16772 RVA: 0x00112B68 File Offset: 0x00110D68
		public virtual void AddEmotionOverride(string emotionName, string overrideLabel, float duration = 0f, int priority = 0)
		{
			AvatarEmotionManager.<>c__DisplayClass25_0 CS$<>8__locals1 = new AvatarEmotionManager.<>c__DisplayClass25_0();
			CS$<>8__locals1.overrideLabel = overrideLabel;
			CS$<>8__locals1.duration = duration;
			CS$<>8__locals1.<>4__this = this;
			EmotionOverride emotionOverride = this.overrideStack.Find((EmotionOverride x) => x.Label.ToLower() == CS$<>8__locals1.overrideLabel.ToLower());
			if (emotionOverride != null)
			{
				emotionOverride.Emotion = emotionName;
				emotionOverride.Priority = priority;
				if (emotionOverride == this.activeEmotionOverride)
				{
					this.activeEmotionOverride = null;
				}
			}
			else
			{
				emotionOverride = new EmotionOverride(emotionName, CS$<>8__locals1.overrideLabel, priority);
				this.overrideStack.Add(emotionOverride);
			}
			this.ClearRemovalRoutine(CS$<>8__locals1.overrideLabel);
			if (CS$<>8__locals1.duration > 0f)
			{
				Coroutine value = Singleton<CoroutineService>.Instance.StartCoroutine(CS$<>8__locals1.<AddEmotionOverride>g__RemoveEmotionAfterDuration|1());
				this.emotionRemovalRoutines.Add(CS$<>8__locals1.overrideLabel.ToLower(), value);
			}
		}

		// Token: 0x06004185 RID: 16773 RVA: 0x00112C28 File Offset: 0x00110E28
		public void RemoveEmotionOverride(string label)
		{
			this.ClearRemovalRoutine(label);
			EmotionOverride emotionOverride = this.overrideStack.Find((EmotionOverride x) => x.Label.ToLower() == label.ToLower());
			if (emotionOverride == null)
			{
				return;
			}
			this.overrideStack.Remove(emotionOverride);
		}

		// Token: 0x06004186 RID: 16774 RVA: 0x00112C78 File Offset: 0x00110E78
		public void ClearOverrides()
		{
			EmotionOverride[] array = this.overrideStack.ToArray();
			for (int i = 0; i < array.Length; i++)
			{
				if (!(array[i].Label == "base_emotion"))
				{
					this.RemoveEmotionOverride(array[i].Label);
				}
			}
		}

		// Token: 0x06004187 RID: 16775 RVA: 0x00112CC4 File Offset: 0x00110EC4
		private void ClearRemovalRoutine(string label)
		{
			label = label.ToLower();
			if (this.emotionRemovalRoutines.ContainsKey(label))
			{
				if (this.emotionRemovalRoutines[label] != null)
				{
					base.StopCoroutine(this.emotionRemovalRoutines[label]);
				}
				this.emotionRemovalRoutines.Remove(label);
			}
		}

		// Token: 0x06004188 RID: 16776 RVA: 0x00112D14 File Offset: 0x00110F14
		public EmotionOverride GetHighestPriorityOverride()
		{
			return (from x in this.overrideStack
			orderby x.Priority descending
			select x).ToList<EmotionOverride>().FirstOrDefault<EmotionOverride>();
		}

		// Token: 0x06004189 RID: 16777 RVA: 0x00112D4C File Offset: 0x00110F4C
		private void LerpEmotion(AvatarEmotionPreset preset, float animationTime = 0.2f)
		{
			AvatarEmotionManager.<>c__DisplayClass30_0 CS$<>8__locals1 = new AvatarEmotionManager.<>c__DisplayClass30_0();
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.preset = preset;
			CS$<>8__locals1.animationTime = animationTime;
			if (this.CurrentEmotionPreset == null)
			{
				this.SetEmotion(CS$<>8__locals1.preset);
				return;
			}
			if (this.emotionLerpRoutine != null)
			{
				base.StopCoroutine(this.emotionLerpRoutine);
			}
			this.emotionLerpRoutine = Singleton<CoroutineService>.Instance.StartCoroutine(CS$<>8__locals1.<LerpEmotion>g__Routine|0());
		}

		// Token: 0x0600418A RID: 16778 RVA: 0x00112DB4 File Offset: 0x00110FB4
		private void SetEmotion(AvatarEmotionPreset preset)
		{
			this.CurrentEmotionPreset = preset;
			this.Avatar.SetFaceTexture(preset.FaceTexture, Color.black);
			this.EyeController.SetLeftEyeRestingLidState(preset.LeftEyeRestingState);
			this.EyeController.SetRightEyeRestingLidState(preset.RightEyeRestingState);
			this.EyeController.LeftRestingEyeState = preset.LeftEyeRestingState;
			this.EyeController.RightRestingEyeState = preset.RightEyeRestingState;
			this.EyebrowController.SetLeftBrowRestingHeight(preset.BrowHeightChange_L);
			this.EyebrowController.SetRightBrowRestingHeight(preset.BrowHeightChange_R);
			this.EyebrowController.leftBrow.SetRestingAngle(preset.BrowAngleChange_L);
			this.EyebrowController.rightBrow.SetRestingAngle(preset.BrowAngleChange_R);
		}

		// Token: 0x0600418B RID: 16779 RVA: 0x00112E70 File Offset: 0x00111070
		public bool HasEmotion(string emotion)
		{
			return this.GetEmotion(emotion) != null;
		}

		// Token: 0x0600418C RID: 16780 RVA: 0x00112E7C File Offset: 0x0011107C
		public AvatarEmotionPreset GetEmotion(string emotion)
		{
			return this.EmotionPresetList.Find((AvatarEmotionPreset x) => x.PresetName.ToLower() == emotion.ToLower());
		}

		// Token: 0x04002F72 RID: 12146
		public const float MAX_UPDATE_DISTANCE = 30f;

		// Token: 0x04002F75 RID: 12149
		[Header("Settings")]
		public List<AvatarEmotionPreset> EmotionPresetList = new List<AvatarEmotionPreset>();

		// Token: 0x04002F76 RID: 12150
		[Header("References")]
		public Avatar Avatar;

		// Token: 0x04002F77 RID: 12151
		public EyeController EyeController;

		// Token: 0x04002F78 RID: 12152
		public EyebrowController EyebrowController;

		// Token: 0x04002F79 RID: 12153
		private EmotionOverride activeEmotionOverride;

		// Token: 0x04002F7A RID: 12154
		private List<EmotionOverride> overrideStack = new List<EmotionOverride>();

		// Token: 0x04002F7B RID: 12155
		private AvatarEmotionPreset neutralPreset;

		// Token: 0x04002F7C RID: 12156
		private Coroutine emotionLerpRoutine;

		// Token: 0x04002F7D RID: 12157
		private Dictionary<string, Coroutine> emotionRemovalRoutines = new Dictionary<string, Coroutine>();

		// Token: 0x04002F7E RID: 12158
		private int tempIndex;
	}
}
