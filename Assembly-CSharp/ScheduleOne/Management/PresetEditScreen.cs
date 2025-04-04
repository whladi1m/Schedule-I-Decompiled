using System;
using System.Collections;
using System.Runtime.CompilerServices;
using ScheduleOne.DevUtilities;
using ScheduleOne.Management.Presets;
using ScheduleOne.Management.Presets.Options.SetterScreens;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace ScheduleOne.Management
{
	// Token: 0x02000579 RID: 1401
	public class PresetEditScreen : MonoBehaviour
	{
		// Token: 0x17000547 RID: 1351
		// (get) Token: 0x06002313 RID: 8979 RVA: 0x0008F99D File Offset: 0x0008DB9D
		public bool isOpen
		{
			get
			{
				return this.EditedPreset != null;
			}
		}

		// Token: 0x06002314 RID: 8980 RVA: 0x0008F9A8 File Offset: 0x0008DBA8
		protected virtual void Awake()
		{
			this.ReturnButton.onClick.AddListener(new UnityAction(this.ReturnButtonClicked));
			this.DeleteButton.onClick.AddListener(new UnityAction(this.DeleteButtonClicked));
			this.InputField.onValueChanged.AddListener(new UnityAction<string>(this.NameFieldChange));
			this.InputField.onEndEdit.AddListener(new UnityAction<string>(this.NameFieldDone));
			GameInput.RegisterExitListener(new GameInput.ExitDelegate(this.Exit), 4);
		}

		// Token: 0x06002315 RID: 8981 RVA: 0x0008FA37 File Offset: 0x0008DC37
		private void Exit(ExitAction action)
		{
			if (action.used)
			{
				return;
			}
			if (!this.isOpen)
			{
				return;
			}
			if (action.exitType == ExitType.Escape)
			{
				action.used = true;
				this.Close();
			}
		}

		// Token: 0x06002316 RID: 8982 RVA: 0x0008FA64 File Offset: 0x0008DC64
		public virtual void Open(Preset preset)
		{
			this.EditedPreset = preset;
			this.InputField.text = this.EditedPreset.PresetName;
			Canvas.ForceUpdateCanvases();
			this.RefreshIcon();
			this.RefreshTransforms();
			base.gameObject.SetActive(true);
			base.StartCoroutine(this.<Open>g__Delay|13_0());
		}

		// Token: 0x06002317 RID: 8983 RVA: 0x0008FAB8 File Offset: 0x0008DCB8
		public void Close()
		{
			this.EditedPreset = null;
			base.gameObject.SetActive(false);
		}

		// Token: 0x06002318 RID: 8984 RVA: 0x0008FACD File Offset: 0x0008DCCD
		private void RefreshIcon()
		{
			this.IconBackground.color = this.EditedPreset.PresetColor;
		}

		// Token: 0x06002319 RID: 8985 RVA: 0x0008FAEC File Offset: 0x0008DCEC
		private void RefreshTransforms()
		{
			this.InputField.ForceLabelUpdate();
			this.InputField.textComponent.ForceMeshUpdate(true, true);
			float renderedWidth = this.InputField.textComponent.renderedWidth;
			if (this.InputField.text == string.Empty)
			{
				renderedWidth = ((TextMeshProUGUI)this.InputField.placeholder).renderedWidth;
			}
			this.InputFieldRect.sizeDelta = new Vector2(renderedWidth + 3f, this.InputFieldRect.sizeDelta.y);
			this.InputFieldRect.anchoredPosition = new Vector2(1.5f, this.InputFieldRect.anchoredPosition.y);
			float num = 1.75f;
			float min = 5f;
			this.IconBackgroundRect.anchoredPosition = new Vector2(-Mathf.Clamp(renderedWidth / 2f + num, min, float.MaxValue), this.IconBackgroundRect.anchoredPosition.y);
			this.EditButtonRect.anchoredPosition = new Vector2(Mathf.Clamp(renderedWidth / 2f + num, min, float.MaxValue), this.IconBackgroundRect.anchoredPosition.y);
		}

		// Token: 0x0600231A RID: 8986 RVA: 0x0008FC15 File Offset: 0x0008DE15
		private void NameFieldChange(string newVal)
		{
			this.RefreshTransforms();
		}

		// Token: 0x0600231B RID: 8987 RVA: 0x0008FC1D File Offset: 0x0008DE1D
		private void NameFieldDone(string piss)
		{
			if (this.IsNameAppropriate(piss))
			{
				this.EditedPreset.SetName(piss);
				return;
			}
			this.InputField.text = this.EditedPreset.PresetName;
			this.RefreshTransforms();
		}

		// Token: 0x0600231C RID: 8988 RVA: 0x0008FC51 File Offset: 0x0008DE51
		private bool IsNameAppropriate(string name)
		{
			return !string.IsNullOrWhiteSpace(name) && !(name == string.Empty) && !(name == "Pablo");
		}

		// Token: 0x0600231D RID: 8989 RVA: 0x0008FC7C File Offset: 0x0008DE7C
		public void DeleteButtonClicked()
		{
			this.EditedPreset.DeletePreset(Preset.GetDefault(this.EditedPreset.ObjectType));
			this.Close();
		}

		// Token: 0x0600231E RID: 8990 RVA: 0x0008FC9F File Offset: 0x0008DE9F
		public void ReturnButtonClicked()
		{
			this.Close();
		}

		// Token: 0x06002320 RID: 8992 RVA: 0x0008FCA7 File Offset: 0x0008DEA7
		[CompilerGenerated]
		private IEnumerator <Open>g__Delay|13_0()
		{
			yield return new WaitForEndOfFrame();
			this.RefreshTransforms();
			yield break;
		}

		// Token: 0x04001A43 RID: 6723
		public Preset EditedPreset;

		// Token: 0x04001A44 RID: 6724
		[Header("References")]
		public RectTransform IconBackgroundRect;

		// Token: 0x04001A45 RID: 6725
		public Image IconBackground;

		// Token: 0x04001A46 RID: 6726
		public RectTransform InputFieldRect;

		// Token: 0x04001A47 RID: 6727
		public TMP_InputField InputField;

		// Token: 0x04001A48 RID: 6728
		public RectTransform EditButtonRect;

		// Token: 0x04001A49 RID: 6729
		public Button ReturnButton;

		// Token: 0x04001A4A RID: 6730
		public Button DeleteButton;

		// Token: 0x0200057A RID: 1402
		[Serializable]
		public class OptionData
		{
			// Token: 0x04001A4B RID: 6731
			public GameObject OptionEntryPrefab;

			// Token: 0x04001A4C RID: 6732
			public OptionSetterScreen OptionSetterScreen;
		}
	}
}
